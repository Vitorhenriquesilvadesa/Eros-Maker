using System;
using System.Collections.Generic;
using ErosScriptingEngine.Engine;
using ErosScriptingEngine.Lexer;
using ErosScriptingEngine.Parse.Event;
using ErosScriptingEngine.Parse.Expression;
using ErosScriptingEngine.Parse.Statement;
using ErosScriptingEngine.Pass;

namespace ErosScriptingEngine.Parse
{
    public class ParsePass : ErosCompilationPass<ScannedData, ErosExecutableScript>
    {
        private List<StatementNode> _statements;
        private ErosScriptStartEvent _startEvent;
        private ErosScriptUpdateEvent _updateEvent;
        private List<Token> _tokens;
        private int _current;

        private bool _startEventEmitted;
        private bool _updateEventEmitted;

        protected override ErosExecutableScript Pass(ScannedData input)
        {
            ParseTokens(input);
            return new ErosExecutableScript(_statements, _startEvent, _updateEvent);
        }

        private void ParseTokens(ScannedData input)
        {
            ResetInternalState(input);

            while (!IsAtEnd())
            {
                StatementNode statement = Statement();

                switch (statement.GetType())
                {
                    case { } t when t == typeof(ErosScriptUpdateEvent):
                        _updateEvent = (ErosScriptUpdateEvent)statement;
                        break;

                    case { } t when t == typeof(ErosScriptUpdateEvent):
                        _startEvent = (ErosScriptStartEvent)statement;
                        break;

                    default:
                        _statements.Add(statement);
                        break;
                }
            }
        }

        private StatementNode Statement()
        {
            if (Match(TokenType.Print)) return PrintStatement();
            if (Match(TokenType.On)) return EventStatement();

            return ExpressionStatement();
        }

        private StatementNode EventStatement()
        {
            if (Match(TokenType.Start)) return StartEventStatement();
            if (Match(TokenType.Update)) return UpdateEventStatement();

            ErosScriptingManager.Error("Invalid 'on' keyword usage.", Previous());
            return null;
        }

        private StatementNode StartEventStatement()
        {
            Consume(TokenType.Colon, "Expect ':' after 'on event' declaration.");

            if (_startEventEmitted)
            {
                ErosScriptingManager.Error("Duplicated 'on start' event.", Previous());
            }
            else
            {
                _startEventEmitted = true;
            }

            List<StatementNode> statements = new();

            while (!IsAtEnd() && !Check(TokenType.End))
            {
                statements.Add(Statement());
            }

            Consume(TokenType.End, "Missing 'end' keyword after 'on start' block.");

            return new ErosScriptStartEvent(statements);
        }

        private StatementNode UpdateEventStatement()
        {
            Consume(TokenType.Colon, "Expect ':' after 'on event' declaration.");

            if (_updateEventEmitted)
            {
                ErosScriptingManager.Error("Duplicated 'on update' event.", Previous());
            }
            else
            {
                _updateEventEmitted = true;
            }

            List<StatementNode> statements = new();

            while (!IsAtEnd() && !Check(TokenType.End))
            {
                statements.Add(Statement());
            }

            Consume(TokenType.End, "Missing 'end' keyword after 'on update' block.");

            return new ErosScriptUpdateEvent(statements);
        }

        private StatementNode ExpressionStatement()
        {
            ExpressionNode expression = Expression();
            return new ExpressionStatementNode(expression);
        }

        private StatementNode PrintStatement()
        {
            ExpressionNode expression = Expression();
            return new PrintStatementNode(expression);
        }

        private ExpressionNode Expression()
        {
            return Term();
        }

        private ExpressionNode Term()
        {
            ExpressionNode expression = Factor();

            while (Match(TokenType.Plus, TokenType.Minus))
            {
                Token @operator = Previous();
                ExpressionNode right = Factor();
                expression = new BinaryExpressionNode(expression, @operator, right);
            }

            return expression;
        }

        private ExpressionNode Factor()
        {
            ExpressionNode expression = Unary();

            while (Match(TokenType.Slash, TokenType.Star))
            {
                Token @operator = Previous();
                ExpressionNode right = Factor();
                expression = new BinaryExpressionNode(expression, @operator, right);
            }

            return expression;
        }

        private ExpressionNode Unary()
        {
            if (Match(TokenType.Not, TokenType.Minus))
            {
                Token @operator = Previous();
                ExpressionNode expression = Expression();
                return new UnaryExpressionNode(@operator, expression);
            }

            return Primary();
        }

        private ExpressionNode Primary()
        {
            if (Match(TokenType.Null, TokenType.Number,
                    TokenType.String, TokenType.Bool, TokenType.Identifier)
               )
            {
                return new LiteralExpressionNode(Previous());
            }

            if (Match(TokenType.LeftParen))
            {
                return Group();
            }

            if (Match(TokenType.Self))
            {
                return Match(TokenType.Dot) ? SelfGetExpression() : SelfExpression();
            }

            Advance();
            ErosScriptingManager.Error($"Invalid expression: '{Previous().lexeme}'.", Previous());
            return null;
        }

        private ExpressionNode Group()
        {
            ExpressionNode expression = Expression();
            Consume(TokenType.RightParen, "Expect ')' after 'group' expression.");
            return new GroupExpression(expression);
        }

        private ExpressionNode SelfGetExpression()
        {
            if (Match(TokenType.Id, TokenType.Name))
            {
                return new GetInternalPropertyExpressionNode(Previous());
            }

            return new SelfGetExpression(Previous());
        }

        private ExpressionNode SelfExpression()
        {
            return new SelfExpression(Previous());
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        private void Consume(TokenType type, string message)
        {
            if (!Match(type))
            {
                ErosScriptingManager.Error(message, Peek());
            }
        }

        private bool Match(params TokenType[] types)
        {
            if (!Check(types)) return false;

            Advance();
            return true;
        }

        private void Advance()
        {
            _current++;
        }

        private bool Check(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Peek().Type == type) return true;
            }

            return false;
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EndOfFile;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private void ResetInternalState(ScannedData input)
        {
            _current = 0;
            _statements = new List<StatementNode>();
            _tokens = input.Tokens;
        }

        public override Type GetInputType()
        {
            return typeof(ScannedData);
        }

        public override Type GetOutputType()
        {
            return typeof(ErosExecutableScript);
        }

        public override string GetDebugName()
        {
            return "Parse Pass";
        }
    }
}
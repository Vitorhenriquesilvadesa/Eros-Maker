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
        private List<Token> _tokens;
        private int _current;

        private readonly Dictionary<Type, ErosScriptEvent> emittedEvents = new();

        protected override ErosExecutableScript Pass(ScannedData input)
        {
            ParseTokens(input);
            return new ErosExecutableScript(_statements,
                (ErosScriptStartEvent)emittedEvents.GetValueOrDefault(typeof(ErosScriptStartEvent), null),
                (ErosScriptUpdateEvent)emittedEvents.GetValueOrDefault(typeof(ErosScriptUpdateEvent), null),
                (ErosScriptDestroyEvent)emittedEvents.GetValueOrDefault(typeof(ErosScriptDestroyEvent), null)
            );
        }

        private void ParseTokens(ScannedData input)
        {
            ResetInternalState(input);

            while (!IsAtEnd())
            {
                StatementNode statement = Statement();

                if (!emittedEvents.ContainsKey(statement.GetType()))
                    _statements.Add(statement);
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
            if (Match(TokenType.Destroy)) return DestroyEventStatement();

            ErosScriptingManager.Error("Invalid 'on' keyword usage.", Previous());
            return null;
        }

        private StatementNode StartEventStatement()
        {
            Consume(TokenType.Colon, "Expect ':' after 'on event' declaration.");

            List<StatementNode> statements = new();

            while (!IsAtEnd() && !Check(TokenType.End))
            {
                statements.Add(Statement());
            }

            Consume(TokenType.End, "Missing 'end' keyword after 'on start' block.");

            ErosScriptStartEvent e = new ErosScriptStartEvent(statements);

            if (emittedEvents.ContainsKey(typeof(ErosScriptStartEvent)))
            {
                ErosScriptingManager.Error("Duplicated 'on start' event.", Previous());
            }
            else
            {
                emittedEvents.Add(typeof(ErosScriptStartEvent), e);
            }

            return e;
        }

        private StatementNode UpdateEventStatement()
        {
            Consume(TokenType.Colon, "Expect ':' after 'on event' declaration.");

            List<StatementNode> statements = new();

            while (!IsAtEnd() && !Check(TokenType.End))
            {
                statements.Add(Statement());
            }

            Consume(TokenType.End, "Missing 'end' keyword after 'on update' block.");

            ErosScriptUpdateEvent e = new ErosScriptUpdateEvent(statements);

            if (emittedEvents.ContainsKey(typeof(ErosScriptUpdateEvent)))
            {
                ErosScriptingManager.Error("Duplicated 'on update' event.", Previous());
            }
            else
            {
                emittedEvents.Add(typeof(ErosScriptUpdateEvent), e);
            }

            return e;
        }

        private StatementNode DestroyEventStatement()
        {
            Consume(TokenType.Colon, "Expect ':' after 'on event' declaration.");

            List<StatementNode> statements = new();

            while (!IsAtEnd() && !Check(TokenType.End))
            {
                statements.Add(Statement());
            }

            Consume(TokenType.End, "Missing 'end' keyword after 'on update' block.");

            ErosScriptDestroyEvent e = new ErosScriptDestroyEvent(statements);

            if (emittedEvents.ContainsKey(typeof(ErosScriptDestroyEvent)))
            {
                ErosScriptingManager.Error("Duplicated 'on destroy' event.", Previous());
            }
            else
            {
                emittedEvents.Add(typeof(ErosScriptDestroyEvent), e);
            }

            return e;
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

            if (Match(TokenType.Vec3))
            {
                Consume(TokenType.LeftParen, "Expect '(' after 'vec3'.");
                ExpressionNode x = Expression();
                Consume(TokenType.Comma, "Expect ',' after 'x' component.");
                ExpressionNode y = Expression();
                Consume(TokenType.Comma, "Expect ',' after 'y' component.");
                ExpressionNode z = Expression();
                Consume(TokenType.RightParen, "Expect ')' after vec3 components.");
                return new Vector3ExpressionNode(x, y, z);
            }

            if (Match(TokenType.LeftParen))
            {
                return Group();
            }

            if (Match(TokenType.Self))
            {
                return Match(TokenType.Dot) ? SelfGetExpression() : SelfExpression();
            }

            if (Match(TokenType.Set))
            {
                return SetExpression();
            }

            Advance();
            ErosScriptingManager.Error($"Invalid expression: '{Previous().lexeme}'.", Previous());
            return null;
        }

        private ExpressionNode SetExpression()
        {
            ExpressionNode target = Expression();
            Consume(TokenType.To, "Expect 'to' keyword after variable to set.");
            ExpressionNode value = Expression();

            return new SetInternalPropertyExpressionNode(target, value);
        }

        private ExpressionNode Group()
        {
            ExpressionNode expression = Expression();
            Consume(TokenType.RightParen, "Expect ')' after 'group' expression.");
            return new GroupExpression(expression);
        }

        private ExpressionNode SelfGetExpression()
        {
            if (Match(TokenType.Id, TokenType.Name, TokenType.Position))
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
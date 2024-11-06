using System;
using System.Collections.Generic;
using ErosScriptingEngine.Lexer;
using ErosScriptingEngine.Parse.Expression;
using ErosScriptingEngine.Pass;
using NUnit.Framework;
using UnityEngine;

namespace ErosScriptingEngine.Parse
{
    public class ParsePass : ErosCompilationPass<ScannedData, ErosExecutableScript>
    {
        private List<AbstractExpressionNode> _expressions;
        private List<Token> _tokens;
        private int _current;

        protected override ErosExecutableScript Pass(ScannedData input)
        {
            ParseTokens(input);
            return new ErosExecutableScript(_expressions);
        }

        private void ParseTokens(ScannedData input)
        {
            ResetInternalState(input);

            while (!IsAtEnd())
            {
                AbstractExpressionNode expressionNode = Expression();
                _expressions.Add(expressionNode);
            }
        }

        private AbstractExpressionNode Expression()
        {
            return Primary();
        }

        private AbstractExpressionNode Primary()
        {
            if(Match(Token))
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
            _expressions = new List<AbstractExpressionNode>();
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
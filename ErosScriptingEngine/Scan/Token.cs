using System;

namespace ErosScriptingEngine.Lexer
{
    public class Token
    {
        public readonly TokenType Type;
        public readonly string lexeme;
        public readonly object literal;
        public readonly uint line;
        public readonly uint size;

        public Token(TokenType type, string lexeme, object literal, uint line, uint size)
        {
            Type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
            this.size = size;
        }

        public override string ToString()
        {
            return $"<{Type.ToString()}, {lexeme}, {literal}>";
        }
    }
}
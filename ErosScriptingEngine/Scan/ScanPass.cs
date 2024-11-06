using System;
using System.Collections.Generic;
using ErosScriptingEngine.Exception;
using ErosScriptingEngine.Lexer;
using ErosScriptingEngine.Pass;
using ErosScriptingEngine.Util;

namespace ErosScriptingEngine.Scan
{
    public class ScanPass : ErosCompilationPass<ErosScriptableFile, ScannedData>
    {
        private readonly Dictionary<string, TokenType> _keywords = new()
        {
            { "print", TokenType.Print },
            { "on", TokenType.On },
            { "start", TokenType.Start }
        };

        private uint _line;
        private int _start;
        private int _length;
        private string _source;
        private List<Token> _tokens;

        protected override ScannedData Pass(ErosScriptableFile input)
        {
            ResetInternalState(input);
            ScanTokens();
            return new ScannedData(_tokens);
        }

        private void ResetInternalState(ErosScriptableFile file)
        {
            _line = 1;
            _start = 0;
            _length = 0;
            _source = file.GetSource();
            _tokens = new List<Token>();
        }

        private void ScanTokens()
        {
            while (!IsAtEnd())
            {
                SyncCursors();
                ScanToken();
            }

            MakeToken(TokenType.EndOfFile);
        }

        private void ScanToken()
        {
            char c = Advance();

            switch (c)
            {
                case ' ':
                case '\t':
                case '\r':
                    break;

                case '\n':
                    _line++;
                    break;

                case ':':
                    MakeToken(TokenType.Colon);
                    break;

                case '"':
                    String();
                    break;

                default:
                    if (IsAlpha(c))
                    {
                        Identifier();
                        break;
                    }

                    throw new ErosUnexpectedCharacterException($"Unexpected character '{c}' at line {_line}");
            }
        }
        
        private void String()
        {
            
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            string name = _source.Substring(_start, _length);
            MakeToken(_keywords.GetValueOrDefault(name, TokenType.Identifier), name, null);
        }


        private void MakeToken(TokenType type)
        {
            string lexeme = _source.Substring(_start, _length);
            MakeToken(type, lexeme, null);
        }

        private void MakeToken(TokenType type, object literal)
        {
            string lexeme = _source.Substring(_start, _length);
            MakeToken(type, lexeme, literal);
        }

        private void MakeToken(TokenType type, string lexeme, object literal)
        {
            Token token = new Token(type, lexeme, literal, _line);
            _tokens.Add(token);
        }

        private void SyncCursors()
        {
            _start += _length;
            _length = 0;
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool IsDigit(char c)
        {
            return c is >= '0' and <= '9';
        }

        private bool IsAlpha(char c)
        {
            return c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
        }

        private bool Match(char c)
        {
            if (IsAtEnd()) return false;
            if (!Check(c)) return false;

            Advance();
            return true;
        }

        private bool Check(char c)
        {
            if (IsAtEnd()) return false;

            return Peek() == c;
        }

        private char Peek()
        {
            return IsAtEnd() ? '\0' : _source[_start + _length];
        }

        private bool IsAtEnd()
        {
            return _start + _length >= _source.Length;
        }

        private char Advance()
        {
            return _source[_start + _length++];
        }

        public override Type GetInputType()
        {
            return typeof(ErosScriptableFile);
        }

        public override Type GetOutputType()
        {
            return typeof(ScannedData);
        }

        public override string GetDebugName()
        {
            return "Lexer Pass";
        }
    }
}
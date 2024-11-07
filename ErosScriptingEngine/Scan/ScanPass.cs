using System;
using System.Collections.Generic;
using System.Globalization;
using ErosScriptingEngine.Engine;
using ErosScriptingEngine.Exception;
using ErosScriptingEngine.Lexer;
using ErosScriptingEngine.Pass;
using ErosScriptingEngine.Util;
using UnityEngine;

namespace ErosScriptingEngine.Scan
{
    public class ScanPass : ErosCompilationPass<ErosScriptableFile, ScannedData>
    {
        private readonly Dictionary<string, TokenType> _keywords = new()
        {
            { "print", TokenType.Print },
            { "on", TokenType.On },
            { "end", TokenType.End },
            { "id", TokenType.Id },
            { "self", TokenType.Self },
            { "start", TokenType.Start },
            { "update", TokenType.Update },
            { "name", TokenType.Name },
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

            MakeToken(TokenType.EndOfFile, null, null, 0);
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

                case '.':
                    MakeToken(TokenType.Dot);
                    break;

                case ':':
                    MakeToken(TokenType.Colon);
                    break;

                case '+':
                    MakeToken(TokenType.Plus);
                    break;

                case '-':
                    MakeToken(TokenType.Minus);
                    break;

                case '*':
                    MakeToken(TokenType.Star);
                    break;

                case '/':
                    MakeToken(TokenType.Slash);
                    break;

                case '(':
                    MakeToken(TokenType.LeftParen);
                    break;

                case ')':
                    MakeToken(TokenType.RightParen);
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
                    else if (IsDigit(c))
                    {
                        Number();
                        break;
                    }

                    throw new ErosUnexpectedCharacterException($"Unexpected character '{c}' at line {_line}");
            }
        }

        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();
                while (IsDigit(Peek())) Advance();
            }

            MakeToken(TokenType.Number, float.Parse(_source.Substring(_start, _length),
                NumberStyles.Float, CultureInfo.InvariantCulture));
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                Advance();
            }

            if (IsAtEnd())
            {
                ErosScriptingManager.Error("Unterminated string.", _line);
                return;
            }

            Advance();

            string lexeme = _source.Substring(_start + 1, _length - 2);
            MakeToken(TokenType.String, lexeme, lexeme, 0);
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            string name = _source.Substring(_start, _length);
            MakeToken(_keywords.GetValueOrDefault(name, TokenType.Identifier), name, null, 0);
        }


        private void MakeToken(TokenType type)
        {
            string lexeme = _source.Substring(_start, _length);
            MakeToken(type, lexeme, null, 0);
        }

        private void MakeToken(TokenType type, object literal)
        {
            string lexeme = _source.Substring(_start, _length);
            MakeToken(type, lexeme, literal, 0);
        }

        private void MakeToken(TokenType type, string lexeme, object literal, uint size)
        {
            Token token = new Token(type, lexeme, literal, _line, size);
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

        private char PeekNext()
        {
            return _start + _length + 1 >= _source.Length ? '\0' : _source[_start + _length + 1];
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
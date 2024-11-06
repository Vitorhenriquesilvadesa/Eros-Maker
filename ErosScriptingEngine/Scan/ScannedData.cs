using System.Collections.Generic;
using ErosScriptingEngine.Component;

namespace ErosScriptingEngine.Lexer
{
    public class ScannedData : ErosScriptingIOComponent<ScannedData>
    {
        public readonly List<Token> Tokens;

        public ScannedData(List<Token> tokens)
        {
            Tokens = tokens;
        }

        public override object Clone()
        {
            return new ScannedData(Tokens);
        }
    }
}
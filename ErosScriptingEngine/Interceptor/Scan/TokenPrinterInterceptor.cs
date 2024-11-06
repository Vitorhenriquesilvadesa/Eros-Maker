using ErosScriptingEngine.Lexer;
using ErosScriptingEngine.Util;
using UnityEngine;

namespace ErosScriptingEngine.Interceptor.Scan
{
    public class TokenPrinterInterceptor : ErosPassiveInterceptor<ErosScriptableFile, ScannedData>
    {
        public void BeforeState(ErosScriptableFile input)
        {
            
        }

        public void AfterState(ScannedData input)
        {
            foreach (Token token in input.Tokens)
            {
                Debug.Log(token);
            }
        }

        public string GetName()
        {
            return "token-printer";
        }
    }
}
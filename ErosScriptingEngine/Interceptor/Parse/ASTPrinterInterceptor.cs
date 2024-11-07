using ErosScriptingEngine.Lexer;
using ErosScriptingEngine.Parse;
using ErosScriptingEngine.Parse.Expression;
using ErosScriptingEngine.Parse.Statement;
using UnityEngine;

namespace ErosScriptingEngine.Interceptor.Parse
{
    public class ASTPrinterInterceptor : ErosPassiveInterceptor<ScannedData, ErosExecutableScript>
    {
        public void BeforeState(ScannedData input)
        {
        }

        public void AfterState(ErosExecutableScript input)
        {
            foreach (StatementNode statement in input.Statements)
            {
                Debug.Log(statement.ToString());
            }
        }

        public string GetName()
        {
            return "ast-printer";
        }
    }
}
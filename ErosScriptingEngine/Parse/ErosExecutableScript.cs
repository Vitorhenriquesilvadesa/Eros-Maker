using System.Collections.Generic;
using ErosScriptingEngine.Component;
using ErosScriptingEngine.Parse.Expression;

namespace ErosScriptingEngine.Parse
{
    public class ErosExecutableScript : ErosScriptingIOComponent<ErosExecutableScript>
    {
        public readonly List<AbstractExpressionNode> Expressions;

        public ErosExecutableScript(List<AbstractExpressionNode> expressions)
        {
            Expressions = expressions;
        }

        public override object Clone()
        {
            return new ErosExecutableScript(Expressions);
        }
    }
}
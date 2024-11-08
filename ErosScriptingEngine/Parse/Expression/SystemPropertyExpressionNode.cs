using ErosScriptingEngine.Lexer;

namespace ErosScriptingEngine.Parse.Expression
{
    public class SystemPropertyExpressionNode : ExpressionNode
    {
        public readonly Token PropertyName;

        public SystemPropertyExpressionNode(Token propertyName)
        {
            PropertyName = propertyName;
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessSystemPropertyExpression(this);
        }

    }
}
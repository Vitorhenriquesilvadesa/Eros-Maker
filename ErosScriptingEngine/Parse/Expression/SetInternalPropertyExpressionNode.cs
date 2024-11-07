using ErosScriptingEngine.Lexer;

namespace ErosScriptingEngine.Parse.Expression
{
    public class SetInternalPropertyExpressionNode : ExpressionNode
    {
        public readonly ExpressionNode Target;
        public readonly ExpressionNode Value;

        public SetInternalPropertyExpressionNode(ExpressionNode target, ExpressionNode value)
        {
            Target = target;
            Value = value;
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessSetInternalPropertyExpression(this);
        }
    }
}
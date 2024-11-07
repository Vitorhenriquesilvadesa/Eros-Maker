using ErosScriptingEngine.Lexer;

namespace ErosScriptingEngine.Parse.Expression
{
    public class SelfGetExpression : ExpressionNode
    {
        public readonly Token propertyName;

        public SelfGetExpression(Token propertyName)
        {
            this.propertyName = propertyName;
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessSelfGetExpression(this);
        }
    }
}
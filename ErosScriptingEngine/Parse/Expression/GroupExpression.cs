namespace ErosScriptingEngine.Parse.Expression
{
    public class GroupExpression : ExpressionNode
    {
        public readonly ExpressionNode Expression;

        public GroupExpression(ExpressionNode expression)
        {
            Expression = expression;
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessGroupExpression(this);
        }
    }
}
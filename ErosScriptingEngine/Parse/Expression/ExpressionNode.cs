namespace ErosScriptingEngine.Parse.Expression
{
    public abstract class ExpressionNode
    {
        public abstract T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor);
    }
}
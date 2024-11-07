namespace ErosScriptingEngine.Parse.Statement
{
    public abstract class StatementNode
    {
        public abstract T AcceptProcessor<T>(IStatementNodeProcessor<T> processor);
    }
}
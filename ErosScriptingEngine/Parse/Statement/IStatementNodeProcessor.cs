using ErosScriptingEngine.Parse.Event;

namespace ErosScriptingEngine.Parse.Statement
{
    public interface IStatementNodeProcessor<out T>
    {
        public T ProcessPrintStatement(PrintStatementNode statement);
        public T ProcessExpressionStatement(ExpressionStatementNode statement);
        public T ProcessStartEvent(ErosScriptStartEvent @event);
        public T ProcessUpdateEvent(ErosScriptUpdateEvent @event);
        public T ProcessDestroyEvent(ErosScriptDestroyEvent @event);
    }
}
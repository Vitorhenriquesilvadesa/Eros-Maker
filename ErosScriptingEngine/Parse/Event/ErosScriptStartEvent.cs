using System.Collections.Generic;
using ErosScriptingEngine.Parse.Statement;

namespace ErosScriptingEngine.Parse.Event
{
    public class ErosScriptStartEvent : ErosScriptEvent
    {
        public readonly List<StatementNode> Statements;

        public ErosScriptStartEvent(List<StatementNode> statements)
        {
            Statements = statements;
        }

        public override T AcceptProcessor<T>(IStatementNodeProcessor<T> processor)
        {
            return processor.ProcessStartEvent(this);
        }
    }
}
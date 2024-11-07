using System.Collections.Generic;
using ErosScriptingEngine.Parse.Statement;

namespace ErosScriptingEngine.Parse.Event
{
    public class ErosScriptUpdateEvent : ErosScriptEvent
    {
        public readonly List<StatementNode> Statements;

        public ErosScriptUpdateEvent(List<StatementNode> statements)
        {
            Statements = statements;
        }

        public override T AcceptProcessor<T>(IStatementNodeProcessor<T> processor)
        {
            return processor.ProcessUpdateEvent(this);
        }
    }
}
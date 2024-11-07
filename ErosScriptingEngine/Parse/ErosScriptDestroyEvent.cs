using System.Collections.Generic;
using ErosScriptingEngine.Parse.Event;
using ErosScriptingEngine.Parse.Statement;

namespace ErosScriptingEngine.Parse
{
    public class ErosScriptDestroyEvent : ErosScriptEvent
    {
        public readonly List<StatementNode> Statements;

        public ErosScriptDestroyEvent(List<StatementNode> statements)
        {
            Statements = statements;
        }

        public override T AcceptProcessor<T>(IStatementNodeProcessor<T> processor)
        {
            return processor.ProcessDestroyEvent(this);
        }
    }
}
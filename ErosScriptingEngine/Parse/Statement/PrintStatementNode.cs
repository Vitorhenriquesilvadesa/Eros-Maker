using ErosScriptingEngine.Parse.Expression;

namespace ErosScriptingEngine.Parse.Statement
{
    public class PrintStatementNode : StatementNode
    {
        public readonly ExpressionNode Expression;

        public PrintStatementNode(ExpressionNode expression)
        {
            Expression = expression;
        }

        public override string ToString()
        {
            return $"Print({Expression})";
        }

        public override T AcceptProcessor<T>(IStatementNodeProcessor<T> processor)
        {
            return processor.ProcessPrintStatement(this);
        }
    }
}
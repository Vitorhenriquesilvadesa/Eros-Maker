using ErosScriptingEngine.Parse.Expression;

namespace ErosScriptingEngine.Parse.Statement
{
    public class ExpressionStatementNode : StatementNode
    {
        public readonly ExpressionNode Expression;

        public ExpressionStatementNode(ExpressionNode expression)
        {
            Expression = expression;
        }

        public override string ToString()
        {
            return $"Expression({Expression})";
        }

        public override T AcceptProcessor<T>(IStatementNodeProcessor<T> processor)
        {
            return processor.ProcessExpressionStatement(this);
        }
    }
}
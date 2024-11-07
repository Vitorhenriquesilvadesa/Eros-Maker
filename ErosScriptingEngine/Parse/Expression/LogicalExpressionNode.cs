using ErosScriptingEngine.Lexer;

namespace ErosScriptingEngine.Parse.Expression
{
    public class LogicalExpressionNode : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly Token Operator;
        public readonly ExpressionNode Right;

        public LogicalExpressionNode(ExpressionNode left, Token @operator, ExpressionNode right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public override string ToString()
        {
            return $"Logical({Left} {Operator.Type} {Right})";
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessLogicalExpression(this);
        }
    }
}
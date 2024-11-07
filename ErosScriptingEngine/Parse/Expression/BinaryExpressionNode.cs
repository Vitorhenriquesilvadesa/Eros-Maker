using ErosScriptingEngine.Lexer;

namespace ErosScriptingEngine.Parse.Expression
{
    public class BinaryExpressionNode : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly Token Operator;
        public readonly ExpressionNode Right;

        public BinaryExpressionNode(ExpressionNode left, Token @operator, ExpressionNode right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public override string ToString()
        {
            return $"Binary({Left} {Operator.Type} {Right})";
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessBinaryExpression(this);
        }
    }
}
using ErosScriptingEngine.Lexer;

namespace ErosScriptingEngine.Parse.Expression
{
    public class UnaryExpressionNode : ExpressionNode
    {
        public readonly Token Operator;
        public readonly ExpressionNode ExpressionNode;

        public UnaryExpressionNode(Token @operator, ExpressionNode expressionNode)
        {
            Operator = @operator;
            ExpressionNode = expressionNode;
        }

        public override string ToString()
        {
            return $"Unary({Operator.Type}, {ExpressionNode})";
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessUnaryExpression(this);
        }
    }
}
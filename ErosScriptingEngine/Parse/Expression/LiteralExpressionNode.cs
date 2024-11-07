using ErosScriptingEngine.Lexer;

namespace ErosScriptingEngine.Parse.Expression
{
    public class LiteralExpressionNode : ExpressionNode
    {
        public readonly Token Literal;

        public LiteralExpressionNode(Token literal)
        {
            Literal = literal;
        }

        public override string ToString()
        {
            return $"Literal({Literal.literal})";
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessLiteralExpression(this);
        }
    }
}
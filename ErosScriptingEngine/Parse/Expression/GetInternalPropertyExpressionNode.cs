using ErosScriptingEngine.Lexer;

namespace ErosScriptingEngine.Parse.Expression
{
    public class GetInternalPropertyExpressionNode : ExpressionNode
    {
        public readonly Token keyword;

        public GetInternalPropertyExpressionNode(Token keyword)
        {
            this.keyword = keyword;
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessGetInternalPropertyExpression(this);
        }
    }
}
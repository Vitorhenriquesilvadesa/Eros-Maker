using ErosScriptingEngine.Lexer;

namespace ErosScriptingEngine.Parse.Expression
{
    public class SelfExpression : ExpressionNode
    {
        public readonly Token keyword;

        public SelfExpression(Token keyword)
        {
            this.keyword = keyword;
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            throw new System.NotImplementedException();
        }
    }
}
namespace ErosScriptingEngine.Parse.Expression
{
    public interface IExpressionNodeProcessor<out T>
    {
        public T ProcessBinaryExpression(BinaryExpressionNode expression);
        public T ProcessUnaryExpression(UnaryExpressionNode expression);
        public T ProcessLogicalExpression(LogicalExpressionNode expression);
        public T ProcessLiteralExpression(LiteralExpressionNode expression);
        public T ProcessGetInternalPropertyExpression(GetInternalPropertyExpressionNode expression);
        public T ProcessSelfGetExpression(SelfGetExpression expression);
        public T ProcessGroupExpression(GroupExpression expression);
        public T ProcessSetInternalPropertyExpression(SetInternalPropertyExpressionNode expression);
        public T ProcessVec3Expression(Vector3ExpressionNode expression);
        public T ProcessSystemPropertyExpression(SystemPropertyExpressionNode expression);
    }
}
using UnityEngine;

namespace ErosScriptingEngine.Parse.Expression
{
    public class Vector3ExpressionNode : ExpressionNode
    {
        public readonly Vector3 Value;

        public Vector3ExpressionNode(Vector3 value)
        {
            Value = value;
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessVec3Expression(this);
        }
    }
}
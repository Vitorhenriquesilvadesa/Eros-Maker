using UnityEngine;

namespace ErosScriptingEngine.Parse.Expression
{
    public class Vector3ExpressionNode : ExpressionNode
    {
        public readonly ExpressionNode X;
        public readonly ExpressionNode Y;
        public readonly ExpressionNode Z;


        public Vector3ExpressionNode(ExpressionNode x, ExpressionNode y, ExpressionNode z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override T AcceptProcessor<T>(IExpressionNodeProcessor<T> processor)
        {
            return processor.ProcessVec3Expression(this);
        }
    }
}
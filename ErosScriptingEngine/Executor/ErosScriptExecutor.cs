using ErosScriptingEngine.Component;
using ErosScriptingEngine.Engine;
using ErosScriptingEngine.Lexer;
using ErosScriptingEngine.Parse;
using ErosScriptingEngine.Parse.Event;
using ErosScriptingEngine.Parse.Expression;
using ErosScriptingEngine.Parse.Statement;
using UnityEngine;

namespace ErosScriptingEngine.Executor
{
    public class ErosScriptExecutor : IExpressionNodeProcessor<object>, IStatementNodeProcessor<object>
    {
        private ErosExecutableScript attachedScript;
        private ErosObject target;

        public void AttachObject(ErosObject target)
        {
            this.target = target;
        }

        public void AttachScript(ErosExecutableScript script)
        {
            attachedScript = script;
        }

        private object Evaluate(ExpressionNode expression)
        {
            return expression.AcceptProcessor(this);
        }

        private object Execute(StatementNode statement)
        {
            return statement.AcceptProcessor(this);
        }

        public object ProcessBinaryExpression(BinaryExpressionNode expression)
        {
            object left = Evaluate(expression.Left);
            object right = Evaluate(expression.Right);

            if (expression.Operator.Type is TokenType.Minus or TokenType.Star or TokenType.Slash)
            {
                CheckNumberOperands(left, right, expression);
            }

            switch (expression.Operator.Type)
            {
                case TokenType.Plus:
                    return (float)left + (float)right;

                case TokenType.Minus:
                    return (float)left - (float)right;

                case TokenType.Slash:
                    return (float)left / (float)right;

                case TokenType.Star:
                    return (float)left * (float)right;

                default:
                    ErosScriptingManager.Error($"Invalid operator: '{expression.Operator.lexeme}'.",
                        expression.Operator);
                    return null;
            }
        }

        public object ProcessUnaryExpression(UnaryExpressionNode expression)
        {
            object value = Evaluate(expression);

            switch (expression.Operator.Type)
            {
                case TokenType.Minus:
                    CheckNumberOperand(value, expression);
                    return -(float)value;

                case TokenType.Not:
                    CheckBooleanOperand(value, expression);
                    return !(bool)value;

                default:
                    ErosScriptingManager.Error($"Invalid operator: {expression.Operator.lexeme}", expression.Operator);
                    return null;
            }
        }

        private void CheckBooleanOperand(object value, UnaryExpressionNode expression)
        {
            if (value is not bool)
            {
                ErosScriptingManager.Error("Operand must be boolean.", expression.Operator);
            }
        }

        private void CheckNumberOperand(object value, UnaryExpressionNode expression)
        {
            if (value is not float)
            {
                ErosScriptingManager.Error("Operand must be a number.", expression.Operator);
            }
        }

        private void CheckBooleanOperands(object a, object b, BinaryExpressionNode expression)
        {
            if (a is not bool || b is not bool)
            {
                ErosScriptingManager.Error("Operands must be booleans.", expression.Operator);
            }
        }

        private void CheckNumberOperands(object a, object b, BinaryExpressionNode expression)
        {
            if (a is not float || b is not float)
            {
                ErosScriptingManager.Error("Operands must be numbers.", expression.Operator);
            }
        }

        public object ProcessLogicalExpression(LogicalExpressionNode expression)
        {
            return null;
        }

        public object ProcessLiteralExpression(LiteralExpressionNode expression)
        {
            return expression.Literal.literal;
        }

        public object ProcessGetInternalPropertyExpression(GetInternalPropertyExpressionNode expression)
        {
            ErosObjectDescriptor objectDescriptor = (ErosObjectDescriptor)target.GetDescriptor();

            switch (expression.keyword.Type)
            {
                case TokenType.Id:
                    return objectDescriptor.Id;

                case TokenType.Name:
                    return objectDescriptor.Name;

                case TokenType.Position:
                    return objectDescriptor.PhysicalObject.transform.position;

                default:
                    ErosScriptingManager.Error($"Unknown internal property '{expression.keyword.lexeme}'.",
                        expression.keyword);
                    return null;
            }
        }

        public object ProcessSelfGetExpression(SelfGetExpression expression)
        {
            return null;
        }

        public object ProcessGroupExpression(GroupExpression expression)
        {
            return Evaluate(expression.Expression);
        }

        public object ProcessSetInternalPropertyExpression(SetInternalPropertyExpressionNode expression)
        {
            ErosObjectDescriptor objectDescriptor = (ErosObjectDescriptor)target.GetDescriptor();
            object value = Evaluate(expression.Value);

            if (expression.Target is GetInternalPropertyExpressionNode)
            {
                Token propertyName = ((GetInternalPropertyExpressionNode)expression.Target).keyword;

                switch (propertyName.Type)
                {
                    case TokenType.Id:
                        ErosScriptingManager.Error("Cannot set property 'id'.",
                            propertyName);
                        return null;

                    case TokenType.Name:
                        ErosScriptingManager.Error("Cannot set property 'name'.",
                            propertyName);
                        return null;

                    case TokenType.Position:
                        if (value is Vector3)
                        {
                            objectDescriptor.PhysicalObject.transform.position = (Vector3)value;
                            return value;
                        }

                        ErosScriptingManager.Error("Position value must be in 'vec3(x, y, z)' format.",
                            propertyName);
                        return null;

                    case TokenType.Rotation:
                        if (value is Vector3)
                        {
                            objectDescriptor.PhysicalObject.transform.eulerAngles = (Vector3)value;
                            return value;
                        }

                        ErosScriptingManager.Error("Position value must be in 'vec3(x, y, z)' format.",
                            propertyName);
                        return null;

                    default:
                        ErosScriptingManager.Error($"Unknown internal property '{propertyName.lexeme}'.",
                            propertyName);
                        return null;
                }
            }

            ErosScriptingManager.Error("Unknown property.");
            return null;
        }

        public object ProcessVec3Expression(Vector3ExpressionNode expression)
        {
            object x = Evaluate(expression.X);
            object y = Evaluate(expression.Y);
            object z = Evaluate(expression.Z);

            return new Vector3((float)x, (float)y, (float)z);
        }

        public object ProcessSystemPropertyExpression(SystemPropertyExpressionNode expression)
        {
            switch (expression.PropertyName.Type)
            {
                case TokenType.DeltaTime:
                    return Time.deltaTime;
                
                case TokenType.Time:
                    return Time.time;

                default:
                    ErosScriptingManager.Error($"Unknown system property '{expression.PropertyName.lexeme}'.");
                    return null;
            }
        }

        public object ProcessPrintStatement(PrintStatementNode statement)
        {
            object result = Evaluate(statement.Expression);
            Debug.Log(result.ToString());
            return null;
        }

        public object ProcessExpressionStatement(ExpressionStatementNode statement)
        {
            Evaluate(statement.Expression);
            return null;
        }

        public object ProcessStartEvent(ErosScriptStartEvent @event)
        {
            foreach (StatementNode statement in @event.Statements)
            {
                Execute(statement);
            }

            return null;
        }

        public object ProcessUpdateEvent(ErosScriptUpdateEvent @event)
        {
            foreach (StatementNode statement in @event.Statements)
            {
                Execute(statement);
            }

            return null;
        }

        public object ProcessDestroyEvent(ErosScriptDestroyEvent @event)
        {
            foreach (StatementNode statement in @event.Statements)
            {
                Execute(statement);
            }

            return null;
        }

        public void CallDestroyEventIfDeclared()
        {
            if (attachedScript.DestroyEvent is not null)
            {
                Execute(attachedScript.DestroyEvent);
            }
        }

        public void CallStartEventIfDeclared()
        {
            if (attachedScript.StartEvent is not null)
            {
                Execute(attachedScript.StartEvent);
            }
        }

        public void CallUpdateEventIfDeclared()
        {
            if (attachedScript.UpdateEvent is not null)
            {
                Execute(attachedScript.UpdateEvent);
            }
        }

        public void RunScriptBody()
        {
            foreach (StatementNode statement in attachedScript.Statements)
            {
                Execute(statement);
            }
        }
    }
}
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
            switch (expression.keyword.Type)
            {
                case TokenType.Id:
                    return target.Id;

                case TokenType.Name:
                    return target.Name;

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
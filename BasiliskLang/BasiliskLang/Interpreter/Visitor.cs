using BasiliskLang.Interpreter.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter
{
    public class Visitor : IVisitor
    {
        public BuiltInFunctions SystemFunctions;
        public Dictionary<(string, int), FunctionDefinition> DefinedFunctions;
        public Stack<FunctionCallContext> FunctionCallContexts;
        public Stack<Value> ValueStack;
        public Visitor()
        {
            SystemFunctions = new BuiltInFunctions();
            DefinedFunctions = new Dictionary<(string, int), FunctionDefinition>();
            FunctionCallContexts = new Stack<FunctionCallContext>();
            ValueStack = new Stack<Value>();
        }
        #region helpers-variablevalue
        public Value? TryGetVariableValue(string identifier, string property) => FunctionCallContexts.Peek().TryGetVariableValue(identifier, property);
        public void SetVariablePropertyValue(string identifier, string property, Value value) => FunctionCallContexts.Peek().SetVariablePropertyValue(identifier, property, value);
        public void AddVariableValue(string identifier, Value value) => FunctionCallContexts.Peek().AddVariableValue(identifier, value);
        #endregion
        #region helpers-context
        public void EnterFunction() => FunctionCallContexts.Push(new FunctionCallContext());
        public void LeaveFunction() => FunctionCallContexts.Pop();
        public void SetReturn() => FunctionCallContexts.Peek().IsReturning = true;
        public bool IsReturning => FunctionCallContexts.Peek().IsReturning;
        #endregion
        public void EvaluateComplexExpression(ComplexExpression expression)
        {
            expression.Components[0].Accept(this);
            for(int i = 1; i<expression.Components.Count; i++)
            {
                var leftValue = ValueStack.Pop();
                expression.Components[i].Accept(this);
                var rightValue = ValueStack.Pop();
                var result = leftValue.Operate(expression.Operations[i - 1], rightValue);
                if (result == null)
                    throw new RuntimeException("Expression evaluation ended in null result (operation not implemented)");
                ValueStack.Push(result);
            }
        }
        public void Visit(ProgramRoot programRoot)
        {
            foreach (var definition in programRoot.functionsDefinitions)
                DefinedFunctions.Add(definition.Key, definition.Value);
            // we create false function call context in order to initialize state for operations
            EnterFunction();
            foreach (var statement in programRoot.Statements)
                Visit(statement);
            LeaveFunction();
        }

        public void Visit(AdditiveExpression additiveExpression)
        {
            EvaluateComplexExpression(additiveExpression);
        }

        public void Visit(Assignable assignable)
        {
            var variableValue = TryGetVariableValue(assignable.Identifier, assignable.Property);
            if (variableValue == null)
                throw new RuntimeException("Use of undefined variable");
            ValueStack.Push(variableValue);
        }

        public void Visit(BlockStatement blockStatement)
        {
            foreach (var statement in blockStatement.Statements)
            {
                statement.Accept(this);
                if (IsReturning)
                    break;
            }
        }

        public void Visit(FunctionDefinition definition)
        {
            definition.BlockStatement.Accept(this);
        }

        public void Visit(AssignStatement assignStatement)
        {
            assignStatement.Right.Accept(this);
            var right = ValueStack.Pop();
            if (assignStatement.Left.Property == null)
                AddVariableValue(assignStatement.Left.Identifier, right);
            else
                SetVariablePropertyValue(assignStatement.Left.Identifier, assignStatement.Left.Property, right);
        }
        public void Visit(IfStatement ifStatement)
        {
            ifStatement.Condition.Accept(this);
            var conditionResult = ValueStack.Pop();
            if (conditionResult.Type != ValueType.Dynamic)
                throw new RuntimeException("Condition evaluation didn't end in numeric expression");
            var conditionResultDynamic = (DynamicValue)conditionResult;
            if ((conditionResultDynamic.Value is bool && (bool)conditionResultDynamic.Value) || (conditionResultDynamic.Value is not bool && conditionResultDynamic.Value != 0))
                ifStatement.TrueBlockStatement.Accept(this);
            else
                ifStatement.FalseBlockStatement?.Accept(this);
        }

        public void Visit(WhileStatement whileStatement)
        {
            whileStatement.Condition.Accept(this);
            var conditionResult = ValueStack.Pop();
            if (conditionResult.Type != ValueType.Dynamic)
                throw new RuntimeException("Condition evaluation didn't end in numeric expression");
            var conditionResultDynamic = (DynamicValue)conditionResult;
            while ((conditionResultDynamic.Value is bool && (bool)conditionResultDynamic.Value) || (conditionResultDynamic.Value is not bool && conditionResultDynamic.Value != 0))
            {
                whileStatement.BlockStatement.Accept(this);
                whileStatement.Condition.Accept(this);
                conditionResult = ValueStack.Pop();
                if (conditionResult.Type != ValueType.Dynamic)
                    throw new RuntimeException("Condition evaluation didn't end in numeric expression");
                conditionResultDynamic = (DynamicValue)conditionResult;
            }
        }

        public void Visit(ReturnStatement returnStatement)
        {
            returnStatement.Expression?.Accept(this);
            SetReturn();
            if (FunctionCallContexts.Count <= 1)
                throw new RuntimeException("Return outside any function");
        }

        public void Visit(FunctionCallStatement functionCallStatement)
        {
            var functionName = functionCallStatement.FunctionName;
            var argumentsCount = functionCallStatement.FunctionArguments.Count;
            if (SystemFunctions.IsDefined(functionName, argumentsCount))
            {
                List<Value> arguments = new List<Value>();
                for (int i = 0; i < argumentsCount; i++)
                {
                    functionCallStatement.FunctionArguments[i].Accept(this);
                    arguments.Add(ValueStack.Pop());
                }
                var returnValue = SystemFunctions.CallFunction(functionName, arguments);
                if (returnValue != null)
                    ValueStack.Push(returnValue);
            }
            else if(DefinedFunctions.TryGetValue((functionName, argumentsCount), out FunctionDefinition functionDefinition))
            {
                if (argumentsCount != functionDefinition.Parameters.Count)
                    throw new RuntimeException("Function not defined for this number of arguments");
                Dictionary<string, Value> arguments = new Dictionary<string, Value>();
                for (int i = 0; i < argumentsCount; i++)
                {
                    functionCallStatement.FunctionArguments[i].Accept(this);
                    if (arguments.ContainsKey(functionDefinition.Parameters[i]))
                        throw new RuntimeException("Duplicate arguments in function");
                    arguments.Add(functionDefinition.Parameters[i], ValueStack.Pop());
                }
                EnterFunction();
                foreach (var argument in arguments)
                    AddVariableValue(argument.Key, argument.Value);
                functionDefinition.Accept(this);
                LeaveFunction();
            }
            else
                throw new RuntimeException("Call to undefined function");
        }

        public void Visit(LogicExpression logicExpression)
        {
            EvaluateComplexExpression(logicExpression);
        }

        public void Visit(RelationExpression relationExpression)
        {
            EvaluateComplexExpression(relationExpression);
        }

        public void Visit(MultiplicativeExpression multiplicativeExpression)
        {
            EvaluateComplexExpression(multiplicativeExpression);
        }

        public void Visit(UnaryExpression unaryExpression)
        {
            unaryExpression.InnerExpression.Accept(this);
            var currentValue = ValueStack.Pop();
            var result = currentValue.Operate(unaryExpression.Operation, null);
            ValueStack.Push(result);
        }

        public void Visit(GroupedExpression groupedExpression)
        {
            groupedExpression.InnerExpression.Accept(this);
        }

        public void Visit(FunctionCallExpression functionCallExpression)
        {
            var functionName = functionCallExpression.FunctionName;
            var argumentsCount = functionCallExpression.FunctionArguments.Count;
            if (SystemFunctions.IsDefined(functionName, argumentsCount))
            {
                List<Value> arguments = new List<Value>();
                for (int i = 0; i < argumentsCount; i++)
                {
                    functionCallExpression.FunctionArguments[i].Accept(this);
                    arguments.Add(ValueStack.Pop());
                }
                var returnValue = SystemFunctions.CallFunction(functionName, arguments);
                if (returnValue != null)
                    ValueStack.Push(returnValue);
            }
            else if (DefinedFunctions.TryGetValue((functionName, argumentsCount), out FunctionDefinition functionDefinition))
            {
                if (argumentsCount != functionDefinition.Parameters.Count)
                    throw new RuntimeException("Function not defined for this number of arguments");
                Dictionary<string, Value> arguments = new Dictionary<string, Value>();
                for (int i = 0; i < argumentsCount; i++)
                {
                    functionCallExpression.FunctionArguments[i].Accept(this);
                    if (arguments.ContainsKey(functionDefinition.Parameters[i]))
                        throw new RuntimeException("Duplicate arguments in function");
                    arguments.Add(functionDefinition.Parameters[i], ValueStack.Pop());
                }
                EnterFunction();
                foreach (var argument in arguments)
                    AddVariableValue(argument.Key, argument.Value);
                functionDefinition.Accept(this);
                LeaveFunction();
            }
            else
                throw new RuntimeException("Call to undefined function");
        }

        public void Visit(BoolValueExpression boolValue) => ValueStack.Push(new DynamicValue(boolValue.value));

        public void Visit(IntValueExpression intValue) => ValueStack.Push(new DynamicValue(intValue.value));

        public void Visit(FloatValueExpression floatValue) => ValueStack.Push(new DynamicValue(floatValue.value));

        public void Visit(StringValueExpression stringValue) => ValueStack.Push(new StringValue(stringValue.value));

        public void Visit(Statement statement)
        {
            if (statement is AssignStatement)
                (statement as AssignStatement).Accept(this);
            else if (statement is FunctionCallStatement)
                (statement as FunctionCallStatement).Accept(this);
            else if (statement is IfStatement)
                (statement as IfStatement).Accept(this);
            else if (statement is WhileStatement)
                (statement as WhileStatement).Accept(this);
            else if (statement is ReturnStatement)
                (statement as ReturnStatement).Accept(this);
        }
    }
}

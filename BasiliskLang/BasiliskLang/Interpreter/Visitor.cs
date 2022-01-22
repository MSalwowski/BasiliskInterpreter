using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter
{
    //public static class Test
    //{
    //    public static void Dupa() {
    //        Node node = new Definition();
            
    //        IVisitor visitor = new Visitor();
    //        node.Accept(visitor);
    //        //visitor.Visit(node);
    //    }
    //}
    public class Visitor : IVisitor
    {
        Dictionary<(string, int), Definition> definedFunctions;
        //Dictionary<string, ValueExpression> globalScope; - ja tego nie posiadam
        Stack<FunctionCallContext> functionCallContexts;

        public void CreateNewScope() => functionCallContexts.Peek().CreateNewScope();
        public void DeleteScope() => functionCallContexts.Peek().DeleteScope();
        public ValueExpression GetVariableValue(string name)
        {
            var value = functionCallContexts.Peek().GetVariableValue(name);
            if(value == null) { /* variable not defined */}
            return value;
        }
        public void Visit(ProgramRoot programRoot)
        {
            definedFunctions = programRoot.functionsDefinitions;
            CreateNewScope();
            foreach (var statement in programRoot.Statements)
                Visit(statement);
            DeleteScope();
        }

        public void Visit(AdditiveExpression additiveExpression)
        {
            throw new NotImplementedException();
        }

        public void Visit(Assignable assignable)
        {
            throw new NotImplementedException();
        }

        public void Visit(BlockStatement blockStatement)
        {
            CreateNewScope();
            foreach (var statement in blockStatement.Statements)
                statement.Accept(this);
            DeleteScope();
        }

        public void Visit(Definition definition)
        {
            definition.BlockStatement.Accept(this);
        }

        public void Visit(Identifier identifier)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignStatement assignStatement)
        {
            throw new NotImplementedException();
        }

        public void Visit(IfStatement ifStatement)
        {
            ifStatement.Condition.Accept(this);
            bool lastResult = true; // -------------------------------- co to???
            if (lastResult)
                ifStatement.TrueBlockStatement.Accept(this);
            else
                ifStatement.FalseBlockStatement?.Accept(this);
        }

        public void Visit(WhileStatement whileStatement)
        {
            whileStatement.Condition.Accept(this);
            bool lastResult = true;//---------------------------------citio
            while (lastResult)
            {
                whileStatement.BlockStatement.Accept(this);
                whileStatement.Condition.Accept(this);
            }
                

        }

        public void Visit(ReturnStatement returnStatement)
        {
            throw new NotImplementedException();
        }

        public void Visit(FunctionCallStatement functionCallStatement)
        {
            definedFunctions.TryGetValue((functionCallStatement.FunctionName.Identifier.Name, functionCallStatement.FunctionArguments.Count()), out Definition functionDefinition);
            if(functionDefinition == null) { /* what happens if not? */}
            functionCallContexts.Push(new FunctionCallContext());
            functionDefinition.Accept(this);
        }

        public void Visit(LogicExpression logicExpression)
        {
            throw new NotImplementedException();
        }

        public void Visit(RelationExpression relationExpression)
        {
            throw new NotImplementedException();
        }

        public void Visit(MultiplicativeExpression multiplicativeExpression)
        {
            throw new NotImplementedException();
        }

        public void Visit(UnaryExpression unaryExpression)
        {
            throw new NotImplementedException();
        }

        public void Visit(GroupedExpression groupedExpression)
        {
            throw new NotImplementedException();
        }

        public void Visit(FunctionCallExpression functionCallExpression)
        {
            throw new NotImplementedException();
        }

        public void Visit(BoolValue boolValue)
        {
            throw new NotImplementedException();
        }

        public void Visit(IntValue intValue)
        {
            throw new NotImplementedException();
        }

        public void Visit(DoubleValue doubleValue)
        {
            throw new NotImplementedException();
        }

        public void Visit(StringValue stringValue)
        {
            throw new NotImplementedException();
        }

        public void Visit(Statement statement)
        {
            throw new NotImplementedException();
        }
    }
}

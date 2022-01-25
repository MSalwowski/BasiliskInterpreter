using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter
{
    public interface IVisitor
    {
        void Visit(ProgramRoot programRoot);
        
        void Visit(AdditiveExpression additiveExpression);
        void Visit(Assignable assignable);
        void Visit(BlockStatement blockStatement);
        void Visit(FunctionDefinition definition);
        void Visit(Statement statement);

        void Visit(AssignStatement assignStatement);
        void Visit(IfStatement ifStatement);
        void Visit(WhileStatement whileStatement);
        void Visit(ReturnStatement returnStatement);
        void Visit(FunctionCallStatement functionCallStatement);


        void Visit(LogicExpression logicExpression);
        void Visit(RelationExpression relationExpression);
        void Visit(MultiplicativeExpression multiplicativeExpression);
        void Visit(UnaryExpression unaryExpression);
        

        void Visit(GroupedExpression groupedExpression);
        void Visit(FunctionCallExpression functionCallExpression);
        void Visit(BoolValueExpression boolValue);
        void Visit(IntValueExpression intValue);
        void Visit(FloatValueExpression floatValue);
        void Visit(StringValueExpression stringValue);
    }
}

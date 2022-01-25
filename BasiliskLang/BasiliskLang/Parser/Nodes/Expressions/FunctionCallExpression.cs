using BasiliskLang.Interpreter;
using System.Collections.Generic;
using System.Linq;

namespace BasiliskLang
{
    // the class is identical as FunctionCallStatement - reason behind implementing that in two different classes
    // is because Class can't inherit from two abstract classes
    public class FunctionCallExpression : Expression
    {
        private string OuterFunctionName { get; }
        private string InnerFunctionName { get; }
        public string FunctionName => InnerFunctionName == null ? OuterFunctionName : OuterFunctionName + "." + InnerFunctionName;
        public List<Expression> FunctionArguments => children.Cast<Expression>().ToList();
        public FunctionCallExpression(Assignable assignable, IEnumerable<Expression> functionArguments) : base(NodeType.Call)
        {
            OuterFunctionName = assignable.Identifier;
            InnerFunctionName = assignable.Property;
            if (functionArguments != null)
                children.AddRange(functionArguments);
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

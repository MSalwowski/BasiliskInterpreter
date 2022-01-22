using BasiliskLang.Interpreter;
using System.Collections.Generic;
using System.Linq;

namespace BasiliskLang
{
    // the class is identical as FunctionCallStatement - reason behind implementing that in two different classes
    // is because Class can't inherit from two abstract classes
    public class FunctionCallExpression : Expression
    {
        public Assignable FunctionName => children[0] as Assignable;
        public List<Expression> FunctionArguments => children.Count > 1 ? children.GetRange(1, children.Count - 1).Cast<Expression>().ToList() : null;
        public FunctionCallExpression(Assignable _functionName, IEnumerable<Expression> _functionArguments) : base(NodeType.Call)
        {
            children.Add(_functionName);
            if (_functionArguments != null)
                children.AddRange(_functionArguments);
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

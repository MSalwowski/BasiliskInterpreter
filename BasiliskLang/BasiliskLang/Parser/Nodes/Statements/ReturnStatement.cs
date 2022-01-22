using BasiliskLang.Interpreter;

namespace BasiliskLang
{
    public class ReturnStatement : Statement
    {
        public Expression Expression => children.Count != 0 ? children[0] as Expression: null;
        public ReturnStatement(Expression _expression = null) : base(NodeType.ReturnStatement)
        {
            if(_expression != null)
                children.Add(_expression);
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

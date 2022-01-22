using BasiliskLang.Interpreter;

namespace BasiliskLang
{
    public class AssignStatement : Statement
    {
        public Assignable Left => children[0] as Assignable;
        public Expression Right => children[1] as Expression;
        public AssignStatement(Assignable left, Expression right) : base(NodeType.AssignStatement)
        {
            children.Add(left);
            children.Add(right);
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

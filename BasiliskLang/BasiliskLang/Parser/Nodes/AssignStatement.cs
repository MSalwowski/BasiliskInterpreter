namespace BasiliskLang
{
    public class AssignStatement : Statement
    {
        public Assignable left;
        public Expression right;
        public AssignStatement() : base(NodeType.AssignStatement)
        {

        }
        public void SetLeft(Assignable _assignable)
        {
            left = _assignable;
            children.Add(_assignable);
        }
        public void SetRight(Expression _expression)
        {
            right = _expression;
            children.Add(_expression);
        }
    }
}

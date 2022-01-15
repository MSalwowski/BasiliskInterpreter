namespace BasiliskLang
{
    public class ReturnStatement : Statement
    {
        public Expression expression;
        public ReturnStatement() : base(NodeType.ReturnStatement)
        {

        }
        public void SetExpression(Expression _expression)
        {
            expression = _expression;
            children.Add(_expression);
        } 
    }
}

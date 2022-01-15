namespace BasiliskLang
{
    public class UnaryExpression : Expression
    {
        public bool isNegated;
        public SimpleExpression expression;
        public UnaryExpression() : base(NodeType.UnaryExpression) { }
        public void SetExpression(SimpleExpression _expression)
        {
            expression = _expression;
            children.Add(_expression);
        }
        public void SetNegation() { isNegated = true; }
    }
}

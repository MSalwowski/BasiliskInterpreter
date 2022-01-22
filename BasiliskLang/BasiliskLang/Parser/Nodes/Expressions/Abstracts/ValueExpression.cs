namespace BasiliskLang
{
    public abstract class ValueExpression : Expression
    {
        public ValueExpression(NodeType _type) : base(_type) { }
    }
    public abstract class ValueExpression<T> : ValueExpression
    {
        public T value;
        public ValueExpression(NodeType _type, T _value) : base(_type)
        {
            value = _value;
        }
    }
}

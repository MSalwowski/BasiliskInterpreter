namespace BasiliskLang
{
    public abstract class Expression : Node
    {
        public Expression(NodeType _type) : base(_type) { }
    }
    public enum OperatorType
    {
        Or, And,
        Greater, GreaterEqual, Equal, NotEqual, LessEqual, Less,
        Add, Subtract,
        Multiply, Divide,
        Negate
    }
}

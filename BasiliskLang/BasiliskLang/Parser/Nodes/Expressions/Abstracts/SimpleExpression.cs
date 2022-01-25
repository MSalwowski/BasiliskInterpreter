using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public abstract class SimpleExpression : Expression
    {
        public Expression InnerExpression => children[0] as Expression;
        public SimpleExpression(NodeType _type, Expression _expression) : base(_type)
        {
            children.Add(_expression);
        }
    }
}

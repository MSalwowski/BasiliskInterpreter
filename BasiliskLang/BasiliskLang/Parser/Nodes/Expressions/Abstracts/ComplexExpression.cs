using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public abstract class ComplexExpression : Expression
    {
        public Expression Left => children[0] as Expression;
        public Expression Right => children[1] as Expression;
        public ComplexExpression(NodeType _type, Expression _left, Expression _right, TokenType _tokenType) : base(_type) 
        {
            children.Add(_left);
            SetOperation(_tokenType);
            children.Add(_right);
        }
        public abstract void SetOperation(TokenType _tokenType);
    }
}

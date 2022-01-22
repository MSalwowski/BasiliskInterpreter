using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class AdditiveExpression : ComplexExpression
    {
        public enum Operation { Addition, Subtraction}
        public Operation operation;
        public AdditiveExpression(Expression _left, Expression _right, TokenType _operation) : base(NodeType.AdditiveExpression, _left, _right, _operation) { }
        public override void SetOperation(TokenType type)
        {
            if (type == TokenType.Plus)
                operation = Operation.Addition;
            if (type == TokenType.Minus)
                operation = Operation.Subtraction;
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

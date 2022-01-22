using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class MultiplicativeExpression : ComplexExpression
    {
        public enum Operation { Multiplication, Division }
        public Operation operation;
        public MultiplicativeExpression(Expression _left, Expression _right, TokenType _operation) : base(NodeType.MultiplicativeExpression, _left, _right, _operation) { }
        public override void SetOperation(TokenType type)
        {
            if (type == TokenType.Multiply)
                operation = Operation.Multiplication;
            if (type == TokenType.Divide)
                operation = Operation.Division;
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

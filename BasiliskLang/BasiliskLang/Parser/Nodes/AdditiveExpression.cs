using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class AdditiveExpression : Expression
    {
        public enum Operation { Addition, Subtraction}
        public MultiplicativeExpression left;
        public MultiplicativeExpression right;
        public Operation operation;
        public AdditiveExpression() : base(NodeType.AdditiveExpression) { }
        public void SetLeft(MultiplicativeExpression expression)
        {
            left = expression;
            children.Add(expression);
        }
        public void SetRight(MultiplicativeExpression expression)
        {
            right = expression;
            children.Add(expression);
        }
        public void SetOperation(TokenType type)
        {
            if (type == TokenType.Plus)
                operation = Operation.Addition;
            if (type == TokenType.Minus)
                operation = Operation.Subtraction;
        }
    }
}

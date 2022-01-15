using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class MultiplicativeExpression : Expression
    {
        public enum Operation { Multiplication, Division }
        public UnaryExpression left;
        public UnaryExpression right;
        public Operation operation;
        public MultiplicativeExpression() : base(NodeType.MultiplicativeExpression) { }
        public void SetLeft(UnaryExpression expression)
        {
            left = expression;
            children.Add(expression);
        }
        public void SetRight(UnaryExpression expression)
        {
            right = expression;
            children.Add(expression);
        }
        public void SetOperation(TokenType type)
        {
            if (type == TokenType.Multiply)
                operation = Operation.Multiplication;
            if (type == TokenType.Divide)
                operation = Operation.Division;
        }
    }
}

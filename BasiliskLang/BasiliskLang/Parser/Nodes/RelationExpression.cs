using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class RelationExpression : Expression
    {
        public enum Operation { Equal, NotEqual, Greater, GreaterEqual, Less, LessEqual }
        public AdditiveExpression left;
        public AdditiveExpression right;
        public Operation operation;
        public RelationExpression() : base(NodeType.RelationExpression) { }
        public void SetLeft(AdditiveExpression expression)
        {
            left = expression;
            children.Add(expression);
        }
        public void SetRight(AdditiveExpression expression)
        {
            right = expression;
            children.Add(expression);
        }
        public void SetOperation(TokenType type)
        {
            if (type == TokenType.Equal)
                operation = Operation.Equal;
            if (type == TokenType.NotEqual)
                operation = Operation.NotEqual;
            if (type == TokenType.Greater)
                operation = Operation.Greater;
            if (type == TokenType.GreaterEqual)
                operation = Operation.GreaterEqual;
            if (type == TokenType.Less)
                operation = Operation.Less;
            if (type == TokenType.LessEqual)
                operation = Operation.LessEqual;
        }
    }
}

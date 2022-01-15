using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class LogicExpression : Expression
    {
        public enum Operation { And, Or }
        public RelationExpression left;
        public RelationExpression right;
        public Operation operation;
        public LogicExpression() : base(NodeType.LogicExpression) { }
        public void SetLeft(RelationExpression expression)
        {
            left = expression;
            children.Add(expression);
        }
        public void SetRight(RelationExpression expression)
        {
            right = expression;
            children.Add(expression);
        }
        public void SetOperation(TokenType type)
        {
            if (type == TokenType.And)
                operation = Operation.And;
            if(type == TokenType.Or)
                operation = Operation.Or;
        }
    }
}

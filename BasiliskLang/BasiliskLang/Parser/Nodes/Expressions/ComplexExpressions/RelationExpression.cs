using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class RelationExpression : ComplexExpression
    {
        public enum Operation { Equal, NotEqual, Greater, GreaterEqual, Less, LessEqual }
        public Operation operation;
        public RelationExpression(Expression _left, Expression _right, TokenType _operation) : base(NodeType.RelationExpression, _left, _right, _operation) { } 
        public override void SetOperation(TokenType type)
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
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

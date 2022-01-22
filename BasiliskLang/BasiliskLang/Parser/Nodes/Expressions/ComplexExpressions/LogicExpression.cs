using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class LogicExpression : ComplexExpression
    {
        public enum Operation { And, Or }

        public Operation operation;
        public LogicExpression(Expression _left, Expression _right, TokenType _operation) : base(NodeType.LogicExpression, _left, _right, _operation) { }
        public override void SetOperation(TokenType type)
        {
            if (type == TokenType.And)
                operation = Operation.And;
            if(type == TokenType.Or)
                operation = Operation.Or;
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class LogicExpression : ComplexExpression
    {

        public LogicExpression(Expression _left, Expression _right, Token token) : base(NodeType.LogicExpression, _left, _right, token) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

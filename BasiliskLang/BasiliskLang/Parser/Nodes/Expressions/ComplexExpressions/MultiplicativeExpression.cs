using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class MultiplicativeExpression : ComplexExpression
    {
        public MultiplicativeExpression(Expression _left, Expression _right, Token token) : base(NodeType.MultiplicativeExpression, _left, _right, token) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

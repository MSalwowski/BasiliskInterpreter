using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class AdditiveExpression : ComplexExpression
    {
        public AdditiveExpression(Expression _left, Expression _right, Token token) : base(NodeType.AdditiveExpression, _left, _right, token) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

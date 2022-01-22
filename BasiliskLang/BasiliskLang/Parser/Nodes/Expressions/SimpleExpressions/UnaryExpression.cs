using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class UnaryExpression : SimpleExpression
    {
        public UnaryExpression(Expression _expression) : base(NodeType.UnaryExpression, _expression) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

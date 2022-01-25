using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class RelationExpression : ComplexExpression
    {
        public RelationExpression(Expression _left, Expression _right, Token token) : base(NodeType.RelationExpression, _left, _right, token) { } 
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

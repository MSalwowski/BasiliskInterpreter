using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class StringValueExpression : ValueExpression<string>
    {
        public StringValueExpression(Token _token) : base(NodeType.String, _token.value) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

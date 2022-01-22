using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class StringValue : ValueExpression<string>
    {
        public StringValue(Token _token) : base(NodeType.String, _token.value) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

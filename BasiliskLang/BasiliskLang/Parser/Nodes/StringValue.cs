using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class StringValue : SimpleExpression
    {
        public string value;
        public StringValue(Token _token) : base(NodeType.String)
        {
            value = _token.value;
        }
    }
}

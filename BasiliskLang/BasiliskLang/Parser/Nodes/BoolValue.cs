using System;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class BoolValue : SimpleExpression
    {
        public bool value;
        public BoolValue(Token _token) : base(NodeType.Bool)
        {
            value = _token.value == "true" ? true : false;
        }
    }
}

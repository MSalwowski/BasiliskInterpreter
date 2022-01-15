using System;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class DoubleValue : SimpleExpression
    {
        public double value;
        public DoubleValue(Token _token) : base(NodeType.Double)
        {
            value = Convert.ToDouble(_token.value);
        }
    }
}

using System;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class DoubleValue : SimpleExpression
    {
        public double value;
        public DoubleValue(Token _token) : base(NodeType.Double)
        {
            value = Double.Parse(_token.value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}

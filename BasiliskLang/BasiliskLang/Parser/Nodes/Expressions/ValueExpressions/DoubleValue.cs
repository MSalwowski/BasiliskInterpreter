using System;
using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class DoubleValue : ValueExpression<double>
    {
        public DoubleValue(Token _token) : base(NodeType.Double, Double.Parse(_token.value, System.Globalization.CultureInfo.InvariantCulture)) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

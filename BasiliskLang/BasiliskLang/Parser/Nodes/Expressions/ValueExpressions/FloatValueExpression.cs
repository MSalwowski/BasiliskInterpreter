using System;
using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class FloatValueExpression : ValueExpression<float>
    {
        public FloatValueExpression(Token _token) : base(NodeType.Float, float.Parse(_token.value, System.Globalization.CultureInfo.InvariantCulture)) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

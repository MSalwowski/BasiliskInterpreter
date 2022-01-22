using System;
using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class BoolValue : ValueExpression<bool>
    {
        public BoolValue(Token _token) : base(NodeType.Bool, _token.value == "true" ? true : false) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

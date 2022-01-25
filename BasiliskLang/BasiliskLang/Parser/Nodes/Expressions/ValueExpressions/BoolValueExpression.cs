using System;
using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class BoolValueExpression : ValueExpression<bool>
    {
        public BoolValueExpression(Token _token) : base(NodeType.Bool, _token.value == "true" ? true : false) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

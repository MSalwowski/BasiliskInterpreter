using System;
using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class IntValueExpression : ValueExpression<int>
    {
        public IntValueExpression(Token _token) : base(NodeType.Int, Convert.ToInt32(_token.value)) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

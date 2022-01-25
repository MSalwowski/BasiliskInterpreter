using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;
using System.Collections.Generic;

namespace BasiliskLang
{
    public class AdditiveExpression : ComplexExpression
    {
        public AdditiveExpression(List<Expression> components, List<Token> tokens) : base(NodeType.AdditiveExpression, components, tokens) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

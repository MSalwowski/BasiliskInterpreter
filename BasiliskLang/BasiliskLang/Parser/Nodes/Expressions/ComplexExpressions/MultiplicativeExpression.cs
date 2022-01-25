using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;
using System.Collections.Generic;

namespace BasiliskLang
{
    public class MultiplicativeExpression : ComplexExpression
    {
        public MultiplicativeExpression(List<Expression> components, List<Token> tokens) : base(NodeType.MultiplicativeExpression, components, tokens) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

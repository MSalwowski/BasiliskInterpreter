using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;
using System.Collections.Generic;

namespace BasiliskLang
{
    public class RelationExpression : ComplexExpression
    {
        public RelationExpression(List<Expression> components, List<Token> tokens) : base(NodeType.RelationExpression, components, tokens) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

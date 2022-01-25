using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;
using System.Collections.Generic;

namespace BasiliskLang
{
    public class LogicExpression : ComplexExpression
    {

        public LogicExpression(List<Expression> components, List<Token> tokens) : base(NodeType.LogicExpression, components, tokens) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

using System;
using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class GroupedExpression : SimpleExpression
    {
        public GroupedExpression(Expression _expression) : base(NodeType.GroupedExpression, _expression) { }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

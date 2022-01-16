using System;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class GroupedExpression : SimpleExpression
    {
        Expression expression;
        public GroupedExpression() : base(NodeType.GroupedExpression) { }
        public void SetExpression(Expression _expression)
        {
            expression = _expression;
            children.Add(expression);
        }
    }
}

using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class UnaryExpression : SimpleExpression
    {
        public OperatorType Operation;
        public UnaryExpression(Expression _expression) : base(NodeType.UnaryExpression, _expression) 
        {
            Operation = OperatorType.Negate;
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

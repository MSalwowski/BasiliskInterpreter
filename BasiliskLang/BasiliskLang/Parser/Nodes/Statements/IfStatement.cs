using BasiliskLang.Interpreter;

namespace BasiliskLang
{
    public class IfStatement : Statement
    {
        public Expression Condition => children[0] as Expression;
        public BlockStatement TrueBlockStatement => children[1] as BlockStatement;
        public BlockStatement FalseBlockStatement => children.Count == 3 ? children[2] as BlockStatement : null;
        public IfStatement(Expression _condition, BlockStatement _trueBlockStatement, BlockStatement _falseBlockStatement = null) : base(NodeType.IfStatement)
        {
            children.Add(_condition);
            children.Add(_trueBlockStatement);
            if(_falseBlockStatement != null)
                children.Add(_falseBlockStatement);
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

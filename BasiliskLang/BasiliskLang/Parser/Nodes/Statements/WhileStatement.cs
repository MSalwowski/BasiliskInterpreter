using BasiliskLang.Interpreter;

namespace BasiliskLang
{
    public class WhileStatement : Statement
    {
        public Expression Condition => children[0] as Expression;
        public BlockStatement BlockStatement => children[1] as BlockStatement;
        public WhileStatement(Expression _condition, BlockStatement _blockStatement) : base(NodeType.WhileStatement) 
        {
            children.Add(_condition);
            children.Add(_blockStatement);
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

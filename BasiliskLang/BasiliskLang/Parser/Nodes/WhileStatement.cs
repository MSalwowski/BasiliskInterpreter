namespace BasiliskLang
{
    public class WhileStatement : Statement
    {
        public Expression condition;
        public BlockStatement blockStatement;
        public WhileStatement() : base(NodeType.WhileStatement) { }
        public void SetCondition(Expression _condition)
        {
            condition = _condition;
            children.Add(_condition);
        }
        public void SetBlockStatement(BlockStatement _statement)
        {
            blockStatement = _statement;
            children.Add(_statement);
        }
    }
}

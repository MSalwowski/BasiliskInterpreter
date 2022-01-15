namespace BasiliskLang
{
    public class IfStatement : Statement
    {
        public Expression condition;
        public BlockStatement trueBlockStatement;
        public BlockStatement falseBlockStatement;
        public IfStatement() : base(NodeType.IfStatement)
        {

        }
        public void SetCondition(Expression _condition)
        {
            condition = _condition;
            children.Add(_condition);
        }
        public void SetTrueBlockStatement(BlockStatement _statement)
        {
            trueBlockStatement = _statement;
            children.Add(_statement);
        }
        public void SetFalseBlockStatement(BlockStatement _statement)
        {
            falseBlockStatement = _statement;
            children.Add(_statement);
        }
    }
}

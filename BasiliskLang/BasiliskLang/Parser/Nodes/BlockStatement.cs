using System.Collections.Generic;

namespace BasiliskLang
{
    public class BlockStatement : Statement
    {
        List<Statement> statements;
        // DISCLAIMER: 
        public BlockStatement() : base(NodeType.BlockStatement) { }

        public void SetStatements(List<Statement> _statements)
        {
            statements = _statements;
            foreach (var statement in _statements)
                children.Add(statement);
        }
    }
}

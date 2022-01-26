using BasiliskLang.Interpreter;
using System.Collections.Generic;
using System.Linq;

namespace BasiliskLang
{
    public class BlockStatement : Statement
    {
        public List<Statement> Statements => children.Cast<Statement>().ToList();
        public BlockStatement(List<Statement> _statements) : base(NodeType.BlockStatement)
        {
            if(_statements != null)
                children.AddRange(_statements);
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

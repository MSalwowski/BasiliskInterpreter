using BasiliskLang.Interpreter;
using System.Collections.Generic;
using System.Linq;

namespace BasiliskLang
{
    public class Definition : Node
    {

        public Identifier Identifier => children[0] as Identifier;
        public List<Assignable> Parameters => children.Count > 2 ? children.GetRange(1, children.Count - 2).Cast<Assignable>().ToList() : null;
        public BlockStatement BlockStatement => children[children.Count - 1] as BlockStatement;
        public Definition(Identifier identifier, IEnumerable<Assignable> parameters, BlockStatement blockStatement) : base(NodeType.Definition) 
        {
            children.Add(identifier);
            if (parameters != null)
                children.AddRange(parameters);
            children.Add(blockStatement);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

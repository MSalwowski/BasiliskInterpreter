using BasiliskLang.Interpreter;
using System.Collections.Generic;

namespace BasiliskLang
{
    public class Assignable : Expression
    {
        public Identifier Identifier => children[0] as Identifier;
        public Identifier Property => children.Count > 1 ? children[1] as Identifier : null;
        public Assignable(Identifier _identifier, Identifier _property = null) : base(NodeType.Assignable) 
        { 
            children.Add(_identifier);
            if(_property != null)
                children.Add(_property);
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

using System.Collections.Generic;

namespace BasiliskLang
{
    public class Assignable : SimpleExpression
    {
        public List<Identifier> identifiers;
        // DISCLAIMER: one identifier or more
        public Assignable() : base(NodeType.Assignable) { }
        public Assignable(Identifier _identifier) : base(NodeType.Assignable) 
        { 
            identifiers = new List<Identifier>(); 
            identifiers.Add(_identifier);
            children.Add(_identifier);
        }

        public void AddIdentifier(Identifier _identifier)
        {
            identifiers.Add(_identifier);
            children.Add(_identifier);
        }
    }
}

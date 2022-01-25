using BasiliskLang.Interpreter;
using System.Collections.Generic;

namespace BasiliskLang
{
    public class Assignable : Expression
    {
        public string Identifier { get; }
        public string Property { get; }
        public string FullName => Identifier + "." + Property;
        public Assignable(string identifier, string property = null) : base(NodeType.Assignable) 
        {
            Identifier = identifier;
            Property = property;
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

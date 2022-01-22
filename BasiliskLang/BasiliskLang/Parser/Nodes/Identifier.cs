using BasiliskLang.Interpreter;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class Identifier : Node
    {
        private string name;
        public string Name => name;
        public Identifier(string _name) : base(NodeType.Identifier)
        {
            name = _name;
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

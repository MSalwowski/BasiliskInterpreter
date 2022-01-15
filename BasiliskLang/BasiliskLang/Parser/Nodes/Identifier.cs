using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class Identifier : Node{
        public string name;
        public Identifier(Token token) : base(NodeType.Identifier)
        {
            name = token.value;
        }
    }
}

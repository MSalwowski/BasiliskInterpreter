using System.Collections.Generic;

namespace BasiliskLang
{
    public abstract class Node {
        public NodeType type;
        public List<Node> children;

        public Node(NodeType _type)
        {
            type = _type;
            children = new List<Node>();
        }
    }
}

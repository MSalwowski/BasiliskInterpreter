using System;
using System.Collections.Generic;

namespace BasiliskLang
{
    public abstract class Node : IEquatable<Node> {
        public NodeType type;
        public List<Node> children;

        public Node(NodeType _type)
        {
            type = _type;
            children = new List<Node>();
        }

        public bool Equals(Node other)
        {
            if (this.type != other.type)
                return false;
            if (this.children.Count != other.children.Count)
                return false;
            for (int i = 0; i < children.Count; i++)
            {
                if (this.children[i].type != other.children[i].type)
                    return false;
                if (this.children[i].Equals(other.children[i])) { }
                else
                    return false;
            }
            return true;
        }
    }
}

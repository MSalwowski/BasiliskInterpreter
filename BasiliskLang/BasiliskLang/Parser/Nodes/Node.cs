using BasiliskLang.Interpreter;
using System;
using System.Collections.Generic;

namespace BasiliskLang
{
    public abstract class Node : IEquatable<Node>, IVisitable {
        public NodeType type;
        public List<Node> children;

        public Node(NodeType _type)
        {
            type = _type;
            children = new List<Node>();
        }

        public abstract void Accept(IVisitor visitor);

        public bool Equals(Node other)
        {
            if (this.type != other.type)
                return false;
            if (this.children.Count != other.children.Count)
                return false;
            return true;
        }
    }
}

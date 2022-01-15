using System.Collections.Generic;

namespace BasiliskLang
{
    public class Definition : Node
    {
        public Identifier identifier;
        public List<Assignable> parameters;
        public BlockStatement blockStatement;

        public Definition() : base(NodeType.Definition) { parameters = new List<Assignable>(); }

        public void SetIdentifier(Identifier _identifier)
        {
            identifier = _identifier;
            children.Add(_identifier);
        }
        public void SetParameters(List<Assignable> _parameters)
        {
            parameters = _parameters;
            if(parameters!=null)
                foreach (var parameter in _parameters)
                    children.Add(parameter);
        }
        public void SetBlock(BlockStatement _blockStatement)
        {
            blockStatement = _blockStatement;
            children.Add(_blockStatement);
        }

    }
}

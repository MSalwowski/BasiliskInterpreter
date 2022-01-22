using BasiliskLang.Interpreter;
using System.Collections.Generic;
using System.Linq;

namespace BasiliskLang
{
    public class ProgramRoot : Node
    {
        // tuple in key of dictionary holds information about function name and number of parameters
        // this approach enables us to define multiple functions with the same name, but different numbers of parameters
        public Dictionary<(string,int), Definition> functionsDefinitions;
        public List<Statement> Statements => children.Cast<Statement>().ToList();
        public ProgramRoot(Dictionary<(string, int), Definition> _functionsDefinitions, List<Statement> _statements) : base(NodeType.ProgramRoot) 
        {
            functionsDefinitions = _functionsDefinitions;
            if(_statements != null)
                children.AddRange(_statements);
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

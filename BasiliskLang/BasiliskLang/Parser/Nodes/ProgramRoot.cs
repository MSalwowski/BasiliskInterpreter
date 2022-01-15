using System.Collections.Generic;

namespace BasiliskLang
{
    public class ProgramRoot : Node
    {
        public List<Definition> definitions;
        public List<Statement> statements;
        public ProgramRoot() : base(NodeType.ProgramRoot) { }
        public void SetDefinitions(List<Definition> _definitions)
        {
            definitions = _definitions;
            foreach (var definition in _definitions)
                children.Add(definition);
        }
        public void SetStatements(List<Statement> _statements)
        {
            statements = _statements;
            foreach (var statement in _statements)
                children.Add(statement);
        }
    }
}

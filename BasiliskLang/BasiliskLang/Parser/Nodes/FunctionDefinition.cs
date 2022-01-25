using BasiliskLang.Interpreter;
using System.Collections.Generic;
using System.Linq;

namespace BasiliskLang
{
    public class FunctionDefinition : Node
    {
        public string FunctionName { get; }
        public List<string> Parameters { get; }
        public BlockStatement BlockStatement => children[0] as BlockStatement;
        public FunctionDefinition(string functionName, List<string> parameters, BlockStatement blockStatement) : base(NodeType.Definition) 
        {
            FunctionName = functionName;
            if (parameters != null)
                Parameters = parameters.ToList();
            children.Add(blockStatement);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

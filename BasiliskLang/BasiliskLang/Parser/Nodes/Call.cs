using System.Collections.Generic;

namespace BasiliskLang
{
    public class Call : SimpleExpression
    {
        Assignable assignable;
        List<Expression> arguments;
        public Call() : base(NodeType.Call) 
        {
            arguments = new List<Expression>();
        }
        public void SetAssignable(Assignable _assignable)
        {
            assignable = _assignable;
            children.Add(_assignable);
        }
        public void SetArguments(List<Expression> _arguments)
        {
            arguments = _arguments;
            foreach (var argument in _arguments)
                children.Add(argument);
        }
    }
}

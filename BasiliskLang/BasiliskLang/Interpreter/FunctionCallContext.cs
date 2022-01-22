using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter
{
    public class FunctionCallContext
    {
        public List<Dictionary<string, ValueExpression>> scopes;
        public Queue<int> q;

        public FunctionCallContext()
        {
            scopes = new List<Dictionary<string, ValueExpression>>();
        }
        public ValueExpression GetVariableValue(string name)
        {
            // searching from the youngest to the oldest scope
            for(int i = scopes.Count - 1; i >= 0; i--)
            {
                scopes[i].TryGetValue(name, out var variableValue);
                if (variableValue != null)
                    return variableValue;
            }
            return null;
        }
        public void CreateNewScope() => scopes.Add(new Dictionary<string, ValueExpression>());
        public void DeleteScope() => scopes.RemoveAt(scopes.Count() - 1);

    }
}

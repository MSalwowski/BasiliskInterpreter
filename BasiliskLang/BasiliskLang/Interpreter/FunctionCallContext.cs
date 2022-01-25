using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter
{
    public class FunctionCallContext
    {
        public List<Dictionary<string, Value>> scopes;
        public Dictionary<string, Value> LatestScope => scopes[0];

        public FunctionCallContext()
        {
            scopes = new List<Dictionary<string, Value>>();
        }

        public Value TryGetVariableValue(string identifier, string property)
        {
            for (int i = 0; i < scopes.Count; i++)
            {
                scopes[i].TryGetValue(identifier, out var value);
                if (value != null)
                {
                    if(property == null)
                        return value;
                    else
                    {
                        if (!value.GetType().GetProperties().Select(p => p.Name).ToList().Exists(n => n == property))
                            return null;
                        var propertyValue = value.GetType().GetProperty(property).GetValue(value);
                        // shall we cast it to exact type?
                        return (Value)value.GetType().GetProperty(property).GetValue(value);
                    }
                }
                    
            }
            return null;
        }

        public void DeleteVariable(string identifier)
        {
            for (int i = 0; i < scopes.Count; i++)
            {
                if (scopes[i].ContainsKey(identifier))
                    scopes[i].Remove(identifier);
            }
        }

        public void AddVariableValue(string identifier, Value value)
        {
            DeleteVariable(identifier);
            LatestScope.Add(identifier, value);
        }

        public void SetVariablePropertyValue(string identifier, string property, Value newPropertyValue)
        {
            var variableValue = TryGetVariableValue(identifier, null);
            if (variableValue == null) 
                AddVariableValue(identifier, newPropertyValue);
            else
                if (!variableValue.GetType().GetProperties().Select(p => p.Name).ToList().Exists(n => n == property)) { /* variable doesnt contain such property */}
            variableValue.GetType().GetProperty(property).SetValue(variableValue, newPropertyValue);
        }
        public void CreateNewScope() => scopes.Insert(0, new Dictionary<string, Value>());
        public void DeleteScope() => scopes.RemoveAt(0);

    }
}

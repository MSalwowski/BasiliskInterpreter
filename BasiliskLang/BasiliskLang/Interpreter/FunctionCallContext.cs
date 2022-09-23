using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter
{
    public class FunctionCallContext
    {
        private Dictionary<string, Value> _scope;

        public bool IsReturning { get; set; }

        public FunctionCallContext()
        {
            _scope = new Dictionary<string, Value>();
        }

        public Value TryGetVariableValue(string identifier, string property)
        {
            _scope.TryGetValue(identifier, out var value);
            if (value != null)
            {
                if (property == null)
                    return value;
                else
                {
                    if (!value.GetType().GetProperties().Select(p => p.Name).ToList().Exists(n => n == property))
                        return null;
                    var propertyValue = value.GetType().GetProperty(property).GetValue(value);
                    return (Value)value.GetType().GetProperty(property).GetValue(value);
                }
            }
            return null;
        }

        public void DeleteVariable(string identifier)
        {
            if (_scope.ContainsKey(identifier))
                _scope.Remove(identifier);
        }

        public void AddVariableValue(string identifier, Value value)
        {
            DeleteVariable(identifier);
            _scope.Add(identifier, value);
        }

        public void SetVariablePropertyValue(string identifier, string property, Value newPropertyValue)
        {
            var variableValue = TryGetVariableValue(identifier, null);
            if (variableValue == null)
                AddVariableValue(identifier, newPropertyValue);
            else
                if (!variableValue.GetType().GetProperties().Select(p => p.Name).ToList().Exists(n => n == property))
                throw new RuntimeException("Variable doesn't contain this property");
            var propertyType = variableValue.GetType().GetProperty(property).PropertyType;
            if (propertyType != newPropertyValue.GetType())
                throw new RuntimeException("Property assignement failed (wrong type or has no public setter)");
            variableValue.GetType().GetProperty(property).SetValue(variableValue, newPropertyValue);
        }

    }
}

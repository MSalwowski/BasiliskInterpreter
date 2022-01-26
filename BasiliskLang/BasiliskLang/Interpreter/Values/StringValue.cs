using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter.Values
{
    public class StringValue : Value
    {
        public string Value { get; }
        public ValueType Type => ValueType.String;

        public StringValue(string value) { Value = value; }

        public override string ToString() { return Value; }

        public Value Operate(OperatorType operationType, Value other)
        {
            if (operationType == OperatorType.Add)
                return new StringValue(Value + other.ToString());
            return null;
        }
    }
}

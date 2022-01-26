using BasiliskLang.Interpreter.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter
{
    public enum ValueType { Dynamic, String, DateTime, Period }
    public interface Value
    {
        public ValueType Type { get; }
        public Value Operate(OperatorType operationType, Value other);
    }
}

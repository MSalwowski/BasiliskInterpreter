using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter.Values
{
    public class DynamicValue : Value
    {
        public dynamic Value { get; }
        ValueType Value.Type => ValueType.Dynamic;
        delegate dynamic OperationHandler(dynamic x, dynamic y);
        static OperationHandler[] s_operationHandlers;
        public DynamicValue(dynamic value) { this.Value = value; }
        public override string ToString() { return Value.ToString(); }

        static DynamicValue()
        {
            int length = Enum.GetNames(typeof(OperatorType)).Length;
            s_operationHandlers = new OperationHandler[length];

            // Register the operation handlers
            s_operationHandlers[(int)OperatorType.Add] = (x, y) => x + y;
            s_operationHandlers[(int)OperatorType.And] = (x, y) => x && y;
            s_operationHandlers[(int)OperatorType.Divide] = (x, y) => (double)x / y;
            s_operationHandlers[(int)OperatorType.Equal] = (x, y) => x == y;
            s_operationHandlers[(int)OperatorType.Greater] = (x, y) => x > y;
            s_operationHandlers[(int)OperatorType.GreaterEqual] = (x, y) => x >= y;
            s_operationHandlers[(int)OperatorType.Less] = (x, y) => x < y;
            s_operationHandlers[(int)OperatorType.LessEqual] = (x, y) => x <= y;
            s_operationHandlers[(int)OperatorType.Multiply] = (x, y) => x * y;
            s_operationHandlers[(int)OperatorType.NotEqual] = (x, y) => x != y;
            s_operationHandlers[(int)OperatorType.Negate] = (x, y) => -1 * x;
            s_operationHandlers[(int)OperatorType.Or] = (x, y) => x || y;
            s_operationHandlers[(int)OperatorType.Subtract] = (x, y) => x - y;
        }

        public Value Operate(OperatorType operationType, Value other)
        {
            if (other == null)
            {
                var result = s_operationHandlers[(int)operationType](Value, null);
                return new DynamicValue(result);
            }
            else
            {
                if (other.Type == ValueType.Dynamic)
                {
                    var val2DV = (DynamicValue)other;
                    if (operationType == OperatorType.Divide && val2DV.Value == 0)
                        throw new RuntimeException("Division by 0");
                    var result = s_operationHandlers[(int)operationType](Value, val2DV.Value);
                    return new DynamicValue(result);
                }
                // place for strings
            }
            return null;
        }
    }
}

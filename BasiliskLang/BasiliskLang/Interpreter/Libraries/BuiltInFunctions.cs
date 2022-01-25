using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BasiliskLang.Interpreter
{
    public class BuiltInFunctions
    {
        internal delegate Value BuiltInFunction(List<Value> parameters);
        // string - function name
        // int? - number of arguments (null => any)
        // BuiltInFunction - function definition

        Dictionary<(string, int?), BuiltInFunction> FunctionsList { get; }
        public BuiltInFunctions()
        {
            FunctionsList = new Dictionary<(string, int?), BuiltInFunction>();
            FunctionsList.Add(("print", 1), Print);
            FunctionsList.Add(("datetime", 1), DateTime);
            FunctionsList.Add(("datetime", 2), DateTime);
            FunctionsList.Add(("datetime", 3), DateTime);
            FunctionsList.Add(("datetime", 4), DateTime);
            FunctionsList.Add(("datetime", 5), DateTime);
            FunctionsList.Add(("datetime", 6), DateTime);
            FunctionsList.Add(("period", 0), Period);
            FunctionsList.Add(("period", 1), Period);
            FunctionsList.Add(("period", 2), Period);
            FunctionsList.Add(("period", 3), Period);
            FunctionsList.Add(("period", 4), Period);
        }

        Value Print(List<Value> parameters)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var parameter in parameters)
                sb.Append(parameter.ToString());
            var stringToPrint = sb.ToString();
            Console.WriteLine(stringToPrint);
            return null;
        }

        Value DateTime(List<Value> parameters)
        {
            switch (parameters.Count)
            {
                case 1:
                    return new DateTimeValue(parameters[0]);
                case 2:
                    return new DateTimeValue(parameters[0], parameters[1]);
                case 3:
                    return new DateTimeValue(parameters[0], parameters[1], parameters[2]);
                case 4:
                    return new DateTimeValue(parameters[0], parameters[1], parameters[2], parameters[3]);
                case 5:
                    return new DateTimeValue(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
                case 6:
                    return new DateTimeValue(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
                default:
                        return null;/*throw exception - no possibility*/
            }
        }

        Value Period(List<Value> parameters)
        {
            switch (parameters.Count)
            {
                case 0:
                    return new PeriodValue();
                case 1:
                    return new PeriodValue(parameters[0]);
                case 2:
                    return new PeriodValue(parameters[0], parameters[1]);
                case 3:
                    return new PeriodValue(parameters[0], parameters[1], parameters[2]);
                case 4:
                    return new PeriodValue(parameters[0], parameters[1], parameters[2], parameters[3]);
                default:
                    return null;/*throw exception - no possibility*/
            }
        }

        public bool IsDefined(string name, int? parametersCount)
        {
            if (FunctionsList.ContainsKey((name, parametersCount)))
                return true;
            else
                return false;
        }

        public Value CallFunction(string name, List<Value> arguments)
        {
            if(!FunctionsList.TryGetValue((name, arguments.Count), out var function)) { /*call to not defined built-in function*/ }
            return function(arguments);
        }
    }

}

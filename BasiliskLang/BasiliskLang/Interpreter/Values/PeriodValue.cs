using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter.Values
{
    public class PeriodValue : Value
    {
        public Value Days => new DynamicValue(period.TotalDays);
        public Value Hours => new DynamicValue(period.TotalHours);
        public Value Minutes => new DynamicValue(period.TotalMinutes);
        public Value Seconds => new DynamicValue(period.TotalSeconds);
        public ValueType Type => ValueType.Period;
        TimeSpan period;

        public PeriodValue(Value days = null, Value hours = null, Value minutes = null, Value seconds = null)
        {
            DynamicValue d;
            if (days != null)
            {
                if (days is not DynamicValue)
                    throw new RuntimeException("Wrong parameter type");
                d = (DynamicValue)days;
            }
            else
                d = new DynamicValue(0);

            DynamicValue h;
            if (hours != null)
            {
                if (hours is not DynamicValue)
                    throw new RuntimeException("Wrong parameter type");
                h = (DynamicValue)hours;
            }
            else
                h = new DynamicValue(0);

            DynamicValue m;
            if (minutes != null)
            {
                if (minutes is not DynamicValue)
                    throw new RuntimeException("Wrong parameter type");
                m = (DynamicValue)minutes;
            }
            else
                m = new DynamicValue(0);

            DynamicValue s;
            if (seconds != null)
            {
                if (seconds is not DynamicValue)
                    throw new RuntimeException("Wrong parameter type");
                s = (DynamicValue)seconds;
            }
            else
                s = new DynamicValue(0);


            period = new TimeSpan(d.Value, h.Value, m.Value, s.Value);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(period.Days.ToString());
            sb.Append(" days ");
            sb.Append(period.Hours.ToString());
            sb.Append(" hours ");
            sb.Append(period.Minutes.ToString());
            sb.Append(" minutes ");
            sb.Append(period.Seconds.ToString());
            sb.Append(" seconds");
            return sb.ToString();
        }

        public Value Operate(OperatorType operationType, Value other)
        {
            if (other is PeriodValue)
            {
                var otherPV = (PeriodValue)other;
                if (operationType == OperatorType.Add)
                {
                    var result = period + otherPV.period;
                    return new PeriodValue(new DynamicValue(result.Days), new DynamicValue(result.Hours), new DynamicValue(result.Minutes), new DynamicValue(result.Seconds));
                }
                if (operationType == OperatorType.Subtract)
                {
                    var result = period - otherPV.period;
                    return new PeriodValue(new DynamicValue(result.Days), new DynamicValue(result.Hours), new DynamicValue(result.Minutes), new DynamicValue(result.Seconds));
                }
            }
            return null;
        }
    }
}

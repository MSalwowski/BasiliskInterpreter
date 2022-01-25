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
                    var val2_dv = (DynamicValue)other;
                    var result = s_operationHandlers[(int)operationType](Value, val2_dv.Value);
                    return new DynamicValue(result);
                }
                // place for strings
            }
            return null;
        }
    }

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

    public static class Months
    {
        // LFeb - special value for february in leap years
        internal enum MonthEnum { LFeb = 0, Jan = 1, NFeb = 2, Mar = 3, Apr = 4, May = 5, Jun = 6, Jul = 7, Aug = 8, Sep = 9, Oct = 10, Nov = 11, Dec = 12 }
        static Dictionary<int, int> DaysInMonth = new Dictionary<int, int>()
        {
            { (int)MonthEnum.LFeb, 29},
            { (int)MonthEnum.Jan, 31},
            { (int)MonthEnum.NFeb, 28},
            { (int)MonthEnum.Mar, 31},
            { (int)MonthEnum.Apr, 30},
            { (int)MonthEnum.May, 31},
            { (int)MonthEnum.Jun, 30},
            { (int)MonthEnum.Jul, 31},
            { (int)MonthEnum.Aug, 31},
            { (int)MonthEnum.Sep, 30},
            { (int)MonthEnum.Oct, 31},
            { (int)MonthEnum.Nov, 30},
            { (int)MonthEnum.Dec, 31}
        };
        public static int GetDaysForMonth(int month, int year)
        {
            int days;
            if (year % 4 == 0 && month == 2)
                DaysInMonth.TryGetValue((int)MonthEnum.LFeb, out days);
            else
                DaysInMonth.TryGetValue(month, out days);
            return days;
        }
    }
    public class DateTimeValue : Value
    {

        internal Dictionary<int, int> _monthDays = new Dictionary<int, int>();
        public int Year => date.Year;
        public int Month => date.Month;
        public int Day => date.Day;
        public int Hour => date.Hour;
        public int Minute => date.Minute;
        public int Second => date.Second;
        public ValueType Type => ValueType.DateTime;

        DateTime date;
        public DateTimeValue(Value year, Value month = null, Value day = null, Value hour = null, Value minute = null, Value second = null)
        {
            if (month == null)
                month = null;

            if(year is not DynamicValue) { /*wrong year attribute*/}
            var y = (DynamicValue)year;
            if (y.ToString().Length != 4) { /*wrong year attribute*/}

            DynamicValue mo;
            if (month != null)
            {
                if (month is not DynamicValue) { /*wrong month attribute*/}
                mo = (DynamicValue)month;
            }
            else
                mo = new DynamicValue(1);
            if (mo.Value > 12 || mo.Value < 1) { /*wrong month attribute*/}

            DynamicValue d;
            if (day != null)
            {
                if (day is not DynamicValue) { /*wrong year attribute*/}
                d = (DynamicValue)day;
            }
            else
                d = new DynamicValue(1);
            if (Months.GetDaysForMonth(mo.Value, y.Value) > d.Value || d.Value < 1) { /*wrong day attribute*/}

            DynamicValue h;
            if (hour != null)
            {
                if (hour is not DynamicValue) { /*wrong year attribute*/}
                h = (DynamicValue)hour;
            }
            else
                h = new DynamicValue(0);
            if (h.Value > 23 || h.Value < 0) { /*wrong hour attribute*/}

            DynamicValue mi;
            if (minute != null)
            {
                if (minute is not DynamicValue) { /*wrong year attribute*/}
                mi = (DynamicValue)minute;
            }
            else
                mi = new DynamicValue(0);
            if (mo.Value > 59 || mo.Value < 0) { /*wrong hour attribute*/}

            DynamicValue s;
            if (second != null)
            {
                if (second is not DynamicValue) { /*wrong year attribute*/}
                s = (DynamicValue)second;
            }
            else
                s = new DynamicValue(0);
            if (s.Value > 59 || s.Value < 0) { /*wrong second attribute*/}

            date = new DateTime(y.Value, mo.Value, d.Value, h.Value, mi.Value, s.Value);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Year.ToString());
            sb.Append("-");
            sb.Append(Month > 9 ? Month.ToString() : "0" + Month.ToString());
            sb.Append("-");
            sb.Append(Day > 9 ? Day.ToString() : "0" + Day.ToString());
            sb.Append(" ");
            sb.Append(Hour > 9 ? Hour.ToString() : "0" + Hour.ToString());
            sb.Append(":");
            sb.Append(Month > 9 ? Minute.ToString() : "0" + Minute.ToString());
            sb.Append(":");
            sb.Append(Month > 9 ? Second.ToString() : "0" + Second.ToString());
            return sb.ToString();
        }

        public Value Operate(OperatorType operationType, Value other)
        {
            if (other is DateTimeValue)
            {
                if (operationType == OperatorType.Subtract)
                {
                    var otherDTV = (DateTimeValue)other;
                    var result = date - otherDTV.date;
                    return new PeriodValue(new DynamicValue(result.Days), new DynamicValue(result.Hours), new DynamicValue(result.Minutes), new DynamicValue(result.Seconds));
                }
            }
            return null;
        }
    }
    public class PeriodValue : Value
    {
        public int Days => period.Days;
        public int Hours => period.Hours;
        public int Minutes => period.Minutes;
        public int Seconds => period.Seconds;
        public ValueType Type => ValueType.Period;
        TimeSpan period;

        public PeriodValue(Value days = null, Value hours = null, Value minutes = null, Value seconds = null)
        {
            //if (days < 0) { /* invalid days attribute*/ }
            //if (hours > 23 || hours < 0) { /*wrong hour attribute*/}
            //if (minutes > 59 || minutes < 0) { /*wrong hour attribute*/}
            //if (seconds > 59 || seconds < 0) { /*wrong second attribute*/}
            DynamicValue d;
            if (days != null)
            {
                if (days is not DynamicValue) { /* wrong type */}
                d = (DynamicValue)days;
            }
            else
                d = new DynamicValue(0);

            DynamicValue h;
            if (hours != null)
            {
                if (hours is not DynamicValue) { /*wrong types*/}
                h = (DynamicValue)hours;
            }
            else
                h = new DynamicValue(0);
            
            DynamicValue m;
            if (minutes != null)
            {
                if (minutes is not DynamicValue) { /*wrong types*/}
                m = (DynamicValue)minutes;
            }
            else
                m = new DynamicValue(0);
            
            DynamicValue s;
            if (seconds != null)
            {
                if (seconds is not DynamicValue) { /*wrong types*/}
                s = (DynamicValue)seconds;
            }
            else
                s = new DynamicValue(0);


            period = new TimeSpan(d.Value, h.Value, m.Value, s.Value);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Days.ToString());
            sb.Append(" days ");
            sb.Append(Hours.ToString());
            sb.Append(" hours ");
            sb.Append(Minutes.ToString());
            sb.Append(" minutes ");
            sb.Append(Seconds.ToString());
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

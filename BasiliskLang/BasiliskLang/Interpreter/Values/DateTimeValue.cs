using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter.Values
{
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
        public DynamicValue Year
        {
            get => new DynamicValue(date.Year);
            set
            {
                ValidateYear(value);
                ValidateDay(Day, value, Month);
                date = new DateTime(value.Value, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            }
        }
        public DynamicValue Month
        {
            get => new DynamicValue(date.Month);
            set
            {
                ValidateMonth(value);
                date = new DateTime(date.Year, value.Value, date.Day, date.Hour, date.Minute, date.Second);
            }
        }
        public DynamicValue Day
        {
            get => new DynamicValue(date.Day);
            set
            {
                ValidateDay(value, Year, Month);
                date = new DateTime(date.Year, date.Month, value.Value, date.Hour, date.Minute, date.Second);
            }
        }
        public DynamicValue Hour
        {
            get => new DynamicValue(date.Hour);
            set
            {
                ValidateHour(value);
                date = new DateTime(date.Year, date.Month, date.Day, value.Value, date.Minute, date.Second);
            }
        }
        public DynamicValue Minute
        {
            get => new DynamicValue(date.Minute);
            set
            {
                ValidateMinuteOrSecond(value);
                date = new DateTime(date.Year, date.Month, date.Day, date.Minute, value.Value, date.Second);
            }
        }
        public DynamicValue Second
        {
            get => new DynamicValue(date.Second);
            set
            {
                ValidateMinuteOrSecond(value);
                date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, value.Value);
            }
        }
        public ValueType Type => ValueType.DateTime;

        DateTime date;
        void ValidateYear(DynamicValue y)
        {
            if (y.Value <= 0)
                throw new RuntimeException("Wrong parameter value");
        }
        void ValidateMonth(DynamicValue mo)
        {
            if (mo.Value > 12 || mo.Value < 1)
                throw new RuntimeException("Wrong parameter value");
        }
        void ValidateDay(DynamicValue d, DynamicValue y, DynamicValue mo)
        {
            if (Months.GetDaysForMonth(mo.Value, y.Value) < d.Value || d.Value < 1)
                throw new RuntimeException("Wrong parameter value");
        }
        void ValidateHour(DynamicValue h)
        {
            if (h.Value > 23 || h.Value < 0)
                throw new RuntimeException("Wrong parameter value");
        }
        void ValidateMinuteOrSecond(DynamicValue t)
        {
            if (t.Value > 59 || t.Value < 0)
                throw new RuntimeException("Wrong parameter value");
        }

        public DateTimeValue(Value year, Value month = null, Value day = null, Value hour = null, Value minute = null, Value second = null)
        {
            if (year is not DynamicValue)
                throw new RuntimeException("Wrong parameter type");
            var y = (DynamicValue)year;
            ValidateYear(y);

            DynamicValue mo;
            if (month != null)
            {
                if (month is not DynamicValue)
                    throw new RuntimeException("Wrong parameter type");
                mo = (DynamicValue)month;
            }
            else
                mo = new DynamicValue(1);
            ValidateMonth(mo);

            DynamicValue d;
            if (day != null)
            {
                if (day is not DynamicValue)
                    throw new RuntimeException("Wrong parameter type");
                d = (DynamicValue)day;
            }
            else
                d = new DynamicValue(1);
            ValidateDay(d, y, mo);

            DynamicValue h;
            if (hour != null)
            {
                if (hour is not DynamicValue)
                    throw new RuntimeException("Wrong parameter type");
                h = (DynamicValue)hour;
            }
            else
                h = new DynamicValue(0);
            ValidateHour(h);

            DynamicValue mi;
            if (minute != null)
            {
                if (minute is not DynamicValue)
                    throw new RuntimeException("Wrong parameter type");
                mi = (DynamicValue)minute;
            }
            else
                mi = new DynamicValue(0);
            ValidateMinuteOrSecond(mi);

            DynamicValue s;
            if (second != null)
            {
                if (second is not DynamicValue)
                    throw new RuntimeException("Wrong parameter type");
                s = (DynamicValue)second;
            }
            else
                s = new DynamicValue(0);
            ValidateMinuteOrSecond(s);

            date = new DateTime(y.Value, mo.Value, d.Value, h.Value, mi.Value, s.Value);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(date.Year.ToString());
            sb.Append("-");
            sb.Append(date.Month > 9 ? date.Month.ToString() : "0" + date.Month.ToString());
            sb.Append("-");
            sb.Append(date.Day > 9 ? date.Day.ToString() : "0" + date.Day.ToString());
            sb.Append(" ");
            sb.Append(date.Hour > 9 ? date.Hour.ToString() : "0" + date.Hour.ToString());
            sb.Append(":");
            sb.Append(date.Minute > 9 ? date.Minute.ToString() : "0" + date.Minute.ToString());
            sb.Append(":");
            sb.Append(date.Second > 9 ? date.Second.ToString() : "0" + date.Second.ToString());
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
}

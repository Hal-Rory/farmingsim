
using System;


namespace GameTime
{

    [Serializable]
    public struct TimeStruct
    {
        [UnityEngine.SerializeField] private DayOfWeek _day;
        [UnityEngine.SerializeField] private int _year;
        [UnityEngine.SerializeField] private int _month;
        [UnityEngine.SerializeField] private int _date;
        [UnityEngine.SerializeField] private int _hour;  
        
        public static TimeStruct Default => new TimeStruct(0, 0, 0, 0);

        public string Day { get => _day.ToString(); }
        public string Year { get => (_year + 1).ToString(); }
        public string Month { get => (_month + 1).ToString(); }
        public string Date { get => (_date + 1).ToString(); }
        public string Hour
        {
            get
            {
                if (_hour == 0)
                    return "12:00 am";
                if (_hour == 12)
                    return $"{_hour}:00 pm";
                else if (_hour > 12)
                    return $"{_hour -12}:00 pm";
                return $"{_hour}:00 am";
            }
        }

        public TimeStruct(TimeStruct other)
        {
            _year = other._year;
            _month = other._month;
            _date = other._date;
            _hour = other._hour;
            _day = other._day;
            
        }
        public TimeStruct(int time, int date, int month, int year)
        {
            _year = year;
            _month = month;
            _date = date;
            _hour = time;
            _day = (DayOfWeek)(date % 7);
        }
        public void AddTime(int time)
        {
            if (time < 0) { throw new ArgumentOutOfRangeException("time"); }
            int actual = _hour + time;
            _hour = actual.Repeat(TimeUtility.DayLength+1);
            if (actual > TimeUtility.DayLength)
            {
                int dayAmt = actual / TimeUtility.DayLength;                
                AddDate(dayAmt);
            }            
        }
        public void AddDate(int date)
        {
            if (date < 0) { throw new ArgumentOutOfRangeException("date"); }
            int actual = _date + date;
            _date = actual.Repeat(TimeUtility.MonthLength +1);
            if (actual >= TimeUtility.MonthLength +1)
            {
                int monthAmt = actual / TimeUtility.MonthLength;
                AddMonth(monthAmt);
            }
            _day = (DayOfWeek)(_date % 7);
        }
        public void AddMonth(int month)
        {
            if (month < 0) { throw new ArgumentOutOfRangeException("month"); }
            int actual = _month + month;
            _month = actual.Repeat(TimeUtility.YearLength + 1);
            if (actual >= TimeUtility.YearLength +1)
            {
                int yearAmt = actual / TimeUtility.YearLength;
                AddYear(yearAmt);
            }
        }
        public void AddYear(int year)
        {
            if (year < 0) { throw new ArgumentOutOfRangeException("year"); }
            _year = Math.Clamp(_year + year, 0, TimeUtility.YearMax);
        }

        public int CompareTime(TimeStruct other)
        {
            return _hour - other._hour;
        }
        public int CompareDays(TimeStruct other)
        {
            return _date - other._date;
        }
        public int CompareMonths(TimeStruct other)
        {
            return _month - other._month;
        }
        public int CompareYears(TimeStruct other)
        {
            return _year - other._year;
        }
        public TimeStruct Compare(TimeStruct other)
        {
            return this - other;
        }

        public static TimeStruct operator +(TimeStruct a, TimeStruct b)
        {
            TimeStruct other = new TimeStruct(a);
            other.AddTime(b._hour);
            other.AddDate(b._date);
            other.AddMonth(b._month);
            other.AddYear(b._year);
            return other;
        }
        public static TimeStruct operator -(TimeStruct a, TimeStruct b)
        {
            TimeStruct other = new TimeStruct(a);
            other.AddTime(-b._hour);
            other.AddDate(-b._date);
            other.AddMonth(-b._month);
            other.AddYear(-b._year);
            return other;
        }
        public override string ToString()
        {
            return $"{(_month+1):00}/{(_date + 1):00}/{(_year + 1):0000}({_day}) {_hour:00}:00";
        }
        
    }
}
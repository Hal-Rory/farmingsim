
using System;
using UnityEngine;

namespace GameTime
{
    public enum DayOfWeek
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
    }
    [Serializable]
    public struct TimeStruct
    {
        public DayOfWeek Day;
        public int Year;
        public int Month;
        public int Date;
        public int Hour;                
        public static TimeStruct Default => new TimeStruct(0, 0, 0, 0);        
        public TimeStruct(TimeStruct other)
        {
            Year = other.Year;
            Month = other.Month;
            Date = other.Date;
            Hour = other.Hour;
            Day = other.Day;
            
        }
        public TimeStruct(int time, int date, int month, int year)
        {
            Year = year;
            Month = month;
            Date = date;
            Hour = time;
            Day = (DayOfWeek)(date % 7);
        }
        public void AddTime(int time)
        {
            if (time < 0) { throw new ArgumentOutOfRangeException("time"); }
            int actual = Hour + time;
            Hour = actual.Repeat(TimeUtility.DayLength+1);
            if (actual > TimeUtility.DayLength)
            {
                int dayAmt = actual / TimeUtility.DayLength;                
                AddDate(dayAmt);
            }            
        }
        public void AddDate(int date)
        {
            if (date < 0) { throw new ArgumentOutOfRangeException("date"); }
            int actual = Date + date;
            Date = actual.Repeat(TimeUtility.MonthLength +1);
            if (actual >= TimeUtility.MonthLength +1)
            {
                int monthAmt = actual / TimeUtility.MonthLength;
                AddMonth(monthAmt);
            }
            Day = (DayOfWeek)(Date % 7);
        }
        public void AddMonth(int month)
        {
            if (month < 0) { throw new ArgumentOutOfRangeException("month"); }
            int actual = Month + month;
            Month = actual.Repeat(TimeUtility.YearLength + 1);
            if (actual >= TimeUtility.YearLength +1)
            {
                int yearAmt = actual / TimeUtility.YearLength;
                AddYear(yearAmt);
            }
        }
        public void AddYear(int year)
        {
            if (year < 0) { throw new ArgumentOutOfRangeException("year"); }
            Year = Math.Clamp(Year + year, 0, TimeUtility.YearMax);
        }

        public int CompareTime(TimeStruct other)
        {
            return Hour - other.Hour;
        }
        public int CompareDays(TimeStruct other)
        {
            return Date - other.Date;
        }
        public int CompareMonths(TimeStruct other)
        {
            return Month - other.Month;
        }
        public int CompareYears(TimeStruct other)
        {
            return Year - other.Year;
        }
        public TimeStruct Compare(TimeStruct other)
        {
            return this - other;
        }

        public static TimeStruct operator +(TimeStruct a, TimeStruct b)
        {
            TimeStruct other = new TimeStruct(a);
            other.AddTime(b.Hour);
            other.AddDate(b.Date);
            other.AddMonth(b.Month);
            other.AddYear(b.Year);
            return other;
        }
        public static TimeStruct operator -(TimeStruct a, TimeStruct b)
        {
            TimeStruct other = new TimeStruct(a);
            other.AddTime(-b.Hour);
            other.AddDate(-b.Date);
            other.AddMonth(-b.Month);
            other.AddYear(-b.Year);
            return other;
        }
        public override string ToString()
        {
            return $"{(Month+1):00}/{(Date + 1):00}/{(Year + 1):0000}({Day}) {Hour:00}:00";
        }
        
    }
}
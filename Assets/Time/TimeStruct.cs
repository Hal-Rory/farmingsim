
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
        public static int DayLength => 23; //hours in day, minus 1 to account for 0
        public static int MonthLength => 27; //days in a month, minus 1 to account for 0
        public static int YearLength => 11; //months in a year, minus 1 to account for 0
        public static int YearMax => 9999; //max amount of years, world ends
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
            Hour = actual.Repeat(DayLength+1);
            if (actual > DayLength)
            {
                int dayAmt = actual / DayLength;                
                AddDate(dayAmt);
            }            
        }
        public void AddDate(int date)
        {
            if (date < 0) { throw new ArgumentOutOfRangeException("date"); }
            int actual = Date + date;
            Date = actual.Repeat(MonthLength+1);
            if (actual >= MonthLength+1)
            {
                int monthAmt = actual / MonthLength;
                AddMonth(monthAmt);
            }
            Day = (DayOfWeek)(Date % 7);
        }
        public void AddMonth(int month)
        {
            if (month < 0) { throw new ArgumentOutOfRangeException("month"); }
            int actual = Month + month;
            Month = actual.Repeat(YearLength + 1);
            if (actual >= YearLength+1)
            {
                int yearAmt = actual / YearLength;
                AddYear(yearAmt);
            }
        }
        public void AddYear(int year)
        {
            if (year < 0) { throw new ArgumentOutOfRangeException("year"); }
            Year = Math.Clamp(Year + year, 0, YearMax);
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
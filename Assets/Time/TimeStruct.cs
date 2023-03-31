
using Codice.Client.Common;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GameTime
{
    public enum DayOfWeek
    {
        Saturday = 0,
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
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
            if (Math.Sign(time) > 0)
            {
                Hour += time;
                if (Hour > DayLength)
                {
                    int dayAmt = Hour / DayLength;
                    Hour -= (DayLength * dayAmt);
                    AddDate(dayAmt);
                }
            } else
            {
                Hour += time;
                if (Hour < 0)
                {
                    int adjusted = DayLength - (Math.Abs(time) - Math.Abs(Hour));
                    Hour = adjusted;
                    int dayAmt = time / DayLength;
                    AddDate(dayAmt);
                }
            }
        }
        public void AddDate(int date)
        {
            if (Math.Sign(date) > 0)
            {
                Date += date;
                if (Date > MonthLength)
                {
                    int monthAmt = Date / MonthLength;
                    Date -= (MonthLength * monthAmt);
                    AddMonth(monthAmt);
                }
            }
            else
            {
                Date += date;
                if (Date < 0)
                {
                    int adjusted = MonthLength - (Math.Abs(date) - Math.Abs(Date));
                    Date = adjusted;
                    int monthAmt = date / MonthLength;
                    AddMonth(monthAmt);
                }
            }
            Day = (DayOfWeek)(date % 7);
        }
        public void AddMonth(int month)
        {
            if (Math.Sign(month) > 0)
            {
                Month += month;
                if (Month > YearLength)
                {
                    int yearAmt = Month / YearLength;
                    Month -= (YearLength * yearAmt);
                    AddYear(yearAmt);
                }
            }
            else
            {
                Month += month;
                if (Month < 0)
                {
                    int adjusted = YearLength - (Math.Abs(month) - Math.Abs(Month));
                    Month = adjusted;
                    int yearAmt = month / YearLength;
                    AddYear(yearAmt);
                }
            }
        }
        public void AddYear(int year)
        {
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
            return $"{Month+1}/{Date + 1}/{Year + 1}({Day + 1}) {Hour + 1}:00";
        }
        
    }
}
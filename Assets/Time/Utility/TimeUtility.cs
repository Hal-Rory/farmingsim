using UnityEngine;

namespace GameTime
{
    public static class TimeUtility
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
        public static int DayLength => 23; //hours in day, minus 1 to account for 0
        public static int MonthLength => 27; //days in a month, minus 1 to account for 0
        public static int YearLength => 11; //months in a year, minus 1 to account for 0
        public static int YearMax => 9998; //max amount of years, world ends
        private static int HoursPerDay => DayLength + 1;
        private static int DaysPerMonth => MonthLength + 1;
        private static int MonthsPerYear => YearLength + 1;
        public static long DaysToHours(int days)
        {
            return days * HoursPerDay;
        }
        public static long MonthsToHours(int months)
        {
            return months * DaysPerMonth * HoursPerDay;
        }
        public static long YearsToHours(int years)
        {
            return years * (DaysPerMonth*MonthsPerYear) * HoursPerDay;
        }
        public static string GetMonthName(string month)
        {
            if(int.TryParse(month, out int mon))
            {
                return GetMonthName(mon);
            }
            else
            {
                Debug.LogWarning("could not parse month");
                return string.Empty;
            }
        }
        public static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "Jan";
                case 2:
                    return "Feb";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "May";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Aug";
                case 9:
                    return "Sep";
                case 10:
                    return "Oct";
                case 11:
                    return "Nov";
                case 12:
                    return "Dec";
            }
            return string.Empty;
        }
    }
}
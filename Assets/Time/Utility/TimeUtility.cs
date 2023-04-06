namespace GameTime
{
    public static class TimeUtility
    {
        public static int DayLength => 23; //hours in day, minus 1 to account for 0
        public static int MonthLength => 27; //days in a month, minus 1 to account for 0
        public static int YearLength => 11; //months in a year, minus 1 to account for 0
        public static int YearMax => 9999; //max amount of years, world ends
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
    }
}
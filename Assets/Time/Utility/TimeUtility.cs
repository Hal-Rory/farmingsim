using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameTime
{
    public static class TimeUtility
    {
        private static int HoursPerDay => TimeStruct.DayLength + 1;
        private static int DaysPerMonth => TimeStruct.MonthLength + 1;
        private static int MonthsPerYear => TimeStruct.YearLength + 1;
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
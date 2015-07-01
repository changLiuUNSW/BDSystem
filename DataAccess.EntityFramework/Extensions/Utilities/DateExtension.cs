using System;

namespace DataAccess.EntityFramework.Extensions.Utilities
{
    public static class DateExtension
    {
        public static bool IsThisWeek(this DateTime dt)
        {
            return dt.Date >= Monday().Date && dt.Date <= Sunday().Date;
        }

        public static bool IsLastWeek(this DateTime dt)
        {
            return dt.Date < Monday().Date;
        }

        public static DateTime Sunday()
        {
            var daysToSunday = 7 - (int) DateTime.Today.DayOfWeek;
            return DateTime.Today.AddDays(daysToSunday);
        }

        public static DateTime Monday()
        {
            var daysToMonday = 1 - (int) DateTime.Today.DayOfWeek;
            return DateTime.Today.AddDays(daysToMonday);
        }
    }
}

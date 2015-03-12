using System;

namespace Common.Extensions
{
    public static class DateExtensions
    {
        public static DateTime NextOccurrence(this TimeSpan midnightOffset, DayOfWeek? day)
        {
            DateTime next = DateTime.Now.Date + midnightOffset.AddDays(midnightOffset.Days);
            if (next < DateTime.Now)
                next = next.AddDays(1);
            while (day == null || next.DayOfWeek != day)
                next = next.AddDays(1);
            return next;
        }

        public static TimeSpan UntilNextOccurrence(this TimeSpan midnightOffset, DayOfWeek? day)
        {
            return NextOccurrence(midnightOffset, day) - DateTime.Now;
        }

        public static TimeSpan AddDays(this TimeSpan values, int days)
        {
            return values.Add(new TimeSpan(days, 0, 0));
        }

        public static TimeSpan AddHours(this TimeSpan values, int hours)
        {
            return values.Add(new TimeSpan(0, hours, 0));
        }

        public static TimeSpan AddMinutes(this TimeSpan values, int minutes)
        {
            return values.Add(new TimeSpan(0, 0, minutes));
        }

        public static TimeSpan AddSeconds(this TimeSpan values, int seconds)
        {
            return values.Add(new TimeSpan(0, 0, 0, seconds));
        }
    }
}

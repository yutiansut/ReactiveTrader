using System;

namespace Adaptive.ReactiveTrader.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToNextWeekday(this DateTime date, int dayCount = 2)
        {
            var plusN = date.AddDays(dayCount);

            while (plusN.DayOfWeek == DayOfWeek.Saturday || plusN.DayOfWeek == DayOfWeek.Sunday)
            {
                plusN = plusN.AddDays(1);
            }

            return plusN;
        }
    }
}
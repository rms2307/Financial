namespace Fina.Core.Common
{
    public static class DateTimeExtension
    {
        public static DateTime GetFirstDayMonth(this DateTime date, int? year = null, int? month = null)
            => new(year ?? date.Year, month ?? date.Month, 1);

        public static DateTime GetLastDayMonth(this DateTime date, int? year = null, int? month = null) 
            => new DateTime(year ?? date.Year, month ?? date.Month, 1).AddMonths(1).AddDays(-1);
    }
}

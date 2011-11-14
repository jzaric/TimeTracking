
namespace System
{
    public static class DateTimeExtension
    {
        public static bool IsWorkday(this DateTime date)
        {
            return (int)date.DayOfWeek > 0 && (int)date.DayOfWeek < 6;
        }
    }
}
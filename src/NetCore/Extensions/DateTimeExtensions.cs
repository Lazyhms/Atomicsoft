namespace System;

public static partial class DateTimeExtensions
{
    public static bool HasOverlap(this DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        => start1 <= end2 && end1 >= start2;

    public static DateTime StartOfDay(this DateTime date)
        => DateTime.SpecifyKind(date.Date, date.Kind);

    public static DateTime EndOfDay(this DateTime date)
        => DateTime.SpecifyKind(date.Date.AddDays(1).AddTicks(-1), date.Kind);

    public static (DateTime Start, DateTime End) DailyRange(this DateTime date)
        => (date.StartOfDay(), date.EndOfDay());

    public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        => DateTime.SpecifyKind(date.Date.AddDays(-1 * (7 + (date.DayOfWeek - startOfWeek)) % 7), date.Kind);

    public static DateTime EndOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        => DateTime.SpecifyKind(date.StartOfWeek(startOfWeek).AddDays(7).AddTicks(-1), date.Kind);

    public static (DateTime Start, DateTime End) WeeklyRange(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        => (date.StartOfWeek(startOfWeek), date.EndOfWeek(startOfWeek));

    public static DateTime StartOfMonth(this DateTime date)
        => DateTime.SpecifyKind(new DateTime(date.Year, date.Month, 1), date.Kind);

    public static DateTime EndOfMonth(this DateTime date)
        => DateTime.SpecifyKind(new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).AddDays(1).AddTicks(-1), date.Kind);

    public static (DateTime Start, DateTime End) MonthlyRange(this DateTime date)
        => (date.StartOfMonth(), date.EndOfMonth());

    public static int Quarter(this DateTime date)
        => (date.Month - 1) / 3 + 1;

    public static DateTime StartOfQuarter(this DateTime date)
    {
        var quarter = date.Quarter();
        var startMonth = (quarter - 1) * 3 + 1;
        return DateTime.SpecifyKind(new DateTime(date.Year, startMonth, 1), date.Kind);
    }

    public static DateTime EndOfQuarter(this DateTime date)
    {
        var quarter = date.Quarter();
        var endMonth = quarter * 3;
        var daysInMonth = DateTime.DaysInMonth(date.Year, endMonth);

        var end = new DateTime(date.Year, endMonth, daysInMonth)
                  .AddDays(1).AddTicks(-1);
        return DateTime.SpecifyKind(end, date.Kind);
    }

    public static (DateTime Start, DateTime End) QuarterlyRange(this DateTime date)
        => (date.StartOfQuarter(), date.EndOfQuarter());

    public static DateTime StartOfYear(this DateTime date)
        => DateTime.SpecifyKind(new DateTime(date.Year, 1, 1), date.Kind);

    public static DateTime EndOfYear(this DateTime date)
    {
        var end = new DateTime(date.Year, 12, 31).AddDays(1).AddTicks(-1);
        return DateTime.SpecifyKind(end, date.Kind);
    }

    public static (DateTime Start, DateTime End) YearlyRange(this DateTime date)
        => (date.StartOfYear(), date.EndOfYear());

    public static async Task WaitForNextMinuteAsync(CancellationToken stoppingToken = default)
    {
        var utcNow = DateTime.UtcNow;
        var nextMinute = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, 0).AddMinutes(1);

        var delay = nextMinute - utcNow;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, stoppingToken);
        }
    }

    public static async Task WaitForNextHourAsync(CancellationToken stoppingToken = default)
    {
        var utcNow = DateTime.UtcNow;
        var nextHour = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0).AddHours(1);

        var delay = nextHour - utcNow;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, stoppingToken);
        }
    }

    public static async Task WaitForNextDayAsync(CancellationToken stoppingToken = default)
    {
        var utcNow = DateTime.UtcNow;
        var nextDay = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0).AddDays(1);

        var delay = nextDay - utcNow;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, stoppingToken);
        }
    }

    public static async Task WaitForNextMonthAsync(CancellationToken stoppingToken = default)
    {
        var utcNow = DateTime.UtcNow;
        var nextMonth = new DateTime(utcNow.Year, utcNow.Month, 1, 0, 0, 0).AddMonths(1);

        var delay = nextMonth - utcNow;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, stoppingToken);
        }
    }

    public static async Task WaitForNextYearAsync(CancellationToken stoppingToken = default)
    {
        var utcNow = DateTime.UtcNow;
        var nextYear = new DateTime(utcNow.Year, 1, 1, 0, 0, 0).AddYears(1);

        var delay = nextYear - utcNow;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, stoppingToken);
        }
    }
}

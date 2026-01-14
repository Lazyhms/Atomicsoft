namespace System;

public static partial class DateTimeExtensions
{
    public static bool HasOverlap(this DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        => start1 <= end2 && end1 >= start2;

    #region Day

    public static DateTime StartOfDay(this DateTime dateTime)
        => DateTime.SpecifyKind(dateTime.Date, dateTime.Kind);

    public static DateTime EndOfDay(this DateTime dateTime)
        => DateTime.SpecifyKind(dateTime.Date.AddDays(1).AddTicks(-1), dateTime.Kind);

    public static (DateTime Start, DateTime End) DailyRange(this DateTime dateTime)
        => (dateTime.StartOfDay(), dateTime.EndOfDay());

    #endregion

    #region Week

    public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Monday)
        => DateTime.SpecifyKind(dateTime.Date.AddDays(-1 * (7 + (dateTime.DayOfWeek - startOfWeek)) % 7), dateTime.Kind);

    public static DateTime EndOfWeek(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Monday)
        => DateTime.SpecifyKind(dateTime.StartOfWeek(startOfWeek).AddDays(7).AddTicks(-1), dateTime.Kind);

    public static (DateTime Start, DateTime End) WeeklyRange(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Monday)
        => (dateTime.StartOfWeek(startOfWeek), dateTime.EndOfWeek(startOfWeek));

    #endregion

    #region Month

    public static DateTime StartOfMonth(this DateTime dateTime)
        => DateTime.SpecifyKind(new DateTime(dateTime.Year, dateTime.Month, 1), dateTime.Kind);

    public static DateTime EndOfMonth(this DateTime dateTime)
        => DateTime.SpecifyKind(dateTime.StartOfMonth().AddMonths(1).AddTicks(-1), dateTime.Kind);

    public static (DateTime Start, DateTime End) MonthlyRange(this DateTime dateTime)
        => (dateTime.StartOfMonth(), dateTime.EndOfMonth());

    #endregion

    #region Quarter

    public static int Quarter(this DateTime dateTime)
        => (dateTime.Month - 1) / 3 + 1;

    public static DateTime StartOfQuarter(this DateTime dateTime)
        => DateTime.SpecifyKind(new DateTime(dateTime.Year, (dateTime.Quarter() - 1) * 3 + 1, 1), dateTime.Kind);

    public static DateTime EndOfQuarter(this DateTime dateTime)
        => DateTime.SpecifyKind(dateTime.StartOfQuarter().AddMonths(3).AddTicks(-1), dateTime.Kind);

    public static (DateTime Start, DateTime End) QuarterlyRange(this DateTime dateTime)
        => (dateTime.StartOfQuarter(), dateTime.EndOfQuarter());

    #endregion

    #region Year

    public static DateTime StartOfYear(this DateTime dateTime)
        => DateTime.SpecifyKind(new DateTime(dateTime.Year, 1, 1), dateTime.Kind);

    public static DateTime EndOfYear(this DateTime dateTime)
    {
        var end = new DateTime(dateTime.Year, 12, 31).AddDays(1).AddTicks(-1);
        return DateTime.SpecifyKind(end, dateTime.Kind);
    }

    public static (DateTime Start, DateTime End) YearlyRange(this DateTime dateTime)
        => (dateTime.StartOfYear(), dateTime.EndOfYear());

    #endregion

    #region Wait For Next

    public static async Task WaitForNextMinuteAsync(this DateTime dateTime, CancellationToken stoppingToken = default)
    {
        var nextMinute = DateTime.SpecifyKind(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0), dateTime.Kind).AddMinutes(1);

        var delay = nextMinute - dateTime;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, stoppingToken);
        }
    }

    public static async Task WaitForNextHourAsync(this DateTime dateTime, CancellationToken stoppingToken = default)
    {
        var nextHour = DateTime.SpecifyKind(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0), dateTime.Kind).AddHours(1);

        var delay = nextHour - dateTime;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, stoppingToken);
        }
    }

    public static async Task WaitForNextDayAsync(this DateTime dateTime, CancellationToken stoppingToken = default)
    {
        var nextDay = DateTime.SpecifyKind(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0), dateTime.Kind).AddDays(1);

        var delay = nextDay - dateTime;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, stoppingToken);
        }
    }

    public static async Task WaitForNextMonthAsync(this DateTime dateTime, CancellationToken stoppingToken = default)
    {
        var nextMonth = DateTime.SpecifyKind(new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0), dateTime.Kind).AddMonths(1);

        var delay = nextMonth - dateTime;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, stoppingToken);
        }
    }

    public static async Task WaitForNextQuarterAsync(this DateTime dateTime, CancellationToken stoppingToken = default)
    {
        var nextQuarter = DateTime.SpecifyKind(dateTime.EndOfQuarter().AddTicks(1), dateTime.Kind);

        var maxDelay = TimeSpan.FromMilliseconds(int.MaxValue);
        var currentTime = dateTime;

        while (true)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var delay = nextQuarter - currentTime;
            if (delay <= TimeSpan.Zero)
            {
                break;
            }

            var actualDelay = delay > maxDelay ? maxDelay : delay;
            await Task.Delay(actualDelay, stoppingToken);

            currentTime = DateTime.Now;
        }
    }

    public static async Task WaitForNextYearAsync(this DateTime dateTime, CancellationToken stoppingToken = default)
    {
        var nextYear = DateTime.SpecifyKind(new DateTime(dateTime.Year, 1, 1, 0, 0, 0).AddYears(1), dateTime.Kind);

        var maxDelay = TimeSpan.FromMilliseconds(int.MaxValue);
        var currentTime = dateTime;

        while (true)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var delay = nextYear - currentTime;
            if (delay <= TimeSpan.Zero)
            {
                break;
            }

            var actualDelay = delay > maxDelay ? maxDelay : delay;
            await Task.Delay(actualDelay, stoppingToken);

            currentTime = DateTime.Now;
        }
    }

    #endregion
}

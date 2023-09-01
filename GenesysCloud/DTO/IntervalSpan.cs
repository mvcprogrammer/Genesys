namespace GenesysCloud.DTO;

public enum IntervalSmoothType
{
    HalfHour = 1,
    Hourly,
    Daily,
    HourlyByDay,
    MonthToDate
}

public sealed record IntervalSpan
{
    private readonly TimeZoneInfo _easternTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    public IntervalSpan(string interval)
    {
        StartTimeUtc = DateTime.Parse(interval[..24]).ToUniversalTime();
        EndTimeUtc = DateTime.Parse(interval[25..49]).ToUniversalTime();
    }

    public IntervalSpan()
    {
    }

    public DateTime StartTimeUtc { get; init; }
    public DateTime EndTimeUtc { get; init; }
    public string GetStartDateUtc() => StartTimeUtc.ToShortDateString();
    public string GetStartTimeUtc() => StartTimeUtc.ToString("HH:mm:ss");
    public string GetEndTimeUtc() => EndTimeUtc.ToString("HH:mm:ss");

    public string ToGenesysInterval => StartTimeUtc.ToString("yyyy'-'MM'-'dd'T'HH':'mm':00.000'") + "Z/"
        + EndTimeUtc.ToString("yyyy'-'MM'-'dd'T'HH':'mm':00.000'") + "Z";

    public DateTime StartTimeEst => TimeZoneInfo.ConvertTimeFromUtc(StartTimeUtc, _easternTimeZoneInfo);
    public DateTime EndTimeEst => TimeZoneInfo.ConvertTimeFromUtc(EndTimeUtc, _easternTimeZoneInfo);
    public string GetStartDateEst() => StartTimeEst.ToShortDateString();
    public string GetStartTimeEst() => StartTimeEst.ToString("HH:mm:ss");
    public string ToReportExtensionUtc => $"{StartTimeUtc:yyyyMMddHHmmss}UTC-{EndTimeUtc:yyyyMMddHHmmss}UTC";
    public string ToReportExtensionEst => $"{StartTimeEst:yyyyMMddHHmmss}EST-{EndTimeEst:yyyyMMddHHmmss}EST";

    public IntervalSpan SmoothInterval(IntervalSmoothType smoothType)
    {
        DateTime smoothedStartTime;
        var timeSpan = EndTimeUtc - StartTimeUtc;

        switch (smoothType)
        {
            case IntervalSmoothType.HalfHour:
                smoothedStartTime = StartTimeUtc.Minute switch
                {
                    < 30 => new DateTime(StartTimeUtc.Year, StartTimeUtc.Month, StartTimeUtc.Day, StartTimeUtc.Hour, 0,
                        0),
                    _ => new DateTime(StartTimeUtc.Year, StartTimeUtc.Month, StartTimeUtc.Day, StartTimeUtc.Hour, 30, 0)
                };
                break;
            case IntervalSmoothType.Hourly:
                smoothedStartTime = new DateTime(StartTimeUtc.Year, StartTimeUtc.Month, StartTimeUtc.Day,
                    StartTimeUtc.Hour, 0, 0);
                break;
            case IntervalSmoothType.Daily:
                timeSpan = new TimeSpan(1, 0, 0, 0);
                smoothedStartTime = new DateTime(StartTimeUtc.Year, StartTimeUtc.Month, StartTimeUtc.Day, 0, 0, 0) +
                                    TimeSpan.FromDays(-1);
                break;
            case IntervalSmoothType.HourlyByDay:
                timeSpan = new TimeSpan(0, DateTime.UtcNow.Hour, 0, 0);
                smoothedStartTime = new DateTime(StartTimeUtc.Year, StartTimeUtc.Month, StartTimeUtc.Day, 0, 0, 0);
                break;
            case IntervalSmoothType.MonthToDate:
                timeSpan = new TimeSpan(StartTimeUtc.Day, 0, 0, 0) + TimeSpan.FromDays(-1);
                smoothedStartTime = new DateTime(StartTimeUtc.Year, StartTimeUtc.Month, 1, 0, 0, 0);
                break;
            default:
                return this;
        }

        smoothedStartTime = DateTime.SpecifyKind(smoothedStartTime, DateTimeKind.Utc);
        var smoothedEndTimeUtc = (smoothedStartTime + timeSpan);

        return new IntervalSpan
        {
            StartTimeUtc = smoothedStartTime,
            EndTimeUtc = smoothedEndTimeUtc
        };
    }

    public IntervalSpan SmoothIntervalEst(IntervalSmoothType smoothType)
    {
        var interval = SmoothInterval(smoothType);

        var timespanStart = _easternTimeZoneInfo.IsDaylightSavingTime(interval.StartTimeEst)
            ? new TimeSpan(0, 4, 0, 0)
            : new TimeSpan(0, 5, 0, 0);

        var timespanEnd = _easternTimeZoneInfo.IsDaylightSavingTime(interval.EndTimeEst)
            ? new TimeSpan(0, 4, 0, 0)
            : new TimeSpan(0, 5, 0, 0);

        return new IntervalSpan
        {
            StartTimeUtc = interval.StartTimeUtc + timespanStart,
            EndTimeUtc = smoothType == IntervalSmoothType.HourlyByDay
                ? interval.EndTimeUtc
                : interval.EndTimeUtc + timespanEnd
        };
    }
}
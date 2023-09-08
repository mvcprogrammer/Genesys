namespace GenesysCloud.Services.Contracts.Fundamental;

public interface IAnalyticsService
{
    public ServiceResponse<List<EvaluationAggregateDataContainer>> GetEvaluationAggregateData(MetricsInterval interval, IReadOnlyCollection<string> queues, IReadOnlyCollection<string> divisions);
}
namespace GenesysCloud.Services.Contracts.Fundamental;

public interface IAnalyticsService
{
    public ServiceResponse<List<EvaluationAggregateDataContainer>> GetEvaluationAggregateData(EvaluationAggregationQuery query);
    public ServiceResponse<List<SurveyAggregateDataContainer>> GetSurveyAggregateData(SurveyAggregationQuery query);
}
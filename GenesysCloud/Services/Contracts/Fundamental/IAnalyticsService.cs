namespace GenesysCloud.Services.Contracts.Fundamental;

public interface IAnalyticsService
{
    public List<EvaluationAggregateDataContainer> GetEvaluationAggregateData(EvaluationAggregationQuery query);
    public List<SurveyAggregateDataContainer> GetSurveyAggregateData(SurveyAggregationQuery query);
    public List<AnalyticsConversationWithoutAttributes> GetConversationDetails(ConversationQuery query);
}
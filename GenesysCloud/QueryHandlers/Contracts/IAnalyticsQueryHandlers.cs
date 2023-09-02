namespace GenesysCloud.QueryHandlers.Contracts;

public interface IAnalyticsQueryHandlers
{
    public GenesysServiceResponse<List<AnalyticsConversationWithoutAttributes>> ConversationDetailQuery(ConversationQuery query);
    public GenesysServiceResponse<List<SurveyAggregateDataContainer>> SurveyAggregatesQuery(SurveyAggregationQuery query);
    public GenesysServiceResponse<List<EvaluationAggregateDataContainer>> EvaluationAggregationQuery(EvaluationAggregationQuery query);
    public GenesysServiceResponse<List<ConversationAggregateDataContainer>> GenesysConversationsAggregatesQuery(ConversationAggregationQuery query);
}
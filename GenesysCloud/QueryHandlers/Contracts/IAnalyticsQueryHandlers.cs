namespace GenesysCloud.QueryHandlers.Contracts;

public interface IAnalyticsQueryHandlers
{
    public ServiceResponse<List<AnalyticsConversationWithoutAttributes>> ConversationDetailQuery(ConversationQuery query);
    public ServiceResponse<List<SurveyAggregateDataContainer>> SurveyAggregatesQuery(SurveyAggregationQuery query);
    public ServiceResponse<List<EvaluationAggregateDataContainer>> EvaluationAggregationQuery(EvaluationAggregationQuery query);
    public ServiceResponse<List<ConversationAggregateDataContainer>> GenesysConversationsAggregatesQuery(ConversationAggregationQuery query);
}
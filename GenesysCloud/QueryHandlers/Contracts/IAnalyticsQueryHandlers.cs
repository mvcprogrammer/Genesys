namespace GenesysCloud.QueryHandlers.Contracts;

public interface IAnalyticsQueryHandlers
{
    public List<AnalyticsConversationWithoutAttributes> ConversationDetailQuery(ConversationQuery query);
    public List<SurveyAggregateDataContainer> SurveyAggregatesQuery(SurveyAggregationQuery query);
    public List<EvaluationAggregateDataContainer> EvaluationAggregationQuery(EvaluationAggregationQuery query);
    public List<ConversationAggregateDataContainer> GenesysConversationsAggregatesQuery(ConversationAggregationQuery query);
}
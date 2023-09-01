namespace GenesysCloud.QueryHandlers.Mock.Analytics.Conversations;

public class AnalyticsQueryHandlers : IAnalyticsQueryHandlers
{
    public GenesysServiceResponse<List<AnalyticsConversationWithoutAttributes>> AnalyticsConversationDetailQuery(ConversationQuery query)
    {
        throw new NotImplementedException();
    }

    public GenesysServiceResponse<List<SurveyAggregateDataContainer>> SurveyAggregatesQuery(SurveyAggregationQuery query)
    {
        throw new NotImplementedException();
    }

    public GenesysServiceResponse<List<EvaluationAggregateDataContainer>> EvaluationAggregationQuery(EvaluationAggregationQuery query)
    {
        throw new NotImplementedException();
    }

    public GenesysServiceResponse<List<ConversationAggregateDataContainer>> GenesysConversationsAggregatesQuery(ConversationAggregationQuery query)
    {
        throw new NotImplementedException();
    }
}
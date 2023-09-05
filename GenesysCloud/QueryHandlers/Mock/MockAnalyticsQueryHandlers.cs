using GenesysCloud.QueryHandlers.Contracts;

namespace GenesysCloud.QueryHandlers.Mock;

public class MockAnalyticsQueryHandlers : IAnalyticsQueryHandlers
{
    public ServiceResponse<List<AnalyticsConversationWithoutAttributes>> ConversationDetailQuery(ConversationQuery query)
    {
        throw new NotImplementedException();
    }

    public ServiceResponse<List<SurveyAggregateDataContainer>> SurveyAggregatesQuery(SurveyAggregationQuery query)
    {
        throw new NotImplementedException();
    }

    public ServiceResponse<List<EvaluationAggregateDataContainer>> EvaluationAggregationQuery(EvaluationAggregationQuery query)
    {
        throw new NotImplementedException();
    }

    public ServiceResponse<List<ConversationAggregateDataContainer>> GenesysConversationsAggregatesQuery(ConversationAggregationQuery query)
    {
        throw new NotImplementedException();
    }
}
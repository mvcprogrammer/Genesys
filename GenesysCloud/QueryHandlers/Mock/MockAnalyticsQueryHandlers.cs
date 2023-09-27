using GenesysCloud.QueryHandlers.Contracts;

namespace GenesysCloud.QueryHandlers.Mock;

public class MockAnalyticsQueryHandlers : IAnalyticsQueryHandlers
{
    public List<AnalyticsConversationWithoutAttributes> ConversationDetailQuery(ConversationQuery query)
    {
        throw new NotImplementedException();
    }

    public List<SurveyAggregateDataContainer> SurveyAggregatesQuery(SurveyAggregationQuery query)
    {
        throw new NotImplementedException();
    }

    public List<EvaluationAggregateDataContainer> EvaluationAggregationQuery(EvaluationAggregationQuery query)
    {
        throw new NotImplementedException();
    }

    public List<ConversationAggregateDataContainer> GenesysConversationsAggregatesQuery(ConversationAggregationQuery query)
    {
        throw new NotImplementedException();
    }
}
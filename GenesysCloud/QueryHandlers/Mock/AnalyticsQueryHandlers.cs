using GenesysCloud.QueryHandlers.Contracts;

namespace GenesysCloud.QueryHandlers.Mock;

public class AnalyticsQueryHandlers : IAnalyticsQueryHandlers
{
    public GenesysServiceResponse<List<AnalyticsConversationWithoutAttributes>> ConversationDetailQuery(ConversationQuery query)
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
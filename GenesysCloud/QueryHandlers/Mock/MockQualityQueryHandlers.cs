using GenesysCloud.QueryHandlers.Contracts;

namespace GenesysCloud.QueryHandlers.Mock;

public class MockQualityQueryHandlers : IQualityQueryHandlers
{
    public EvaluationResponse ConversationEvaluationDetail(string conversationId, string evaluationId, string expand)
    {
        throw new NotImplementedException();
    }

    public List<Survey> ConversationSurveyDetail(string conversationId)
    {
        throw new NotImplementedException();
    }
}
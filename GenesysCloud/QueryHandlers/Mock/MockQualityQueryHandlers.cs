using GenesysCloud.QueryHandlers.Contracts;

namespace GenesysCloud.QueryHandlers.Mock;

public class MockQualityQueryHandlers : IQualityQueryHandlers
{
    public ServiceResponse<EvaluationResponse> ConversationEvaluationDetail(string conversationId, string evaluationId, string expand)
    {
        throw new NotImplementedException();
    }
}
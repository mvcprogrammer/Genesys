using GenesysCloud.QueryHandlers.Contracts;

namespace GenesysCloud.QueryHandlers.Mock;

public class QualityQueryHandlers : IQualityQueryHandlers
{
    public GenesysServiceResponse<EvaluationResponse> GetConversationEvaluationDetail(string conversationId, string evaluationId, string expand)
    {
        throw new NotImplementedException();
    }
}
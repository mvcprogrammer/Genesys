namespace GenesysCloud.QueryHandlers.Mock.Quality;

public class QualityQueryHandlers : IQualityQueryHandlers
{
    public GenesysServiceResponse<EvaluationResponse> GetConversationEvaluationDetail(string conversationId, string evaluationId, string expand)
    {
        throw new NotImplementedException();
    }
}
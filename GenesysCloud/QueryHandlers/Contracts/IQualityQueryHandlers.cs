namespace GenesysCloud.QueryHandlers.Contracts;

public interface IQualityQueryHandlers
{
    ServiceResponse<EvaluationResponse> GetConversationEvaluationDetail(string conversationId, string evaluationId, string expand);
}
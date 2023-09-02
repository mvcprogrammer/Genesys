namespace GenesysCloud.QueryHandlers.Contracts;

public interface IQualityQueryHandlers
{
    GenesysServiceResponse<EvaluationResponse> GetConversationEvaluationDetail(string conversationId, string evaluationId, string expand);
}
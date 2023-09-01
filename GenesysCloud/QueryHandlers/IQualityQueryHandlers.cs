namespace GenesysCloud.QueryHandlers;

public interface IQualityQueryHandlers
{
    GenesysServiceResponse<EvaluationResponse> GetConversationEvaluationDetail(string conversationId, string evaluationId, string expand);
}
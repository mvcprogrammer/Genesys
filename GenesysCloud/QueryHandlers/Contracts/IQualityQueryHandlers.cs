namespace GenesysCloud.QueryHandlers.Contracts;

public interface IQualityQueryHandlers
{
    ServiceResponse<EvaluationResponse> ConversationEvaluationDetail(string conversationId, string evaluationId, string expand);
}
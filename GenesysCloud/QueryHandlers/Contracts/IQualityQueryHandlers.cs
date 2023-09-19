namespace GenesysCloud.QueryHandlers.Contracts;

public interface IQualityQueryHandlers
{
    public ServiceResponse<EvaluationResponse> ConversationEvaluationDetail(string conversationId, string evaluationId, string expand);
    public ServiceResponse<List<Survey>> ConversationSurveyDetail(string conversationId);
}
namespace GenesysCloud.Services.Contracts.Fundamental;

public interface IQualityService
{
    public ServiceResponse<EvaluationResponse> GetConversationEvaluationDetail(string conversationId, string evaluationId, string expand);
    public ServiceResponse<List<Survey>> GetConversationSurveyDetail(string conversationId);
}
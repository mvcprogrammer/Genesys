namespace GenesysCloud.Services.Contracts.Fundamental;

public interface IQualityService
{
    public EvaluationResponse GetConversationEvaluationDetail(string conversationId, string evaluationId, string expand);
    public List<Survey> GetConversationSurveyDetail(string conversationId);
}
namespace GenesysCloud.QueryHandlers.Contracts;

public interface IQualityQueryHandlers
{
    public EvaluationResponse ConversationEvaluationDetail(string conversationId, string evaluationId, string expand);
    public List<Survey> ConversationSurveyDetail(string conversationId);
}
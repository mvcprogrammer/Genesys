namespace GenesysCloud.QueryHandlers.Contracts;

public interface ISpeechTextQueryHandlers
{
    public ServiceResponse<ConversationMetrics> GetConversationAnalytics(string conversationId);
}
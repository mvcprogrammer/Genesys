namespace GenesysCloud.QueryHandlers.Contracts;

public interface ISpeechTextQueryHandlers
{
    public ConversationMetrics GetConversationAnalytics(string conversationId);
}
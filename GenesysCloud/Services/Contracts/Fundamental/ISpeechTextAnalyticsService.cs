namespace GenesysCloud.Services.Contracts.Fundamental;

public interface ISpeechTextAnalyticsService
{
    public ConversationMetrics GetConversationAnalytics(string conversationId);
}
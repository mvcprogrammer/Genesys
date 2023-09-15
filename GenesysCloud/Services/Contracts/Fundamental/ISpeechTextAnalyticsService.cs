namespace GenesysCloud.Services.Contracts.Fundamental;

public interface ISpeechTextAnalyticsService
{
    public ServiceResponse<ConversationMetrics> GetConversationAnalytics(string conversationId);
}
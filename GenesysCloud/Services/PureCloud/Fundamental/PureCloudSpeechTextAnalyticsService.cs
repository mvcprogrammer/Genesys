using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;

namespace GenesysCloud.Services.PureCloud.Fundamental;

public class PureCloudSpeechTextAnalyticsService : ISpeechTextAnalyticsService
{
    private readonly ISpeechTextQueryHandlers _speechTextQueryHandlers;
    

    public PureCloudSpeechTextAnalyticsService(ISpeechTextQueryHandlers speechTextAnalyticsHandlers)
    {
        _speechTextQueryHandlers = speechTextAnalyticsHandlers;
    }

    public ServiceResponse<ConversationMetrics> GetConversationAnalytics(string conversationId)
    {
        var response = _speechTextQueryHandlers.GetConversationAnalytics(conversationId);
        return response;
    }
}
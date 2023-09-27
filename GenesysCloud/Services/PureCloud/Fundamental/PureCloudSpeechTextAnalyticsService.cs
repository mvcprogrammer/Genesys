using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Static;

namespace GenesysCloud.Services.PureCloud.Fundamental;

public class PureCloudSpeechTextAnalyticsService : ISpeechTextAnalyticsService
{
    private readonly ISpeechTextQueryHandlers _speechTextQueryHandlers;
    
    public PureCloudSpeechTextAnalyticsService(ISpeechTextQueryHandlers speechTextAnalyticsHandlers)
    {
          _speechTextQueryHandlers = speechTextAnalyticsHandlers;
    }

    public ConversationMetrics GetConversationAnalytics(string conversationId)
    {
        return AuthorizedAction(() => ServiceResponse.LogAndReturnResponse(_speechTextQueryHandlers.GetConversationAnalytics(conversationId)));
    }
    
    /// <summary>
    /// This method ensures all calls have authorization and handles not authorized responses.
    /// <param name="action">
    /// This delegate will invoke its action if authorized=true
    /// </param>
    /// </summary>
    private static T AuthorizedAction<T>(Func<T> action)
    {
        if (AuthorizeService.IsAuthorized())
            return action();
        
        throw new UnauthorizedAccessException(Constants.NotAuthorized);
    }
}
using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

public class PureCloudSpeechTextQueryHandlers : ISpeechTextQueryHandlers
{
    private readonly SpeechTextAnalyticsApi _speechTextAnalyticsApi = new();

    public ServiceResponse<ConversationMetrics> GetConversationAnalytics(string conversationId)
    {
        try
        {
            var response = _speechTextAnalyticsApi.GetSpeechandtextanalyticsConversation(conversationId);
            return SystemResponse.SuccessResponse(response);
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<ConversationMetrics>(exception);
        }
    }
}
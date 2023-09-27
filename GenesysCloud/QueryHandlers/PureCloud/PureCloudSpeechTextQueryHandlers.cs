using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;

namespace GenesysCloud.QueryHandlers.PureCloud;

/// <summary>
/// PureCloud Speech and Text Analytics Query Handler is responsible for submitting an PureCloud.Client.V2.Model SpeechTextAnalyticsApi query to PureCloud.
/// The API can be called multiple times if the response is paged.
/// Handlers are not dependent on query filters; 1 handler, many queries.
/// The query handler always returns a Genesys Client.V2.Model in the data field of a ServiceResponse.
/// DTO must never be done at this level for dependency segmentation.
/// Responses are always a ServiceResponse to bubble up handled exception messages and response ids.
/// </summary>
public class PureCloudSpeechTextQueryHandlers : ISpeechTextQueryHandlers
{
    private readonly SpeechTextAnalyticsApi _speechTextAnalyticsApi = new();

    public ConversationMetrics GetConversationAnalytics(string conversationId)
    {
        try
        {
            var response = _speechTextAnalyticsApi.GetSpeechandtextanalyticsConversation(conversationId: conversationId);
            return response ?? new ConversationMetrics();
        }
        catch (ApiException exception)
        {
            ServiceResponse.ExceptionHandler.HandleException<ConversationMetrics>(exception);

            // Conversation has been deleted
            if (exception.ErrorCode is 404)
            {
                // so return empty, will be handled as N/A
                return new ConversationMetrics();
            }
            
            throw;
        }
    }
}
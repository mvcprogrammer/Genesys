using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

/// <summary>
/// PureCloud Quality Query Handler is responsible for submitting an PureCloud.Client.V2.Model QualityApi query to PureCloud.
/// The API can be called multiple times if the response is paged.
/// Handlers are not dependent on query filters; 1 handler, many queries.
/// The query handler always returns a Genesys Client.V2.Model in the data field of a ServiceResponse.
/// DTO must never be done at this level for dependency segmentation.
/// Responses are always a ServiceResponse to bubble up handled exception messages and response ids.
/// </summary>
internal sealed class PureCloudQualityQueryHandlers : IQualityQueryHandlers
{
    private readonly QualityApi _qualityApi = new();
    
    public ServiceResponse<EvaluationResponse> ConversationEvaluationDetail(string conversationId, string evaluationId, string expand)
    {
        try
        {
            var response = _qualityApi.GetQualityConversationEvaluation(conversationId: conversationId, evaluationId: evaluationId, expand: expand);
            return SystemResponse.SuccessResponse(response ?? new EvaluationResponse());
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<EvaluationResponse>(exception, 
                $"conversationId:{conversationId}, evaluationId:{evaluationId}, expand:{expand}");
        }
    }
}
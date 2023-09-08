using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

internal sealed class PureCloudQualityQueryHandlers : IQualityQueryHandlers
{
    private readonly QualityApi _qualityApi = new();
    
    public ServiceResponse<EvaluationResponse> ConversationEvaluationDetail(string conversationId, string evaluationId, string expand)
    {
        try
        {
            var response = _qualityApi.GetQualityConversationEvaluation(conversationId, evaluationId, expand);
            return SystemResponse.SuccessResponse(response ?? new EvaluationResponse());
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<EvaluationResponse>(exception, 
                $"conversationId:{conversationId}, evaluationId:{evaluationId}, expand:{expand}");
        }
    }
}
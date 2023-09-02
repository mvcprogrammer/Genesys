using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

public class QualityQueryHandlers : IQualityQueryHandlers
{
    private readonly QualityApi _qualityApi;

    public QualityQueryHandlers(QualityApi qualityApi)
    {
        _qualityApi = qualityApi  ?? throw new ArgumentNullException(nameof(qualityApi));
    }
    
    public GenesysServiceResponse<EvaluationResponse> GetConversationEvaluationDetail(string conversationId, string evaluationId, string expand)
    {
        try
        {
            var response = _qualityApi.GetQualityConversationEvaluation(conversationId, evaluationId, expand);

            return response is null 
                ? GenesysResponse.FailureResponse<EvaluationResponse>(Constants.NullResponse) 
                : GenesysResponse.SuccessResponse(response);
        }
        catch (Exception exception)
        {
            return GenesysResponse.ExceptionHandler.HandleException<EvaluationResponse>(exception);
        }
    }
}
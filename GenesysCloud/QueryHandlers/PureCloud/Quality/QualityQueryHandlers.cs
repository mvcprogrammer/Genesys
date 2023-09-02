using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;

namespace GenesysCloud.QueryHandlers.PureCloud.Quality;

public class QualityQueryHandlers : IQualityQueryHandlers
{
    private readonly QualityApi _qualityApi = new ();
    
    public GenesysServiceResponse<EvaluationResponse> GetConversationEvaluationDetail(string conversationId, string evaluationId, string expand)
    {
        try
        {
            var response = _qualityApi.GetQualityConversationEvaluation(conversationId, evaluationId, expand);
            return GenesysResponse.SuccessResponse(response);
        }
        catch (ApiException exception)
        {
            return FailureResponse(exception.Message, exception.ErrorCode);
        }
        catch (Exception exception)
        {
            return FailureResponse(exception.Message);
        }
    }
    
    private static GenesysServiceResponse<EvaluationResponse> FailureResponse(string errorMessage, int errorCode = Constants.Invalid)
    {
        return new GenesysServiceResponse<EvaluationResponse>
        {
            Success = false,
            ErrorMessage = errorMessage,
            ErrorCode = errorCode,
            Data = new EvaluationResponse()
        };
    }
}
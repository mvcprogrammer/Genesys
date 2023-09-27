using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Static;
using PureCloudPlatform.Client.V2.Client;

namespace GenesysCloud.Services.PureCloud.Fundamental;

public class PureCloudQualityService : IQualityService
{
    private readonly IQualityQueryHandlers _qualityQueryHandlers;
    
    public PureCloudQualityService(IQualityQueryHandlers qualityQueryHandlers)
    {
        _qualityQueryHandlers = qualityQueryHandlers ?? throw new ArgumentException("Quality Query Handler Cannot be null.");
    }

    public EvaluationResponse GetConversationEvaluationDetail(string conversationId, string evaluationId, string expand)
    {
        return AuthorizedAction(() => ServiceResponse.LogAndReturnResponse(_qualityQueryHandlers.ConversationEvaluationDetail(conversationId, evaluationId, expand)));
    }
    
    public List<Survey> GetConversationSurveyDetail(string conversationId)
    {
        return AuthorizedAction(() => ServiceResponse.LogAndReturnResponse(_qualityQueryHandlers.ConversationSurveyDetail(conversationId)));
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
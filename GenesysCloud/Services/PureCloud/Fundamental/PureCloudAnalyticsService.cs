using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Static;

namespace GenesysCloud.Services.PureCloud.Fundamental;

/// <summary>
/// PureCloud Analytics Service handles calls to Genesys/Mock and is never called directly from outside this assembly.
/// A fundamental service must never return DTO data; it must only return V2.Client.Models
/// A fundamental service may only be called by derived services and authorizations must be called at this level, if needed.
/// When methods are given parameters, this class is responsible for calling query.build from those parameters and calling IPresenceQueryHandler.
/// It's permissible to shape returned data into dictionaries, lookups, etc., or return data as received, and always as the type received from api.
/// Responses are always a ServiceResponse to bubble up handled exception messages and response ids.
/// </summary>
public class PureCloudAnalyticsService : IAnalyticsService
{
    private readonly IAnalyticsQueryHandlers _analyticsQueryHandlers;
    
    public PureCloudAnalyticsService(IAnalyticsQueryHandlers analyticsQueryHandlers)
    {
        _analyticsQueryHandlers = analyticsQueryHandlers;
    }

    public List<EvaluationAggregateDataContainer> GetEvaluationAggregateData(EvaluationAggregationQuery query)
    {
        return AuthorizedAction(() => ServiceResponse.LogAndReturnResponse(_analyticsQueryHandlers.EvaluationAggregationQuery(query)));
    }

    public List<SurveyAggregateDataContainer> GetSurveyAggregateData(SurveyAggregationQuery query)
    {
        return AuthorizedAction(() => ServiceResponse.LogAndReturnResponse(_analyticsQueryHandlers.SurveyAggregatesQuery(query)));
    }

    public List<AnalyticsConversationWithoutAttributes> GetConversationDetails(ConversationQuery query)
    {
        return AuthorizedAction(() => ServiceResponse.LogAndReturnResponse(_analyticsQueryHandlers.ConversationDetailQuery(query)));
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
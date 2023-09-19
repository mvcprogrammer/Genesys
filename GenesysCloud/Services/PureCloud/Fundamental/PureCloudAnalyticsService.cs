using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Static;
using PureCloudPlatform.Client.V2.Client;

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
    private const string NotAuthorized = "Not Authorized";
    private bool _isAuthorized;
    
    public PureCloudAnalyticsService(IAnalyticsQueryHandlers analyticsQueryHandlers)
    {
        _analyticsQueryHandlers = analyticsQueryHandlers;
    }
    
    /// <summary>
    /// This method ensures all calls have authorization and handles not authorized responses.
    /// </summary>
    private ServiceResponse<T> AuthorizedAction<T>(Func<ServiceResponse<T>> action)
    {
        return Authorized()
            ? action() 
            : SystemResponse.FailureResponse<T>(NotAuthorized, (int)HttpStatusCode.Unauthorized);
    }

    public ServiceResponse<List<EvaluationAggregateDataContainer>> GetEvaluationAggregateData(EvaluationAggregationQuery query)
    {
        return AuthorizedAction(() =>
        {
            var response = _analyticsQueryHandlers.EvaluationAggregationQuery(query);
            return response;
        });
    }

    public ServiceResponse<List<SurveyAggregateDataContainer>> GetSurveyAggregateData(SurveyAggregationQuery query)
    {
        return AuthorizedAction(() =>
        {
            var response = _analyticsQueryHandlers.SurveyAggregatesQuery(query);
            return response;
        });
    }

    public ServiceResponse<List<AnalyticsConversationWithoutAttributes>> GetConversationDetails(ConversationQuery query)
    {
        return AuthorizedAction(() =>
        {
            var q = new ConversationQuery();
            var response = _analyticsQueryHandlers.ConversationDetailQuery(q);
            return response;
        });
    }
    
    /// <summary>
    /// /// Gets an authorization token before making calls.
    /// </summary>
    private bool Authorized()
    {
        if (_isAuthorized) return true;
        
        var authorizeResponse = AuthorizeService.Authorize(
            clientId: "6cad8911-28ca-40ee-97f5-01136dba9087",
            clientSecret: "44hAG2qlkWCCfUVHU7xnZgL323OyaQ7KKIi297s25eY",
            cloudRegion: PureCloudRegionHosts.eu_west_2);

        _isAuthorized = authorizeResponse is { Success: true, Data: true };
        return _isAuthorized;
    }
}
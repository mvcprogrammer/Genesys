using GenesysCloud.Queries.Reports.EvaluationReport;
using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Static;
using PureCloudPlatform.Client.V2.Client;

namespace GenesysCloud.Services.PureCloud.Fundamental;

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

    public ServiceResponse<List<EvaluationAggregateDataContainer>> GetEvaluationAggregateData(MetricsInterval interval, 
        IReadOnlyCollection<string> queues, IReadOnlyCollection<string> divisions)
    {
        return AuthorizedAction(() =>
        {
            var queryBuilder = new GenesysEvaluationAggregateQuery(interval, queues, divisions);
            var query = queryBuilder.Build();

            var response = _analyticsQueryHandlers.EvaluationAggregationQuery(query);
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
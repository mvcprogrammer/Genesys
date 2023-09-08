using GenesysCloud.Queries.Users;
using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Static;
using PureCloudPlatform.Client.V2.Client;
using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;
using UserStatusAggregateQuery = GenesysCloud.Queries.Users.UserStatusAggregateQuery;

namespace GenesysCloud.Services.PureCloud.Fundamental;

/// <summary>
/// Users service handles calls to Genesys/Mock and is never called directly from outside this assembly.
/// When methods are given parameters, this class is responsible for creating the query from those parameters and calling IUserQueryHandler.
/// This fundamental class should only be called by derived services and authorizations should only be called at this level.
/// It's permissible to shape returned data into dictionaries, lookups, etc., or return data as received.
/// Responses should always as a ServiceResponse to bubble up handled exception messages and response ids.
/// </summary>
internal sealed class PureCloudUsersService : IUsersService
{
    private readonly IUsersQueryHandlers _usersQueryHandlers;
    private const string NotAuthorized = "Not Authorized";
    private bool _isAuthorized;

    public PureCloudUsersService(IUsersQueryHandlers usersQueryHandlers)
    {
        _usersQueryHandlers = usersQueryHandlers;
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
    
    /// <summary>
    /// Use this sparingly, it returns all users and can take awhile.
    /// This might be a good place for breaking the users into a couple of groups and calling with an async span
    /// </summary>
    public ServiceResponse<List<User>> GetUsers()
    {
        return AuthorizedAction(() =>
        {
            var userListResponse = _usersQueryHandlers.GetAllUsers();
            return userListResponse;
        });
    }

    /// <summary>
    /// /// Used to get dictionary for getting profile information by Genesys GUID id.
    /// Use a lot, response data should be cached when practical.
    /// </summary>
    public ServiceResponse<Dictionary<string, UserProfile>> GetAgentProfileLookup()
    {
        return AuthorizedAction(() =>
        {
            var userList = _usersQueryHandlers.GetAllUsers();

            if (userList.Success is false || userList.Data is null)
                return SystemResponse.FailureResponse<Dictionary<string, UserProfile>>(userList.ErrorMessage, userList.ErrorCode);

            var agentProfileLookup = userList.Data
                .Select(x => new { x.Id, x.Name, x.Email, x.Title })
                .ToDictionary(x => x.Id, x => new UserProfile { Email = x.Email, Name = x.Name, Title = x.Title });

            return SystemResponse.SuccessResponse(agentProfileLookup);
        });
    }
    
    /// <summary>
    /// Gets Aggregated user presence data.
    /// This method is useful for getting relevant data before getting detail info. 
    /// </summary>
    public ServiceResponse<List<UserAggregateDataContainer>> GetUserStatusAggregates(MetricsInterval interval, string[] userIds, string granularity)
    {
        return AuthorizedAction(() =>
        {
            var queryBuilder = new UserStatusAggregateQuery(interval, userIds, granularity);
            var query = queryBuilder.Build();

            return _usersQueryHandlers.GetUsersStatusAggregates(query);
        });
    }

    /// <summary>
    /// /// Gets detailed user presence information
    /// </summary>
    public ServiceResponse<List<AnalyticsUserDetail>> GetUsersStatusDetail(MetricsInterval interval, string[] userIds)
    {
        return AuthorizedAction(() =>
        {
            var queryBuilder = new UserStatusDetailsQuery(interval, userIds);
            var query = queryBuilder.Build();

            return _usersQueryHandlers.GetUsersStatusDetail(query);
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
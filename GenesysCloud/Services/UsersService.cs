using PureCloudPlatform.Client.V2.Client;
using GenesysCloud.Queries.Users;
using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;
using UserStatusAggregateQuery = GenesysCloud.Queries.Users.UserStatusAggregateQuery;

namespace GenesysCloud.Services;

public class UsersService : IUsersService
{
    private readonly IUsersQueryHandlers _usersQueryHandlers;
    private const string NotAuthorized = "Not Authorized";
    private bool _isAuthorized;

    public UsersService(IUsersQueryHandlers usersQueryHandlers)
    {
        _usersQueryHandlers = usersQueryHandlers;
    }
    
    private ServiceResponse<T> AuthorizedAction<T>(Func<ServiceResponse<T>> action)
    {
        return Authorized()
            ? action() 
            : SystemResponse.FailureResponse<T>(NotAuthorized, (int)HttpStatusCode.Unauthorized);
    }

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

    public ServiceResponse<List<AnalyticsUserDetail>> GetUsersStatusDetail(MetricsInterval interval, string[] userIds)
    {
        return AuthorizedAction(() =>
        {
            var queryBuilder = new UserStatusDetailsQuery(interval, userIds);
            var query = queryBuilder.Build();

            return _usersQueryHandlers.GetUsersStatusDetail(query);
        });
    }

    public ServiceResponse<List<UserAggregateDataContainer>> GetUserStatusAggregates(MetricsInterval interval, string[] userIds, string granularity)
    {
        return AuthorizedAction(() =>
        {
            var queryBuilder = new UserStatusAggregateQuery(interval, userIds, granularity);
            var query = queryBuilder.Build();

            return _usersQueryHandlers.GetUsersStatusAggregates(query);
        });
    }

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
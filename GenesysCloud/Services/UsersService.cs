using GenesysCloud.Queries.Users;
using GenesysCloud.QueryHandlers.Contracts;
using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;
using UserStatusAggregateQuery = GenesysCloud.Queries.Users.UserStatusAggregateQuery;

namespace GenesysCloud.Services;

public class UsersService
{
    private readonly IUsersQueryHandlers _usersQueryHandlers;

    public UsersService(IUsersQueryHandlers usersQueryHandlers)
    {
        _usersQueryHandlers = usersQueryHandlers;
    }

    public ServiceResponse<Dictionary<string, UserProfile>> GetAgentProfileLookup()
    {
        var userList = _usersQueryHandlers.GetAllUsers();

        if (userList.Success is false || userList.Data is null)
            return SystemResponse.FailureResponse<Dictionary<string, UserProfile>>(userList.ErrorMessage, userList.ErrorCode);
        
        var agentProfileLookup = userList.Data
            .Select(x => new { x.Id, x.Name, x.Email, x.Title })
            .ToDictionary(x => x.Id, x => new UserProfile{Email = x.Email, Name = x.Name, Title = x.Title});
        
        return SystemResponse.SuccessResponse(agentProfileLookup);
    }

    public ServiceResponse<List<AnalyticsUserDetail>> GetUsersStatusDetail(MetricsInterval interval, string[] userIds)
    {
        var queryBuilder = new UserStatusDetailsQuery(interval, userIds);
        var query = queryBuilder.Build();
        
        return _usersQueryHandlers.GetUsersStatusDetail(query);
    }

    public ServiceResponse<List<UserAggregateDataContainer>> GetUserStatusAggregates(MetricsInterval interval, string[] userIds, string granularity)
    {
        var queryBuilder = new UserStatusAggregateQuery(interval, userIds, granularity);
        var query = queryBuilder.Build();
        
        return _usersQueryHandlers.GetUsersStatusAggregates(query);
    }
}
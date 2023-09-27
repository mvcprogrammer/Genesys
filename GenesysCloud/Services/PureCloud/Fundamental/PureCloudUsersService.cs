using GenesysCloud.Queries.Users;
using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Static;
using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;
using UserStatusAggregateQuery = GenesysCloud.Queries.Users.UserStatusAggregateQuery;

namespace GenesysCloud.Services.PureCloud.Fundamental;

/// <summary>
/// Users service handles calls to Genesys/Mock and is never called directly from outside this assembly.
/// When methods are given parameters, this class is responsible for creating the query from those parameters and calling IUserQueryHandler.
/// This fundamental class should only be called by derived services and authorizations should only be called at this level.
/// It's permissible to shape returned data into dictionaries, lookups, etc., or return data as received.
/// Responses are always a ServiceResponse to bubble up handled exception messages and response ids.
/// </summary>
internal sealed class PureCloudUsersService : IUsersService
{
    private readonly IUsersQueryHandlers _usersQueryHandlers;

    public PureCloudUsersService(IUsersQueryHandlers usersQueryHandlers)
    {
        _usersQueryHandlers = usersQueryHandlers;
    }
    
    /// <summary>
    /// /// Used to get dictionary for getting profile information by Genesys GUID id.
    /// </summary>
    public Dictionary<string, UserProfile> GetAgentProfileLookup(IReadOnlyCollection<string> userIds)
    {
        return AuthorizedAction(() =>
        {
            var userList = _usersQueryHandlers.GetUsers(userIds);

            var agentProfileLookup = userList 
                .Select(x => new { x.Id, x.Name, x.Email, x.Title })
                .ToDictionary(x => x.Id, x => new UserProfile { Email = x.Email, Name = x.Name, Title = x.Title });

            return ServiceResponse.LogAndReturnResponse(agentProfileLookup);
        });
    }
    
    /// <summary>
    /// Gets Aggregated user presence data.
    /// This method is particularly useful for determining which users have presence data before requesting detail info. 
    /// </summary>
    public List<UserAggregateDataContainer> GetUserStatusAggregates(MetricsInterval interval, string[] userIds, string granularity)
    {
        return AuthorizedAction(() =>
        {
            var queryBuilder = new UserStatusAggregateQuery(interval, userIds, granularity);
            var query = queryBuilder.Build();
            var userStatusAggregates = _usersQueryHandlers.GetUsersStatusAggregates(query);
            return ServiceResponse.LogAndReturnResponse(userStatusAggregates);
        });
    }

    /// <summary>
    /// /// Gets detailed user presence information
    /// </summary>
    public List<AnalyticsUserDetail> GetUsersStatusDetail(MetricsInterval interval, string[] userIds)
    {
        return AuthorizedAction(() =>
        {
            var queryBuilder = new UserStatusDetailsQuery(interval, userIds);
            var query = queryBuilder.Build();
            var userStatusDetail = _usersQueryHandlers.GetUsersStatusDetail(query);
            return ServiceResponse.LogAndReturnResponse(userStatusDetail);
        });
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
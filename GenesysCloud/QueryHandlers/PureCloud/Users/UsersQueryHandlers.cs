namespace GenesysCloud.QueryHandlers.PureCloud.Users;

public class UsersQueryHandlers : IUsersQueryHandlers
{
    public GenesysServiceResponse<List<User>> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public GenesysServiceResponse<List<AnalyticsUserDetail>> GetUserDetails(UserDetailsQuery userDetailsQuery)
    {
        throw new NotImplementedException();
    }

    public UserAggregateQueryResponse GetUsersAggregates(UserAggregationQuery userAggregationQuery)
    {
        throw new NotImplementedException();
    }
}
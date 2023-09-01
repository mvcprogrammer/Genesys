namespace GenesysCloud.QueryHandlers;

public interface IUsersQueryHandlers
{
    public GenesysServiceResponse<List<User>>GetAllUsers();
    public GenesysServiceResponse<List<AnalyticsUserDetail>> GetUserDetails(UserDetailsQuery userDetailsQuery);
    public UserAggregateQueryResponse GetUsersAggregates(UserAggregationQuery userAggregationQuery);
}
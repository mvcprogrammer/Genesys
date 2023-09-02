namespace GenesysCloud.QueryHandlers.Contracts;

public interface IUsersQueryHandlers
{
    public GenesysServiceResponse<List<User>>GetAllUsers(int pageSize = 100, string state = "any");
    public GenesysServiceResponse<List<AnalyticsUserDetail>> GetUserDetails(UserDetailsQuery query);
    public GenesysServiceResponse<List<UserAggregateDataContainer>> GetUsersAggregates(UserAggregationQuery query);
}
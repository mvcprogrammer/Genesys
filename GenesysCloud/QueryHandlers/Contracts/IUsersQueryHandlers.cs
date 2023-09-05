namespace GenesysCloud.QueryHandlers.Contracts;

public interface IUsersQueryHandlers
{
    public ServiceResponse<List<User>>GetAllUsers(int pageSize = 100, string state = "any");
    public ServiceResponse<List<AnalyticsUserDetail>> GetUsersStatusDetail(UserDetailsQuery query);
    public ServiceResponse<List<UserAggregateDataContainer>> GetUsersStatusAggregates(UserAggregationQuery query);
}
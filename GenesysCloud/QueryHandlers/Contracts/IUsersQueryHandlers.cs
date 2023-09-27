namespace GenesysCloud.QueryHandlers.Contracts;

public interface IUsersQueryHandlers
{
    public IEnumerable<User> GetUsers(IReadOnlyCollection<string>userIds);
    public List<AnalyticsUserDetail> GetUsersStatusDetail(UserDetailsQuery query);
    public List<UserAggregateDataContainer> GetUsersStatusAggregates(UserAggregationQuery query);
}
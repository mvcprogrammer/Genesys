using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.QueryHandlers.Mock.DataGenerators;

namespace GenesysCloud.QueryHandlers.Mock;

public class MockUsersQueryHandlers : IUsersQueryHandlers
{
    public IEnumerable<User> GetUsers(IReadOnlyCollection<string> userIds)
    {
        throw new NotImplementedException();
    }

    public List<AnalyticsUserDetail> GetUsersStatusDetail(UserDetailsQuery query)
    {
        throw new NotImplementedException();
    }

    public List<UserAggregateDataContainer> GetUsersStatusAggregates(UserAggregationQuery query)
    {
        throw new NotImplementedException();
    }
}
using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.QueryHandlers.Mock.DataGenerators;

namespace GenesysCloud.QueryHandlers.Mock;

public class MockUsersQueryHandlers : IUsersQueryHandlers
{
    public ServiceResponse<List<User>> GetAllUsers(int pageSize = 100, string state = "any")
    {
        var userEntityListing = UserDataGenerator.GenerateUserEntityListing();
        return SystemResponse.SuccessResponse(userEntityListing.Entities);
    }

    public ServiceResponse<List<User>> GetUsers(IReadOnlyCollection<string> userIds)
    {
        return GetAllUsers();
    }

    public ServiceResponse<List<AnalyticsUserDetail>> GetUsersStatusDetail(UserDetailsQuery query)
    {
        throw new NotImplementedException();
    }

    public ServiceResponse<List<UserAggregateDataContainer>> GetUsersStatusAggregates(UserAggregationQuery query)
    {
        throw new NotImplementedException();
    }
}
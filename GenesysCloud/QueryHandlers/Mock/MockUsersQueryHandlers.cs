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

    public ServiceResponse<List<AnalyticsUserDetail>> GetUserDetails(UserDetailsQuery query)
    {
        throw new NotImplementedException();
    }

    public ServiceResponse<List<UserAggregateDataContainer>> GetUsersAggregates(UserAggregationQuery query)
    {
        throw new NotImplementedException();
    }
}
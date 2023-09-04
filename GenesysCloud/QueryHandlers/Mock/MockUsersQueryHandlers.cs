using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.QueryHandlers.Mock.DataGenerators;

namespace GenesysCloud.QueryHandlers.Mock;

public class UsersQueryHandlers : IUsersQueryHandlers
{
    public GenesysServiceResponse<List<User>> GetAllUsers(int pageSize = 100, string state = "any")
    {
        var userEntityListing = UserDataGenerator.GenerateUserEntityListing();
        return GenesysResponse.SuccessResponse(userEntityListing.Entities);
    }

    public GenesysServiceResponse<List<AnalyticsUserDetail>> GetUserDetails(UserDetailsQuery query)
    {
        throw new NotImplementedException();
    }

    public GenesysServiceResponse<List<UserAggregateDataContainer>> GetUsersAggregates(UserAggregationQuery query)
    {
        throw new NotImplementedException();
    }
}
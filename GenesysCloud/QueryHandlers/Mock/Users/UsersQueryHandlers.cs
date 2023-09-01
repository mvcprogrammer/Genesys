using GenesysCloud.DTO.Response;

namespace GenesysCloud.QueryHandlers.Mock.Users;

public class UsersQueryHandlers : IUsersQueryHandlers
{
    public GenesysServiceResponse<List<User>> GetAllUsers()
    {
        return GenesysResponse.SuccessResponse(DataGenerator.GenerateUserEntityListing().Entities);
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
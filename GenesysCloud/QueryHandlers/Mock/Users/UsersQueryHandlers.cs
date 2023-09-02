namespace GenesysCloud.QueryHandlers.Mock.Users;

public class UsersQueryHandlers : IUsersQueryHandlers
{
    public GenesysServiceResponse<List<User>> GetAllUsers(int pageSize = 100, string state = "any")
    {
        return GenesysResponse.SuccessResponse(DataGenerator.GenerateUserEntityListing().Entities);
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
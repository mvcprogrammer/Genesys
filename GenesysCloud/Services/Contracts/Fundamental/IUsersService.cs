using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;

namespace GenesysCloud.Services.Contracts.Fundamental;

public interface IUsersService
{
    ServiceResponse<List<User>> GetUsers();
    public ServiceResponse<Dictionary<string, UserProfile>> GetAgentProfileLookup();
    ServiceResponse<List<AnalyticsUserDetail>> GetUsersStatusDetail(MetricsInterval interval, string[] userIds);
    ServiceResponse<List<UserAggregateDataContainer>> GetUserStatusAggregates(MetricsInterval interval, string[] userIds, string granularity);
}
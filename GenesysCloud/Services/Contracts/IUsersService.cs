using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;

namespace GenesysCloud.Services.Contracts;

public interface IUsersService
{
    ServiceResponse<Dictionary<string, UserProfile>> GetAgentProfileLookup();
    ServiceResponse<List<AnalyticsUserDetail>> GetUsersStatusDetail(MetricsInterval interval, string[] userIds);
    ServiceResponse<List<UserAggregateDataContainer>> GetUserStatusAggregates(MetricsInterval interval, string[] userIds, string granularity);
}
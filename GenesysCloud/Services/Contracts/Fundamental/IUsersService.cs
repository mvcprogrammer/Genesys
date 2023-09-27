using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;

namespace GenesysCloud.Services.Contracts.Fundamental;

public interface IUsersService
{
    public Dictionary<string, UserProfile> GetAgentProfileLookup(IReadOnlyCollection<string>userIds);
    List<AnalyticsUserDetail> GetUsersStatusDetail(MetricsInterval interval, string[] userIds);
    List<UserAggregateDataContainer> GetUserStatusAggregates(MetricsInterval interval, string[] userIds, string granularity);
}
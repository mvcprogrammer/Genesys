using GenesysCloud.Services.Contracts.Fundamental;

namespace Genesphere.Controllers;

/// <summary>
/// Information about Genesys agents and other users
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    
    /// <summary>
    /// <param name="usersService">DI for GenesysCloud.IUsersService</param>
    /// </summary>
    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    /// <summary>
    /// Retrieves user status (presence) details for the specified interval and user IDs.
    /// </summary>
    /// <param name="startTime">Report start date/time in UTC. ex: 2023-09-05T00:00:00Z</param>
    /// <param name="endTime">Report end date/time in UTC. ex: 2023-09-06T00:00:00Z</param>
    /// <param name="userIds">The Genesys user IDs ex: 6caf7a96-fcf2-498b-bb40-d678fb90f61e</param>
    /// <returns>A list of AnalyticsUserData.</returns>
    [HttpPost("UsersStatusDetail")]
    public IActionResult GetUsersStatusDetail([FromQuery] DateTime startTime, [FromQuery] DateTime endTime, [FromQuery] string[] userIds)
    {
        var interval = new MetricsInterval { StartTimeUtc = startTime, EndTimeUtc = endTime };
        var response = _usersService.GetUsersStatusDetail(interval, userIds);
        return Ok(response.JsonSerializeToString(response.GetType()));
    }

    /// <summary>
    /// Retrieves user status aggregates for the specified interval, user IDs, and granularity.
    /// </summary>
    /// <param name="startTime">Report start date/time in UTC. ex. (2023-09-05T00:00:00Z)</param>
    /// <param name="endTime">Report end date/time in UTC. (ex. 2023-09-06T00:00:00Z)</param>
    /// <param name="userIds">The Genesys user IDs (ex. 6caf7a96-fcf2-498b-bb40-d678fb90f61e)</param>
    /// <param name="granularity">The period of time to aggregate by. (ex. PT30M)</param>
    /// <returns>A list of Aggregated User Status.</returns>
    [HttpPost("UserStatusAggregates")]
    public IActionResult GetUserStatusAggregates(
        [FromQuery] DateTime startTime, 
        [FromQuery] DateTime endTime, 
        [FromQuery] string[] userIds,
        [FromQuery] string granularity = Constants.TwentyFourHourInterval)
    {
        var interval = new MetricsInterval { StartTimeUtc = startTime, EndTimeUtc = endTime };
        var userStatusAggregates = _usersService.GetUserStatusAggregates(interval, userIds, granularity);
        return Ok(userStatusAggregates.JsonSerializeToString(userStatusAggregates.GetType()));
    }
}

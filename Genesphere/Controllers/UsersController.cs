using System.ComponentModel;
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
    /// Retrieves agent profile lookup.
    /// </summary>
    /// <returns>A SystemResponse with List of User data.</returns>
    [HttpGet("Agents")]
    public IActionResult GetAgents()
    {
        var response = _usersService.GetUsers();
        return GenerateResponse(response);
    }

    /// <summary>
    /// Retrieves user status (presence) details for the specified interval and user IDs.
    /// </summary>
    /// <param name="startTime">Report start date/time in UTC. ex: 2023-09-05T00:00:00Z</param>
    /// <param name="endTime">Report end date/time in UTC. ex: 2023-09-06T00:00:00Z</param>
    /// <param name="userIds">The Genesys user IDs ex: 6caf7a96-fcf2-498b-bb40-d678fb90f61e</param>
    /// <returns>A SystemResponse with List of AnalyticsUserData.</returns>
    [HttpPost("UsersStatusDetail")]
    public IActionResult GetUsersStatusDetail([FromQuery] DateTime startTime, [FromQuery] DateTime endTime, [FromQuery] string[] userIds)
    {
        var interval = new MetricsInterval { StartTimeUtc = startTime, EndTimeUtc = endTime };
        var response = _usersService.GetUsersStatusDetail(interval, userIds);
        return GenerateResponse(response);
    }

    /// <summary>
    /// Retrieves user status aggregates for the specified interval, user IDs, and granularity.
    /// </summary>
    /// <param name="startTime">Report start date/time in UTC. ex. (2023-09-05T00:00:00Z)</param>
    /// <param name="endTime">Report end date/time in UTC. (ex. 2023-09-06T00:00:00Z)</param>
    /// <param name="userIds">The Genesys user IDs (ex. 6caf7a96-fcf2-498b-bb40-d678fb90f61e)</param>
    /// <param name="granularity">The period of time to aggregate by. (ex. PT30M)</param>
    /// <returns>A SystemResponse with a List of Aggregated User Status.</returns>
    [HttpPost("UserStatusAggregates")]
    public IActionResult GetUserStatusAggregates(
        [FromQuery] DateTime startTime, 
        [FromQuery] DateTime endTime, 
        [FromQuery] string[] userIds,
        [FromQuery] string granularity = Constants.TwentyFourHourInterval)
    {
        var interval = new MetricsInterval { StartTimeUtc = startTime, EndTimeUtc = endTime };
        var response = _usersService.GetUserStatusAggregates(interval, userIds, granularity);
        return GenerateResponse(response);
    }

    [Description("Generic method to handle ServiceReponse.Success")]
    private IActionResult GenerateResponse<T>(ServiceResponse<T> response)
    {
        if(response.Success)
            return Ok(response);

        return BadRequest(new { error = $"Message:{response.ErrorMessage},Link:https://service.mvcprogrammer.com/new/{response.Id}"});
    }
}

using System.ComponentModel;

namespace Genesphere.Controllers;

/// <summary>
/// Retrieves agent profile lookup.
/// </summary>
/// <returns>A response with agent profile data.</returns>
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
    /// <returns>A response with agent profile data.</returns>
    [HttpGet("AgentProfileLookup")]
    public IActionResult GetAgentProfileLookup()
    {
        var response = _usersService.GetAgentProfileLookup();
        return GenerateResponse(response);
    }

    /// <summary>
    /// Retrieves user status details (presence) for the specified interval and user IDs.
    /// </summary>
    /// <param name="startTime">Report start date/time in UTC. ex. (2023-09-05T00:00:00Z)</param>
    /// <param name="endTime">Report end date/time in UTC. (ex. 2023-09-06T00:00:00Z)</param>
    /// <param name="userIds">The Genesys user IDs (ex. 6caf7a96-fcf2-498b-bb40-d678fb90f61e)</param>
    /// <returns>A response with user status details.</returns>
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
    /// <returns>A response with user status aggregates.</returns>
    [HttpGet("UserStatusAggregates")]
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
            return Ok(response.Data);

        return BadRequest(new { error = response.ErrorMessage });
    }
}

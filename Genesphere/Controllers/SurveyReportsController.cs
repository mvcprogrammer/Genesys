using GenesysCloud.Services.Contracts.Derived;

namespace Genesphere.Controllers;

/// <summary>
/// Customer Service Survey Reports
/// </summary>
[ApiController]
[Route("[controller]")]
public class SurveyReportsController : ControllerBase
{
    private readonly ISurveyReportDataService _surveyReportDataService;
    private readonly string[] _divisions = new[] { "d176b581-76c3-4d66-9686-7e2233e8eeb5" };
    
    /// <summary>
    /// Creates on demand survey report data for Customer Service
    /// </summary>
    /// <param name="surveyReportDataService">Dependency Injection for survey report data service.</param>
    public SurveyReportsController(ISurveyReportDataService surveyReportDataService)
    {
        _surveyReportDataService = surveyReportDataService;
    }

    /// <summary>
    /// Retrieves report data for surveys completed by customer for an agent interaction.
    /// </summary>
    /// <param name="startTime">Minimum interaction start date/time. ex: 2023-09-05T00:00:00Z</param>
    /// <param name="endTime">Maximum interaction start date/time. ex: 2023-09-06T00:00:00Z</param>
    [HttpPost("Surveys")]
    public IActionResult GetSurveyData([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var response = _surveyReportDataService.GetSurveyData(startTime, endTime, _divisions, Array.Empty<string>());
        return Ok(response.JsonSerializeToString(response.GetType()));
    }
}
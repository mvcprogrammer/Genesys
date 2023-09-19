using System.ComponentModel;
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
    /// Creates on demand evaluation report data for Customer Service
    /// </summary>
    /// <param name="surveyReportDataService">Dependency Injection for survey report data service.</param>
    public SurveyReportsController(ISurveyReportDataService surveyReportDataService)
    {
        _surveyReportDataService = surveyReportDataService;
    }

    /// <summary>
    /// Retrieves by date range evaluation report data completed by an supervisor for an agent
    /// </summary>
    /// <param name="startTime">Date range min re: evaluation creation date. ex: 2023-09-05T00:00:00Z</param>
    /// <param name="endTime">Date range max re: evaluation creation date. ex: 2023-09-06T00:00:00Z</param>
    [HttpPost("Surveys")]
    public IActionResult GetSurveyData([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var response = _surveyReportDataService.GetSurveyData(startTime, endTime, _divisions, Array.Empty<string>());
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
using System.ComponentModel;
using GenesysCloud.Services.Contracts.Derived;

namespace Genesphere.Controllers;

/// <summary>
/// Customer Service Evaluation Reports
/// </summary>
[ApiController]
[Route("[controller]")]
public sealed class EvaluationReportsController : ControllerBase
{
    private readonly IEvaluationReportDataService _evaluationReportDataService;
    private readonly string[] _divisions = new[] { "d176b581-76c3-4d66-9686-7e2233e8eeb5" };
    
    /// <summary>
    /// Creates on demand evaluation report data for Customer Service
    /// </summary>
    /// <param name="evaluationReportDataService">Dependency Injection for evaluation report data service.</param>
    public EvaluationReportsController(IEvaluationReportDataService evaluationReportDataService)
    {
        _evaluationReportDataService = evaluationReportDataService;
    }

    /// <summary>
    /// Retrieves by date range evaluation report data completed by an supervisor for an agent
    /// </summary>
    /// <param name="startTime">Date range min re: evaluation creation date. ex: 2023-09-05T00:00:00Z</param>
    /// <param name="endTime">Date range max re: evaluation creation date. ex: 2023-09-06T00:00:00Z</param>
    [HttpPost("Evaluations")]
    public IActionResult GetEvaluationData([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var response = _evaluationReportDataService.GetEvaluationRecords(startTime, endTime, _divisions, Array.Empty<string>());
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
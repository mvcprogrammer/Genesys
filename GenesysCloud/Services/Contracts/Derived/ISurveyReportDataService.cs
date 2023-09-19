using GenesysCloud.DTO.Response.Reports.Survey;

namespace GenesysCloud.Services.Contracts.Derived;

public interface ISurveyReportDataService
{
    public ServiceResponse<List<SurveyRecord>> GetSurveyData(DateTime startTime, DateTime endTime,
        IReadOnlyCollection<string> divisions, IReadOnlyCollection<string> queues);
}
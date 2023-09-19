using GenesysCloud.DTO.Response.Reports;
using GenesysCloud.DTO.Response.Reports.Evaluation;

namespace GenesysCloud.Services.Contracts.Derived;

public interface IEvaluationReportDataService
{
    public ServiceResponse<List<EvaluationRecord>> GetEvaluationRecords(DateTime startTime, DateTime endTime,
        IReadOnlyCollection<string> divisions, IReadOnlyCollection<string> queues);
}
using GenesysCloud.DTO.Response.Reports;

namespace GenesysCloud.Services.Contracts.Derived;

public interface IEvaluationReportDataService
{
    public ServiceResponse<List<EvaluationRecord>> GetEvaluationRecords(DateTime startTime, DateTime endTime,
        IReadOnlyCollection<string> divisions, IReadOnlyCollection<string> queues);
}
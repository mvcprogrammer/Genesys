using GenesysCloud.DTO.Response.Reports;
using GenesysCloud.Mappers;
using GenesysCloud.Services.Contracts.Derived;
using GenesysCloud.Services.Contracts.Fundamental;

namespace GenesysCloud.Services.PureCloud.Derived;

public sealed class PureCloudReportDataService : IReportDataService
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IQualityService _qualityService;
    private readonly ISpeechTextAnalyticsService _speechTextAnalyticsService;
    
    public PureCloudReportDataService(
        IAnalyticsService analyticsService, 
        IQualityService qualityService,
        ISpeechTextAnalyticsService speechTextAnalyticsService)
    {
        _analyticsService = analyticsService;
        _qualityService = qualityService;
        _speechTextAnalyticsService = speechTextAnalyticsService;
    }

    public ServiceResponse<List<EvaluationRecord>> GetEvaluationRecords(DateTime startTime, DateTime endTime,
        IReadOnlyCollection<string> divisions, IReadOnlyCollection<string> queues)
    {
        var interval = new MetricsInterval(startTime, endTime);
        
        var response = _analyticsService.GetEvaluationAggregateData(interval, queues, divisions);
        var evaluationAggregateDataContainers = response.Data ?? new List<EvaluationAggregateDataContainer>();
        
        var evaluationRecords = new List<EvaluationRecord>();

        foreach (var evaluationAggregateDataContainer in evaluationAggregateDataContainers)
        {
            var evaluationAggregateDataGroupDictionary = evaluationAggregateDataContainer.Group;
            var evaluationAggregateStatisticalResponses =
                evaluationAggregateDataContainer.Data ?? new List<StatisticalResponse>();

            foreach (var statisticalResponse in evaluationAggregateStatisticalResponses)
            {
                var metricDictionary = statisticalResponse.Metrics.ToDictionary(x => x.Metric, x => x.Stats);

                if (metricDictionary.TryGetValue("nEvaluations", out var nEvaluations) is false) continue;
                if (nEvaluations.Count is null or 0) continue;

                if (evaluationAggregateDataGroupDictionary.TryGetValue("conversationId", out var conversationId) is false)
                    continue;

                if (evaluationAggregateDataGroupDictionary.TryGetValue("evaluationId", out var evaluationId) is false)
                    continue;

                var evaluationResponse = _qualityService.GetConversationEvaluationDetail(conversationId, evaluationId, expand: "agent,evaluator,evaluationForm");
                if (evaluationResponse.Success is false || evaluationResponse.Data is null)
                    return SystemResponse.FailureResponse<List<EvaluationRecord>>(response.ErrorMessage, response.ErrorCode);
                
                var speechTextAnalyticsResponse = _speechTextAnalyticsService.GetConversationAnalytics(conversationId);
                if(speechTextAnalyticsResponse.Success is false)
                    return SystemResponse.FailureResponse<List<EvaluationRecord>>(response.ErrorMessage, response.ErrorCode);
                
                var mapper = new MapperEvaluationResponseToEvaluationRecords(interval, evaluationResponse.Data, speechTextAnalyticsResponse.Data ?? new ConversationMetrics());
                var evaluationRecordResponse = mapper.Map();
                
                if (evaluationRecordResponse.Success is false || evaluationRecordResponse.Data is null)
                    continue;

                evaluationRecords.Add(evaluationRecordResponse.Data);
            }
        }

        return SystemResponse.SuccessResponse(evaluationRecords);
    }
}
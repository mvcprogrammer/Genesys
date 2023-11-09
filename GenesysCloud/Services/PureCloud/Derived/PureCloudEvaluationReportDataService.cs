using GenesysCloud.DTO.Response.Reports.Evaluation;
using GenesysCloud.Mappers.Reports.Evaluations;
using GenesysCloud.Queries.Reports.EvaluationReport;
using GenesysCloud.Services.Contracts.Derived;
using GenesysCloud.Services.Contracts.Fundamental;

namespace GenesysCloud.Services.PureCloud.Derived;

/// <summary>
/// Evaluation Report Data Service may only invoke calls to fundamental services and cannot call Genesys API directly.
/// The service can call multiple fundamental services and will always return DTO data; any call to this service must not be dependent on V2.Client.Models
/// The query needed by the fundamental service must be created in the method unless service doesn't have a query param, then pass params as needed.
/// Always use named parameters on API calls to Genesys
/// Responses are always a ServiceResponse to bubble up handled exception messages, errors and response ids.
/// </summary>
public sealed class PureCloudEvaluationReportDataService : IEvaluationReportDataService
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IQualityService _qualityService;
    private readonly ISpeechTextAnalyticsService _speechTextAnalyticsService;
    private readonly IUsersService _usersService;
    
    private const string NEvaluationsKey = "nEvaluations";
    private const string ConversationIdKey = "conversationId";
    private const string EvaluationIdKey = "evaluationId";
    
    public PureCloudEvaluationReportDataService(
        IAnalyticsService analyticsService, 
        IQualityService qualityService,
        ISpeechTextAnalyticsService speechTextAnalyticsService,
        IUsersService usersService)
    {
        _analyticsService = analyticsService;
        _qualityService = qualityService;
        _speechTextAnalyticsService = speechTextAnalyticsService;
        _usersService = usersService;
    }

    /// <summary>
    /// <param name="startTime">Filters lower bound date when evaluation was created for agent by evaluator (usually previous day midnight UTC)</param>
    /// <param name="endTime">Filters upper bound date when evaluation was created for agent by evaluator (usually current day midnight UTC)</param>
    /// <param name="divisions">If divisions are supplied, will filter on that division. May be combined with queues. No nulls, use empty array if not used.</param>
    /// <param name="queues">If queues are supplied, will filter on that queue. May be combined with divisions. No nulls, use empty array if not used</param>
    /// <returns>A list of evaluation records consumed by evaluation report with agent scores, comments, questions and answers.</returns>
    /// </summary>
    public List<EvaluationRecord> GetEvaluationRecords(DateTime startTime, DateTime endTime, IReadOnlyCollection<string> divisions, IReadOnlyCollection<string> queues)
    {
        var interval = new MetricsInterval(startTime, endTime);
        
        var queryBuilder = new GenesysEvaluationAggregateQuery(interval, queues, divisions);
        var query = queryBuilder.Build();
        var evaluationAggregateData = _analyticsService.GetEvaluationAggregateData(query);
        
        // get the user id of all agents with an evaluation(s)
        var userIds = evaluationAggregateData
            .Where(data => data.Group.ContainsKey("userId"))
            .Select(data => data.Group["userId"])
            .Distinct()
            .ToArray();

        // this lookup will only include users who are in the "divisions" collection from the parameter
        var userLookup = _usersService.GetAgentProfileLookup(userIds)
            .Where(x => divisions.Contains(x.Value.DivisionId))
            .Select(x => x.Key)
            .ToHashSet();
        
        var evaluationRecords = new List<EvaluationRecord>();

        foreach (var evaluationAggregateDataContainer in evaluationAggregateData)
        {
            var evaluationAggregateDataGroupDictionary = evaluationAggregateDataContainer.Group;
            var evaluationAggregateStatisticalResponses = evaluationAggregateDataContainer.Data ?? new List<StatisticalResponse>();

            var user = evaluationAggregateDataContainer.Group.SingleOrDefault(x => x.Key.Equals("userId")).Value ?? "";
            
            // don't process users not in the "divisions" parameter
            if(userLookup.Contains(user) is false) continue;

            foreach (var metricDictionary in evaluationAggregateStatisticalResponses.Select(statisticalResponse => 
                         statisticalResponse.Metrics.ToDictionary(x => x.Metric, x => x.Stats)))
            {
                if (metricDictionary.TryGetValue(NEvaluationsKey, out var nEvaluations) is false)
                    continue;
                
                if (nEvaluations.Count is null or 0) 
                    continue;

                if (evaluationAggregateDataGroupDictionary.TryGetValue(ConversationIdKey, out var conversationId) is false)
                    continue;

                if (evaluationAggregateDataGroupDictionary.TryGetValue(EvaluationIdKey, out var evaluationId) is false)
                    continue;
                
                var evaluationResponse = _qualityService.GetConversationEvaluationDetail(conversationId, evaluationId, expand: "agent,evaluator,evaluationForm");
                
                var speechTextAnalytics = _speechTextAnalyticsService.GetConversationAnalytics(conversationId);
                
                var mapper = new MapperEvaluationResponseToEvaluationRecords(interval, evaluationResponse, speechTextAnalytics);
                var evaluationRecord = mapper.Map();
                
                evaluationRecords.Add(evaluationRecord);
            }
        }

        return ServiceResponse.LogAndReturnResponse(evaluationRecords, stackTraceIndex: 3);
    }
}
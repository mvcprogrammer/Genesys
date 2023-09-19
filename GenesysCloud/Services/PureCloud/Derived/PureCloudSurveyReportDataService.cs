using GenesysCloud.DTO.Response.Reports.Survey;
using GenesysCloud.Mappers.Reports.Surveys;
using GenesysCloud.Queries.Reports.SurveyReport;
using GenesysCloud.Services.Contracts.Derived;
using GenesysCloud.Services.Contracts.Fundamental;

namespace GenesysCloud.Services.PureCloud.Derived;

/// <summary>
/// Survey Report Data Service may only invoke calls to fundamental services and cannot call Genesys API directly.
/// The service can call multiple fundamental services and will always return DTO data; any call to this service must not be dependent on V2.Client.Models
/// The query needed by the fundamental service must be created in the method unless service doesn't have a query param, then pass params as needed.
/// Always use named parameters on API calls to Genesys
/// Responses are always a ServiceResponse to bubble up handled exception messages, errors and response ids.
/// </summary>
public sealed class PureCloudSurveyReportDataService : ISurveyReportDataService
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IQualityService _qualityService;
    private readonly IUsersService _usersService;
    
    private const string NEvaluationsKey = "nSurveys";
    private const string ConversationIdKey = "conversationId";
    private const string UserIdKey = "userId";
    private const string EvaluationIdKey = "surveyId";

    public PureCloudSurveyReportDataService(IAnalyticsService analyticsService, IQualityService qualityService, IUsersService usersService)
    {
        _analyticsService = analyticsService;
        _qualityService = qualityService;
        _usersService = usersService;
    }

    public ServiceResponse<List<SurveyRecord>> GetSurveyData(DateTime startTime, DateTime endTime, IReadOnlyCollection<string> divisions, IReadOnlyCollection<string> queues)
    {
        var interval = new MetricsInterval(startTime, endTime);

        var queryBuilder = new GenesysSurveyAggregateQueryByDivisionByQueue(interval, divisions, queues);
        var query = queryBuilder.Build();

        var response = _analyticsService.GetSurveyAggregateData(query);
        var surveyAggregateDataContainers = response.Data ?? new List<SurveyAggregateDataContainer>();

        var userList = surveyAggregateDataContainers.SelectMany(dataContainers => 
            dataContainers.Group.Where(group => group.Key.Equals("userId")))
            .Select(x => x.Value)
            .Distinct()
            .ToList();

        var userLookupResponse = _usersService.GetAgentProfileLookup(userList);
        if (userLookupResponse.Success is false || userLookupResponse.Data is null)
            return SystemResponse.FailureResponse<List<SurveyRecord>>(response.ErrorMessage);
        var userLookup = userLookupResponse.Data;

        var surveyRecords = new List<SurveyRecord>();

        foreach (var surveyAggregateDataContainer in surveyAggregateDataContainers)
        {
            var surveyAggregateDataGroupDictionary = surveyAggregateDataContainer.Group;

            if (surveyAggregateDataGroupDictionary.TryGetValue(ConversationIdKey, out var conversationId) is false)
                continue;

            var conversationQualityResponses = _qualityService.GetConversationSurveyDetail(conversationId);
            if (conversationQualityResponses.Success is false || conversationQualityResponses.Data is null)
                return SystemResponse.FailureResponse<List<SurveyRecord>>(response.ErrorMessage, response.ErrorCode);

            if (surveyAggregateDataGroupDictionary.TryGetValue(UserIdKey, out var userId) is false) 
                continue;

            if (userLookup.TryGetValue(userId, out var profile) is false) 
                continue;
            
            var mapper = new MapperSurveyResponseToSurveyRecord(conversationQualityResponses.Data, profile, conversationId);
            var surveyRecordsResponse = mapper.Map();
            if (surveyRecordsResponse.Success is false || surveyRecordsResponse.Data is null) continue;
            surveyRecords.AddRange(surveyRecordsResponse.Data);
        }

        return SystemResponse.SuccessResponse(surveyRecords);
    }
}
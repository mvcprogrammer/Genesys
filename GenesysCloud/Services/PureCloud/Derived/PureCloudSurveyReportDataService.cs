using System.ComponentModel.DataAnnotations;
using GenesysCloud.DTO.Response.Reports.Survey;
using GenesysCloud.Mappers.Reports.Surveys;
using GenesysCloud.Queries.Reports.SurveyReport;
using GenesysCloud.Services.Contracts.Derived;
using GenesysCloud.Services.Contracts.Fundamental;
using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;

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
    private readonly IRoutingService _routingService;
    
    private const string ConversationIdKey = "conversationId";

    public PureCloudSurveyReportDataService(
        IAnalyticsService analyticsService, 
        IQualityService qualityService, 
        IUsersService usersService,
        IRoutingService routingService)
    {
        _analyticsService = analyticsService;
        _qualityService = qualityService;
        _usersService = usersService;
        _routingService = routingService;
    }

    /// <summary>
    /// This method returns all the data needed for a survey report.
    /// It first gets a survey aggregate for the specified interval to get the agents who had a survey(s) done on an interaction in which they participated.
    /// The aggregate is used to fetch the user profiles so this can be joined in the mapping method.
    /// The mapper creates the DTO. Only genesys data goes in, only Arise data comes out.
    /// Responses are always a ServiceResponse to bubble up handled exception messages, errors and response ids.
    /// <param name="startTime">Minimum value for interaction start date/time</param>
    /// <param name="endTime">Maximum value for interaction start date/time</param>
    /// <param name="divisions">Will filter to only users within a division, not required but must be empty collection if not used.</param>
    /// <param name="queues">Will filter to only users in listed queues, not required but mut be empty collection if not used.</param>
    /// </summary>
    public List<SurveyRecord> GetSurveyData(DateTime startTime, DateTime endTime,
        IReadOnlyCollection<string> divisions, IReadOnlyCollection<string> queues)
    {
        var interval = new MetricsInterval(startTime, endTime);

        var queueProfiles = _routingService.GetQueueProfileLookup(divisions.ToList());
        var surveyAggregateData = GetSurveyAggregateData(interval, divisions, queues);
        var userProfiles = GetAgentsWithSurveys(surveyAggregateData);
        var conversationDetailDictionary = GetConversationDetails(surveyAggregateData, interval);
        
        var surveyRecords = new List<SurveyRecord>();

        foreach (var surveyAggregateDataContainer in surveyAggregateData)
        {
            var surveyAggregateDataGroupDictionary = surveyAggregateDataContainer.Group;

            if (surveyAggregateDataGroupDictionary.TryGetValue(ConversationIdKey, out var conversationId) is false)
                throw new ValidationException("Failed to find interaction key.");

            var surveyDetail = _qualityService.GetConversationSurveyDetail(conversationId);
            
            var mapper = new MapperSurveyResponseToSurveyRecord(surveyAggregateDataContainer, surveyDetail, userProfiles, queueProfiles, conversationDetailDictionary);
            var surveyRecord = mapper.Map();
            surveyRecords.AddRange(surveyRecord);
        }
        
        return ServiceResponse.LogAndReturnResponse(surveyRecords, stackTraceIndex: 3);
    }

    private List<SurveyAggregateDataContainer> GetSurveyAggregateData(MetricsInterval interval, IReadOnlyCollection<string> divisions, IReadOnlyCollection<string> queues)
    {
        var surveyQueryBuilder = new GenesysSurveyAggregateQuery(interval, divisions, queues);
        var surveyQuery = surveyQueryBuilder.Build();
        var surveyAggregateData = _analyticsService.GetSurveyAggregateData(surveyQuery);
        return ServiceResponse.LogAndReturnResponse(surveyAggregateData, stackTraceIndex: 3);
    }

    private Dictionary<string, UserProfile> GetAgentsWithSurveys(IEnumerable<SurveyAggregateDataContainer> surveyAggregateData)
    {
        var userList = surveyAggregateData.SelectMany(dataContainers => 
                dataContainers.Group.Where(group => group.Key.Equals("userId")))
            .Select(x => x.Value)
            .Distinct()
            .ToList();

        var agentProfileLookup = _usersService.GetAgentProfileLookup(userList);
        return ServiceResponse.LogAndReturnResponse(agentProfileLookup, stackTraceIndex: 3);
    }

    private Dictionary<string, AnalyticsConversationWithoutAttributes> GetConversationDetails(IEnumerable<SurveyAggregateDataContainer> surveyAggregateData, MetricsInterval interval)
    {
        // con only take 200 conversation ids at a time
        const int take = 200;
        
        //surveys don't always happen on day of conversation, so search back 30 days.
        var thirtyDayInterval = new MetricsInterval { EndTimeUtc = interval.EndTimeUtc, StartTimeUtc = interval.StartTimeUtc - new TimeSpan(30, 0, 0, 0) };
        
        var conversationList = surveyAggregateData.SelectMany(dataContainers => 
                dataContainers.Group.Where(group => group.Key.Equals("conversationId")))
            .Select(x => x.Value)
            .Distinct()
            .ToArray();

        var conversationDetails = new List<AnalyticsConversationWithoutAttributes>();
        
        for (var skip = 0; skip < conversationList.Length; skip += take)
        {
            var conversationIds = conversationList.Skip(skip).Take(take).ToArray();
            var conversationQueryBuilder = new GenesysConversationDetailQuery(thirtyDayInterval, conversationIds);
            var conversationQuery = conversationQueryBuilder.Build();
            var conversationsDetail = _analyticsService.GetConversationDetails(conversationQuery);
            conversationDetails.AddRange(conversationsDetail);
        }
        
        var conversationDetailDictionary = conversationDetails.ToDictionary(x =>
            x.ConversationId, x => x);

        return ServiceResponse.LogAndReturnResponse(conversationDetailDictionary, stackTraceIndex: 3);
    }
}
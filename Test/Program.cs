using System.Diagnostics;
using GenesysCloud.Helpers;
using GenesysCloud.QueryHandlers.PureCloud;
using GenesysCloud.Services.PureCloud.Derived;
using GenesysCloud.Services.PureCloud.Fundamental;

var interval = new MetricsInterval
{
    StartTimeUtc = new DateTime(2023, 09, 26, 0, 0, 0).ToUniversalTime(),
    EndTimeUtc = new DateTime(2023, 09, 27, 0, 0, 0).ToUniversalTime()
};

#region FakeDI

var analyticsQueryHandlers = new PureCloudAnalyticsQueryHandlers();
var analyticService = new PureCloudAnalyticsService(analyticsQueryHandlers);

var qualityQueryHandlers = new PureCloudQualityQueryHandlers();
var qualityService = new PureCloudQualityService(qualityQueryHandlers);

var usersServiceHandlers = new PureCloudUsersQueryHandlers();
var usersService = new PureCloudUsersService(usersServiceHandlers);

var speechTextAnalyticsHandler = new PureCloudSpeechTextQueryHandlers();
var speechTextAnalyticsService = new PureCloudSpeechTextAnalyticsService(speechTextAnalyticsHandler);

var evaluationReportDataService = new PureCloudEvaluationReportDataService(analyticService, qualityService, speechTextAnalyticsService);

var routingQueryHandlers = new PureCloudRoutingQueryHandlers();
var routingService = new PureCloudRoutingService(routingQueryHandlers);

var divisions = new[] { "d176b581-76c3-4d66-9686-7e2233e8eeb5" };

#endregion

#region EvaluationReports
/*
var evaluationRecords = evaluationReportDataService.GetEvaluationRecords(interval.StartTimeUtc, interval.EndTimeUtc, divisions, Array.Empty<string>());

var evalJsonData = evaluationRecords.JsonSerializeToString(evaluationRecords.GetType());
try
{
    var fileInfo = new FileInfo("evaluationData.json");
    using var streamWriter = fileInfo.CreateText();
    streamWriter.Write(evalJsonData);
    streamWriter.Close();
}
catch (Exception)
{
    Debug.Assert(false);
    return;
}
*/
#endregion

var surveyReportsDataService = new PureCloudSurveyReportDataService(analyticService, qualityService, usersService, routingService);
var surveyRecords = surveyReportsDataService.GetSurveyData(interval.StartTimeUtc, interval.EndTimeUtc, divisions, Array.Empty<string>());
var surveyJsonData = surveyRecords.JsonSerializeToString(surveyRecords.GetType());

try
{
    var fileInfo = new FileInfo("surveyData.json");
    using var streamWriter = fileInfo.CreateText();
    streamWriter.Write(surveyJsonData);
    streamWriter.Close();
}
catch (Exception)
{
    Debug.Assert(false);
    return;
}
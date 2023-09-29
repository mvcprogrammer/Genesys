using System.Configuration;
using System.Diagnostics;
using GenesysCloud.Helpers;
using GenesysCloud.QueryHandlers.PureCloud;
using GenesysCloud.Services.PureCloud.Derived;
using GenesysCloud.Services.PureCloud.Fundamental;

var outputPath = ConfigurationManager.AppSettings["OutputPath"];


#region FakeDI

var analyticsQueryHandlers = new PureCloudAnalyticsQueryHandlers();
var analyticService = new PureCloudAnalyticsService(analyticsQueryHandlers);

var qualityQueryHandlers = new PureCloudQualityQueryHandlers();
var qualityService = new PureCloudQualityService(qualityQueryHandlers);

var usersServiceHandlers = new PureCloudUsersQueryHandlers();
var usersService = new PureCloudUsersService(usersServiceHandlers);

var routingQueryHandlers = new PureCloudRoutingQueryHandlers();
var routingService = new PureCloudRoutingService(routingQueryHandlers);

var divisions = new[] { "d176b581-76c3-4d66-9686-7e2233e8eeb5" };

#endregion

// This is designed to be run automated with date range of day before
// if you want to specify a date range, use Genesphere Swagger
var now = DateTime.UtcNow;       // get the current date and time in UTC
var startTimeUtc = now.Date.AddDays(-1); // get midnight (UTC) of the day before
var endTimeUtc = now.Date;               // get midnight (UTC) of the day of
var interval = new MetricsInterval { StartTimeUtc = startTimeUtc, EndTimeUtc = endTimeUtc };

var surveyReportsDataService = new PureCloudSurveyReportDataService(analyticService, qualityService, usersService, routingService);
var surveyRecords = surveyReportsDataService.GetSurveyData(startTimeUtc, endTimeUtc, divisions, Array.Empty<string>());
var surveyJsonData = surveyRecords.JsonSerializeToString(surveyRecords.GetType());

try
{
    var fileInfo = new FileInfo($"{outputPath}surveys-{interval.ToReportExtensionUtc}.json");
    using var streamWriter = fileInfo.CreateText();
    streamWriter.Write(surveyJsonData);
    streamWriter.Close();
}
catch (Exception)
{
    Debug.Assert(false);
}
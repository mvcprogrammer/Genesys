using System.Diagnostics;
using GenesysCloud.Helpers;
using GenesysCloud.QueryHandlers.PureCloud;
using GenesysCloud.Services.PureCloud.Derived;
using GenesysCloud.Services.PureCloud.Fundamental;

// outputPath is the where you want the JSON file to be deposited
// command line: NespressoEvaluations.exe c:\output\data\ <- after the space is the path
// Visual Studio: right click solution explorer, choose properties, debug tab,
// in the 'Application arguments' field, enter your command-line arguments separated by spaces.

var outputPath = string.Empty;
if (args.Length > 0) outputPath = args[0];

#region FakeDI
var analyticsQueryHandlers = new PureCloudAnalyticsQueryHandlers();
var analyticService = new PureCloudAnalyticsService(analyticsQueryHandlers);

var qualityQueryHandlers = new PureCloudQualityQueryHandlers();
var qualityService = new PureCloudQualityService(qualityQueryHandlers);

var speechTextAnalyticsHandler = new PureCloudSpeechTextQueryHandlers();
var speechTextAnalyticsService = new PureCloudSpeechTextAnalyticsService(speechTextAnalyticsHandler);

var evaluationReportDataService = new PureCloudEvaluationReportDataService(analyticService, qualityService, speechTextAnalyticsService);

var divisions = new[] { "d176b581-76c3-4d66-9686-7e2233e8eeb5" };
#endregion

var now = DateTime.UtcNow;       // get the current date and time in UTC
var startTimeUtc = now.Date.AddDays(-1); // get midnight (UTC) of the day before
var endTimeUtc = now.Date;               // get midnight (UTC) of the day of
var interval = new MetricsInterval { StartTimeUtc = startTimeUtc, EndTimeUtc = endTimeUtc };

// not filtering by queues right now, so using Array.Empty<string> because null is not allowed.
var evaluationRecords = evaluationReportDataService.GetEvaluationRecords(startTimeUtc, endTimeUtc, divisions, Array.Empty<string>());

var evalJsonData = evaluationRecords.JsonSerializeToString(evaluationRecords.GetType());

try
{
    var fileInfo = new FileInfo($"{outputPath}/evaluations-{interval.ToReportExtensionUtc}.json");
    using var streamWriter = fileInfo.CreateText();
    streamWriter.Write(evalJsonData);
    streamWriter.Close();
}
catch (Exception)
{
    Debug.Assert(false);
}
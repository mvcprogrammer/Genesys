using GenesysCloud.DTO.Response;
using GenesysCloud.DTO.Response.Users;
using GenesysCloud.Helpers;
using GenesysCloud.Helpers.Logger;
using GenesysCloud.Helpers.Logger.Console;
using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.QueryHandlers.Mock;
using GenesysCloud.QueryHandlers.PureCloud;
using GenesysCloud.Services;
using GenesysCloud.Services.PureCloud;
using GenesysCloud.Services.PureCloud.Derived;
using GenesysCloud.Services.PureCloud.Fundamental;
using PureCloudPlatform.Client.V2.Api;

var interval = new MetricsInterval
{
    StartTimeUtc = new DateTime(2023, 09, 8, 0, 0, 0),
    EndTimeUtc = new DateTime(2023, 09, 9, 0, 0, 0)
};

var queueProfileLookup = new Dictionary<string, QueueProfile>
{
    { "3c38175b-64be-498b-8cf8-b647aaf3d59b", new QueueProfile { Group = "voice", QueueName = "Starbucks" }},
    { "8e4f918b-203f-440e-8006-da5248643250", new QueueProfile { Group = "voice", QueueName = "B2C-Voice" }},
    { "9996e10d-c261-4622-923a-97df5983ef60", new QueueProfile { Group = "chat", QueueName = "B2C Live Chat" }},
    { "5a1ce433-4c50-4876-9ed1-b0373a11fee7", new QueueProfile { Group = "chat", QueueName = "B2C Whatsapp" }},
    { "27b23169-498d-4670-83c7-97187b95c359", new QueueProfile { Group = "email", QueueName = "General" }},
    { "e516ed93-d7bb-45b8-9429-f210f9252282", new QueueProfile { Group = "email", QueueName = "CBT Arise" }},
    { "b6663c40-7164-46ba-92e9-9711c6b3ed7d", new QueueProfile { Group = "email", QueueName = "Club Relations" }},
    { "7d8fdec6-e18a-4401-97e1-cc208e2d394c", new QueueProfile { Group = "email", QueueName = "SightCall_Test" }},
    { "9856b2db-e784-469e-8aaf-8429e581b603", new QueueProfile { Group = "email", QueueName = "DM_Test" }},
    { "c526329b-d196-42bb-b210-37d469479573", new QueueProfile { Group = "both", QueueName = "Stage 2" }},
    { "7b084016-1424-41f7-a2f0-6cc3ae7688e8", new QueueProfile { Group = "both", QueueName = "Stage 3" }}
    
};

#region Users
/*
IUsersQueryHandlers usersQueryHandlers = new PureCloudUsersQueryHandlers();
var usersService = new UsersService(usersQueryHandlers);

var usersProfileResponse = usersService.GetAgentProfileLookup();
if (usersProfileResponse.Success is false || usersProfileResponse.Data is null)
    return;
var userProfileLookup = usersProfileResponse.Data;

var usersStatusAggregateResponse = usersService.GetUserStatusAggregates(interval, userProfileLookup.Keys.Take(10).ToArray(), Constants.TwentyFourHourInterval);
if (usersStatusAggregateResponse.Success is false || usersStatusAggregateResponse.Data is null)
    return;
var usersStatusAggregates = usersStatusAggregateResponse.Data;

var usersStatusDetailResponse = usersService.GetUsersStatusDetail(interval, userProfileLookup.Keys.Take(10).ToArray());
if (usersStatusDetailResponse.Success is false || usersStatusDetailResponse.Data is null)
    return;
var usersStatusDetail = usersStatusDetailResponse.Data;
*/
#endregion

#region Reports

var analyticsQueryHandlers = new PureCloudAnalyticsQueryHandlers();
var analyticService = new PureCloudAnalyticsService(analyticsQueryHandlers);

var qualityQueryHandlers = new PureCloudQualityQueryHandlers();
var qualityService = new PureCloudQualityService(qualityQueryHandlers);

var usersServiceHandlers = new PureCloudUsersQueryHandlers();
var usersService = new PureCloudUsersService(usersServiceHandlers);

var reportDataService = new PureCloudReportDataService(analyticService, qualityService);

var divisions = new[] { "d176b581-76c3-4d66-9686-7e2233e8eeb5" };
var queue = queueProfileLookup.Keys.ToArray();
var evaluationRecordsResponse = reportDataService.GetEvaluationRecords(interval.StartTimeUtc, interval.EndTimeUtc, divisions, queue);

if (evaluationRecordsResponse.Success is false || evaluationRecordsResponse.Data is null)
    return;

var evaluationRecords = evaluationRecordsResponse.Data;

var jsonData = evaluationRecords.JsonSerializeToString(evaluationRecords.GetType());
Console.WriteLine(jsonData);
#endregion

Console.WriteLine($"{ClassHelpers.GetMethodName(1)} completed");
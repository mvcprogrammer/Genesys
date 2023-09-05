using GenesysCloud.DTO.Response.Users;
using GenesysCloud.Helpers;
using GenesysCloud.Helpers.Logger;
using GenesysCloud.Helpers.Logger.Console;
using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.QueryHandlers.Mock;
using GenesysCloud.QueryHandlers.PureCloud;
using GenesysCloud.Services;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using static System.AppDomain;

CurrentDomain.UnhandledException += GlobalExceptionHandler;

// this should NEVER happen, but just in case...
static void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs args)
{
    Exception ex = (Exception)args.ExceptionObject;
    Console.WriteLine("GlobalExceptionHandler caught : " + ex.Message);
}

var authResponse = AuthorizeService.Authorize(
    clientId: "6cad8911-28ca-40ee-97f5-01136dba9087",
    clientSecret: "44hAG2qlkWCCfUVHU7xnZgL323OyaQ7KKIi297s25eY",
    cloudRegion: PureCloudRegionHosts.eu_west_2);

if (authResponse.Success is false)
    return;

var interval = new MetricsInterval
{
    StartTimeUtc = new DateTime(2023, 09, 01, 0, 0, 0),
    EndTimeUtc = new DateTime(2023, 09, 4, 0, 0, 0)
};

#region Users

var usersApi = new UsersApi();

IUsersQueryHandlers usersQueryHandlers = new PureCloudUsersQueryHandlers(usersApi);
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

#endregion

Console.WriteLine($"{ClassHelpers.GetMethodName(1)} completed");
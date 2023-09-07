using GenesysCloud.DTO.Response.Users;
using GenesysCloud.Helpers;
using GenesysCloud.Helpers.Logger;
using GenesysCloud.Helpers.Logger.Console;
using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.QueryHandlers.Mock;
using GenesysCloud.QueryHandlers.PureCloud;
using GenesysCloud.Services;

var interval = new MetricsInterval
{
    StartTimeUtc = new DateTime(2023, 09, 01, 0, 0, 0),
    EndTimeUtc = new DateTime(2023, 09, 4, 0, 0, 0)
};

#region Users
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

#endregion

Console.WriteLine($"{ClassHelpers.GetMethodName(1)} completed");
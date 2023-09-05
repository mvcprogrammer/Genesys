using GenesysCloud.Helpers.Logger;
using GenesysCloud.Helpers.Logger.Console;
using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.QueryHandlers.Mock;
using GenesysCloud.Services;

IUsersQueryHandlers usersQueryHandlers = new MockUsersQueryHandlers();
ILogger logger = new ConsoleLogger();
var usersService = new UsersService(usersQueryHandlers, logger);
var userProfileLookup = usersService.GetAgentProfileLookup();
var t = "";
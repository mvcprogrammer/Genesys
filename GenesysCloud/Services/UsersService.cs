using GenesysCloud.Helpers.Logger;
using GenesysCloud.QueryHandlers.Contracts;
using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;

namespace GenesysCloud.Services;

public class UsersService
{
    private readonly IUsersQueryHandlers _usersQueryHandlers;
    private readonly ILogger _logger;

    public UsersService(IUsersQueryHandlers usersQueryHandlers, ILogger logger)
    {
        _usersQueryHandlers = usersQueryHandlers;
        _logger = logger;
    }

    public ServiceResponse<Dictionary<string, UserProfile>> GetAgentProfileLookup()
    {
        var userList = _usersQueryHandlers.GetAllUsers();

        if (userList.Success is false || userList.Data is null)
            return SystemResponse.FailureResponse<Dictionary<string, UserProfile>>(userList.ErrorMessage, userList.ErrorCode);
        
        var agentProfileLookup = userList.Data
            .Select(x => new { x.Id, x.Name, x.Email, x.Title })
            .ToDictionary(x => x.Id, x => new UserProfile{Email = x.Email, Name = x.Name, Title = x.Title});
        
        return SystemResponse.SuccessResponse(agentProfileLookup);
    }
    
}
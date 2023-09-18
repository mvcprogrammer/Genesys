using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Static;
using PureCloudPlatform.Client.V2.Client;
using PresenceDefinition = GenesysCloud.DTO.Response.Presence.PresenceDefinition;

namespace GenesysCloud.Services.PureCloud.Fundamental;

/// <summary>
/// Presence service handles calls to Genesys/Mock and is never called directly from outside this assembly.
/// A fundamental service must never return DTO data; it must only return V2.Client.Models
/// A fundamental service may only be called by derived services and authorizations must be called at this level, if needed.
/// When methods are given parameters, this class is responsible for calling query.build from those parameters and calling IPresenceQueryHandler.
/// It's permissible to shape returned data into dictionaries, lookups, etc., or return data as received, and always as the type received from api.
/// Responses are always a ServiceResponse to bubble up handled exception messages and response ids.
/// </summary>
public class PureCloudPresenceService : IPresenceService
{
    private readonly IPresenceQueryHandlers _presenceQueryHandlers;
    private const string NotAuthorized = "Not Authorized";
    private const string LocalCode = "en_US";
    private bool _isAuthorized;
    
    public PureCloudPresenceService(IPresenceQueryHandlers presenceQueryHandlers)
    {
        _presenceQueryHandlers = presenceQueryHandlers;
    }

    /// <summary>
    /// This method ensures all calls have authorization and handles not authorized responses.
    /// <param name="action">
    /// The delegate that will be invoked if authorized=true
    /// </param>
    /// </summary>
    private ServiceResponse<T> AuthorizedAction<T>(Func<ServiceResponse<T>> action)
    {
        return Authorized()
            ? action() 
            : SystemResponse.FailureResponse<T>(NotAuthorized, (int)HttpStatusCode.Unauthorized);
    }
    
    public ServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions()
    {
        return AuthorizedAction(() =>
        {
            var response = _presenceQueryHandlers.GetPresenceDefinitions();
            return response;
        });
    }

    /// <summary>
    /// This method provides a lookup for matching presence ids with their name and other attributes.
    /// </summary>
    
    // ToDo: this method has a DTO object and must be moved to a higher level.
    public ServiceResponse<Dictionary<string, PresenceDefinition>> GetPresenceDefinitionsDictionary()
    {
        var response = GetPresenceDefinitions();
        if (response.Success is false || response.Data is null)
            return SystemResponse.FailureResponse<Dictionary<string, PresenceDefinition>>(response.ErrorMessage, response.ErrorCode);
        
        var presenceDefinitions = response.Data
            .ToDictionary(x => x.Id, x =>  new PresenceDefinition
            {
                SystemPresence = x.SystemPresence,
                IsDeactivated = x.Deactivated ?? false,
                IsPrimary = x.Primary ?? false,
                Label = x.LanguageLabels[LocalCode]
            });

        return SystemResponse.SuccessResponse(presenceDefinitions);
    }
    
    /// <summary>
    /// Gets an authorization token before making calls, if not already authorized.
    /// </summary>
    private bool Authorized()
    {
        if (_isAuthorized) return true;
        
        var authorizeResponse = AuthorizeService.Authorize(
            clientId: "6cad8911-28ca-40ee-97f5-01136dba9087",
            clientSecret: "44hAG2qlkWCCfUVHU7xnZgL323OyaQ7KKIi297s25eY",
            cloudRegion: PureCloudRegionHosts.eu_west_2);

        _isAuthorized = authorizeResponse is { Success: true, Data: true };
        return _isAuthorized;
    }
}
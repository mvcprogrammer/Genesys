using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Static;
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
    private const string LocalCode = "en_US";
    
    public PureCloudPresenceService(IPresenceQueryHandlers presenceQueryHandlers)
    {
        _presenceQueryHandlers = presenceQueryHandlers;
    }
    
    public List<OrganizationPresence> GetPresenceDefinitions()
    {
        return AuthorizedAction(() => ServiceResponse.LogAndReturnResponse(_presenceQueryHandlers.GetPresenceDefinitions()));
    }

    /// <summary>
    /// This method provides a lookup for matching presence ids with their name and other attributes.
    /// </summary>
    
    // ToDo: this method has a DTO object and must be moved to a higher level.
    public Dictionary<string, PresenceDefinition> GetPresenceDefinitionsDictionary()
    {
        var presenceDefinitions = GetPresenceDefinitions();
        
        var presenceDefinitionsLookup = presenceDefinitions
            .ToDictionary(x => x.Id, x =>  new PresenceDefinition
            {
                SystemPresence = x.SystemPresence,
                IsDeactivated = x.Deactivated ?? false,
                IsPrimary = x.Primary ?? false,
                Label = x.LanguageLabels[LocalCode]
            });

        return ServiceResponse.LogAndReturnResponse(presenceDefinitionsLookup);
    }
    
    /// <summary>
    /// This method ensures all calls have authorization and handles not authorized responses.
    /// <param name="action">
    /// This delegate will invoke its action if authorized=true
    /// </param>
    /// </summary>
    private static T AuthorizedAction<T>(Func<T> action)
    {
        if (AuthorizeService.IsAuthorized())
            return action();
        
        throw new UnauthorizedAccessException(Constants.NotAuthorized);
    }
}
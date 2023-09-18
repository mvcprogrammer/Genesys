namespace GenesysCloud.Services.Contracts.Fundamental;
using PresenceDefinition = GenesysCloud.DTO.Response.Presence.PresenceDefinition;

public interface IPresenceService
{
    public ServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions();
    public ServiceResponse<Dictionary<string, PresenceDefinition>> GetPresenceDefinitionsDictionary();
}
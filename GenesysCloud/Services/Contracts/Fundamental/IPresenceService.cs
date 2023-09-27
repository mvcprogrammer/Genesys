namespace GenesysCloud.Services.Contracts.Fundamental;
using PresenceDefinition = GenesysCloud.DTO.Response.Presence.PresenceDefinition;

public interface IPresenceService
{
    public List<OrganizationPresence> GetPresenceDefinitions();
    public Dictionary<string, PresenceDefinition> GetPresenceDefinitionsDictionary();
}
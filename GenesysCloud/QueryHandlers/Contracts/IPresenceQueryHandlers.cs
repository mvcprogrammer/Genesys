namespace GenesysCloud.QueryHandlers.Contracts;

public interface IPresenceQueryHandlers
{
    public List<OrganizationPresence> GetPresenceDefinitions();
}
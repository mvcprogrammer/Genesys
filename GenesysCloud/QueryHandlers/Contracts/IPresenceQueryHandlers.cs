namespace GenesysCloud.QueryHandlers.Contracts;

public interface IPresenceQueryHandlers
{
    public GenesysServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions();
}
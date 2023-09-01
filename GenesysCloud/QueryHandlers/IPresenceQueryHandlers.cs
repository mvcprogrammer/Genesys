namespace GenesysCloud.QueryHandlers;

public interface IPresenceQueryHandlers
{
    public GenesysServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions();
}
namespace GenesysCloud.QueryHandlers.Contracts;

public interface IPresenceQueryHandlers
{
    public ServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions();
}
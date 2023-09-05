using GenesysCloud.QueryHandlers.Contracts;

namespace GenesysCloud.QueryHandlers.Mock;

public class MockPresenceQueryHandlers : IPresenceQueryHandlers
{
    public ServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions()
    {
        throw new NotImplementedException();
    }
}
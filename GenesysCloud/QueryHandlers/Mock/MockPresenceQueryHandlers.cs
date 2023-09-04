using GenesysCloud.QueryHandlers.Contracts;

namespace GenesysCloud.QueryHandlers.Mock;

public class MockPresenceQueryHandlers : IPresenceQueryHandlers
{
    public GenesysServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions()
    {
        throw new NotImplementedException();
    }
}
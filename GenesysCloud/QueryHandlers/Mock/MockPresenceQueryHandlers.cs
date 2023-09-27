using GenesysCloud.QueryHandlers.Contracts;

namespace GenesysCloud.QueryHandlers.Mock;

public class MockPresenceQueryHandlers : IPresenceQueryHandlers
{
    public List<OrganizationPresence> GetPresenceDefinitions()
    {
        throw new NotImplementedException();
    }
}
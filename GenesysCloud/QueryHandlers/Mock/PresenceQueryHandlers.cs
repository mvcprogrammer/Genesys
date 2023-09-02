using GenesysCloud.QueryHandlers.Contracts;

namespace GenesysCloud.QueryHandlers.Mock;

public class PresenceQueryHandlers : IPresenceQueryHandlers
{
    public GenesysServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions()
    {
        throw new NotImplementedException();
    }
}
using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

internal sealed class PureCloudPresenceQueryHandlers : IPresenceQueryHandlers
{
    private readonly PresenceApi _presenceApi = new();
    
    public ServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions()
    {
        var pageCount = Constants.Unknown;
        var currentPage = Constants.FirstPage;
        const int pageSize = 100;
        var organizationPresenceList = new List<OrganizationPresence>();
        
        try
        {
            while (pageCount >= currentPage || pageCount is Constants.Unknown)
            {
                var presenceDefinitions =  _presenceApi.GetPresencedefinitions(pageNumber: currentPage, pageSize: pageSize);
                organizationPresenceList.AddRange(presenceDefinitions.Entities ?? Enumerable.Empty<OrganizationPresence>());
                pageCount = presenceDefinitions.PageCount ?? Constants.FirstPage;
                currentPage++;
            }

            return SystemResponse.SuccessResponse(organizationPresenceList);
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<List<OrganizationPresence>>(exception);
        }
    }
}
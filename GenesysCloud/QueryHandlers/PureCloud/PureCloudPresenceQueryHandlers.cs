using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

/// <summary>
/// PureCloud Presence Query Handler is responsible for submitting an PureCloud.Client.V2.Model PresenceApi query to PureCloud.
/// The API can be called multiple times if the response is paged.
/// Handlers are not dependent on query filters; 1 handler, many queries.
/// The query handler always returns a Genesys Client.V2.Model in the data field of a ServiceResponse.
/// DTO must never be done at this level for dependency segmentation.
/// </summary>
internal sealed class PureCloudPresenceQueryHandlers : IPresenceQueryHandlers
{
    private readonly PresenceApi _presenceApi = new();
    
    public List<OrganizationPresence> GetPresenceDefinitions()
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

            return ServiceResponse.LogAndReturnResponse(organizationPresenceList);
        }
        catch (Exception exception)
        {
            ServiceResponse.ExceptionHandler.HandleException<List<OrganizationPresence>>(exception);
            throw;
        }
    }
}
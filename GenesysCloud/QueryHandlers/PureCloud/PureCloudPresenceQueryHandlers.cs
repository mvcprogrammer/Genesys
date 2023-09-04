using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

public class PureCloudPresenceQueryHandlers : IPresenceQueryHandlers
{
    private readonly PresenceApi _presenceApi;
    
    public PureCloudPresenceQueryHandlers(PresenceApi presenceApi)
    {
        _presenceApi = presenceApi ?? throw new ArgumentNullException(nameof(presenceApi));
    }
    
    public GenesysServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions()
    {
        var pageCount = Constants.Unknown;
        var currentPage = Constants.FirstPage;
        var organizationPresenceList = new List<OrganizationPresence>();
        
        try
        {
            while (pageCount >= currentPage || pageCount is Constants.Unknown)
            {
                var presenceDefinitions =  _presenceApi.GetPresencedefinitions(pageNumber: currentPage, pageSize: 100);
                
                if(presenceDefinitions is null)
                    return GenesysResponse.FailureResponse<List<OrganizationPresence>>(
                        Constants.NullResponse, Constants.HtmlServerError);
                
                organizationPresenceList.AddRange(presenceDefinitions.Entities ?? Enumerable.Empty<OrganizationPresence>());
                
                pageCount = presenceDefinitions.PageCount ?? Constants.FirstPage;
                currentPage++;
            }

            return GenesysResponse.SuccessResponse(organizationPresenceList);
        }
        catch (Exception exception)
        {
            return GenesysResponse.ExceptionHandler.HandleException<List<OrganizationPresence>>(exception);
        }
    }
}
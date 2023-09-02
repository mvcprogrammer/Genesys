using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;

namespace GenesysCloud.QueryHandlers.PureCloud.Presence;

public class PresenceQueryHandlers : IPresenceQueryHandlers
{
    private readonly PresenceApi _presenceApi = new();
    
    public GenesysServiceResponse<List<OrganizationPresence>> GetPresenceDefinitions()
    {
        var pageCount = Constants.Unknown;
        var curPage = Constants.FirstPage;
        var organizationPresenceList = new List<OrganizationPresence>();
        
        try
        {
            while (pageCount >= curPage || pageCount is Constants.Unknown)
            {
                var presenceDefinitions =  _presenceApi.GetPresencedefinitions(pageNumber: curPage, pageSize: 100);
                organizationPresenceList.AddRange(presenceDefinitions.Entities);
                
                pageCount = presenceDefinitions.PageCount ?? Constants.FirstPage;
                curPage++;
            }

            return GenesysResponse.SuccessResponse(organizationPresenceList);
        }
        catch (ApiException exception)
        {
            return GenesysResponse.FailureResponse<List<OrganizationPresence>>(exception.Message, exception.ErrorCode);
        }
        catch (Exception exception)
        {
            return GenesysResponse.FailureResponse<List<OrganizationPresence>>(exception.Message);
        }
    }
}
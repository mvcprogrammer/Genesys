using GenesysCloud.DTO.Response;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;

namespace GenesysCloud.QueryHandlers.PureCloud.Users;

public class UsersQueryHandlers : IUsersQueryHandlers
{
    private readonly UsersApi _usersApi = new();

    public GenesysServiceResponse<List<User>> GetAllUsers()
    {
        var pageCount = Constants.Unknown;
        var curPage = Constants.FirstPage;
        var userEntityListingList = new List<User>();

        try
        {
            while (pageCount >= curPage || pageCount is Constants.Unknown)
            {
                var userEntityListings =  _usersApi.GetUsers(pageNumber: curPage, pageSize: 100, state: "any");
                userEntityListingList.AddRange(userEntityListings.Entities);
                
                pageCount = userEntityListings.PageCount ?? 1;
                curPage++;
            }
        }
        catch (ApiException exception)
        {
            return GenesysResponse.FailureResponse<List<User>>(exception.Message, exception.ErrorCode);
        }
        catch (Exception exception)
        {
            return GenesysResponse.FailureResponse<List<User>>(exception.Message);
        }

        return GenesysResponse.SuccessResponse(userEntityListingList.ToList());
    }

    public GenesysServiceResponse<List<AnalyticsUserDetail>> GetUserDetails(UserDetailsQuery userDetailsQuery)
    {
        var pageCount = Constants.Unknown;
        var curPage = Constants.FirstPage;
        var analyticsUserDetailList = new List<AnalyticsUserDetail>();
        
        try
        {
            while (pageCount >= curPage || pageCount is Constants.Unknown)
            {
                var response = _usersApi.PostAnalyticsUsersDetailsQuery(userDetailsQuery);
                
                if(response.UserDetails is not null) 
                    analyticsUserDetailList.AddRange(response.UserDetails);
                
                if (pageCount is Constants.Unknown)
                    pageCount = (int)Math.Ceiling((double)response.TotalHits.GetValueOrDefault(0) / 100);
                
                userDetailsQuery.Paging.PageNumber = ++curPage;
            }

            return GenesysResponse.SuccessResponse(analyticsUserDetailList.ToList());
        }
        catch (ApiException exception)
        {
            return GenesysResponse.FailureResponse<List<AnalyticsUserDetail>>(exception.Message, exception.ErrorCode);
        }
        catch (Exception exception)
        {
            return GenesysResponse.FailureResponse<List<AnalyticsUserDetail>>(exception.Message);
        }
    }

    public UserAggregateQueryResponse GetUsersAggregates(UserAggregationQuery userAggregationQuery)
    {
        throw new NotImplementedException();
    }
}
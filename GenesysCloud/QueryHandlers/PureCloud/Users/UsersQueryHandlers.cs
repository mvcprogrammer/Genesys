using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;

namespace GenesysCloud.QueryHandlers.PureCloud.Users;

public class UsersQueryHandlers : IUsersQueryHandlers
{
    private readonly UsersApi _usersApi = new();

    public GenesysServiceResponse<List<User>> GetAllUsers(int pageSize = 100, string state = "any")
    {
        var pageCount = Constants.Unknown;
        var curPage = Constants.FirstPage;
        var userList = new List<User>();

        try
        {
            while (pageCount >= curPage || pageCount is Constants.Unknown)
            {
                var response =  _usersApi.GetUsers(pageNumber: curPage, pageSize: pageSize, state: state);
                
                if (response is null)
                    return GenesysResponse.FailureResponse<List<User>>("Null Response", Constants.HtmlServerError);
                    
                if(response.Entities is not null) 
                    userList.AddRange(response.Entities);
                
                pageCount = response.PageCount ?? Constants.FirstPage;
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

        return GenesysResponse.SuccessResponse(userList.ToList());
    }

    public GenesysServiceResponse<List<AnalyticsUserDetail>> GetUserDetails(UserDetailsQuery query)
    {
        var pageCount = Constants.Unknown;
        var curPage = Constants.FirstPage;
        var analyticsUserDetailList = new List<AnalyticsUserDetail>();
        
        try
        {
            while (pageCount >= curPage || pageCount is Constants.Unknown)
            {
                var response = _usersApi.PostAnalyticsUsersDetailsQuery(query);

                if (response is null)
                    return GenesysResponse.FailureResponse<List<AnalyticsUserDetail>>("Null response", Constants.HtmlServerError);
                
                if(response.UserDetails is not null) 
                    analyticsUserDetailList.AddRange(response.UserDetails);
                
                if (pageCount is Constants.Unknown)
                    pageCount = (int)Math.Ceiling((double)response.TotalHits.GetValueOrDefault(0) / 100);
                
                query.Paging.PageNumber = ++curPage;
            }

            return GenesysResponse.SuccessResponse(analyticsUserDetailList.ToList());
        }
        catch (ApiException exception)
        {
            return GenesysResponse.FailureResponse<List<AnalyticsUserDetail>>(
                exception.Message, 
                exception.ErrorCode,
                query.JsonSerializeToString(query.GetType()));
        }
        catch (Exception exception)
        {
            return GenesysResponse.FailureResponse<List<AnalyticsUserDetail>>(exception.Message);
        }
    }

    public GenesysServiceResponse<List<UserAggregateDataContainer>> GetUsersAggregates(UserAggregationQuery query)
    {
        try
        {
            var response = _usersApi.PostAnalyticsUsersAggregatesQuery(query);

            return response is null 
                ? GenesysResponse.FailureResponse<List<UserAggregateDataContainer>>(errorMessage:"Null Response", query:query.JsonSerializeToString(query.GetType())) 
                : GenesysResponse.SuccessResponse(response.Results);
        }
        catch (ApiException exception)
        {
            return GenesysResponse.FailureResponse<List<UserAggregateDataContainer>>(
                exception.Message, 
                exception.ErrorCode, 
                query.JsonSerializeToString(query.GetType()));
        }
        catch (Exception exception)
        {
            return GenesysResponse.FailureResponse<List<UserAggregateDataContainer>>(exception.Message);
        }
    }
}
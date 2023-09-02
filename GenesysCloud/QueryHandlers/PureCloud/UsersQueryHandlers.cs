using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

public class UsersQueryHandlers : IUsersQueryHandlers
{
    private readonly UsersApi _usersApi;

    public UsersQueryHandlers(UsersApi usersApi)
    {
        _usersApi = usersApi ?? throw new ArgumentNullException(nameof(usersApi));
    }

    public GenesysServiceResponse<List<User>> GetAllUsers(int pageSize = 100, string state = "any")
    {
        var pageCount = Constants.Unknown;
        var currentPage = Constants.FirstPage;
        var userList = new List<User>();

        try
        {
            while (pageCount >= currentPage || pageCount is Constants.Unknown)
            {
                var response = _usersApi.GetUsers(pageNumber: currentPage, pageSize: pageSize, state: state);

                if (response is null)
                    return GenesysResponse.FailureResponse<List<User>>(Constants.NullResponse,
                        Constants.HtmlServerError);

                userList.AddRange(response.Entities ?? Enumerable.Empty<User>());

                pageCount = response.PageCount ?? Constants.FirstPage;
                currentPage++;
            }

            return GenesysResponse.SuccessResponse(userList);
        }
        catch (Exception exception)
        {
            return GenesysResponse.ExceptionHandler.HandleException<List<User>>(exception);
        }
    }

    public GenesysServiceResponse<List<AnalyticsUserDetail>> GetUserDetails(UserDetailsQuery query)
    {
        var pageCount = Constants.Unknown;
        var currentPage = Constants.FirstPage;
        var analyticsUserDetailList = new List<AnalyticsUserDetail>();
        
        try
        {
            while (pageCount >= currentPage || pageCount is Constants.Unknown)
            {
                var response = _usersApi.PostAnalyticsUsersDetailsQuery(query);

                if (response is null)
                    return GenesysResponse.FailureResponse<List<AnalyticsUserDetail>>(
                        Constants.NullResponse, Constants.HtmlServerError);
                
                if(response.UserDetails is not null) 
                    analyticsUserDetailList.AddRange(response.UserDetails);
                
                if (pageCount is Constants.Unknown)
                    pageCount = (int)Math.Ceiling(response.TotalHits.GetValueOrDefault(0) / 100.0);
                
                query.Paging.PageNumber = ++currentPage;
            }

            return GenesysResponse.SuccessResponse(analyticsUserDetailList);
        }
        catch (Exception exception)
        {
            return GenesysResponse.ExceptionHandler.HandleException<List<AnalyticsUserDetail>>(
                exception,
                query.JsonSerializeToString(query.GetType()));
        }
    }

    public GenesysServiceResponse<List<UserAggregateDataContainer>> GetUsersAggregates(UserAggregationQuery query)
    {
        try
        {
            var response = _usersApi.PostAnalyticsUsersAggregatesQuery(query);

            return response is null
                ? GenesysResponse.FailureResponse<List<UserAggregateDataContainer>>(
                    errorMessage: Constants.NullResponse,
                    query: query.JsonSerializeToString(query.GetType()))
                : GenesysResponse.SuccessResponse(response.Results);
        }
        catch (Exception exception)
        {
            return GenesysResponse.ExceptionHandler.HandleException<List<UserAggregateDataContainer>>(
                exception,
                query.JsonSerializeToString(query.GetType()));
        }
    }
}
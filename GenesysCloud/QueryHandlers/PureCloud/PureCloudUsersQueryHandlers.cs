using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

internal sealed class PureCloudUsersQueryHandlers : IUsersQueryHandlers
{
    private readonly UsersApi _usersApi = new();

    public ServiceResponse<List<User>> GetAllUsers(int pageSize = 100, string state = "any")
    {
        var pageCount = Constants.Unknown;
        var currentPage = Constants.FirstPage;
        var userList = new List<User>();

        try
        {
            while (pageCount >= currentPage || pageCount is Constants.Unknown)
            {
                var response = _usersApi.GetUsers(pageNumber: currentPage, pageSize: pageSize, state: state);
                userList.AddRange(response.Entities ?? Enumerable.Empty<User>());
                pageCount = response.PageCount ?? Constants.FirstPage;
                currentPage++;
            }

            return SystemResponse.SuccessResponse(userList);
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<List<User>>(exception,
                $"pageNumber:{currentPage}, pageSize:{pageSize}, state:{state}");
        }
    }

    public ServiceResponse<List<AnalyticsUserDetail>> GetUsersStatusDetail(UserDetailsQuery query)
    {
        var pageCount = Constants.Unknown;
        var currentPage = Constants.FirstPage;
        var analyticsUserDetailList = new List<AnalyticsUserDetail>();
        
        try
        {
            while (pageCount >= currentPage || pageCount is Constants.Unknown)
            {
                var response = _usersApi.PostAnalyticsUsersDetailsQuery(query);
                analyticsUserDetailList.AddRange(response.UserDetails ?? Enumerable.Empty<AnalyticsUserDetail>());
                
                if (pageCount is Constants.Unknown)
                    pageCount = (int)Math.Ceiling(response.TotalHits.GetValueOrDefault(0) / 100.0);
                
                query.Paging.PageNumber = ++currentPage;
            }

            return SystemResponse.SuccessResponse(analyticsUserDetailList);
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<List<AnalyticsUserDetail>>(
                exception,
                query.JsonSerializeToString(query.GetType()));
        }
    }

    public ServiceResponse<List<UserAggregateDataContainer>> GetUsersStatusAggregates(UserAggregationQuery query)
    {
        try
        {
            var response = _usersApi.PostAnalyticsUsersAggregatesQuery(query);
            return SystemResponse.SuccessResponse<List<UserAggregateDataContainer>>(response.Results ?? new List<UserAggregateDataContainer>());
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<List<UserAggregateDataContainer>>(
                exception,
                query.JsonSerializeToString(query.GetType()));
        }
    }
}
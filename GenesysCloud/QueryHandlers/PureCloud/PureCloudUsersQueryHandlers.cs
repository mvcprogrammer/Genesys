using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

/// <summary>
/// PureCloud Users Analytics Query Handler is responsible for submitting an PureCloud.Client.V2.Model UsersApi query to the PureCloud API.
/// The API can be called multiple times if the response is paged.
/// Handlers are not dependent on query filters; 1 handler, many queries.
/// The query handler always returns a Genesys Client.V2.Model in the data field of a ServiceResponse.
/// DTO must never be done at this level for dependency segmentation.
/// Responses are always a ServiceResponse to bubble up handled exception messages and response ids.
/// </summary>
internal sealed class PureCloudUsersQueryHandlers : IUsersQueryHandlers
{
    private readonly UsersApi _usersApi = new();
    
    public IEnumerable<User> GetUsers(IReadOnlyCollection<string> userIds)
    {
        const int pageSize = 100;
        const string state = "any";
        var pageCount = Constants.Unknown;
        var currentPage = 0;
        var userList = new List<User>();

        try
        {
            while (pageCount > currentPage || pageCount is Constants.Unknown)
            {
                var response = _usersApi.GetUsers(pageSize: pageSize, pageNumber: currentPage, id: userIds.Skip(pageSize*currentPage).Take(pageSize).ToList(), state:state);
                userList.AddRange(response.Entities ?? Enumerable.Empty<User>());
                pageCount = (int)Math.Ceiling((double)userIds.Count / pageSize);
                currentPage++;
            }

            return userList;
        }
        catch (Exception exception)
        {
            ServiceResponse.ExceptionHandler.HandleException<List<User>>(exception, $"pageNumber:{currentPage}, pageSize:{pageSize}, state:{state}");
            throw;
        }
    }

    public List<AnalyticsUserDetail> GetUsersStatusDetail(UserDetailsQuery query)
    {
        var pageCount = Constants.Unknown;
        var currentPage = Constants.FirstPage;
        var analyticsUserDetailList = new List<AnalyticsUserDetail>();
        
        try
        {
            while (pageCount >= currentPage || pageCount is Constants.Unknown)
            {
                var response = _usersApi.PostAnalyticsUsersDetailsQuery(body: query);
                analyticsUserDetailList.AddRange(response.UserDetails ?? Enumerable.Empty<AnalyticsUserDetail>());
                
                if (pageCount is Constants.Unknown)
                    pageCount = (int)Math.Ceiling(response.TotalHits.GetValueOrDefault(0) / 100.0);
                
                query.Paging.PageNumber = ++currentPage;
            }

            return analyticsUserDetailList;
        }
        catch (Exception exception)
        {
            ServiceResponse.ExceptionHandler.HandleException<List<AnalyticsUserDetail>>(exception, query.JsonSerializeToString(query.GetType()));
            throw;
        }
    }

    public List<UserAggregateDataContainer> GetUsersStatusAggregates(UserAggregationQuery query)
    {
        try
        {
            var response = _usersApi.PostAnalyticsUsersAggregatesQuery(body: query);
            return response.Results ?? new List<UserAggregateDataContainer>();
        }
        catch (Exception exception)
        {
            ServiceResponse.ExceptionHandler.HandleException<List<UserAggregateDataContainer>>(exception, query.JsonSerializeToString(query.GetType()));
            throw;
        }
    }
}
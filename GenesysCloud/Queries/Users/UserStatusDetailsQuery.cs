namespace GenesysCloud.Queries.Users;

public class UserStatusDetailsQuery
{
    private readonly MetricsInterval _interval;
    private readonly string[] _userIds;

    public UserStatusDetailsQuery(MetricsInterval interval, string[] userIds)
    {
        _interval = interval;
        _userIds = userIds;
    }

    /// <summary>
    /// Builds a User Status Detail Query.
    /// User Status Detail queries show the finest level of user activity. The query data reflects the user's
    /// Genesys Cloud presence as well as their ACD routing status at the individual status-change level.
    /// For more details, refer to the official documentation: 
    /// <see href="https://developer.genesys.cloud/analyticsdatamanagement/analytics/detail/user-query"/>
    /// </summary>
    public UserDetailsQuery Build()
    {
        var interval = _interval.ToGenesysInterval;

        var predicates = _userIds.Select(userId =>
            new UserDetailQueryPredicate
            {
                Dimension = UserDetailQueryPredicate.DimensionEnum.Userid,
                Value = userId
            }).ToList();

        var filters = new List<UserDetailQueryFilter>
        {
            new()
            {
                Type = UserDetailQueryFilter.TypeEnum.Or,
                Predicates = predicates
            }
        };

        var paging = new PagingSpec
        {
            PageNumber = 1,
            PageSize = 100
        };

        var userDetailsQuery = new UserDetailsQuery
        {
            Interval = interval,
            UserFilters = filters,
            Paging = paging
        };

        return userDetailsQuery;
    }
}
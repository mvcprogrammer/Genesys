namespace GenesysCloud.Queries.Users;

public class UserStatusAggregateQuery
{
    private readonly MetricsInterval _interval;
    private readonly string[] _users;
    private readonly string _granularity;

    public UserStatusAggregateQuery(MetricsInterval interval, string[] users, string granularity = Constants.TwentyFourHourInterval)
    {
        _interval = interval ?? throw new ArgumentNullException(nameof(interval), "Interval cannot be null");
        _users = users ?? throw new ArgumentNullException(nameof(users), "Users cannot be null");
        _granularity = granularity ?? throw new ArgumentNullException(nameof(granularity), "Granularity cannot be null");
    }
    
    /// <summary>
    /// Builds a User Aggregation Query.
    /// The User Status Aggregate queries show a high-level summary of user activity. 
    /// The query data reflects the user's Genesys Cloud presence as well as their ACD routing status.
    /// For more details, refer to the official documentation: 
    /// <see href="https://developer.genesys.cloud/analyticsdatamanagement/analytics/aggregate/user-query"/>
    /// Pay particular attention to query limitations:
    /// <see href="https://developer.genesys.cloud/analyticsdatamanagement/analytics/aggregate/user-query#limitations"/>
    /// </summary>
    public UserAggregationQuery Build()
    {
        var interval = _interval.ToGenesysInterval;

        var predicates = _users.Select(userId =>
                new UserAggregateQueryPredicate
                {
                    Dimension = UserAggregateQueryPredicate.DimensionEnum.Userid,
                    Operator = UserAggregateQueryPredicate.OperatorEnum.Matches,
                    Value = userId
                })
            .ToList();

        var filters = new UserAggregateQueryFilter
        {
            Type = UserAggregateQueryFilter.TypeEnum.Or,
            Predicates = predicates
        };

        var metrics = new List<UserAggregationQuery.MetricsEnum>
        {
            UserAggregationQuery.MetricsEnum.Tsystempresence,
            UserAggregationQuery.MetricsEnum.Tagentroutingstatus,
            UserAggregationQuery.MetricsEnum.Torganizationpresence
        };

        var groupBy = new List<UserAggregationQuery.GroupByEnum>
        {
            UserAggregationQuery.GroupByEnum.Userid
        };

        var userAggregationQuery = new UserAggregationQuery
        {
            Interval = interval,
            Filter = filters,
            Metrics = metrics,
            Granularity = _granularity,
            GroupBy = groupBy
        };

        return userAggregationQuery;
    }
}
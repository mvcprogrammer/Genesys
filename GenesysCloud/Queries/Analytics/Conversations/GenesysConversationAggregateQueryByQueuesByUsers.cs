namespace GenesysCloud.Queries.Analytics.Conversations;

public class GenesysConversationAggregateQueryByQueuesByUsers
{
    private readonly MetricsInterval _interval;
    private readonly string[] _users;
    private readonly string[] _queueIds;
    private readonly string _granularity;

    public GenesysConversationAggregateQueryByQueuesByUsers(MetricsInterval interval, string granularity, string[] queueIds, string[] users)
    {
        _interval = interval ?? throw new ArgumentNullException(nameof(interval), "Interval cannot be null");
        _users = users ?? throw new ArgumentNullException(nameof(users), "Users cannot be null (empty ok)");
        _queueIds = queueIds ?? throw new ArgumentNullException(nameof(queueIds), "Queue Id's cannot be null (empty ok)");
        _granularity = granularity ?? throw new ArgumentNullException(nameof(granularity), "Granularity must be specified. (See GenesysCloud.Helpers.Constants");
    }
    
    public ConversationAggregationQuery Build()
    {
        var interval = _interval.ToGenesysInterval;
        
        var filterClauses = new List<ConversationAggregateQueryClause>();

        if (_queueIds.Any())
        {
            var queuePredicates = _queueIds.Select(queueId => 
                new ConversationAggregateQueryPredicate
                {
                    Dimension = ConversationAggregateQueryPredicate.DimensionEnum.Queueid,
                    Operator = ConversationAggregateQueryPredicate.OperatorEnum.Matches,
                    Value = queueId
                }).ToList();
            
            var conversationAggregateQueryClause = new ConversationAggregateQueryClause
            {
                Type = ConversationAggregateQueryClause.TypeEnum.Or,
                Predicates = queuePredicates
            };
            
            filterClauses.Add(conversationAggregateQueryClause);
        }
        
        if (_users.Any())
        {
            var userPredicates = _users.Select(userId => 
                new ConversationAggregateQueryPredicate
                {
                    Dimension = ConversationAggregateQueryPredicate.DimensionEnum.Userid,
                    Operator = ConversationAggregateQueryPredicate.OperatorEnum.Matches,
                    Value = userId
                }).ToList();
            
            var conversationAggregateQueryClause = new ConversationAggregateQueryClause
            {
                Type = ConversationAggregateQueryClause.TypeEnum.Or,
                Predicates = userPredicates
            };
            
            filterClauses.Add(conversationAggregateQueryClause);
        }

        var filter = new ConversationAggregateQueryFilter
        {
            Type = ConversationAggregateQueryFilter.TypeEnum.And,
            Clauses = filterClauses
        };
        
        // get all metrics
        var metricList = Enum
            .GetValues(typeof(ConversationAggregationQuery.MetricsEnum))
            .Cast<ConversationAggregationQuery.MetricsEnum>()
            .ToList();
        
        // except those unsupported types
        metricList.Remove(ConversationAggregationQuery.MetricsEnum.OutdatedSdkVersion);
        metricList.Remove(ConversationAggregationQuery.MetricsEnum.Tcallback);
        metricList.Remove(ConversationAggregationQuery.MetricsEnum.Tcallbackcomplete);
        metricList.Remove(ConversationAggregationQuery.MetricsEnum.Tactivecallback);
        metricList.Remove(ConversationAggregationQuery.MetricsEnum.Tactivecallbackcomplete);

        var groupBy = new List<ConversationAggregationQuery.GroupByEnum>
        {
            ConversationAggregationQuery.GroupByEnum.Userid,
            ConversationAggregationQuery.GroupByEnum.Queueid,
            ConversationAggregationQuery.GroupByEnum.Activeskillid
        };

        return new ConversationAggregationQuery
        {
            Filter = filter,
            GroupBy = groupBy,
            Interval = interval,
            Metrics = metricList,
            Granularity = _granularity,
            FlattenMultivaluedDimensions = true
        };
    }
}
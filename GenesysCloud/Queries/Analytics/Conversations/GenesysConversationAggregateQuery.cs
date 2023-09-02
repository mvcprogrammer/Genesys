namespace GenesysCloud.Queries.Analytics.Conversations;

public class GenesysConversationAggregateQuery
{
    private readonly IntervalSpan _interval;
    private readonly string[] _users;

    public GenesysConversationAggregateQuery(IntervalSpan interval, string[] users)
    {
        _interval = interval;
        _users = users;
    }
    
    public ConversationAggregationQuery BuildQuery()
    {
        var interval = _interval.ToGenesysInterval;
        
        var userPredicates = _users.Select(userId => 
                new ConversationAggregateQueryPredicate
                {
                    Dimension = ConversationAggregateQueryPredicate.DimensionEnum.Userid, 
                    Value = userId
                })
            .ToList();

        var filterClause = new List<ConversationAggregateQueryClause>
        {
            new()
            {
                Type = ConversationAggregateQueryClause.TypeEnum.Or,
                Predicates = userPredicates
            }
        };
        
        var predicateList = new List<ConversationAggregateQueryPredicate>
        {
            new()
            {
                Dimension = ConversationAggregateQueryPredicate.DimensionEnum.Mediatype,
                Value = "voice"
            },
            new()
            {
                Dimension = ConversationAggregateQueryPredicate.DimensionEnum.Mediatype,
                Value = "chat"
            }
        };

        var filter = new ConversationAggregateQueryFilter
        {
            Type = ConversationAggregateQueryFilter.TypeEnum.And,
            Predicates = predicateList,
            Clauses = filterClause
        };

        var metricList = new List<ConversationAggregationQuery.MetricsEnum>
        {
            ConversationAggregationQuery.MetricsEnum.Tanswered,
            ConversationAggregationQuery.MetricsEnum.Thandle,
            ConversationAggregationQuery.MetricsEnum.Ttalk,
            ConversationAggregationQuery.MetricsEnum.Theld,
            ConversationAggregationQuery.MetricsEnum.Tacw,
            ConversationAggregationQuery.MetricsEnum.Tdialing,
            ConversationAggregationQuery.MetricsEnum.Tcontacting,
            ConversationAggregationQuery.MetricsEnum.Ntransferred,
            ConversationAggregationQuery.MetricsEnum.Noutbound,
            ConversationAggregationQuery.MetricsEnum.Tnotresponding,
            ConversationAggregationQuery.MetricsEnum.Talert,
            ConversationAggregationQuery.MetricsEnum.Tmonitoring,
            ConversationAggregationQuery.MetricsEnum.Nblindtransferred,
            ConversationAggregationQuery.MetricsEnum.Nconsulttransferred,
            ConversationAggregationQuery.MetricsEnum.Tflowout
        };

        var groupBy = new List<ConversationAggregationQuery.GroupByEnum>
        {
            ConversationAggregationQuery.GroupByEnum.Userid
        };

        return new ConversationAggregationQuery
        {
            Filter = filter,
            GroupBy = groupBy,
            Interval = interval,
            Metrics = metricList,
            Granularity = Constants.TwentyFourHourInterval
        };
    }
}
namespace GenesysCloud.Queries.Quality;

public class GenesysEvaluationAggregateQuery
{
    private readonly IntervalSpan _interval;
    private readonly IReadOnlyCollection<string> _queueIds;
    private readonly IReadOnlyCollection<string> _divisionIds;

    public GenesysEvaluationAggregateQuery(IntervalSpan interval, IReadOnlyCollection<string> queueIds, IReadOnlyCollection<string> divisions)
    {
        _interval = interval;
        _queueIds = queueIds;
        _divisionIds = divisions;
    }
    
    public EvaluationAggregationQuery Build()
    {
        var interval = _interval.ToGenesysInterval;
        
        var divisionPredicates = _divisionIds.Select(divisionId => 
                new EvaluationAggregateQueryPredicate
                {
                    Dimension = EvaluationAggregateQueryPredicate.DimensionEnum.Divisionid,
                    Operator = EvaluationAggregateQueryPredicate.OperatorEnum.Matches,
                    Value = divisionId,
                    Type = EvaluationAggregateQueryPredicate.TypeEnum.Dimension
                })
            .ToList();
        
        var queuePredicates = _queueIds.Select(queueId =>
                new EvaluationAggregateQueryPredicate
                {
                    Dimension = EvaluationAggregateQueryPredicate.DimensionEnum.Queueid,
                    Operator = EvaluationAggregateQueryPredicate.OperatorEnum.Matches,
                    Value = queueId,
                    Type = EvaluationAggregateQueryPredicate.TypeEnum.Dimension
                })
            .ToList();

        var queryClauses = new List<EvaluationAggregateQueryClause>
        {   
            new()
            {
                Predicates = divisionPredicates,
                Type = EvaluationAggregateQueryClause.TypeEnum.Or
            },
            new()
            {
                Predicates = queuePredicates,
                Type = EvaluationAggregateQueryClause.TypeEnum.Or
            }
        };

        var filter = new EvaluationAggregateQueryFilter
        {
            Clauses = queryClauses,
            Type = EvaluationAggregateQueryFilter.TypeEnum.And
        };
        
        var metricList = Enum
            .GetValues(typeof(EvaluationAggregationQuery.MetricsEnum))
            .Cast<EvaluationAggregationQuery.MetricsEnum>()
            .ToList();
            
        metricList.Remove(EvaluationAggregationQuery.MetricsEnum.OutdatedSdkVersion);

        var groupBy = new List<EvaluationAggregationQuery.GroupByEnum>
        {
            EvaluationAggregationQuery.GroupByEnum.Conversationid,
            EvaluationAggregationQuery.GroupByEnum.Evaluationid,
            EvaluationAggregationQuery.GroupByEnum.Userid
        };

        return new EvaluationAggregationQuery
        {
            Filter = filter,
            GroupBy = groupBy,
            Interval = interval,
            Metrics = metricList,
            Granularity = "PT24H",
            FlattenMultivaluedDimensions = true
        };
    }
}
namespace GenesysCloud.Queries.Reports.EvaluationReport;

public class GenesysEvaluationAggregateQuery
{
    private readonly MetricsInterval _interval;
    private readonly IReadOnlyCollection<string> _queueIds;
    private readonly IReadOnlyCollection<string> _divisionIds;

    public GenesysEvaluationAggregateQuery(MetricsInterval interval, IReadOnlyCollection<string> queues, IReadOnlyCollection<string> divisions)
    {
        _interval = interval ?? throw new ArgumentNullException(nameof(interval), "Interval cannot be null");
        _queueIds = queues ?? throw new ArgumentNullException(nameof(queues), "Queue Id's cannot be null (empty ok)");
        _divisionIds = divisions ?? throw new ArgumentNullException(nameof(divisions), "Divisions cannot be null (empty ok)");
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

        var clauses = new List<EvaluationAggregateQueryClause>
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
            Clauses = clauses,
            Type = EvaluationAggregateQueryFilter.TypeEnum.And
        };
        
        var metrics = Enum
            .GetValues(typeof(EvaluationAggregationQuery.MetricsEnum))
            .Cast<EvaluationAggregationQuery.MetricsEnum>()
            .ToList();
            
        metrics.Remove(EvaluationAggregationQuery.MetricsEnum.OutdatedSdkVersion);

        var groupBy = new List<EvaluationAggregationQuery.GroupByEnum>
        {
            EvaluationAggregationQuery.GroupByEnum.Conversationid,
            EvaluationAggregationQuery.GroupByEnum.Evaluationid,
            EvaluationAggregationQuery.GroupByEnum.Userid
        };

        return new EvaluationAggregationQuery
        {
            //Filter = filter,
            GroupBy = groupBy,
            Interval = interval,
            Metrics = metrics,
            Granularity = Constants.TwentyFourHourInterval,
            FlattenMultivaluedDimensions = true
        };
    }
}
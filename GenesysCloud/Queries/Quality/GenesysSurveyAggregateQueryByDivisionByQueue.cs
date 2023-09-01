namespace GenesysCloud.Queries.Quality;

public class GenesysSurveyAggregateQueryByDivisionByQueue
{
    private readonly IntervalSpan _interval;
    private readonly IReadOnlyCollection<string> _queueIds;
    private readonly IReadOnlyCollection<string> _divisionIds;

    public GenesysSurveyAggregateQueryByDivisionByQueue(IntervalSpan interval, IReadOnlyCollection<string> queueIds, IReadOnlyCollection<string> divisions)
    {
        _interval = interval;
        _queueIds = queueIds;
        _divisionIds = divisions;
    }
    
    public SurveyAggregationQuery Build()
    {
        var interval = _interval.ToGenesysInterval;
        
        var divisionPredicates = _divisionIds.Select(divisionId => 
                new SurveyAggregateQueryPredicate
                {
                    Dimension = SurveyAggregateQueryPredicate.DimensionEnum.Divisionid,
                    Operator = SurveyAggregateQueryPredicate.OperatorEnum.Matches,
                    Value = divisionId,
                    Type = SurveyAggregateQueryPredicate.TypeEnum.Dimension
                })
            .ToList();
        
        var queuePredicates = _queueIds.Select(queueId =>
                new SurveyAggregateQueryPredicate
                {
                    Dimension = SurveyAggregateQueryPredicate.DimensionEnum.Queueid,
                    Operator = SurveyAggregateQueryPredicate.OperatorEnum.Matches,
                    Value = queueId,
                    Type = SurveyAggregateQueryPredicate.TypeEnum.Dimension
                })
            .ToList();

        var queryClause = new List<SurveyAggregateQueryClause>
        {
            new()
            {
                Predicates = divisionPredicates,
                Type = SurveyAggregateQueryClause.TypeEnum.Or
            },
            new()
            {
                Predicates = queuePredicates,
                Type = SurveyAggregateQueryClause.TypeEnum.Or
            }
        };

        var filter = new SurveyAggregateQueryFilter
        {
            Clauses = queryClause,
            Type = SurveyAggregateQueryFilter.TypeEnum.And
        };
        
        var metricList = Enum
            .GetValues(typeof(SurveyAggregationQuery.MetricsEnum))
            .Cast<SurveyAggregationQuery.MetricsEnum>()
            .ToList();
            
        metricList.Remove(SurveyAggregationQuery.MetricsEnum.OutdatedSdkVersion);
        
        var groupBy = new List<SurveyAggregationQuery.GroupByEnum>
        {
            SurveyAggregationQuery.GroupByEnum.Queueid,
            SurveyAggregationQuery.GroupByEnum.Userid,
            SurveyAggregationQuery.GroupByEnum.Surveyerrorreason
        };

        return new SurveyAggregationQuery
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
namespace GenesysCloud.Queries.Reports.SurveyReport;

public class GenesysSurveyAggregateQueryByDivisionByQueue
{
    private readonly MetricsInterval _interval;
    private readonly IReadOnlyCollection<string> _queueIds;
    private readonly IReadOnlyCollection<string> _divisionIds;
    
    /// <summary>
    /// Builds queries for surveys reports.
    /// For more details, refer to the official documentation: 
    /// <see href="https://developer.genesys.cloud/useragentman/quality/"/>
    /// </summary>
    public GenesysSurveyAggregateQueryByDivisionByQueue(MetricsInterval interval, IReadOnlyCollection<string> queueIds, IReadOnlyCollection<string> divisions)
    {
        _interval = interval ?? throw new ArgumentNullException(nameof(interval), "Interval cannot be null");
        _queueIds = queueIds ?? throw new ArgumentNullException(nameof(queueIds), "Queue Id's cannot be null (use Array.Empty<string>())");
        _divisionIds = divisions ?? throw new ArgumentNullException(nameof(divisions), "Divisions cannot be null (use Array.Empty<string>())");
    }
    
    /// <summary>
    /// Builds a Survey Aggregate Query.
    /// This is a starting point to find survey ids for agents who had a survey within a time period
    /// </summary>
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

        var clauses = new List<SurveyAggregateQueryClause>
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
            Clauses = clauses,
            Type = SurveyAggregateQueryFilter.TypeEnum.And
        };
        
        var metrics = Enum
            .GetValues(typeof(SurveyAggregationQuery.MetricsEnum))
            .Cast<SurveyAggregationQuery.MetricsEnum>()
            .ToList();
            
        metrics.Remove(SurveyAggregationQuery.MetricsEnum.OutdatedSdkVersion);
        
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
            Metrics = metrics,
            Granularity = Constants.TwentyFourHourInterval,
            FlattenMultivaluedDimensions = true
        };
    }
}
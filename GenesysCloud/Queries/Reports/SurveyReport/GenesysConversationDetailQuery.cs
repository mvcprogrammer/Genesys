namespace GenesysCloud.Queries.Reports.SurveyReport;

public sealed class GenesysConversationDetailQuery
{
    private readonly MetricsInterval _interval;
    private readonly string[] _conversationIds;
    
    public GenesysConversationDetailQuery(MetricsInterval interval, string[] conversationIds)
    {
        _interval = interval ?? throw new ArgumentNullException(nameof(interval), "Interval cannot be null");
        _conversationIds = conversationIds ?? throw new ArgumentNullException(nameof(conversationIds), "Conversation Ids cannot be null. (empty ok)");
    }

    public ConversationQuery Build()
    {
        var interval = _interval.ToGenesysInterval;
        
        var conversationPredicates = _conversationIds.Select(conversationId =>
            new ConversationDetailQueryPredicate
            {
                Dimension = ConversationDetailQueryPredicate.DimensionEnum.Conversationid,
                Operator = ConversationDetailQueryPredicate.OperatorEnum.Matches,
                Value = conversationId
            })
            .ToList();

        var clauses = new List<ConversationDetailQueryClause>
        {
            new()
            {
                Predicates = conversationPredicates,
                Type = ConversationDetailQueryClause.TypeEnum.Or
            }
        };

        var filter = new List<ConversationDetailQueryFilter>
        {
            new()
            {
                Clauses = clauses,
                Type = ConversationDetailQueryFilter.TypeEnum.And
            }
        };
        
        var paging = new PagingSpec
        {
            PageNumber = 1,
            PageSize = 100
        };

        return new ConversationQuery
        {
            Interval = interval,
            ConversationFilters = filter,
            Paging = paging
        };  
    }
}
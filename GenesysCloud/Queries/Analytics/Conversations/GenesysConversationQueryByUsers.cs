namespace GenesysCloud.Queries.Analytics.Conversations;

public sealed class GenesysConversationQueryByUsers
{
    private readonly IntervalSpan _interval;
    private readonly string[] _conversationIds;
    
    public GenesysConversationQueryByUsers(IntervalSpan interval, string[] conversationIds)
    {
        _interval = interval;
        _conversationIds = conversationIds;
    }

    public ConversationQuery Build()
    {
        var interval = _interval.ToGenesysInterval;
        var conversationPredicates = new ConversationDetailQueryPredicate
        {
        };

        return new ConversationQuery
        {
        };  
    }
}
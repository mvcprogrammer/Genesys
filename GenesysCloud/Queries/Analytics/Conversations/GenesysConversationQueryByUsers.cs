namespace GenesysCloud.Queries.Analytics.Conversations;

public sealed class GenesysConversationQueryByUsers
{
    private readonly MetricsInterval _interval;
    private readonly string[] _conversationIds;
    
    public GenesysConversationQueryByUsers(MetricsInterval interval, string[] conversationIds)
    {
        _interval = interval ?? throw new ArgumentNullException(nameof(interval), "Interval cannot be null");
        _conversationIds = conversationIds ?? throw new ArgumentNullException(nameof(conversationIds), "Conversation Ids cannot be null. (empty ok)");
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
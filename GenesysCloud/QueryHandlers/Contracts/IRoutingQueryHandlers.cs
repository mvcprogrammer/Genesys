namespace GenesysCloud.QueryHandlers.Contracts;

public interface IRoutingQueryHandlers
{
    public QueueEntityListing GetRoutingQueues(List<string> divisions);
}
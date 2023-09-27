using GenesysCloud.DTO.Response;

namespace GenesysCloud.Services.Contracts.Fundamental;

public interface IRoutingService
{
    public QueueEntityListing GetRoutingQueues(List<string> divisions);
    public Dictionary<string, QueueProfile> GetQueueProfileLookup(List<string> divisions);
}
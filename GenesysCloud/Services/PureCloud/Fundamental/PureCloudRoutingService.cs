using GenesysCloud.DTO.Response;
using GenesysCloud.QueryHandlers.Contracts;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Static;

namespace GenesysCloud.Services.PureCloud.Fundamental;

public class PureCloudRoutingService : IRoutingService
{
    private readonly IRoutingQueryHandlers _routingQueryHandlers;

    public PureCloudRoutingService(IRoutingQueryHandlers routingQueryHandlers)
    {
        _routingQueryHandlers = routingQueryHandlers ?? throw new ArgumentException("Routing Query Handler cannot be null.");
    }
    
    public QueueEntityListing GetRoutingQueues(List<string> divisions)
    {
        return AuthorizedAction(() => ServiceResponse.LogAndReturnResponse(_routingQueryHandlers.GetRoutingQueues(divisions)));
    }

    public Dictionary<string, QueueProfile> GetQueueProfileLookup(List<string> divisions)
    {
        var response = GetRoutingQueues(divisions);

        var queueProfileLookup = response.Entities.ToDictionary(queue =>
            queue.Id, queue => new QueueProfile { Group = string.Empty, QueueName = queue.Name });

        return ServiceResponse.LogAndReturnResponse(queueProfileLookup, stackTraceIndex: 3);
    }
    
    /// <summary>
    /// This method ensures all calls have authorization and handles not authorized responses.
    /// <param name="action">
    /// This delegate will invoke its action if authorized=true
    /// </param>
    /// </summary>
    private static T AuthorizedAction<T>(Func<T> action)
    {
        if (AuthorizeService.IsAuthorized())
            return action();
        
        throw new UnauthorizedAccessException(Constants.NotAuthorized);
    }
}
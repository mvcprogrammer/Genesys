using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

public class PureCloudRoutingQueryHandlers : IRoutingQueryHandlers
{
    private readonly RoutingApi _routingApi = new();
    
    public QueueEntityListing GetRoutingQueues(List<string> divisions)
    {
        try
        {
            return _routingApi.GetRoutingQueues(divisionId: divisions);
        }
        catch (Exception exception)
        {
            ServiceResponse.ExceptionHandler.HandleException<QueueEntityListing>(exception);
            throw;
        }
    }
}
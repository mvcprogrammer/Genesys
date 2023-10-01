using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

/// <summary>
/// PureCloud Routing Query Handler is responsible for submitting an PureCloud.Client.V2.Model Routing query to the PureCloud API.
/// The API can be called multiple times if the response is paged.
/// Handlers are not dependent on query filters; 1 handler, many queries.
/// The query handler always returns a Genesys Client.V2.Model in the data field of a ServiceResponse.
/// DTO must never be done at this level for dependency segmentation.
/// </summary>
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
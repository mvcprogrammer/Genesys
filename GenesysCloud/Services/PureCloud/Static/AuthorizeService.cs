using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;

namespace GenesysCloud.Services.PureCloud.Static;

/// <summary>
/// Gets Access Token from Genesys *** NEVER ALLOW OTHER ASSEMBLIES ACCESS ***
/// </summary>
internal static class AuthorizeService
{
    public static ServiceResponse<bool> Authorize(string clientId, string clientSecret, PureCloudRegionHosts cloudRegion)
    {
        Configuration.Default.ApiClient.setBasePath(cloudRegion);

        try
        {
            Configuration.Default.ApiClient.PostToken(clientId, clientSecret);
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<bool>(exception);
        }

        return SystemResponse.SuccessResponse(true);
    }
}
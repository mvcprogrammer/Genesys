using System.Configuration;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;
using Configuration = PureCloudPlatform.Client.V2.Client.Configuration;

namespace GenesysCloud.Services.PureCloud.Static;

/// <summary>
/// Gets Access Token from Genesys *** NEVER ALLOW OTHER ASSEMBLIES ACCESS ***
/// </summary>
internal static class AuthorizeService
{
    private static bool _isAuthorized;

    public static bool IsAuthorized()
    {
        //if (_isAuthorized)
        //    return true;
        
        var clientId = ConfigurationManager.AppSettings["ClientId"] ?? string.Empty;
        var clientSecret = ConfigurationManager.AppSettings["ClientSecret"] ?? string.Empty;
        
        var isAuthorized = Authorize(clientId: clientId, clientSecret: clientSecret, cloudRegion: PureCloudRegionHosts.eu_west_2);

        _isAuthorized = isAuthorized;
        return isAuthorized;
    }

    private static bool Authorize(string clientId, string clientSecret, PureCloudRegionHosts cloudRegion)
    {
        Configuration.Default.ApiClient.setBasePath(cloudRegion);

        try
        {
            Configuration.Default.ApiClient.PostToken(clientId, clientSecret);
        }
        catch (ApiException exception)
        {
            return ServiceResponse.LogFailure<bool>(exception.ErrorContent, exception.ErrorCode);
        }

        return ServiceResponse.LogAndReturnResponse(true);
    }
}
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;

namespace GenesysCloud.Services.PureCloud.Static;

/// <summary>
/// Gets Access Token from Genesys *** NEVER ALLOW OTHER ASSEMBLIES ACCESS ***
/// </summary>
internal static class AuthorizeService
{
    private static bool _isAuthorized = false;

    public static bool IsAuthorized()
    {
        if (_isAuthorized)
            return true;
        
        var isAuthorized = Authorize(clientId: "6cad8911-28ca-40ee-97f5-01136dba9087",
            clientSecret: "44hAG2qlkWCCfUVHU7xnZgL323OyaQ7KKIi297s25eY",
            cloudRegion: PureCloudRegionHosts.eu_west_2);

        _isAuthorized = isAuthorized;
        return _isAuthorized;
    }
    public static bool Authorize(string clientId, string clientSecret, PureCloudRegionHosts cloudRegion)
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
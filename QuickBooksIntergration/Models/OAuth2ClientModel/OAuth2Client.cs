using System.Collections.Generic;
using Intuit.Ipp.OAuth2PlatformClient;

namespace QuickBooksIntergration.Models.OAuth2ClientModel
{
    public static class OAuth2ClientModel
    {
        public static string OAuth2Client(List<OidcScopes> scopes)
        {
            OAuth2Client client = new OAuth2Client(clientID: ConfigurationTuples.ReturnIntuitAppSettings().clientId, clientSecret: ConfigurationTuples.ReturnIntuitAppSettings().clientSecret, redirectURI: ConfigurationTuples.ReturnIntuitAppSettings().redirectUrl, environment: ConfigurationTuples.ReturnIntuitAppSettings().appEnvironment);

            //AS OFF 24 Nov 2021, DO NOT UPGRADE Serilog as it throws the following error: System.MissingMethodException: Method not found:
            return client.GetAuthorizationURL(scopes:scopes);
        }
        
    }
}
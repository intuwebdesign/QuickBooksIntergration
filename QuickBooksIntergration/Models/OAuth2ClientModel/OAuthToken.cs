using System;
using System.IO;
using System.Web;
using Intuit.Ipp.OAuth2PlatformClient;
using Newtonsoft.Json;
using SystemTask = System.Threading.Tasks.Task;

namespace QuickBooksIntergration.Models.OAuth2ClientModel
{
    public class OAuthToken
    {
        public void GetOAuthToken(string code, string realmId)
        {
            OAuth2Client client                     = new OAuth2Client(clientID: ConfigurationTuples.ReturnIntuitAppSettings().clientId, clientSecret: ConfigurationTuples.ReturnIntuitAppSettings().clientSecret, redirectURI: ConfigurationTuples.ReturnIntuitAppSettings().redirectUrl, environment: ConfigurationTuples.ReturnIntuitAppSettings().appEnvironment);
            var intuitResponse                      = SystemTask.Run(() => client.GetBearerTokenAsync(code)).Result;

            DateTime dtAccessTokenExpiresIn         = DateTime.Now.AddSeconds(intuitResponse.AccessTokenExpiresIn);  //Valid for 60 minutes
            DateTime dtAccessRefreshTokenExpires    = DateTime.Now.AddSeconds(intuitResponse.RefreshTokenExpiresIn); //Valid for 100 days
            
            //save access token and refresh token to disk
            TokenModel model = new TokenModel
            {
                AccessToken                 = intuitResponse.AccessToken,
                AccessTokenExpires          = dtAccessTokenExpiresIn,
                AccessRefreshToken          = intuitResponse.RefreshToken,
                AccessRefreshTokenExpires   = dtAccessRefreshTokenExpires,
                RealmId                     = realmId
            };

            var dir         = HttpContext.Current.Server.MapPath("~/IntuitToken");
            var file        = Path.Combine(dir, "intuitToken.json");
            string serilizeToken   = JsonConvert.SerializeObject(model);
            File.WriteAllText(file, serilizeToken);
        }

        public static bool DoesIntuitTokenFileExist()
        {
            var dir = HttpContext.Current.Server.MapPath("~/IntuitToken");
            var file = Path.Combine(dir, "intuitToken.json");

            bool fileExists = File.Exists(file);

            return fileExists;
        }

        public static TokenModel RetrieveToken()
        {
            var dir         = HttpContext.Current.Server.MapPath("~/IntuitToken");
            var file        = Path.Combine(dir, "intuitToken.json");

            using (StreamReader streamReader = new StreamReader(file))
            {
                string json     = streamReader.ReadToEnd();
                var token       = JsonConvert.DeserializeObject<TokenModel>(json);
                return token;
            }
        }
    }
}
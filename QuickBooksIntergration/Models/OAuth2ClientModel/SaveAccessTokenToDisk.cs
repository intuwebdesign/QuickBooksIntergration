using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace QuickBooksIntergration.Models.OAuth2ClientModel
{
    public static class SaveAccessTokenToDisk
    {
        public static void SaveTokensToDisk(string accessToken, long accessTokenExpiresIn, string refreshToken, long refreshTokenExpires, string realmId)
        {
            DateTime dtAccessTokenExpiresIn         = DateTime.Now.AddSeconds(accessTokenExpiresIn);
            DateTime dtAccessRefreshTokenExpires    = DateTime.Now.AddSeconds(refreshTokenExpires);

            //save access token and refresh token to disk
            TokenModel model = new TokenModel
            {
                AccessToken                 = accessToken,
                AccessTokenExpires          = dtAccessTokenExpiresIn,
                AccessRefreshToken          = refreshToken,
                AccessRefreshTokenExpires   = dtAccessRefreshTokenExpires,
                RealmId                     = realmId
            };

            var dir          = HttpContext.Current.Server.MapPath("~/IntuitToken");
            var file         = Path.Combine(dir, "intuitToken.json");
            string serilizeToken    = JsonConvert.SerializeObject(model);
            File.WriteAllText(file, serilizeToken);
        }
    }
}
using System;

namespace QuickBooksIntergration.Models.OAuth2ClientModel
{
    public class TokenModel
    {
        public string AccessToken                   { get; set; }
        public DateTime AccessTokenExpires          { get; set; }
        public string AccessRefreshToken            { get; set; }
        public DateTime AccessRefreshTokenExpires   { get; set; }
        public string RealmId                       { get; set; }
    }
}
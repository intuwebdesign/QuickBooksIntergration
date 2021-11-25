using System.Configuration;

namespace QuickBooksIntergration.Models
{
    public static class ConfigurationTuples
    {
        public static (string clientId, string clientSecret, string redirectUrl, string appEnvironment) ReturnIntuitAppSettings()
        {
            string clientId         = ConfigurationManager.AppSettings["clientId"];
            string clientSecret     = ConfigurationManager.AppSettings["clientSecret"];
            string redirectUrl      = ConfigurationManager.AppSettings["redirectUrl"];
            string appEnvironment   = ConfigurationManager.AppSettings["appEnvironment"];

            return (clientId, clientSecret, redirectUrl, appEnvironment);
        }
    }
}
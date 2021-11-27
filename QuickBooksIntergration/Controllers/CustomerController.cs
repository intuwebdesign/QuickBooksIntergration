using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Intuit.Ipp.OAuth2PlatformClient;
using QuickBooksIntergration.Models;
using QuickBooksIntergration.Models.CustomerModel;
using QuickBooksIntergration.Models.OAuth2ClientModel;

namespace QuickBooksIntergration.Controllers
{
    public class CustomerController : Controller
    {
        [HttpGet]
        public ActionResult NewCustomer()
        {
            try
            {
                bool doesFileExist = OAuthToken.DoesIntuitTokenFileExist();

                if (!doesFileExist) return null;

                var token = OAuthToken.RetrieveToken();

                if (token.AccessTokenExpires < DateTime.Now)
                {
                    OAuth2Client client     = new OAuth2Client(clientID: ConfigurationTuples.ReturnIntuitAppSettings().clientId, clientSecret: ConfigurationTuples.ReturnIntuitAppSettings().clientSecret, redirectURI: ConfigurationTuples.ReturnIntuitAppSettings().redirectUrl, environment: ConfigurationTuples.ReturnIntuitAppSettings().appEnvironment);
                    var newToken            = Task.Run(() => client.RefreshTokenAsync(token.AccessRefreshToken)).Result;

                    SaveAccessTokenToDisk.SaveTokensToDisk(newToken.AccessToken, newToken.AccessTokenExpiresIn, newToken.RefreshToken, newToken.RefreshTokenExpiresIn, token.RealmId);
                }

                return View();
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }

        [HttpPost]
        public ActionResult NewCustomer(CustomerViewModel model)
        {
            try
            {
                //You would do any validation and whatever else required here before submitting
                var invoiceNumber = IntuitNewCustomer.CreateNewCustomer(model);
                TempData["InvoiceNumber"] = $"Invoice created, the number is {invoiceNumber}";
                return View();
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
    }
}
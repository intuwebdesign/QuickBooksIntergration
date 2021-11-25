using System.Collections.Generic;
using System.Web.Mvc;
using Intuit.Ipp.OAuth2PlatformClient;
using QuickBooksIntergration.Models.OAuth2ClientModel;

namespace QuickBooksIntergration.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authenticate()
        {
            List<OidcScopes> scopes = new List<OidcScopes>
            {
                OidcScopes.Accounting
            };

            string authorizeUrl = OAuth2ClientModel.OAuth2Client(scopes);
            return Redirect(authorizeUrl);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult CallBack(string code = "none", string realmId = "none")
        {

            OAuthToken token = new OAuthToken();

            token.GetOAuthToken(code, realmId);

            return View();
        }
    }
}
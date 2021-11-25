using System;
using System.IO;
using System.Web;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Newtonsoft.Json;
using System.Linq;
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

           

            OAuth2RequestValidator oAuth2RequestValidator = new OAuth2RequestValidator(intuitResponse.AccessToken);
            ServiceContext serviceContext = new ServiceContext(realmId: realmId, IntuitServicesType.QBO,oAuth2RequestValidator);
            serviceContext.IppConfiguration.MinorVersion.Qbo = "62";
            serviceContext.IppConfiguration.BaseUrl.Qbo = "https://sandbox-quickbooks.api.intuit.com/";


            QueryService<Customer> customerQueryService = new QueryService<Customer>(serviceContext);

            DataService dataService = new DataService(serviceContext);


            Customer customer = new Customer
            {
                CompanyName = "Company Name",
                DisplayName = "Display Name",
                Title = "Mr",
                FamilyName = "Family Name",
                GivenName = "Given Name",
                MiddleName = "Middle Name",
                BillAddr = new PhysicalAddress
                {
                    Line1 = "Line 1",
                    City = "Washington",
                    CountrySubDivisionCode = "Tyne Wear",
                    PostalCode = "NE36 9PP",
                    Country = "United Kingdom"
                },
                PrimaryEmailAddr = new EmailAddress
                {
                    Address = "test@test.com"
                },
                PrimaryPhone = new TelephoneNumber
                {
                    FreeFormNumber = "+44191789456"
                }
            };

            dataService.Add(customer);

            //Sanity check that the customer was created.
            Customer customerCount = customerQueryService.ExecuteIdsQuery("SELECT * FROM Customer WHERE PrimaryEmailAddr = 'test@test.com'").FirstOrDefault();
        }
    }
}
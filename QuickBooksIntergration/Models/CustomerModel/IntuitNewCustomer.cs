using System;
using System.Configuration;
using System.Linq;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using QuickBooksIntergration.Models.CustomerInvoice;
using QuickBooksIntergration.Models.OAuth2ClientModel;

namespace QuickBooksIntergration.Models.CustomerModel
{
    public static class IntuitNewCustomer
    {
        public static string CreateNewCustomer(CustomerViewModel model)
        {
            try
            {
                bool doesFileExist = OAuthToken.DoesIntuitTokenFileExist();

                if (!doesFileExist) return null;

                var token = OAuthToken.RetrieveToken();

                string baseUrl = ConfigurationManager.AppSettings["baseUrl"];

                OAuth2RequestValidator oAuth2RequestValidator       = new OAuth2RequestValidator(token.AccessToken);
                ServiceContext serviceContext                       = new ServiceContext(realmId: token.RealmId, IntuitServicesType.QBO, oAuth2RequestValidator);
                serviceContext.IppConfiguration.MinorVersion.Qbo    = "62";
                serviceContext.IppConfiguration.BaseUrl.Qbo         = baseUrl;

                string displayName = $"{model.CompanyName}-{model.PostCode}";

                Customer customer = new Customer
                {
                    CompanyName         = model.CompanyName,
                    DisplayName         = displayName, //Must be unique
                    Title               = model.Title,
                    FamilyName          = model.FamilyName,
                    GivenName           = model.GivenName,
                    MiddleName          = model.MiddleName,
                    BillAddr = new PhysicalAddress
                    {
                        Line1                   = model.AddressLineOne,
                        City                    = model.City,
                        CountrySubDivisionCode  = model.County,
                        PostalCode              = model.PostCode,
                        Country                 = model.Country
                    },
                    ShipAddr = new PhysicalAddress
                    {
                        Line1                   = model.AddressLineOne,
                        City                    = model.City,
                        CountrySubDivisionCode  = model.County,
                        PostalCode              = model.PostCode,
                        Country                 = model.Country
                    },
                    PrimaryEmailAddr = new EmailAddress
                    {
                        Address = model.Email
                    },
                    PrimaryPhone = new TelephoneNumber
                    {
                        FreeFormNumber = model.PhoneNumber
                    }
                };

                QueryService<Customer> customerQueryService = new QueryService<Customer>(serviceContext);

                DataService dataService = new DataService(serviceContext);

                dataService.Add(customer);

                //Create our SQL select case to get customer by DisplayName
                string whereClause = " WHERE DisplayName = '" + customer.DisplayName + "'";
                string selectQuery = "SELECT * FROM Customer" + whereClause;

                //Now get the customer as we will need it for the invoice.
                Customer customerDetails = customerQueryService.ExecuteIdsQuery(selectQuery).FirstOrDefault();
                //List<Customer> customers = customerQueryService.ExecuteIdsQuery("SELECT * FROM Customer").ToList();

                var invoice             = CreateInvoice.GetInvoice(customerDetails);

                //Can also use AddAsync if required
                Invoice invoiceAdded    = dataService.Add(invoice);
                string invoiceNumber    = invoiceAdded.DocNumber;//Get the invoice number

                //Now send email, can also use SendEmailAsync if required
                dataService.SendEmail(invoiceAdded);

                return invoiceNumber;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
    }
}
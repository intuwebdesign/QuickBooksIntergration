using System.ComponentModel.DataAnnotations;

namespace QuickBooksIntergration.Models.CustomerModel
{
    public class CustomerViewModel
    {
        [Display(Name = "Company Name")]
        public string CompanyName       { get; set; }
        public string DisplayName       { get; set; }
        [Display(Name = "Title")]
        public string Title             { get; set; }
        [Display(Name = "Surname")]
        public string FamilyName        { get; set; }
        [Display(Name = "First Name")]
        public string GivenName         { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName        { get; set; }
        [Display(Name = "Address Line 1")]
        public string AddressLineOne    { get; set; }
        [Display(Name = "City")]
        public string City              { get; set; }
        [Display(Name = "County/Region")]
        public string County            { get; set; }
        [Display(Name = "Post-Code/Zip")]
        public string PostCode          { get; set; }
        [Display(Name = "Country")]
        public string Country           { get; set; }
        [Display(Name = "Email")]
        public string Email             { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber       { get; set; }
    }
}
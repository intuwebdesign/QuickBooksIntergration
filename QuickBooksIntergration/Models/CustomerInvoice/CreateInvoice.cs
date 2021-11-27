using System;
using System.Collections.Generic;
using Intuit.Ipp.Data;

namespace QuickBooksIntergration.Models.CustomerInvoice
{
    public static class CreateInvoice
    {
        public static Invoice GetInvoice(Customer customer)
        {
            PhysicalAddress physicalAddress = new PhysicalAddress
            {
                Line1                   = customer.BillAddr.Line1,
                City                    = customer.BillAddr.City,
                CountrySubDivisionCode  = customer.BillAddr.CountrySubDivisionCode,
                PostalCode              = customer.BillAddr.PostalCode,
                Country                 = customer.BillAddr.Country
            };

            Invoice invoice = new Invoice
            {
                Deposit             = 0m,
                DepositSpecified    = true,

                CustomerRef = new ReferenceType
                {
                    name    = customer.DisplayName,
                    Value   = customer.Id
                },
                DueDate                         = DateTime.Now.AddDays(14).Date,
                DueDateSpecified                = true,
                TotalAmt                        = 3000m,
                TotalAmtSpecified               = true,
                ApplyTaxAfterDiscount           = false,
                ApplyTaxAfterDiscountSpecified  = true,
                PrintStatus                     = PrintStatusEnum.NotSet,
                PrintStatusSpecified            = true,
                EmailStatus                     = EmailStatusEnum.NeedToSend,
                EmailStatusSpecified            = true
            };

            EmailAddress billEmailAddress = new EmailAddress
            {
                Address             = customer.PrimaryEmailAddr.Address,
                Default             = true,
                DefaultSpecified    = true
            };

            invoice.BillEmail   = billEmailAddress;
            invoice.BillAddr    = physicalAddress;
            invoice.ShipAddr    = physicalAddress;

            invoice.Balance             = 3000m;
            invoice.BalanceSpecified    = true;

            invoice.TxnDate             = DateTime.Now.Date;
            invoice.TxnDateSpecified    = true;

            //Hard coded values, would really come from whatever system you are using to create sales item
            List<Line> lines = new List<Line>();
            Line line = new Line
            {
                Description         = "Consulation Service",
                Amount              = 3000m,
                AmountSpecified     = true,
                DetailType          = LineDetailTypeEnum.SalesItemLineDetail,
                DetailTypeSpecified = true
            };

            SalesItemLineDetail salesItemLineDetail = new SalesItemLineDetail
            {
                ItemElementName = ItemChoiceType.UnitPrice,

                Qty             = 1,
                QtySpecified    = true,

                TaxCodeRef = new ReferenceType
                {
                    Value = "TAX"
                },
                AnyIntuitObject = 3000m
            };

            line.AnyIntuitObject = salesItemLineDetail;

            lines.Add(line);

            invoice.Line = lines.ToArray();

            return invoice;
        }
    }
}
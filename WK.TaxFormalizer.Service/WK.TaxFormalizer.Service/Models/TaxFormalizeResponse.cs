using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WK.TaxFormalizer.Service.Models
{
    public class TaxFormalizeResponse
    {
        public decimal AppliedSalesTax { get; set; }
        public decimal ToBeAppliedSalesTax { get; set; }
        public string CompanyName { get; set; }
    }
}
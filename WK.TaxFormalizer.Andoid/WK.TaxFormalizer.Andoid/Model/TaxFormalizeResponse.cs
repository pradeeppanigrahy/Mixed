

namespace WK.TaxFormalizer.Andoid.Models
{
    public class TaxFormalizeResponse
    {
        public decimal AppliedSalesTax { get; set; }
        public decimal ToBeAppliedSalesTax { get; set; }
        public string CompanyName { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WK.TaxFormalizer.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransTax2
    {
        public int TransactionID { get; set; }
        public int TaxID { get; set; }
        public Nullable<int> TaxAuthorityID { get; set; }
        public string TaxAuthorityName { get; set; }
        public string AuthorityTypeDescription { get; set; }
        public string TaxType { get; set; }
        public string TaxCat { get; set; }
        public Nullable<int> EntitySurrogate { get; set; }
        public Nullable<decimal> TaxableAmount { get; set; }
        public Nullable<decimal> TaxableQuantity { get; set; }
        public Nullable<decimal> NonTaxableQty { get; set; }
        public Nullable<decimal> ExemptTaxableAmt { get; set; }
        public Nullable<decimal> NonTaxableAmt { get; set; }
        public Nullable<decimal> ExemptTaxableQty { get; set; }
        public Nullable<decimal> TaxApplied { get; set; }
        public Nullable<decimal> FeeApplied { get; set; }
        public Nullable<decimal> TaxableTaxAmount { get; set; }
        public Nullable<int> BasicDataID { get; set; }
        public string PassFlagDescription { get; set; }
        public string PassTypeDescription { get; set; }
        public string BaseTypeDescription { get; set; }
        public Nullable<int> ReportingDataID { get; set; }
        public Nullable<int> ReportToAuthority { get; set; }
    }
}

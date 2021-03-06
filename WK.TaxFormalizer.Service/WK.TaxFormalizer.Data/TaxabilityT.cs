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
    
    public partial class TaxabilityT
    {
        public int TaxabilityID { get; set; }
        public Nullable<int> TaxID { get; set; }
        public string ProductGroup { get; set; }
        public string Item { get; set; }
        public string Customer { get; set; }
        public string Provider { get; set; }
        public string TransactionType { get; set; }
        public Nullable<int> OverrideReasonB { get; set; }
        public Nullable<System.DateTime> LEGALEFFECTIVEDATE { get; set; }
        public Nullable<int> ReleaseNo { get; set; }
        public string TAXABLE { get; set; }
        public Nullable<System.DateTime> LEGALEXPIRATIONDATE { get; set; }
        public string PercentTaxableType { get; set; }
        public Nullable<decimal> PercentTaxable { get; set; }
    
        public virtual CustomerT CustomerT { get; set; }
        public virtual ProviderT ProviderT { get; set; }
        public virtual ReleaseT ReleaseT { get; set; }
        public virtual TransactionTypeT TransactionTypeT { get; set; }
        public virtual TaxAuthorityTaxT TaxAuthorityTaxT { get; set; }
    }
}

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
    
    public partial class CustomerT
    {
        public CustomerT()
        {
            this.TaxabilityStateTs = new HashSet<TaxabilityStateT>();
            this.TaxabilityTs = new HashSet<TaxabilityT>();
        }
    
        public string Customer { get; set; }
        public string CustomerDescription { get; set; }
        public string CustomerXRef { get; set; }
        public Nullable<int> ReleaseNo { get; set; }
    
        public virtual ReleaseT ReleaseT { get; set; }
        public virtual ICollection<TaxabilityStateT> TaxabilityStateTs { get; set; }
        public virtual ICollection<TaxabilityT> TaxabilityTs { get; set; }
    }
}

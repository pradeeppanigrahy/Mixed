
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// Class holds line item taxability information
    /// </summary>
    [Serializable]
    public class TransactionTaxabilityModel
    {
       
        public string TransactionType { get; set; }
        public string TransactionTypeDescription { get; set; }

        
        public string ProviderType { get; set; }
        public string ProviderTypeDescription { get; set; }

       
        public string CustomerOrUsageType { get; set; }
        public string CustomerOrUsageTypeDescription { get; set; }

        public string ExemptionClass { get; set; }
        public string ExemptionClassDescription { get; set; }
    }
}

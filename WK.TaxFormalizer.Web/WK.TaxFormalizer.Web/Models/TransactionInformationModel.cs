
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
     /// <summary>
     /// Transaction information model
     /// </summary>
    public class TransactionInformationModel
    {
        #region Transaction Information
        public long TransactionId { get; set; }
        public short? TransactionStatus { get; set; }
        public string TransactionStatusDescription { get; set; }
        public DateTime? TransactionDate { get; set; }        
        
        public string TransactionDescription { get; set; }
        
       
        public string InvoiceId { get; set; }
        
        
        public DateTime? InvoiceDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalTaxAmount { get; set; }
        public string CreatedBy { get; set; }
        public string LiabilityType { get; set; }
        public string LiabilityTypeDescription { get; set; }
        public string TestTransaction { get; set; }
        
        
        public string Customer { get; set; }
        
        public bool UnattributedReturn { get; set; }
        public bool FinalizeTransaction { get; set; }
        public bool IsTestTransaction { get; set; }
        
        /// <summary>
        /// Customer ID / Vendor ID field
        /// </summary>
        
        public string VendorID { get; set; }

        
        public string SourceSystem { get; set; }
        public long? OriginalTransactionId { get; set; }
        #endregion

        #region Taxability

       
        public string TransactionType { get; set; }
        public string TransactionTypeDescription { get; set; }

       
        public string ProviderType { get; set; }
        public string ProviderTypeDescription { get; set; }

        
        public string CustomerOrUsageType { get; set; }
        public string CustomerOrUsageTypeDescription { get; set; }

        public string ExemptionClass { get; set; }
        public string ExemptionClassDescription { get; set; }

        #endregion

        #region Situs Information
        public TransactionSitusInformationModel SitusInformationModel { get; set; }
        #endregion

    }
}

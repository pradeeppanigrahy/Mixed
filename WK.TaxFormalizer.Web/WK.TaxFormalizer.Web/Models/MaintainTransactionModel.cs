
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    public class MaintainTransactionModel
    {
        public int AccountSurrogate { get; set; }

        public int CompanySurrogate { get; set; }

        public int TransactionID { get; set; }

        public Nullable<int> DivisionSurrogate { get; set; }

        public string InvoiceID { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? InvoiceDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? TransactionDate { get; set; }

        public string SourceSystem { get; set; }

        public Nullable<short> TransactionStatus { get; set; }

        public string TransactionStatusDescription { get; set; }

        public string TransactionType { get; set; }

        public string LiabilityType { get; set; }

        public string LiabilityTypeDescription { get; set; }

        public string TestTransaction { get; set; }

        public string ShipToGeocode { get; set; }

        public Nullable<int> NXSShipToGeoID { get; set; }

        public Nullable<int> NXSShipFromGeoID { get; set; }

        public string CT39pA { get; set; }

        public Nullable<int> OrderPlacementGeoID { get; set; }

        public string CT39qA { get; set; }

        public Nullable<int> OrderApprovalGeoID { get; set; }

        public string CT39rA { get; set; }

        public string SitusSolicitedOutside { get; set; }

        public string SitusCanRejectDelivery { get; set; }

        public string SitusFOB { get; set; }

        public string SitusMailOrder { get; set; }

        public string SitusDeliveryBy { get; set; }

        public string SitusShipFromPOB { get; set; }

        public string CustomerID { get; set; }

        public string Customer { get; set; }

        public string Provider { get; set; }

        public Nullable<long> OriginalTransactionID { get; set; }

        public Nullable<int> SUID { get; set; }

        public string APPLICATION_LOGIN_ID { get; set; }

        public string TransactionDescription { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy H:mm:ss}")]
        public DateTime? CreationTime { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? TSCHG { get; set; }

        public Nullable<int> OrderApprovalAddressId { get; set; }

        public Nullable<int> OrderPlacementAddressId { get; set; }

        public Nullable<int> ShipFromAddressId { get; set; }

        public Nullable<int> ShipToAddressId { get; set; }

        public string TransactionReturnStatus { get; set; }
    }
}

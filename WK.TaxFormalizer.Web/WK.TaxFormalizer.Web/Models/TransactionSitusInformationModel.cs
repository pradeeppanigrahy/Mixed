
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// Class holds line item situs information
    /// </summary>
    [Serializable]
    public class TransactionSitusInformationModel
    {
        public bool IsSameAsHeader { get; set; }


        public string FOB { get; set; }
        public string FOBDescription { get; set; }
       
        public string MailOrder { get; set; }
        public string MailOrderDescription { get; set; }
       
        public string DeliveryBy { get; set; }
        public string DeliveryByDescription { get; set; }
        
        public string SolicitedOutside { get; set; }
        public string SolicitedOutsideDescription { get; set; }
        
        public string CanRejectDelivery { get; set; }
        public string CanRejectDeliveryDescription { get; set; }
        
        public string ShipFromPOB { get; set; }
        public string ShipFromPOBDescription { get; set; }

        #region Address Information
        public string ShipFromGeocode { get; set; }

        public string ShipToGeocode { get; set; }

        public string OrderPlacementGeocode { get; set; }

        public string OrderApprovalGeocode { get; set; } 
        #endregion

    }
}

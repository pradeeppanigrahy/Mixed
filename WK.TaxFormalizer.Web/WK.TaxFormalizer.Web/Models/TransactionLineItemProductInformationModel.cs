
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// Class holds line item product information
    /// </summary>
    [Serializable()]
    public class TransactionLineItemProductInformationModel
    {
        public int? LineItemID { get; set; }
        
      
        public string UsrLineItemID { get; set; }

        public decimal? OriginalQuantity { get; set; }

        

        public decimal? Quantity { get; set; }
        
        public string SkuCategory { get; set; }
        
        public string GroupId { get; set; }

        public string GroupDescription { get; set; }
        
        public string ItemId { get; set; }

        public string ItemDescription { get; set; }

        public decimal? OriginalUnitCharge { get; set; }
        
       
        public decimal? UnitCharge { get; set; }

        public decimal? OriginalLineItemAmount { get; set; }

        //[RegularExpression(@"(^\$?0*[0-9]{0,11}(\.[0-9]{0,3})?$)", ErrorMessage = "Line item amount must be a numeric/decimal value greater than 0 and equal to or less than 99,999,999,999.999")]
        public decimal? LineItemAmount { get; set; }

        public int? LocationSurrogate { get; set; }

        public string DataString { get; set; }

        public string Note { get; set; }

        public bool IsLineItemIdChanged { get; set; }

        /// <summary>
        /// Generates unique ID
        /// </summary>
        public string AutoGenerateID
        {
            get
            {
                return LineItemID.ToString() + Guid.NewGuid().ToString();
            }
        }
    }
}

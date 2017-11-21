
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// holds line item address information
    /// </summary>
    [Serializable]
    public class TransactionLineItemAddressModel
    {
        
        public string LineItemLocationId { get; set; }
        public string LineItemLocationDescription { get; set; }
        public bool IsSameAsHeader { get; set; }
        public AddressModel LineItemShipTo { get; set; }
        public AddressModel LineItemShipFrom { get; set; }
        public AddressModel LineItemOrderApproval { get; set; }
        public AddressModel LineItemOrderPlacement { get; set; }  
    }
}

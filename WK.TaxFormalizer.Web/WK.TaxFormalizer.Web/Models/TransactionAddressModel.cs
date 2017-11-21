
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// Class holds transaction address information
    /// </summary>
    [Serializable]
    public class TransactionAddressModel
    {
        public string LocationId { get; set; }
        public string LocationDescription { get; set; }
        public AddressModel ShipTo { get; set; }
        public AddressModel ShipFrom { get; set; }
        public AddressModel OrderApproval { get; set; }
        public AddressModel OrderPlacement { get; set; }               
    }
}

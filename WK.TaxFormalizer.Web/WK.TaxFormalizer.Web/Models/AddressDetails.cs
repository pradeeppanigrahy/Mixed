

using System;
namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// Address details model
    /// </summary>
   
    public class AddressDetails
    {
        public AddressModel ShipToAddress { get; set; }
        public AddressModel ShipFromAddress { get; set; }
        public AddressModel OrderApprovalAddress { get; set; }
        public AddressModel OrderPlacementAddress { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// holds transaction address information
    /// </summary>
    //[Serializable]
    public class AddressModel
    {
        public int AddressId { get; set; }

        //[StringLength(60,ErrorMessage = "Street Address value cannot exceed 60 characters. ")]
        public string StreetAddress { get; set; }
        //[StringLength(60, ErrorMessage = "Address2 value cannot exceed 60 characters. ")]
        public string Address2 { get; set; }

        //[StringLength(255, ErrorMessage = "City value cannot exceed 255 characters. ")]
        public string City { get; set; }
        //[StringLength(2, ErrorMessage = "State value cannot exceed 2 characters. ")]
        public string State { get; set; }
        public string StateDescription { get; set; }
        //[StringLength(2, ErrorMessage = "Country value cannot exceed 2 characters. ")]
        public string Country { get; set; }
        public string CountryDescription { get; set; }
        //[StringLength(75, ErrorMessage = "County value cannot exceed 75 characters. ")]
        public string County { get; set; }
        //[StringLength(15, ErrorMessage = "Zip value cannot exceed 15 characters. ")]
        //[RegularExpression(@"^([0-9]|[a-zA-Z0-9]|[a-zA-Z])*$", ErrorMessage = "Zip is invalid")]
        public string Zip { get; set; }
        /// <summary>
        /// Plus 4 digits of zip code
        /// </summary>
        //[StringLength(4, ErrorMessage = "Plus4 value cannot exceed 4 characters. ")]
        //[RegularExpression(@"^\d{4}", ErrorMessage = "Plus4 is invalid.")]
        public string Plus4 { get; set; }

        //[StringLength(12, ErrorMessage = "Geocode value cannot exceed 12 characters. ")]
        public string Geocode { get; set; }
    }
}

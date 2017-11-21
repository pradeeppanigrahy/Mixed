
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// Transaction model holds all the information related to a transaction
    /// </summary>
    public class TransactionModel
    {
        public int SUID { get; set; }

        public string CompanySurrogate { get; set; }

        public string LfeSurrogate { get; set; }

        /// <summary>
        /// holds transaction information
        /// </summary>
        public TransactionInformationModel TransactionInformation { get; set; }

        /// <summary>
        /// Holds transaction situs/nexus information
        /// </summary>
        public TransactionSitusInformationModel SitusInformationModel { get; set; }

        /// <summary>
        /// holds transaction line item details(product,taxability,situs/nexus,address)
        /// </summary>
        public List<TransactionLineItemModel> TransactionLineItems { get; set; }

        /// <summary>
        /// holds transaction address information
        /// </summary>
        public TransactionAddressModel TransactionAddressModel { get; set; }

    }
}

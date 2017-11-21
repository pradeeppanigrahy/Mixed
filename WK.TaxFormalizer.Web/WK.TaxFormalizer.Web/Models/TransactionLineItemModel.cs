
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// Transaction line item
    /// </summary>
    [Serializable()]
    public class TransactionLineItemModel
    {
        #region Product information
        public TransactionLineItemProductInformationModel TransactionLineItemProductInformationModel { get; set; }
        #endregion

        #region Taxability
        public TransactionTaxabilityModel TransactionTaxabilityModel { get; set; }
        #endregion

        #region Address Information
        public TransactionLineItemAddressModel TransactionLineItemAddressModel { get; set; }
        #endregion

        #region Situs Information
        public TransactionSitusInformationModel TransactionSitusInformationModel { get; set; }
        #endregion

        /// <summary>
        /// Generates unique ID
        /// </summary>
        public string AutoGenerateID
        {
            get
            {
                return  Guid.NewGuid().ToString();
            }
        }
    }
}

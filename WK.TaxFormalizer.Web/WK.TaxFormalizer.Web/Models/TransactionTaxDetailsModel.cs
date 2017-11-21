//
//  Copyright © 2015, CCH Incorporated. All rights reserved.
//
/*Created on : 21 Apr 2015 , Author: Pradeep Panigrahy */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// Transaction tax details
    /// </summary>
    public class TransactionTaxDetailsModel
    {
        public long TransactionId { get; set; }

        public string SystemOrCustom { get; set; }

        public string TaxAuthorityDescription { get; set; }

        public string TaxTypeDescription { get; set; }

        public string TaxCategoryDescription { get; set; }

        public string PassOnRule { get; set; }

        public string AppearsOnBillAs { get; set; }

        public string TaxIsImposedOn { get; set; }

        public decimal? TaxableTaxAmount { get; set; }

        public decimal? TaxApplied { get; set; }

        public decimal? TaxableSaleAmount { get; set; }

        public decimal? TaxableQuantity { get; set; }

        public decimal? FeeApplied { get; set; }

        public decimal? TaxExemptAmount { get; set; }

        public decimal? TaxExemptQuantity { get; set; }

        public decimal? NonTaxableAmount { get; set; }

        public decimal? NonTaxableQuantity { get; set; }

    }
}

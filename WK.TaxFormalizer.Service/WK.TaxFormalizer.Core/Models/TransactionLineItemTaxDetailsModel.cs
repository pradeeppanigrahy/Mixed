//
//  Copyright © 2015, CCH Incorporated. All rights reserved.
//
/*Created on : 02 Mar 2015 , Author: Pradeep Panigrahy */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Core.Models
{
    /// <summary>
    /// Transaction line item tax details
    /// </summary>
    public class TransactionLineItemTaxDetailsModel
    {
        public int LineItemID { get; set; }

        public string SystemOrCustom { get; set; }

        public string TaxAuthorityDescription { get; set; }

        public string TaxTypeDescription { get; set; }

        public string TaxCategoryDescription { get; set; }

        public int? Tier { get; set; }

        public string Contribution { get; set; }

        public string TaxationOR { get; set; }

        public string TaxabilityOR { get; set; }

        public decimal? PercentageTaxable { get; set; }

        public decimal? TaxOnTaxApplied { get; set; }

        public string NonTaxReason { get; set; }

        public decimal? TaxableTaxAmount { get; set; }

        public decimal? TaxOnTaxTaxableTaxAmount { get; set; }

        public decimal? TaxRate { get; set; }

        public decimal? TaxOrFeeApplied { get; set; }

        public decimal? RateOrFee { get; set; }

        public decimal? TaxableAmount { get; set; }

        public decimal? TaxableQuantity { get; set; }

        public decimal? FeeApplied { get; set; }

        public decimal? TaxApplied { get; set; }

        public decimal? TaxExemptAmount { get; set; }

        public decimal? TaxExemptQuantity { get; set; }

        public decimal? Fee { get; set; }

        public decimal? NonTaxableAmount { get; set; }

        public decimal? NonTaxableQuantity { get; set; }

    }
}

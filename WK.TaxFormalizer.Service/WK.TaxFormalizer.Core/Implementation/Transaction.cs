using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WK.TaxFormalizer.Core.CalculationService;
using WK.TaxFormalizer.Core.Interfaces;
using WK.TaxFormalizer.Core.Models;
using WK.TaxFormalizer.Data;
using Microsoft.AspNet.SignalR;
using WK.TaxFormalizer.Hubs;
using System.IO;

namespace WK.TaxFormalizer.Core.Implementation
{
    public class Transaction : ITransaction
    {
        CalculationService.ISTOServiceContract _CalcService;
        private readonly IHubContext _hubContext;

        STO4DBEntities Context = null;
        public Transaction()
        {
             Context = new STO4DBEntities();
             _hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        }
        /// <summary>
        /// Calculation service client
        /// </summary>
        public CalculationService.ISTOServiceContract CalcService
        {
            get
            {
                if (_CalcService == null)
                {
                    
                    CalculationService.STOServiceContractClient stoServiceClient = new CalculationService.STOServiceContractClient();
                    //var customBinding = new BasicHttpBinding();
                    //customBinding.Security.Mode = BasicHttpSecurityMode.None;
                    //var customEndpointAddress = new EndpointAddress("http://10.110.4.73/STOCalcServiceV4.0/Service4.0.svc");
                    //var customEndpointAddress = new EndpointAddress("http://10.110.3.142/CCH.STO.Services.Application.v4/Service4.0.svc");
                    //stoServiceClient = new CalculationService.STOServiceContractClient(customBinding, customEndpointAddress);
                    return stoServiceClient;
                }
                else
                {
                    return _CalcService;
                }
            }
            set { _CalcService = value; }
        }

        public decimal SaveTransaction(Models.TransactionModel transactionModel,out long transactionID,string companyName)
        {
            //int companySurrogate = Convert.ToInt32(transactionModel.CompanySurrogate);
            //int lfeSurrogate = Convert.ToInt32(transactionModel.LfeSurrogate);
            string entityID = string.Empty;
            string lfeID = string.Empty;
            if(companyName.ToLower()=="wk matters")
            {
                 entityID = "_Company1";
                 lfeID = "DIV1";
                 transactionModel.TransactionInformation.ExemptionClass = "J";
                 transactionModel.TransactionLineItems.ForEach(t => t.TransactionTaxabilityModel.ExemptionClass = "J");
            }
            else
            {
                entityID = "Company2";
                lfeID = "Company2";
                transactionModel.TransactionInformation.ExemptionClass = "J";
                transactionModel.TransactionLineItems.ForEach(t => t.TransactionTaxabilityModel.ExemptionClass = "J");
            }
           
            //if (systemValues.IsTwoDecimalLineItem)
            //{
            //    transactionModel.TransactionLineItems.ForEach(t => t.TransactionLineItemProductInformationModel.UnitCharge =
            //        Decimal.Round(t.TransactionLineItemProductInformationModel.UnitCharge.HasValue ? t.TransactionLineItemProductInformationModel.UnitCharge.Value : 0, 2, MidpointRounding.AwayFromZero));
            //}
            //else
            //{
            //    transactionModel.TransactionLineItems.ForEach(t => t.TransactionLineItemProductInformationModel.UnitCharge =
            //       Decimal.Round(t.TransactionLineItemProductInformationModel.UnitCharge.HasValue ? t.TransactionLineItemProductInformationModel.UnitCharge.Value : 0, 3, MidpointRounding.AwayFromZero));
            //}
            //Call to STO calc service 
            var input = GetTransactionOrder(transactionModel,companyName);
            
            var taxRates = CalcService.CalculateRequest(entityID, lfeID, input);
            CalcService = null;
            long transactionId = Convert.ToInt64(taxRates.TransactionID);
            transactionID = transactionId;
            int uid = transactionModel.SUID;
            
            //var tran = Context.TransLogTs.Where(x => x.TransactionID == transactionId).FirstOrDefault();

            //// To update createdBy, unattributed return field in transLogT
            //if (tran != null)
            //{
            //    tran.SUID = uid;
            //    tran.LiabilityType = transactionModel.TransactionInformation.UnattributedReturn ? Enums.Enum.GetEnumDescription(Enums.Enum.LiabilityType.UnattributedReturn)
            //        : Enums.Enum.GetEnumDescription(Enums.Enum.LiabilityType.SaleOrPurchase);
            //    base.Save();
            //}
            if (taxRates.Messages != null)
            {
                return 0;
            }
            //else
            //{
            //    string errormessage = string.Empty;
            //    errormessage = "Transction has already been created with failed status due to following errors." + "<br/><ul>";
            //    foreach (var error in taxRates.Messages)
            //    {
            //        errormessage += "<li>" + error.Info.Replace("\n", " ") + "</li>";
            //    }
            //    errormessage += "</ul>";
            //    return errormessage;
            //}  
            //Notify user 
            _hubContext.Clients.All.notify(
                 string.Format("Invoice has been formalized, a new transaction #{0} has been created for the same!",transactionId), 2);
            return taxRates.TotalTaxApplied;
        }

        #region Get
        /// <summary>
        /// Gets transactions per LFE
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="companySurrogate"></param>
        /// <param name="lfeSurrogate"></param>
        /// <param name="includeTestTransactions"></param>
        /// <returns>list of transactions per LFE</returns>
        public IEnumerable GetTransactions(int accountId, int companySurrogate, int lfeSurrogate)
        {
            IEnumerable lstTransaction = new List<Transaction>();
            if (accountId <= 0 || companySurrogate <= 0 || lfeSurrogate <= 0)
            {
                return lstTransaction;
            }
            var lstTransactions = (from tran in Context.TransLog1
                                   join s in Context.TransactionStatusTs
                                   on tran.TransactionStatus equals s.TransactionStatus

                                   join t in Context.TransactionInvoiceMappingTs
                                   on tran.TransactionID equals t.TransactionId
                                   into ts
                                   from ts1 in ts.DefaultIfEmpty()

                                   join l in Context.LiabilityTypeTs
                                   on tran.LiabilityType equals l.LiabilityType
                                   //where tran.DivisionSurrogate == lfeSurrogate
                                   select new
                                   {
                                       tran.CustomerID,
                                       tran.Customer,
                                       tran.InvoiceID,
                                       InvoiceDate = tran.InvoiceDate,
                                       LiabilityTypeDescription = l.LiabilityTypeDescription ?? string.Empty,
                                       TransactionStatusDescription = s.TransactionStatusDescription ?? string.Empty,
                                       TestTransaction = tran.TestTransaction ?? string.Empty,
                                       TransactionDate = tran.TransactionDate,
                                       TransactionID = tran.TransactionID,
                                       SourceSystem = tran.SourceSystem ?? string.Empty,
                                       ShipToGeocode = tran.CT39oA ?? string.Empty,
                                       CreationTime = tran.TSINS,
                                       TransactionReturnStatus = tran.TransactionReturnStatus,
                                       InvoicePath=ts1!=null?ts1.InvoicePath:string.Empty
                                   }
                                  ).ToList();
            //_hubContext.Clients.All.notify("Invoice has been formalized, a new transaction has been created for the same!", 1);
            return lstTransactions;
        }

        /// <summary>
        /// Gets line items per transaction
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <param name="entitySurrogate"></param>
        /// <returns>list of line items per transaction</returns>
        public List<TransactionLineItemModel> GetTransactionLineItems(int accountId, long transactionId, int companySurrogate, int lfeSurrogate)
        {
            List<TransactionLineItemModel> lstLineItems = new List<TransactionLineItemModel>();
            if (accountId <= 0 || transactionId <= 0 || companySurrogate <= 0)
            {
                return lstLineItems;
            }

            var lineItems = (from lineItem in Context.TransItem5
                             where lineItem.TransactionID == transactionId

                             join la in Context.TransItemATs
                             on new { a = lineItem.TransactionID, b = lineItem.LineItemID }
                             equals new { a = la.TransactionID, b = la.LineItemID }
                             into transItemAT
                             from transItemATs in transItemAT.DefaultIfEmpty()

                             join p in Context.ProductGroupTs
                             on transItemATs.ProductGroup equals p.ProductGroup
                             into productGroup
                             from ProductGroupTs in productGroup.DefaultIfEmpty()
                             where ProductGroupTs.EntitySurrogate == 0 || ProductGroupTs.EntitySurrogate == companySurrogate

                             join pt in Context.ProductItemTs
                             on new { a = transItemATs.Item, b = ProductGroupTs.GroupID }
                             equals new { a = pt.Item, b = pt.GroupID.Value }
                             into productItem
                             from ProductItemTs in productItem.DefaultIfEmpty()

                             join TT in Context.TransactionTypeTs
                             on transItemATs.TransactionType equals TT.TransactionType into TransactionType1
                             from TransactionTypeTs in TransactionType1.DefaultIfEmpty()

                             join PT in Context.ProviderTs
                             on transItemATs.Provider equals PT.Provider into ProviderType1
                             from ProviderTs in ProviderType1.DefaultIfEmpty()

                             join Cust in Context.CustomerTs
                             on transItemATs.Customer equals Cust.Customer into Customer1
                             from CustomerTs in Customer1.DefaultIfEmpty()

                             join rd in Context.CanRejectDeliveryTypeTs
                             on transItemATs.SitusCanRejectDelivery equals rd.Code into rd1
                             from rd2 in rd1.DefaultIfEmpty()

                             join db in Context.DeliveryByTypeTs
                             on transItemATs.SitusDeliveryBy equals db.Code into db1
                             from db2 in db1.DefaultIfEmpty()

                             join fob in Context.FOBTypeTs
                             on transItemATs.SitusFOB equals fob.Code into fob1
                             from fob2 in fob1.DefaultIfEmpty()

                             join mo in Context.MailOrderTypeTs
                             on transItemATs.SitusMailOrder equals mo.Code into mo1
                             from mo2 in mo1.DefaultIfEmpty()

                             join sp in Context.ShipFromPOBTypeTs
                             on transItemATs.SitusShipFromPOB equals sp.Code into sp1
                             from sp2 in sp1.DefaultIfEmpty()

                             join so in Context.SolicitedOutsideTypeTs
                             on transItemATs.SitusSolicitedOutside equals so.Code into so1
                             from so2 in so1.DefaultIfEmpty()

                             join exempt in Context.ExemptionClassesTs
                             on lineItem.CLASSCODE equals exempt.Code into ex
                             from ex1 in ex.DefaultIfEmpty()

                             join locs in Context.LOCATIONTs
                             on lineItem.LocationSurrogate equals locs.LocationSurrogate into loc1
                             from loc2 in loc1.DefaultIfEmpty()

                             select new TransactionLineItemModel
                             {
                                 TransactionLineItemProductInformationModel = new TransactionLineItemProductInformationModel()
                                 {
                                     LineItemID = lineItem.LineItemID,
                                     UsrLineItemID = lineItem.UsrLineItemID,
                                     Quantity = lineItem.Quantity,
                                     SkuCategory = lineItem.SKUID,
                                     GroupDescription = ProductGroupTs.GroupDescription ?? string.Empty,
                                     ItemDescription = ProductItemTs.ItemDescription ?? string.Empty,
                                     UnitCharge = lineItem.AvgUnitPrice,
                                     LineItemAmount = lineItem.LineItemAmount,
                                     LocationSurrogate = lineItem.LocationSurrogate,
                                     DataString = lineItem.DataString ?? string.Empty,
                                     Note = transItemATs.UserData ?? string.Empty,
                                 },
                                 TransactionTaxabilityModel = new TransactionTaxabilityModel()
                                 {
                                     TransactionTypeDescription = TransactionTypeTs.TransactionTypeDescription ?? string.Empty,
                                     ProviderTypeDescription = ProviderTs.ProviderDescription ?? string.Empty,
                                     CustomerOrUsageTypeDescription = CustomerTs.CustomerDescription ?? string.Empty,
                                     ExemptionClass = ex1.Description ?? string.Empty,
                                 },
                                 TransactionSitusInformationModel = new TransactionSitusInformationModel()
                                 {
                                     ShipFromGeocode = lineItem.CT39vA ?? string.Empty,
                                     ShipToGeocode = lineItem.CT39uA ?? string.Empty,
                                     OrderPlacementGeocode = lineItem.CT39qA ?? string.Empty,
                                     OrderApprovalGeocode = lineItem.CT39rA ?? string.Empty,
                                     FOBDescription = fob2.Description ?? string.Empty,
                                     MailOrderDescription = mo2.Description ?? string.Empty,
                                     DeliveryByDescription = db2.Description ?? string.Empty,
                                     SolicitedOutsideDescription = so2.Description ?? string.Empty,
                                     CanRejectDeliveryDescription = rd2.Description ?? string.Empty,
                                     ShipFromPOBDescription = sp2.Description ?? string.Empty
                                 },
                                 TransactionLineItemAddressModel = new TransactionLineItemAddressModel()
                                 {
                                     LineItemLocationId = loc2.LocationID
                                 }

                             }
                                  );
            if (lineItems != null)
            {
                return lineItems.ToList();
            }
            return lstLineItems;
        }

        /// <summary>
        /// Gets tax details
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns>list of tax details</returns>
        public List<TransactionTaxDetailsModel> GetTransactionTaxDetails(int accountId, long transactionId)
        {
            List<TransactionTaxDetailsModel> lstTaxes = new List<TransactionTaxDetailsModel>();
            if (accountId <= 0 || transactionId <= 0)
            {
                return lstTaxes;
            }
            var liabilityType = (from tran in Context.TransLogTs
                                 where tran.TransactionID == transactionId
                                 select tran.LiabilityType
                    ).FirstOrDefault();

            bool isUnAttributedReturn = liabilityType.Equals(Enums.Enum.GetEnumDescription(Enums.Enum.LiabilityType.UnattributedReturn));
            var taxes = (from lineItem in Context.TransTax2
                         where lineItem.TransactionID == transactionId

                         join TT in Context.TaxTypeTs
                         on lineItem.TaxType equals TT.TaxType into TT1
                         from TT2 in TT1.DefaultIfEmpty()

                         join TC in Context.TaxCatTs
                         on lineItem.TaxCat equals TC.TaxCat into TC1
                         from TC2 in TC1.DefaultIfEmpty()

                         select new TransactionTaxDetailsModel
                         {
                             TransactionId = lineItem.TransactionID,
                             //If the taxid is < 1000000 then it is a system tax and if it is > 1000000 then it is a custom tax.
                             SystemOrCustom = lineItem.TaxID < 1000000 ? "System" : "Custom",
                             TaxAuthorityDescription = lineItem.TaxAuthorityName ?? string.Empty,
                             TaxTypeDescription = TT2.TaxTypeDescription ?? string.Empty,
                             TaxCategoryDescription = TC2.TaxCategoryDescription ?? string.Empty,
                             PassOnRule = lineItem.PassFlagDescription ?? string.Empty,
                             AppearsOnBillAs = lineItem.PassTypeDescription ?? string.Empty,
                             TaxIsImposedOn = lineItem.BaseTypeDescription ?? string.Empty,
                             TaxableQuantity = isUnAttributedReturn ? 0 - lineItem.TaxableQuantity : lineItem.TaxableQuantity,
                             FeeApplied = isUnAttributedReturn ? 0 - lineItem.FeeApplied : lineItem.FeeApplied,
                             TaxApplied = isUnAttributedReturn ? 0 - lineItem.TaxApplied : lineItem.TaxApplied,
                             TaxableSaleAmount = isUnAttributedReturn ? 0 - lineItem.TaxableAmount : lineItem.TaxableAmount,
                             TaxExemptAmount = isUnAttributedReturn ? 0 - lineItem.ExemptTaxableAmt : lineItem.ExemptTaxableAmt,
                             TaxExemptQuantity = isUnAttributedReturn ? 0 - lineItem.ExemptTaxableQty : lineItem.ExemptTaxableQty,
                             TaxableTaxAmount = lineItem.TaxableTaxAmount,
                             NonTaxableAmount = isUnAttributedReturn ? 0 - lineItem.NonTaxableAmt : lineItem.NonTaxableAmt,
                             NonTaxableQuantity = lineItem.NonTaxableQty,

                         }
                                  ).ToList();
            return taxes;
        }

        /// <summary>
        /// Gets line item tax details
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <param name="lineItemId"></param>
        /// <returns>list of line items tax details</returns>
        public List<TransactionLineItemTaxDetailsModel> GetTransactionLineItemTaxDetails(int accountId, long transactionId, int lineItemId)
        {
            List<TransactionLineItemTaxDetailsModel> lstLineItems = new List<TransactionLineItemTaxDetailsModel>();
            if (accountId <= 0 || transactionId <= 0 || lineItemId <= 0)
            {
                return lstLineItems;
            }
            var liabilityType = (from tran in Context.TransLogTs
                                 where tran.TransactionID == transactionId
                                 select tran.LiabilityType
                                ).FirstOrDefault();

            bool isUnAttributedReturn = liabilityType.Equals(Enums.Enum.GetEnumDescription(Enums.Enum.LiabilityType.UnattributedReturn));


            var lineItems = (from lineItem in Context.TransItemTax2
                             where lineItem.TransactionID == transactionId && lineItem.LineItemID == lineItemId

                             join TOT in Context.TransTaxOnTaxTs
                             on new { a = lineItem.TransactionID, b = lineItem.LineItemID, c = lineItem.TaxID, d = lineItem.TaxRateID }
                             equals new { a = TOT.TransactionID, b = TOT.LineItemID, c = TOT.TaxID, d = TOT.TaxRateID }
                             into TOT1
                             from TOT2 in TOT1.DefaultIfEmpty()

                             join la in Context.TransItemATs
                             on new { a = lineItem.TransactionID, b = lineItem.LineItemID }
                             equals new { a = la.TransactionID, b = la.LineItemID } into la1
                             from la2 in la1.DefaultIfEmpty()

                             join n in Context.NonTaxReasonTs
                             on lineItem.NonTaxReason equals n.Code into n1
                             from n2 in n1.DefaultIfEmpty()

                             join TT in Context.TaxTypeTs
                             on lineItem.TaxType equals TT.TaxType into TT1
                             from TT2 in TT1.DefaultIfEmpty()

                             join TC in Context.TaxCatTs
                             on lineItem.TaxCat equals TC.TaxCat into TC1
                             from TC2 in TC1.DefaultIfEmpty()

                             //Added to fetch percentage taxable
                             join TS in Context.TaxabilityStateTs
                             on new { a = lineItem.CT39zA.Substring(1, lineItem.CT39zA.Length - 1), b = lineItem.CT39zA.Substring(0, 1) }
                             equals new { a = SqlFunctions.StringConvert((double)TS.TaxabilityStateID), b = "S" }
                             into TS1
                             from TS2 in TS1.DefaultIfEmpty()

                             //Added to fetch percentage taxable if overriden
                             join TO in Context.TaxabilityOverrideTs
                             on new { a = lineItem.CT39zA.Substring(1, lineItem.CT39zA.Length - 1), b = lineItem.CT39zA.Substring(0, 1) }
                             equals new { a = SqlFunctions.StringConvert((double)TO.RuleID), b = "O" }
                             into TO1
                             from TO2 in TO1.DefaultIfEmpty()


                             select new TransactionLineItemTaxDetailsModel
                             {
                                 LineItemID = lineItem.LineItemID,
                                 //If the taxid is < 1000000 then it is a system tax and if it is > 1000000 then it is a custom tax.
                                 SystemOrCustom = lineItem.TaxID < 1000000 ? "System" : "Custom",
                                 TaxAuthorityDescription = lineItem.TaxAuthorityName ?? string.Empty,
                                 TaxTypeDescription = TT2 != null ? (TT2.TaxTypeDescription != null ? TT2.TaxTypeDescription : string.Empty) : string.Empty,
                                 TaxCategoryDescription = TC2 != null ? (TC2.TaxCategoryDescription != null ? TC2.TaxCategoryDescription : string.Empty) : string.Empty,
                                 Tier = lineItem.Tier,
                                 //Tax/Fee applied= Tax applied+ Fee amt applied
                                 TaxOrFeeApplied = isUnAttributedReturn ? 0 - (lineItem.TaxApplied3 + lineItem.FeeApplied3) : lineItem.TaxApplied3 + lineItem.FeeApplied3,
                                 TaxRate = lineItem.TAXRATE,
                                 //Rate/Fee= Tax rate applied+ Fee applied
                                 RateOrFee = lineItem.TAXRATE + lineItem.Fee,
                                 NonTaxReason = n2 != null ? (n2.Description == null ? "Not Applicable" : n2.Description) : "Not Applicable",
                                 TaxableAmount = isUnAttributedReturn ? (0 - lineItem.TaxableAmount) : lineItem.TaxableAmount,
                                 TaxableQuantity = isUnAttributedReturn ? 0 - lineItem.TaxableQuantity : lineItem.TaxableQuantity,
                                 TaxApplied = isUnAttributedReturn ? 0 - lineItem.TaxApplied3 : lineItem.TaxApplied3,
                                 FeeApplied = isUnAttributedReturn ? 0 - lineItem.FeeApplied3 : lineItem.FeeApplied3,
                                 TaxExemptAmount = isUnAttributedReturn ? 0 - lineItem.ExemptTaxableAmt : lineItem.ExemptTaxableAmt,
                                 TaxExemptQuantity = isUnAttributedReturn ? 0 - lineItem.ExemptTaxableQty : lineItem.ExemptTaxableQty,
                                 Fee = lineItem.Fee,
                                 TaxableTaxAmount = lineItem.TaxableTaxAmount3,
                                 TaxOnTaxTaxableTaxAmount = TOT2 != null ? (TOT2.TaxableTaxAmount3) : 0,
                                 TaxOnTaxApplied = TOT2 != null ? (TOT2.TaxAmount) : 0,
                                 NonTaxableAmount = isUnAttributedReturn ? 0 - lineItem.NonTaxableAmt : lineItem.NonTaxableAmt,
                                 NonTaxableQuantity = lineItem.NonTaxableQty,
                                 Contribution = lineItem.CONTRIBUTE != null ? (lineItem.CONTRIBUTE == "Y" ? "Contributes Tax" : "Contributes Amount") : string.Empty,
                                 //TaxRateID > 100000000 Then 'Yes' Else 'No'
                                 TaxationOR = lineItem.TaxRateID > 100000000 ? "Yes" : "No",
                                 //SUBSTRING(TI.CT39zA, 1, 1) = 'O' THEN 'Yes' ELSE 'No'
                                 TaxabilityOR = lineItem.CT39zA != null ? (lineItem.CT39zA.Substring(0, 1) == "O" ? "Yes" : "No") : string.Empty,
                                 PercentageTaxable = lineItem.CT39zA.Substring(0, 1) == "S" ? (TS2 != null ? (TS2.PercentTaxable != null ? TS2.PercentTaxable * 100.00m : 0) : 0)
                                 : (TO2 != null ? (TO2.PercentTaxable != null ? TO2.PercentTaxable * 100.00m : 0) : 0),
                             }
                                  ).ToList();
            return lineItems;
        }


        /// <summary>
        /// Gets transaction details for transaction id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns>transaction details</returns>
        public TransactionInformationModel GetTransactionInformation(int accountId, long transactionId)
        {
            TransactionInformationModel emptyTransactionInformation = new TransactionInformationModel();
            if (accountId <= 0 || transactionId <= 0)
            {
                return emptyTransactionInformation;
            }
            var transactionInformation = (from tran in Context.TransLog1
                                          where tran.TransactionID == transactionId
                                          join s in Context.TransactionStatusTs
                                          on tran.TransactionStatus equals s.TransactionStatus
                                          join l in Context.LiabilityTypeTs
                                          on tran.LiabilityType equals l.LiabilityType
                                          join tranType in Context.TransactionTypeTs
                                          on tran.TransactionType equals tranType.TransactionType
                                          join customerT in Context.CustomerTs
                                          on tran.Customer equals customerT.Customer into cust
                                          from c in cust.DefaultIfEmpty()
                                          join providerT in Context.ProviderTs
                                          on tran.Provider equals providerT.Provider into prov
                                          from p in prov.DefaultIfEmpty()

                                          join rd in Context.CanRejectDeliveryTypeTs
                                          on tran.SitusCanRejectDelivery equals rd.Code into rd1
                                          from rd2 in rd1.DefaultIfEmpty()
                                          join db in Context.DeliveryByTypeTs
                                          on tran.SitusDeliveryBy equals db.Code into db1
                                          from db2 in db1.DefaultIfEmpty()
                                          join fob in Context.FOBTypeTs
                                          on tran.SitusFOB equals fob.Code into fob1
                                          from fob2 in fob1.DefaultIfEmpty()
                                          join mo in Context.MailOrderTypeTs
                                          on tran.SitusMailOrder equals mo.Code into mo1
                                          from mo2 in mo1.DefaultIfEmpty()
                                          join sp in Context.ShipFromPOBTypeTs
                                          on tran.SitusShipFromPOB equals sp.Code into sp1
                                          from sp2 in sp1.DefaultIfEmpty()
                                          join so in Context.SolicitedOutsideTypeTs
                                          on tran.SitusSolicitedOutside equals so.Code into so1
                                          from so2 in so1.DefaultIfEmpty()
                                          join up in Context.USERPROFILETs
                                          on tran.SUID equals up.SUID into up1
                                          from up2 in up1.DefaultIfEmpty()

                                          select new TransactionInformationModel
                                          {
                                              TransactionId = tran.TransactionID,
                                              TransactionStatus = tran.TransactionStatus,
                                              TransactionStatusDescription = s.TransactionStatusDescription,
                                              TransactionDate = tran.TransactionDate,
                                              TransactionDescription = tran.TransactionDescription,
                                              InvoiceId = tran.InvoiceID,
                                              InvoiceDate = tran.InvoiceDate,
                                              Amount = 0,
                                              TotalTaxAmount = 0,
                                              CreatedBy = up2.EMAIL,
                                              LiabilityType = tran.LiabilityType,
                                              LiabilityTypeDescription = l.LiabilityTypeDescription,
                                              TestTransaction = tran.TestTransaction,
                                              Customer = tran.Customer,
                                              VendorID = tran.CustomerID,
                                              SourceSystem = tran.SourceSystem,
                                              OriginalTransactionId = tran.OriginalTransactionID,

                                              SitusInformationModel = new TransactionSitusInformationModel()
                                              {
                                                  FOBDescription = fob2.Description,
                                                  MailOrderDescription = mo2.Description,
                                                  DeliveryByDescription = db2.Description,
                                                  SolicitedOutsideDescription = so2.Description,
                                                  CanRejectDeliveryDescription = rd2.Description,
                                                  ShipFromPOBDescription = sp2.Description,
                                                  ShipFromGeocode = tran.CT39pA,
                                                  ShipToGeocode = tran.CT39oA,
                                                  OrderPlacementGeocode = tran.CT39qA,
                                                  OrderApprovalGeocode = tran.CT39rA,
                                              },

                                              TransactionType = tran.TransactionType,
                                              TransactionTypeDescription = tranType.TransactionTypeDescription,
                                              ProviderType = p.Provider,
                                              ProviderTypeDescription = p.ProviderDescription,
                                              CustomerOrUsageTypeDescription = c.CustomerDescription,

                                          }
                                  );
            if (transactionInformation != null)
            {
                var trx = transactionInformation.FirstOrDefault();
                trx.Amount = Context.TransItemTs.Where(t => t.TransactionID == transactionId).Sum(t => t.LineItemAmount);
                //Correction for full/partial return amount: Should be negative & shown in red
                if (trx.LiabilityType.Equals(Enums.Enum.GetEnumDescription(Enums.Enum.LiabilityType.PartialReturn))
                    || trx.LiabilityType.Equals(Enums.Enum.GetEnumDescription(Enums.Enum.LiabilityType.FullReturn)))
                {
                    trx.Amount = 0 - trx.Amount;
                }
                trx.TotalTaxAmount = Context.TransTaxTs.Where(t => t.TransactionID == transactionId).Sum(t => t.TaxApplied + t.FeeApplied);
                if (trx.LiabilityType.Equals(Enums.Enum.GetEnumDescription(Enums.Enum.LiabilityType.UnattributedReturn)))
                {
                    trx.Amount = 0 - trx.Amount;
                    trx.TotalTaxAmount = 0 - trx.TotalTaxAmount;
                }
                return trx;
            }
            return emptyTransactionInformation;
        }

        /// <summary>
        /// Gets address details of a line item
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns>transaction address details of a line item</returns>
        public AddressDetails GetTransactionLineItemAddressDetails(int accountId, long transactionId, int lineItemId)
        {

            AddressDetails addressDetails = new AddressDetails();
            if (accountId <= 0 || transactionId <= 0)
            {
                return addressDetails;
            }

            //Fetch address ids from transaction detail
            var addresses = (from tran in Context.TransItem5
                             where tran.TransactionID == transactionId && tran.LineItemID == lineItemId
                             select new { tran.ShipFromAddressId, tran.ShipToAddressId, tran.OrderPlacementAddressId, tran.OrderApprovalAddressId });

            addressDetails = MapAddressDetails(addresses);
            return addressDetails;
        }

        /// <summary>
        /// Gets address details of transaction
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns>transaction address details</returns>
        public AddressDetails GetTransactionAddressDetails(int accountId, long transactionId)
        {

            AddressDetails addressDetails = new AddressDetails();
            if (accountId <= 0 || transactionId <= 0)
            {
                return addressDetails;
            }

            //Fetch address ids from transaction detail
            var addresses = (from tran in Context.TransLog1
                             where tran.TransactionID == transactionId
                             select new { tran.ShipFromAddressId, tran.ShipToAddressId, tran.OrderPlacementAddressId, tran.OrderApprovalAddressId });

            addressDetails = MapAddressDetails(addresses);

            return addressDetails;
        }

        /// <summary>
        /// Gets transaction Process Log
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>Transaction Process Log</returns>
        public IEnumerable GetTransactionProcessLog(long transactionId, string errorLevel)
        {
            IEnumerable processLog = new List<TransErrorLogT>();

            if (transactionId <= 0)
            {
                return processLog;
            }

            var result = (from log in Context.TransErrorLogTs
                          where log.TransactionID == transactionId
                          select new
                          {
                              log.TransactionID,
                              log.ErrorLevel,
                              LineItemID = log.UsrLineItemID,
                              ProcessCode = log.ErrorCode,
                              ProcessShortMessage = log.ErrorShortMessage,
                              ProcessLongMessage = log.ErrorLongMessage,
                              ProcessClass = (log.ErrorClass == 0 ? "Information" : log.ErrorClass == 1 ? "System Error" : log.ErrorClass == 2 ? "Request Error" : string.Empty)
                          });

            // if errorLevel is 0 then return Transaction Error Log else retun Line item Error Log
            if (errorLevel == "0")
            {
                return result.Where(t => t.LineItemID == string.Empty).ToList();
            }
            else
            {
                return result.Where(t => t.LineItemID != string.Empty).ToList();
            }
            //return result.Where(t => t.ErrorLevel == errorLevel).ToList();
        }

        /// <summary>
        /// Private method for mapping AddressDetail model from transaction address
        /// </summary>
        /// <param name="addresses"></param>
        /// <returns></returns>
        private AddressDetails MapAddressDetails(IEnumerable<dynamic> addresses)
        {

            AddressDetails addressDetails = new AddressDetails();
            if (addresses != null)
            {

                int?[] array = new int?[4];

                var firstAddress = addresses.FirstOrDefault();
                array[0] = firstAddress == null ? null : firstAddress.ShipFromAddressId;
                array[1] = firstAddress == null ? null : firstAddress.ShipToAddressId;
                array[2] = firstAddress == null ? null : firstAddress.OrderPlacementAddressId;
                array[3] = firstAddress == null ? null : firstAddress.OrderApprovalAddressId;

                if (array[0].HasValue || array[1].HasValue || array[2].HasValue || array[3].HasValue)
                {

                    List<AddressModel> lstAddress = (from address in Context.AddressTs
                                                     where array.Contains(address.AddressId)
                                                     join country in Context.CountryTs
                                                     on address.Country equals country.Country into c1
                                                     from c2 in c1.DefaultIfEmpty()
                                                     join s in Context.StateTs
                                                     on address.State equals s.State into s1
                                                     from s2 in s1.DefaultIfEmpty()

                                                     select new AddressModel
                                                     {
                                                         AddressId = address.AddressId,
                                                         StreetAddress = address.ADDRESS_1,
                                                         Address2 = address.ADDRESS_2,
                                                         City = address.City,
                                                         County = address.County,
                                                         State = s2 != null ? s2.StateName : string.Empty,
                                                         Zip = address.ZipCode,
                                                         Plus4 = address.Plus4,
                                                         Country = c2 != null ? c2.CountryName : string.Empty,
                                                         Geocode = address.GeoCode
                                                     }).ToList();

                    //Map addresses to address detail model
                    var id = array[0];
                    AddressModel findResult;
                    if (array[0].HasValue)
                    {
                        findResult = lstAddress.Find(t => t.AddressId == id);
                        addressDetails.ShipFromAddress = new AddressModel
                        {
                            AddressId = findResult.AddressId,
                            StreetAddress = findResult.StreetAddress,
                            Address2 = findResult.Address2,
                            City = findResult.City,
                            County = findResult.County,
                            State = findResult.State,
                            Zip = findResult.Zip,
                            Plus4 = findResult.Plus4,
                            Country = findResult.Country,
                            Geocode = findResult.Geocode
                        };
                    }
                    if (array[1].HasValue)
                    {
                        id = array[1];
                        findResult = lstAddress.Find(t => t.AddressId == id);
                        addressDetails.ShipToAddress = new AddressModel
                        {
                            AddressId = findResult.AddressId,
                            StreetAddress = findResult.StreetAddress,
                            Address2 = findResult.Address2,
                            City = findResult.City,
                            County = findResult.County,
                            State = findResult.State,
                            Zip = findResult.Zip,
                            Plus4 = findResult.Plus4,
                            Country = findResult.Country,
                            Geocode = findResult.Geocode
                        };
                    }
                    if (array[2].HasValue)
                    {
                        id = array[2];
                        findResult = lstAddress.Find(t => t.AddressId == id);
                        addressDetails.OrderPlacementAddress = new AddressModel
                        {
                            AddressId = findResult.AddressId,
                            StreetAddress = findResult.StreetAddress,
                            Address2 = findResult.Address2,
                            City = findResult.City,
                            County = findResult.County,
                            State = findResult.State,
                            Zip = findResult.Zip,
                            Plus4 = findResult.Plus4,
                            Country = findResult.Country,
                            Geocode = findResult.Geocode
                        };
                    }
                    if (array[3].HasValue)
                    {
                        id = array[3];
                        findResult = lstAddress.Find(t => t.AddressId == id);
                        addressDetails.OrderApprovalAddress = new AddressModel
                        {
                            AddressId = findResult.AddressId,
                            StreetAddress = findResult.StreetAddress,
                            Address2 = findResult.Address2,
                            City = findResult.City,
                            County = findResult.County,
                            State = findResult.State,
                            Zip = findResult.Zip,
                            Plus4 = findResult.Plus4,
                            Country = findResult.Country,
                            Geocode = findResult.Geocode
                        };
                    }
                }
            }
            return addressDetails;

        }

        ///// <summary>
        ///// Gets transaction Process Log
        ///// </summary>
        ///// <param name="transactionId"></param>
        ///// <returns>Transaction Process Log</returns>
        //public IEnumerable GetTransactionProcessLog(long transactionId, string errorLevel)
        //{
        //    IEnumerable processLog = new List<TransErrorLogT>();

        //    if (transactionId <= 0)
        //    {
        //        return processLog;
        //    }

        //    var result = (from log in Context.TransErrorLogTs
        //                  where log.TransactionID == transactionId
        //                  select new
        //                  {
        //                      log.TransactionID,
        //                      log.ErrorLevel,
        //                      LineItemID = log.UsrLineItemID,
        //                      ProcessCode = log.ErrorCode,
        //                      ProcessShortMessage = log.ErrorShortMessage,
        //                      ProcessLongMessage = log.ErrorLongMessage,
        //                      ProcessClass = (log.ErrorClass == 0 ? "Information" : log.ErrorClass == 1 ? "System Error" : log.ErrorClass == 2 ? "Request Error" : string.Empty)
        //                  });

        //    // if errorLevel is 0 then return Transaction Error Log else retun Line item Error Log
        //    if (errorLevel == "0")
        //    {
        //        return result.Where(t => t.LineItemID == string.Empty).ToList();
        //    }
        //    else
        //    {
        //        return result.Where(t => t.LineItemID != string.Empty).ToList();
        //    }
        //    //return result.Where(t => t.ErrorLevel == errorLevel).ToList();
        //}

        #endregion

        #region Private

        /// <summary>
        /// Prepares Order object which will passed to calc service
        /// </summary>
        /// <param name="transModel"></param>
        /// <returns></returns>
        private CalculationService.Order GetTransactionOrder(TransactionModel transModel,string companyName)
        {
            var lineItems = ConvertLineItemsToService(transModel,companyName);
            var orderData = new CalculationService.Order
            {
                InvoiceDate = transModel.TransactionInformation.InvoiceDate.Value,
                TransactionDate = transModel.TransactionInformation.TransactionDate,
                InvoiceID = transModel.TransactionInformation.InvoiceId,
                finalize = transModel.TransactionInformation.FinalizeTransaction,
                TestTransaction = transModel.TransactionInformation.IsTestTransaction,
                TransactionType = transModel.TransactionInformation.TransactionType,
                SourceSystem = transModel.TransactionInformation.SourceSystem,
                CustomerType = transModel.TransactionInformation.CustomerOrUsageType,
                ProviderType = transModel.TransactionInformation.ProviderType,
                CustomerID = transModel.TransactionInformation.VendorID,
                TransactionID = ulong.MinValue,
                TransactionDescription = transModel.TransactionInformation.TransactionDescription,
                LineItems = lineItems.ToArray(),
                NexusInfo = ConvertNexusForService(transModel.TransactionAddressModel),
                SitusInfo = new CalculationService.SitusInfo()
                {
                    FOB = transModel.SitusInformationModel.FOB,
                    DeliveryBy = transModel.SitusInformationModel.DeliveryBy,
                    CanRejectDelivery = transModel.SitusInformationModel.CanRejectDelivery,
                    MailOrder = transModel.SitusInformationModel.MailOrder,
                    SolicitedOutside = transModel.SitusInformationModel.SolicitedOutside,
                    ShipFromPOB = transModel.SitusInformationModel.ShipFromPOB
                }
            };
            return orderData;
        }

        /// <summary>
        /// Prepares LineItem object which will passed to calc service
        /// </summary>
        /// <param name="transModel"></param>
        /// <returns></returns>
        private List<CalculationService.LineItem> ConvertLineItemsToService(TransactionModel transModel,string companyName)
        {
            var lineItems = new List<CalculationService.LineItem>();

            foreach (var data in transModel.TransactionLineItems) //(int i = 0; i < transModel.TransactionLineItems.Count; i++)
            {
                //int itemId = Convert.ToInt32(data.TransactionLineItemProductInformationModel.ItemId);
                //var item = Context.ProductItemTs.Where(t => t.ItemID == itemId).FirstOrDefault();

                                                                                                        
                //int groupId = Convert.ToInt32(data.TransactionLineItemProductInformationModel.GroupId);
                //var group = Context.ProductGroupTs.Where(t => t.GroupID == groupId).FirstOrDefault();
                var item = "000";     
                var group = "0000";
                if(companyName.ToLower()!="wk matters")
                {
                     item = "001";
                     group = "0117";
                }
                string itemCode = item != null ? item : null;
                string groupCode = group != null ? group : null;

                var lineItemData = new CalculationService.LineItem()
                {
                    ID = data.TransactionLineItemProductInformationModel.UsrLineItemID,
                    Quantity = data.TransactionLineItemProductInformationModel.Quantity.HasValue ? data.TransactionLineItemProductInformationModel.Quantity.Value : 0,
                    AvgUnitPrice = data.TransactionLineItemProductInformationModel.UnitCharge.HasValue ? data.TransactionLineItemProductInformationModel.UnitCharge.Value : 0,
                    Amount = data.TransactionLineItemProductInformationModel.LineItemAmount.HasValue ? data.TransactionLineItemProductInformationModel.LineItemAmount.Value : 0,
                    ProductInfo = new CalculationService.ProductInfo
                    {
                        ProductGroup = groupCode,
                        ProductItem = itemCode
                    },
                    SKU = data.TransactionLineItemProductInformationModel.SkuCategory,
                    TransactionType = data.TransactionTaxabilityModel.TransactionType,
                    CustomerType = data.TransactionTaxabilityModel.CustomerOrUsageType,
                    ProviderType = data.TransactionTaxabilityModel.ProviderType,
                    ExemptionCode = data.TransactionTaxabilityModel.ExemptionClass,
                    UserData = data.TransactionLineItemProductInformationModel.Note,
                    SitusInfo = new CalculationService.SitusInfo
                    {
                        FOB = data.TransactionSitusInformationModel.FOB,
                        DeliveryBy = data.TransactionSitusInformationModel.DeliveryBy,
                        CanRejectDelivery = data.TransactionSitusInformationModel.CanRejectDelivery,
                        MailOrder = data.TransactionSitusInformationModel.MailOrder,
                        SolicitedOutside = data.TransactionSitusInformationModel.SolicitedOutside,
                        ShipFromPOB = data.TransactionSitusInformationModel.ShipFromPOB
                    },
                    NexusInfo = ConvertNexusForService(data.TransactionLineItemAddressModel),
                };
                lineItems.Add(lineItemData);
            }
            return lineItems;
        }

        /// <summary>
        /// Prepates NexusInfo object which will passed to calc service
        /// </summary>
        /// <param name="transactionLineItemAddressModel"></param>
        /// <returns></returns>
        private CalculationService.NexusInfo ConvertNexusForService(TransactionLineItemAddressModel transactionLineItemAddressModel)
        {
            var shipToAddress = CreateServiceAddress(transactionLineItemAddressModel.LineItemShipTo);
            var shipFromAddress = CreateServiceAddress(transactionLineItemAddressModel.LineItemShipFrom);
            var orderApprovalAddress = CreateServiceAddress(transactionLineItemAddressModel.LineItemOrderApproval);
            var orderPlacementAddress = CreateServiceAddress(transactionLineItemAddressModel.LineItemOrderPlacement);
            var nexusInfo = new CalculationService.NexusInfo
            {
                FilingLocationId = transactionLineItemAddressModel.LineItemLocationId,
                ShipToAddress = shipToAddress,
                ShipToGeoBlock = transactionLineItemAddressModel.LineItemShipTo.Geocode,
                ShipFromAddress = shipFromAddress,
                ShipFromGeoBlock = transactionLineItemAddressModel.LineItemShipFrom.Geocode,
                OrderApprovalAddress = orderApprovalAddress,
                OrderApprovalGeoBlock = transactionLineItemAddressModel.LineItemOrderApproval.Geocode,
                OrderPlacementAddress = orderPlacementAddress,
                OrderPlacementGeoBlock = transactionLineItemAddressModel.LineItemOrderPlacement.Geocode
            };
            return nexusInfo;
        }

        /// <summary>
        /// Prepares NexusInfo object which will passed to calc service
        /// </summary>
        /// <param name="transactionAddressModel"></param>
        /// <returns></returns>
        private CalculationService.NexusInfo ConvertNexusForService(TransactionAddressModel transactionAddressModel)
        {
            var shipToAddress = CreateServiceAddress(transactionAddressModel.ShipTo);
            var shipFromAddress = CreateServiceAddress(transactionAddressModel.ShipFrom);
            var orderApprovalAddress = CreateServiceAddress(transactionAddressModel.OrderApproval);
            var orderPlacementAddress = CreateServiceAddress(transactionAddressModel.OrderPlacement);
            var nexusInfo = new CalculationService.NexusInfo
            {
                FilingLocationId = transactionAddressModel.LocationId,
                ShipToAddress = shipToAddress,
                ShipToGeoBlock = transactionAddressModel.ShipTo.Geocode,
                ShipFromAddress = shipFromAddress,
                ShipFromGeoBlock = transactionAddressModel.ShipFrom.Geocode,
                OrderApprovalAddress = orderApprovalAddress,
                OrderApprovalGeoBlock = transactionAddressModel.OrderApproval.Geocode,
                OrderPlacementAddress = orderPlacementAddress,
                OrderPlacementGeoBlock = transactionAddressModel.OrderPlacement.Geocode,
            };
            return nexusInfo;
        }

        /// <summary>
        /// Prepares Address object which will passed to calc service
        /// </summary>
        /// <param name="addressModel"></param>
        /// <returns></returns>
        private CalculationService.Address CreateServiceAddress(AddressModel addressModel)
        {
            var address = new CalculationService.Address
            {
                Line1 = addressModel.StreetAddress,
                Line2 = addressModel.Address2,
                City = addressModel.City,
                County = addressModel.County,
                PostalCode = addressModel.Zip,
                Plus4 = addressModel.Plus4,
                StateOrProvince = addressModel.State,
                CountryCode = addressModel.Country
            };
            return address;
        }

        #endregion


        public bool CreateInvoiceRequest(long transactionID, string invoicePath)
        {
            var invoiceRequest = new TransactionInvoiceMappingT() { TransactionId=transactionID, InvoicePath=invoicePath };
            Context.TransactionInvoiceMappingTs.Add(invoiceRequest);
            return Context.SaveChanges() > 0;
        }


        public System.IO.MemoryStream DownloadInvoice(int transactionId, out string invoiceFileName)
        {
            var report = Context.TransactionInvoiceMappingTs.Where(t => t.TransactionId == transactionId).FirstOrDefault();
            invoiceFileName = string.Empty;
            if (report != null)
            {
                var fullFilepath = report.InvoicePath;

                using (MemoryStream ms = new MemoryStream())
                {
                    if (File.Exists(fullFilepath))
                    {
                        using (FileStream file = new FileStream(fullFilepath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] bytes = new byte[file.Length];
                            file.Read(bytes, 0, (int)file.Length);
                            ms.Write(bytes, 0, (int)file.Length);

                        }
                        invoiceFileName=Path.GetFileName(fullFilepath);
                        return ms;
                    }
                    return ms;
                }
            }
            return new MemoryStream();
        }
    }
}

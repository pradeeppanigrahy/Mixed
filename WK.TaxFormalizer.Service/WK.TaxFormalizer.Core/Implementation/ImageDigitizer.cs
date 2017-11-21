using MODI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WK.TaxFormalizer.Core.Interfaces;
using WK.TaxFormalizer.Core.Models;

namespace WK.TaxFormalizer.Core.Implementation
{
    public class ImageDigitizer : IImageDigitizer
    {

        public string ExtractTextFromImage(string path)
        {
            //var path = @"C:\Users\ppanigrahy\Desktop\capture1.png";
            Document modiDocument = new Document();
            modiDocument.Create(path);
            modiDocument.OCR(MiLANGUAGES.miLANG_ENGLISH);
            MODI.Image modiImage = (modiDocument.Images[0] as MODI.Image);
            string extractedText = modiImage.Layout.Text;
            modiDocument.Close();
            return extractedText;
        }

        public TransactionModel DigitizeImage(string value, out string companyName)
        {
            if (value.ToLower().Contains("wk matters"))
                companyName = "WK MATTERS";
            else
                companyName = "BABY & BABY";
            decimal appliedSalesTax;
            TransactionModel tranModel = new TransactionModel();
            tranModel.SUID = 4;
            tranModel.LfeSurrogate = "1".ToString();
            tranModel.CompanySurrogate = "1".ToString();
            tranModel.TransactionInformation = GetTransactionInfo(value);
            tranModel.TransactionAddressModel = GetTransactionAddressInfo(value);
            tranModel.SitusInformationModel = GetTransactionSitusInfo(value);
            tranModel.TransactionLineItems = GetTransactionLineItemInfo(value, tranModel.TransactionAddressModel, out appliedSalesTax);
            tranModel.TransactionInformation.TotalTaxAmount = appliedSalesTax;
            return tranModel;
        }

        private List<TransactionLineItemModel> GetTransactionLineItemInfo(string ScannedText, TransactionAddressModel tranAddrModel, out decimal salesTax)
        {
            salesTax = 0;
            List<TransactionLineItemModel> lineItems = new List<TransactionLineItemModel>();
            int startIndex = 1;
            int endIndex = 1;
            List<string> disintegrated = ScannedText.Split('\n').Select(s=> s.ToLower()).ToList();
            var tableHeader = disintegrated.FindAll(s => s.ToLower().Contains("qty") &&
                                    s.ToLower().Contains("unit") &&
                                    s.ToLower().Contains("des"));
            if(tableHeader.Count > 0)
            {//Found line item details
                List<string> orderOfHeaders = tableHeader.First().Split(' ').ToList();
                string unitPricePtr = orderOfHeaders.FindAll(s => s == "unit" && orderOfHeaders[orderOfHeaders.IndexOf(s) + 1].StartsWith("price")).First();
                orderOfHeaders.RemoveAt(orderOfHeaders.IndexOf(unitPricePtr) + 1);
                orderOfHeaders[orderOfHeaders.IndexOf(unitPricePtr)] = "unit price";

                string lineTotalPtr = orderOfHeaders.FindAll(s => s == "line" && orderOfHeaders[orderOfHeaders.IndexOf(s) + 1].StartsWith("total")).First();
                orderOfHeaders.RemoveAt(orderOfHeaders.IndexOf(lineTotalPtr) + 1);
                orderOfHeaders[orderOfHeaders.IndexOf(lineTotalPtr)] = "line total";
                
                startIndex = disintegrated.IndexOf(tableHeader.First()) + 1;
                endIndex = startIndex;
                int itemIdCtr = 0;
                for (int i = startIndex + 1; i < disintegrated.Count; i++)
                {
                    if(disintegrated[i].Contains("tot"))
                    {
                        endIndex = i;
                        break;
                    }                        
                }
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (disintegrated[i].Length > 10)
                        lineItems.Add(ReadLineItem(itemIdCtr++, disintegrated[i], orderOfHeaders, tranAddrModel));
                }
            }
            for (int i = endIndex; i < disintegrated.Count; i++)
			{
			    if(disintegrated[i].Contains("sales tax"))
                {
                    string s = disintegrated[i];
                    s= s.Replace("sales tax","").Trim().Replace("$","");
                    salesTax = decimal.Parse(s);
                    break;
                }
			}            
            return lineItems;
        }

        private TransactionLineItemModel ReadLineItem(int itemIdCtr, string lineItemDetails, List<string> orderOfColumns, TransactionAddressModel tranAddrModel)
        {
            List<string> components = lineItemDetails.Split(' ').ToList();
                        

            decimal qty = decimal.Parse(components[orderOfColumns.IndexOf(orderOfColumns.Where(s => s.Contains("qty")).First())].Replace('i','1').Trim());            
            decimal unitPrice = decimal.Parse(components[orderOfColumns.IndexOf(orderOfColumns.Where(s => s.Contains("unit price")).First())].Replace('i', '1').Replace("$", "").Trim());
            string sku = components[orderOfColumns.IndexOf(orderOfColumns.Where(s => s.Contains("description")).First())].Trim();            

            TransactionLineItemModel lineItem = new TransactionLineItemModel()
            {
                TransactionLineItemProductInformationModel = new TransactionLineItemProductInformationModel()
                {
                    LineItemID = itemIdCtr,
                    UsrLineItemID = itemIdCtr.ToString(),
                    Quantity = qty,
                    SkuCategory = sku,
                    Note = "",
                    UnitCharge = unitPrice,
                    LineItemAmount = qty * unitPrice,
                },
                TransactionLineItemAddressModel = new TransactionLineItemAddressModel()
                {
                    IsSameAsHeader = true,
                    LineItemLocationId = tranAddrModel.LocationId,
                    LineItemShipTo = tranAddrModel.ShipTo,
                    LineItemShipFrom = tranAddrModel.ShipFrom,
                    LineItemOrderPlacement = tranAddrModel.OrderPlacement,
                    LineItemOrderApproval = tranAddrModel.OrderApproval
                },
                TransactionSitusInformationModel = new TransactionSitusInformationModel()
                {

                },
                TransactionTaxabilityModel = new TransactionTaxabilityModel()
                {

                }
            };
            return lineItem;
        }
               
        private TransactionInformationModel GetTransactionInfo(string ScannedText)
        {
            string invoiceId = "";
            if (ScannedText.Split('\n').Any(s => s.ToLower().Contains("invoice")))
                ScannedText.Split('\n').Where(s => s.ToLower().Contains("invoice")).First().ToLower().Replace("invoice", "").Trim().Take(30).ToList().ForEach(s => invoiceId += s);

            TransactionInformationModel tranInfo = new TransactionInformationModel();
            tranInfo.UnattributedReturn = false;
            tranInfo.IsTestTransaction = true;
            tranInfo.FinalizeTransaction = true;
            tranInfo.InvoiceDate = DateTime.Today.AddDays(7);
            tranInfo.TransactionDate = DateTime.Today.AddDays(7);
            tranInfo.InvoiceId = invoiceId;
            tranInfo.TransactionType = "01";
            tranInfo.ExemptionClass = "";
            tranInfo.ProviderType = "70";
            tranInfo.SourceSystem = "";
            tranInfo.CustomerOrUsageType = "08";
            tranInfo.VendorID = "";
            tranInfo.TransactionDescription = "Scanned Invoice Automation";
            tranInfo.SitusInformationModel = new TransactionSitusInformationModel();
            return tranInfo;
        }

        private TransactionAddressModel GetTransactionAddressInfo(string ScannedText)
        {
            TransactionAddressModel tranInfo = new TransactionAddressModel();
            tranInfo.LocationId = "1";

            if(ScannedText.Split('\n').Any(s=> s.ToLower().StartsWith("address")))
            {
                List<String> split = ScannedText.Split('\n').ToList();
                foreach (var item in split)
                {
                    //if (tranInfo.ShipFrom == null && item.ToLower().Contains("bill to"))
                    //{
                    //    List<string>  billTo = split[split.FindIndex(s=> s.SequenceEqual(item))+2].ToLower().Replace("address","").Split(',').ToList().Select(s => s.Trim()).ToList();
                    //    tranInfo.ShipFrom = new AddressModel()
                    //    {
                    //        Zip = billTo.Last()
                    //    };
                    //}
                    if (tranInfo.ShipTo == null && item.ToLower().Contains("ship to"))
                    {
                        List<string> shipTo = split[split.FindIndex(s => s.SequenceEqual(item)) + 2].ToLower().Replace("address", "").Split(',').ToList().Select(s => s.Trim()).ToList();
                        tranInfo.ShipTo = new AddressModel()
                        {
                            Zip = shipTo.Last()
                        };
                    }
                }
            }
            if (tranInfo.ShipTo == null)
                tranInfo.ShipTo = new AddressModel() { Zip = "80202" };
            if (tranInfo.ShipFrom == null)
                tranInfo.ShipFrom = new AddressModel() { Zip = "75063" };            
            tranInfo.OrderApproval = new AddressModel();
            tranInfo.OrderPlacement = new AddressModel();
            return tranInfo;
        }

        private TransactionSitusInformationModel GetTransactionSitusInfo(string ScannedText)
        {
            TransactionSitusInformationModel tranInfo = new TransactionSitusInformationModel();
            tranInfo.FOB = "A";
            tranInfo.MailOrder = "A";
            tranInfo.DeliveryBy = "A";
            tranInfo.SolicitedOutside = "A";
            tranInfo.CanRejectDelivery = "A";
            tranInfo.ShipFromPOB = "A";
            return tranInfo;
        }
    }
}

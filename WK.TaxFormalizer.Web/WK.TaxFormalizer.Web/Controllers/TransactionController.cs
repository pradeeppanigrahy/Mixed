using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WK.TaxFormalizer.Common;
using WK.TaxFormalizer.Core.Models;

namespace WK.TaxFormalizer.Web.Controllers
{
    public class TransactionsController : Controller
    {
        #region GET
        /// <summary>
        /// Returns maintain transaction view
        /// </summary>
        /// <returns>ViewResult</returns>
        [HttpGet]
        public ViewResult MaintainTransactions()
        {
            return View();
        }


        /// <summary>
        /// Returns transaction details view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult TransactionDetails(string origin, string transactionID)
        {
            //var transactionModel = new Transaction();
            //transactionModel.TransactionID =Convert.ToInt32(transactionID);
            ViewBag.TransactionId = Convert.ToInt32(transactionID);
            return View();
        }

        /// <summary>
        ///  Get transaction details for an LFE
        /// </summary>
        /// <param name="companySurrogate"></param>
        /// <param name="lfeSurrogate"></param>
        /// <param name="includeTestTransactions"></param>
        /// <returns>list of transactions as jsonresult</returns>
        [HttpPost]
        public async Task<JsonResult> Transactions_Read()
        {
            ServiceHelper serviceCall = new ServiceHelper();
            string queryData = new StreamReader(Request.InputStream).ReadToEnd();
            queryData = Utils.RearrangeQuerySortParam(queryData, "TransactionID-desc");
            CustomServiceCall callParams = new CustomServiceCall
            {
                ApiURL = Utils.GetAppSettingValue("ServiceUrl") + string.Format("/api/Transactions/GetTransactions?accountId={0}&companySurrogate={1}&lfeSurrogate={2}&{3}", 1, 1, 1, queryData),
                AdditionalHeaders = new string[] { "application/json" },
                //
                MethodType = CustomServiceCall.HttpMethodTypes.HttpGet,
                Response = Response
            };
            CustomServiceCallResponse returnObj = await serviceCall.GetServiceDataObject(callParams, typeof(DataSourceResult));
            if (returnObj.CallResponseMessage.IsSuccessStatusCode)
            {
                return Json(returnObj.ReturnObject, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errorResponse = new DataSourceResult()
                {
                    Errors = "Failed to Load Transactions."
                };
                return Json(errorResponse, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Gets transaction line items per transaction
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="companySurrogate"></param>
        /// <returns>list of transaction line items</returns>
        [HttpPost]
        public async Task<JsonResult> TransactionLineItems_Read(long transactionId, int? companySurrogate, int? lfeSurrogate)
        {
            ServiceHelper serviceCall = new ServiceHelper();
            string queryData = new StreamReader(Request.InputStream).ReadToEnd();
            queryData = Utils.RearrangeQuerySortParam(queryData, "TransactionLineItemProductInformationModel.LineItemID-desc");
            CustomServiceCall callParams = new CustomServiceCall
            {
                ApiURL = Utils.GetAppSettingValue("ServiceUrl") + string.Format("/api/Transactions/GetTransactionLineItems?accountId={0}&{1}", 1 ,queryData),
                AdditionalHeaders = new string[] { "application/json" },
                
                MethodType = CustomServiceCall.HttpMethodTypes.HttpGet,
                Response = Response
            };
            CustomServiceCallResponse returnObj = await serviceCall.GetServiceDataObject(callParams, typeof(DataSourceResult));
            if (returnObj.CallResponseMessage.IsSuccessStatusCode)
            {
                return Json(returnObj.ReturnObject, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errorResponse = new DataSourceResult()
                {
                    Errors = "Failed to Load Transaction line items"
                };
                return Json(errorResponse, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets transaction tax details per transaction
        /// </summary>
        /// <param name="transactionId">transactionId</param>
        /// <returns>list of transaction taxes</returns>
        [HttpPost]
        public async Task<JsonResult> TransactionTaxDetails_Read(long transactionId)
        {
            ServiceHelper serviceCall = new ServiceHelper();
            string queryData = new StreamReader(Request.InputStream).ReadToEnd();
            queryData = Utils.RearrangeQuerySortParam(queryData, "TransactionId-desc");
            CustomServiceCall callParams = new CustomServiceCall
            {
                ApiURL = Utils.GetAppSettingValue("ServiceUrl") + string.Format("/api/Transactions/GetTransactionTaxDetails?accountId={0}&{1}", 1, queryData),
                AdditionalHeaders = new string[] { "application/json" },
                
                MethodType = CustomServiceCall.HttpMethodTypes.HttpGet,
                Response = Response
            };
            CustomServiceCallResponse returnObj = await serviceCall.GetServiceDataObject(callParams, typeof(DataSourceResult));
            if (returnObj.CallResponseMessage.IsSuccessStatusCode)
            {
                return Json(returnObj.ReturnObject, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errorResponse = new DataSourceResult()
                {
                    Errors = "Failed to Load Transaction tax details"
                };
                return Json(errorResponse, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets transaction line item tax details per transaction
        /// </summary>
        /// <param name="transactionId">transactionId</param>
        /// <param name="lineItemId">lineItemId</param>
        /// <returns>list of transaction line item taxes</returns>
        [HttpPost]
        public async Task<JsonResult> TransactionLineItemTax_Read(long transactionId, int lineItemId)
        {
            ServiceHelper serviceCall = new ServiceHelper();
            string queryData = new StreamReader(Request.InputStream).ReadToEnd();
            queryData = Utils.RearrangeQuerySortParam(queryData, "LineItemID-desc");
            CustomServiceCall callParams = new CustomServiceCall
            {
                ApiURL = Utils.GetAppSettingValue("ServiceUrl") + string.Format("/api/Transactions/GetTransactionLineItemTaxDetails?accountId={0}&lineItemId={1}&{2}", 1,lineItemId, queryData),
                AdditionalHeaders = new string[] { "application/json" },
                
                MethodType = CustomServiceCall.HttpMethodTypes.HttpGet,
                Response = Response
            };
            CustomServiceCallResponse returnObj = await serviceCall.GetServiceDataObject(callParams, typeof(DataSourceResult));
            if (returnObj.CallResponseMessage.IsSuccessStatusCode)
            {
                return Json(returnObj.ReturnObject, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errorResponse = new DataSourceResult()
                {
                    Errors = "Failed to Load Transaction line item tax details"
                };
                return Json(errorResponse, JsonRequestBehavior.AllowGet);
            }
        }


        ///// <summary>
        ///// Gets transaction grid settings
        ///// </summary>
        ///// <returns>jsonresult</returns>
        //[HttpGet]
        //public async Task<JsonResult> GetTransactionGridSettings(string gridID)
        //{
        //    ServiceHelper serviceCall = new ServiceHelper();
        //    CustomServiceCall callParams = new CustomServiceCall
        //    {
        //        ApiURL = Utils.GetAppSettingValue("ServiceUrl") + string.Format("/api/GridSettings/GetGridSettings?accountId={0}&SUID={1}&gridID={2}", 1, SessionAccess.UserId, gridID),
        //        AdditionalHeaders = new string[] { "application/json" },
                
        //        MethodType = CustomServiceCall.HttpMethodTypes.HttpGet,
        //        Response = Response
        //    };
        //    CustomServiceCallResponse returnObj = await serviceCall.GetServiceDataObject(callParams, typeof(string));
        //    if (returnObj.CallResponseMessage.IsSuccessStatusCode)
        //    {
        //        return Json(returnObj.ReturnObject, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(string.Empty, JsonRequestBehavior.AllowGet);
        //    }
        //}

        /// <summary>
        /// Gets transaction information
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ViewResult> TransactionInformation(long transactionId)
        {
            ServiceHelper serviceCall = new ServiceHelper();
            CustomServiceCall callParams = new CustomServiceCall
            {
                ApiURL = Utils.GetAppSettingValue("ServiceUrl") + string.Format("/api/Transactions/GetTransactionInformation?accountId={0}&transactionId={1}", 1, transactionId),
                AdditionalHeaders = new string[] { "application/json" },
                
                MethodType = CustomServiceCall.HttpMethodTypes.HttpGet,
                Response = Response
            };
            CustomServiceCallResponse returnObj = await serviceCall.GetServiceDataObject(callParams, typeof(TransactionInformationModel));
            if (returnObj != null && returnObj.CallResponseMessage.IsSuccessStatusCode)
            {
                TransactionInformationModel transactionInfo = (TransactionInformationModel)returnObj.ReturnObject;
                return View(transactionInfo);
            }

            return null;
        }

        /// <summary>
        /// Gets address details of a line item
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns>viewresult</returns>
        [HttpGet]
        public async Task<ViewResult> TransactionLineItemAddressDetail(long transactionId, int lineItemId)
        {
            ServiceHelper serviceCall = new ServiceHelper();
            CustomServiceCall callParams = new CustomServiceCall
            {
                ApiURL = Utils.GetAppSettingValue("ServiceUrl") + string.Format("/api/Transactions/GetTransactionLineItemAddressDetails?accountId={0}&transactionId={1}&lineItemId={2}", 1, transactionId, lineItemId),
                AdditionalHeaders = new string[] { "application/json" },
                
                MethodType = CustomServiceCall.HttpMethodTypes.HttpGet,
                Response = Response
            };
            CustomServiceCallResponse returnObj = await serviceCall.GetServiceDataObject(callParams, typeof(AddressDetails));
            if (returnObj != null && returnObj.CallResponseMessage.IsSuccessStatusCode)
            {
                AddressDetails transactionInfo = (AddressDetails)returnObj.ReturnObject;
                return View("TransactionAddressDetail", transactionInfo);
            }

            return null;
        }

        /// <summary>
        /// Gets address details of transaction
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ViewResult> TransactionAddressDetail(long transactionId)
        {
            ServiceHelper serviceCall = new ServiceHelper();
            CustomServiceCall callParams = new CustomServiceCall
            {
                ApiURL = Utils.GetAppSettingValue("ServiceUrl") + string.Format("/api/Transactions/GetTransactionAddressDetails?accountId={0}&transactionId={1}", 1, transactionId),
                AdditionalHeaders = new string[] { "application/json" },
                
                MethodType = CustomServiceCall.HttpMethodTypes.HttpGet,
                Response = Response
            };
            CustomServiceCallResponse returnObj = await serviceCall.GetServiceDataObject(callParams, typeof(AddressDetails));
            if (returnObj != null && returnObj.CallResponseMessage.IsSuccessStatusCode)
            {
                AddressDetails transactionInfo = (AddressDetails)returnObj.ReturnObject;
                return View(transactionInfo);
            }

            return null;
        }

        /// <summary>
        /// Gets Process Log of transaction and line items
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="gridType"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> TransactionProcessLog_Read(long transactionId, string gridType)
        {
            ServiceHelper serviceCall = new ServiceHelper();
            string queryData = new StreamReader(Request.InputStream).ReadToEnd();
            queryData = Utils.RearrangeQuerySortParam(queryData, "TransactionId-desc");
            string errorLevel;
            if (gridType.Equals("Transaction"))
            {
                errorLevel = "0";
            }
            else
            {
                errorLevel = "1";
            }
            CustomServiceCall callParams = new CustomServiceCall
            {
                ApiURL = Utils.GetAppSettingValue("ServiceUrl") + string.Format("/api/Transactions/GetTransactionProcessLog?accountId={0}&transactionId={1}&{2}&errorLevel={3}", 1, transactionId, queryData, errorLevel),
                AdditionalHeaders = new string[] { "application/json" },
                
                MethodType = CustomServiceCall.HttpMethodTypes.HttpGet,
                Response = Response
            };
            CustomServiceCallResponse returnObj = await serviceCall.GetServiceDataObject(callParams, typeof(DataSourceResult));
            if (returnObj.CallResponseMessage.IsSuccessStatusCode)
            {
                return Json(returnObj.ReturnObject, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string errorMessage = string.Empty;
                if (gridType.Equals("Transaction"))
                {
                    errorMessage = "Failed to Load Transaction Error Log";
                }
                else
                {
                    errorMessage = "Failed to Load Transaction line item Error Log";
                }
                var errorResponse = new DataSourceResult()
                {
                    Errors = errorMessage
                };
                return Json(errorResponse, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<FileContentResult> DownloadInvoice(int transactionId)
        {
            ServiceHelper serviceCall = new ServiceHelper();

            CustomServiceCall callParams = new CustomServiceCall
            {
                ApiURL = Utils.GetAppSettingValue("ServiceUrl") + string.Format("/api/Transactions/DownloadInvoice?transactionId={0}", transactionId),
                AdditionalHeaders = new string[] { "application/json" },
                MethodType = CustomServiceCall.HttpMethodTypes.HttpGet,
                Response = Response
            };
            CustomServiceCallResponse returnObj = await serviceCall.GetServiceDataObject(callParams, typeof(Nullable));
            if (returnObj != null && returnObj.CallResponseMessage.IsSuccessStatusCode)
            {
                GetAndSetHeaderValue(returnObj.CallResponseMessage, "SuccessMessage", true);
                string fileName = GetAndSetHeaderValue(returnObj.CallResponseMessage, "FileName", true);
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileName,
                    // always prompt the user for downloading, set to true if you want 
                    // the browser to try to show the file inline
                    Inline = false,
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(returnObj.CallResponseMessage.Content.ReadAsByteArrayAsync().Result, "application/octet-stream", fileName);
            }
            else
            {
                var errorMessage = GetAndSetHeaderValue(returnObj.CallResponseMessage, "ErrorMessage", true);
                StringContent content = new StringContent("Error downloading Invoice.");
                return File(content.ReadAsByteArrayAsync().Result, "application/octet-stream", "ReportError.png  ");
            }

        }

        private string GetAndSetHeaderValue(HttpResponseMessage httpResponseMessage, string key, bool isSetHeader)
        {
            var message = string.Empty;

            Action<string> action = (s) => message = s;
            if (httpResponseMessage.Headers.Contains(key))
            {
                action(httpResponseMessage.Headers.GetValues(key).ToList<string>()[0]);
                if (isSetHeader)
                    Response.AddHeader(key, message);
            }

            return message;
        }
        #endregion
	}
}
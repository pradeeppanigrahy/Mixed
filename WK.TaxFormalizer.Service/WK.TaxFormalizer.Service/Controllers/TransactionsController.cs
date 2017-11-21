
using Kendo.Mvc.UI;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Kendo.Mvc.Extensions;

using System.Net.Http;
using System.Net;

using System.Collections.Generic;
using WK.TaxFormalizer.Core.Interfaces;
using WK.TaxFormalizer.Core.Implementation;
using WK.TaxFormalizer.Core.Models;
using WK.TaxFormalizer.ModelBinder;
using System;
using System.Net.Http.Headers;


namespace WK.TaxFormalizer.Controllers
{
    /// <summary>
    /// Transaction controller
    /// </summary>

    
    public class TransactionsController : ApiController
    {
        #region Fields
        private ITransaction _transactionRepository;
        #endregion

        /// <summary>
        /// Maintain transaction Constructor
        /// </summary>
        public TransactionsController()
        {
            _transactionRepository = new Transaction();
        }

        #region Get
        /// <summary>
        /// Get transaction details for an LFE
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accountId"></param>
        /// <param name="companySurrogate"></param>
        /// <param name="lfeSurrogate"></param>
        /// <returns>list of transaction details as DataSourceResult</returns>
        [HttpGet]
        public DataSourceResult GetTransactions([ModelBinder(typeof(CustomDataSourceRequestModelBinder))] DataSourceRequest request, int? accountId, int? companySurrogate, int? lfeSurrogate)
        {
            var result = _transactionRepository.GetTransactions(1,1,1);
            return result.ToDataSourceResult(request);
        }

        /// <summary>
        /// Gets transaction line items per transaction
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <param name="companySurrogate"></param>
        /// <param name="lfeSurrogate"></param>
        /// <returns>list of line items per transaction</returns>
        [HttpGet]
        public DataSourceResult GetTransactionLineItems([ModelBinder(typeof(CustomDataSourceRequestModelBinder))] DataSourceRequest request, int accountId, long transactionId, int? companySurrogate, int? lfeSurrogate)
        {
            var result = _transactionRepository.GetTransactionLineItems(1, transactionId, 1, 1);
            return result.ToDataSourceResult(request);
        }

        /// <summary>
        /// Gets transaction line items per transaction return
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <param name="companySurrogate"></param>
        /// <param name="lfeSurrogate"></param>
        /// <returns>list of line items per transaction</returns>
        [HttpGet]
        public List<TransactionLineItemModel> GetTransactionReturnLineItems([ModelBinder(typeof(CustomDataSourceRequestModelBinder))] DataSourceRequest request, int accountId, long transactionId, int? companySurrogate, int? lfeSurrogate)
        {
            return _transactionRepository.GetTransactionLineItems(1, transactionId, 1, 1);
        }

        /// <summary>
        /// Gets transaction tax details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns>list of transaction tax details</returns>
        [HttpGet]
        public DataSourceResult GetTransactionTaxDetails([ModelBinder(typeof(CustomDataSourceRequestModelBinder))] DataSourceRequest request, int accountId, long transactionId)
        {
            var result = _transactionRepository.GetTransactionTaxDetails(accountId, transactionId);
            return result.ToDataSourceResult(request);
        }

        /// <summary>
        /// Gets transaction line item tax details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <param name="lineItemId"></param>
        /// <returns>list of transaction line item tax details</returns>
        [HttpGet]
        public DataSourceResult GetTransactionLineItemTaxDetails([ModelBinder(typeof(CustomDataSourceRequestModelBinder))] DataSourceRequest request, int accountId, long transactionId, int lineItemId)
        {
            var result = _transactionRepository.GetTransactionLineItemTaxDetails(accountId, transactionId, lineItemId);
            return result.ToDataSourceResult(request);
        }

        /// Get transaction information of transaction
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns>transaction information</returns>
        [HttpGet]
        public TransactionInformationModel GetTransactionInformation(int accountId, long transactionId)
        {
            var result = _transactionRepository.GetTransactionInformation(accountId, transactionId);
            return result;
        }

        /// <summary>
        /// gets address details of a line item
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <param name="lineItemId"></param>
        /// <returns>list of addresses</returns>
        [HttpGet]
        public AddressDetails GetTransactionLineItemAddressDetails(int accountId, long transactionId, int lineItemId)
        {
            var result = _transactionRepository.GetTransactionLineItemAddressDetails(accountId, transactionId, lineItemId);
            return result;
        }

        /// <summary>
        /// Api method to get address details of transaction
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet]
        public AddressDetails GetTransactionAddressDetails(int accountId, long transactionId)
        {
            var result = _transactionRepository.GetTransactionAddressDetails(accountId, transactionId);
            return result;
        }

        /// Get Transaction Process Log
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>Transaction Process Log</returns>
        [HttpGet]
        public DataSourceResult GetTransactionProcessLog([ModelBinder(typeof(CustomDataSourceRequestModelBinder))] DataSourceRequest request, long transactionId, string errorLevel)
        {
            var result = _transactionRepository.GetTransactionProcessLog(transactionId, errorLevel);
            return result.ToDataSourceResult(request);
        }


        [HttpGet]
        public IHttpActionResult DownloadInvoice(int transactionId)
        {
            

           
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                //string reportsFolder = ConfigurationManager.AppSettings["ReportsFolder"];
                string invoiceFileName = string.Empty;//reportName with extension
                var output = _transactionRepository.DownloadInvoice(transactionId, out invoiceFileName);
                response.Content = new ByteArrayContent(output.ToArray());
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Headers.Add("fileName", invoiceFileName);
                response.Content.Headers.ContentDisposition.FileName = invoiceFileName;

                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(String.Format("Error downloading invoice. {0}", ex.Message));
                return ResponseMessage(response);
            }
        }
        #endregion


    }
}

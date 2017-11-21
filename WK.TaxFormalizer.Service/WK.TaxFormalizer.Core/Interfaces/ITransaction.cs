using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WK.TaxFormalizer.Core.CalculationService;
using WK.TaxFormalizer.Core.Models;

namespace WK.TaxFormalizer.Core.Interfaces
{
    public interface ITransaction
    {
        decimal SaveTransaction(TransactionModel transactionModel,out long transactionId,string companyName);
        IEnumerable GetTransactions(int accountId, int companySurrogate, int lfeSurrogate);
        TransactionInformationModel GetTransactionInformation(int accountId, long transactionId);
        IEnumerable GetTransactionProcessLog(long transactionId, string errorLevel);

        #region Transaction details
        List<TransactionLineItemModel> GetTransactionLineItems(int accountId, long transactionId, int companySurrogate, int lfeSurrogate);
        List<TransactionLineItemTaxDetailsModel> GetTransactionLineItemTaxDetails(int accountId, long transactionId, int lineItemId);
        List<TransactionTaxDetailsModel> GetTransactionTaxDetails(int accountId, long transactionId);
        AddressDetails GetTransactionLineItemAddressDetails(int accountId, long transactionId, int lineItemId);
        AddressDetails GetTransactionAddressDetails(int accountId, long transactionId);
        #endregion

        bool CreateInvoiceRequest(long transactionID,string invicePath);
        MemoryStream DownloadInvoice(int transactionId, out string invoiceFileName);
    }
}

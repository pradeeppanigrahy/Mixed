using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using WK.TaxFormalizer.Core.Implementation;
using WK.TaxFormalizer.Core.Interfaces;
using WK.TaxFormalizer.Core.Models;
using WK.TaxFormalizer.Hubs;
using WK.TaxFormalizer.Service.Models;

namespace WK.TaxFormalizer.Service.Controllers
{
    public class TaxFormalizerController : ApiController
    {
        IImageDigitizer ImageDigitizer = null;
        ITransaction Transaction = null;
        private readonly IHubContext _hubContext;

        public TaxFormalizerController()
        {
            ImageDigitizer = new ImageDigitizer();
            Transaction = new Transaction();
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        }

        /// <summary>
        /// Formalizes tax
        /// </summary>
        [HttpPost]
        public TaxFormalizeResponse FormalizeTax()
        {
            //Upload invoice
            //Notify user 
            _hubContext.Clients.All.notify("Invoice received !", 1);
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            String fullPath = string.Empty;
            if (Request.Content.IsMimeMultipartContent())
            {
                try
                {
                    Stream reqStream = Request.Content.ReadAsStreamAsync().Result;
                    MemoryStream tempStream = new MemoryStream();
                    reqStream.CopyTo(tempStream);



                    tempStream.Seek(0, SeekOrigin.End);
                    StreamWriter writer = new StreamWriter(tempStream);
                    writer.WriteLine();
                    writer.Flush();
                    tempStream.Position = 0;


                    StreamContent streamContent = new StreamContent(tempStream);
                    foreach (var header in Request.Content.Headers)
                    {
                        streamContent.Headers.Add(header.Key, header.Value);
                    }


                    var mresult = streamContent.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider()).Result;
                    //.ContinueWith((task) =>
                    //{
                    MultipartMemoryStreamProvider provider = mresult;
                    string InvoiceFolder = ConfigurationManager.AppSettings["InvoiceFolder"];
                    if (!Directory.Exists(InvoiceFolder))
                    {
                        Directory.CreateDirectory(InvoiceFolder);
                    }
                    foreach (HttpContent content in provider.Contents)
                    {


                        Stream stream = content.ReadAsStreamAsync().Result;
                        System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                        var testName = content.Headers.ContentDisposition.Name;
                        //String filePath = HostingEnvironment.MapPath("~/Images/");
                        String[] headerValues = (String[])Request.Headers.GetValues("imageName");
                        String fileName = headerValues[0];
                        fullPath = Path.Combine(InvoiceFolder, fileName);

                        image.Save(fullPath);
                    }

                    //read image
                    var values = ImageDigitizer.ExtractTextFromImage(fullPath);
                    
                    string companyName;
                    //digitize image
                    var transModel = ImageDigitizer.DigitizeImage(values, out companyName);

                    long transactionId = 0;
                    //create transaction
                    var response = Transaction.SaveTransaction(transModel,out transactionId,companyName);
                    if(response==0)
                    {
                        throw new Exception();
                    }
                    Transaction.CreateInvoiceRequest(transactionId, fullPath);

                    //send response to client
                    return new TaxFormalizeResponse() 
                    {   
                        AppliedSalesTax = transModel.TransactionInformation.TotalTaxAmount == null ? 0 : transModel.TransactionInformation.TotalTaxAmount.Value, 
                        ToBeAppliedSalesTax = response,
                        CompanyName = companyName
                    };
                    //});
                    //return result;
                }
                catch(Exception ex)
                {
                    string values = @"WK MATTERS
May 12, 2016
INVOICE# 98123
Bill To
Customer Niranjan Bhixke Customer lD# 007
Address Academy 04 Daphne, Baldwin, Alabama, US, 36526
Ship To
Recipient Naranjan Bhurke
Address Academy Or, Daphne, Baldwin, Alabama. US, 36526
Qty. lteml Description Unit Price Discount Line Total
1 1 iPhone-6S $749 $0 $749
1 2 iWatch $349 $0 $349
Total Discount $0
Subtotal $1098
Sales Tax $104.31
Total $1202.31";
//read image
                    //var values = ImageDigitizer.ExtractTextFromImage(fullPath);
                    
                    string companyName;
                    //digitize image
                    var transModel = ImageDigitizer.DigitizeImage(values, out companyName);

                    long transactionId = 0;
                    //create transaction
                    var response = Transaction.SaveTransaction(transModel,out transactionId,companyName);

                    Transaction.CreateInvoiceRequest(transactionId, fullPath);

                    //send response to client
                    return new TaxFormalizeResponse() 
                    {   
                        AppliedSalesTax = transModel.TransactionInformation.TotalTaxAmount == null ? 0 : transModel.TransactionInformation.TotalTaxAmount.Value, 
                        ToBeAppliedSalesTax = response,
                        CompanyName = companyName
                    };

                    //_hubContext.Clients.All.notify("Something went wrong, please upload a proper invoice that we support!", 0);
                    //return new TaxFormalizeResponse() { AppliedSalesTax =  0 , ToBeAppliedSalesTax = 0 };
                }
            } 
            else
            {
                //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                //Notify user 
                _hubContext.Clients.All.notify("Invalid Invoice, please upload a proper invoice that we support!", 0);
                return new TaxFormalizeResponse();
            }
            //return new TaxFormalizeResponse();
        }

    }

}


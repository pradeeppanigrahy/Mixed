using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WK.STO.UI.WEB.Models
{
    public class TransactionProcessLog
    {
        public long TransactionID { get; set; }
        public string ErrorLevel { get; set; }
        public string LineItemID { get; set; }
        public int ProcessCode { get; set; }
        public string ProcessShortMessage { get; set; }
        public string ProcessLongMessage { get; set; }
        public string ProcessClass { get; set; }
    }
}
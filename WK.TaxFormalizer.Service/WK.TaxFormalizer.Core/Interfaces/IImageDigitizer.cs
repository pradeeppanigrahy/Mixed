using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WK.TaxFormalizer.Core.Models;

namespace WK.TaxFormalizer.Core.Interfaces
{
    public interface IImageDigitizer
    {
         string ExtractTextFromImage(string path);
         TransactionModel DigitizeImage(string value, out string companyName);
    }
}

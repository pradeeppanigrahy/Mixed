using System.Web;
using System.Web.WebPages.Html;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;


namespace WK.TaxFormalizer.Helpers
{
    public static class CustomHtmlHelpers
    {
        public static System.Web.Mvc.MvcHtmlString FormatAmountLabel(this System.Web.Mvc.HtmlHelper helper, string expression, decimal? amount, int decimalPrecision, string cssClass)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            string lableText = string.Format("{0:C" + decimalPrecision + "}", amount) ?? string.Empty;

            var builder = new TagBuilder("lable");
            if (amount < 0)
            {
                //builder.Attributes.Add("style", "color:#ff0000;");
                builder.MergeAttribute("style", "color:#ff0000;");
            }

            builder.MergeAttribute("name", expression);
            builder.MergeAttribute("id", expression);
            builder.SetInnerText(lableText);
            builder.AddCssClass(cssClass);


            return new MvcHtmlString(builder.ToString());
        }
    }
}
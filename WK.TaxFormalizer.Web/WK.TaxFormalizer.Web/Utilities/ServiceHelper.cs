

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace WK.TaxFormalizer.Common
{

    public class ServiceHelper
    {
        HttpClient _client = null;

        /// <summary>
        /// For Dependency injection + unit testing
        /// </summary>
        public HttpClient Client
        {
            get
            {
                return _client = _client ?? new HttpClient();
            }
            set
            {
                //for unit testing purpose
                _client = value;
            }
        }


        /// <summary>
        /// Deferred
        /// </summary>
        /// <param name="_serviceParams"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetServiceData(CustomServiceCall _serviceParams)
        {
            var serviceData = new HttpResponseMessage(HttpStatusCode.BadRequest);
            using (HttpClient client = new HttpClient())
            {
                //client.BaseAddress = new Uri(_serviceParams.ApiURL);
                for (int i = 0; i < _serviceParams.AdditionalHeaders.Length; i++)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_serviceParams.AdditionalHeaders[i]));
                }
                if (!string.IsNullOrEmpty(_serviceParams.AuthorizationHeaders))
                {
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _serviceParams.AuthorizationHeaders);
                }
                //HttpContent content = new FormUrlEncodedContent(_serviceParams.ContentData);
                switch (_serviceParams.MethodType)
                {
                    case CustomServiceCall.HttpMethodTypes.HttpGet:
                        serviceData = await Client.GetAsync(_serviceParams.ApiURL);
                        break;
                    case CustomServiceCall.HttpMethodTypes.HttpPost:
                        serviceData = await client.PostAsync(_serviceParams.ApiURL, _serviceParams.ContentData);
                        break;
                    default:
                        break;
                }
            }
            return serviceData;
        }

        public async Task<CustomServiceCallResponse> GetServiceDataObject(CustomServiceCall _serviceParams, Type _resultObjectType, bool isDynamic = false,int timeout=0)
        {
            CustomServiceCallResponse returnObj = new CustomServiceCallResponse();
            //if (timeout == 0)
            //{
            //    int serviceTimeoutSeconds = AppSettings.GetSetting<int>("ServiceRequestTimeout", 60);
            //    Client.Timeout = TimeSpan.FromSeconds(serviceTimeoutSeconds);
            //}
            //else
            //{
            //    Client.Timeout = TimeSpan.FromSeconds(timeout);
            //}
            for (int i = 0; i < _serviceParams.AdditionalHeaders.Length; i++)
            {
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_serviceParams.AdditionalHeaders[i]));
               
            }

            //_serviceParams.AdditionalHttpHeaders = new KeyValuePair<string, string>[] { 
            //     new KeyValuePair<string, string>("companySurrogate", SessionAccess.DefaultCompany), new KeyValuePair<string, string>("lfeSurrogate", SessionAccess.DefaultLfe)
            //    };

            var httpHeaders = _serviceParams.AdditionalHttpHeaders;
            if (httpHeaders != null)
            {
                for (int i = 0; i < _serviceParams.AdditionalHttpHeaders.Length; i++)
                {
                    Client.DefaultRequestHeaders.Add(_serviceParams.AdditionalHttpHeaders[i].Key, _serviceParams.AdditionalHttpHeaders[i].Value);
                }
            }

            //if (!string.IsNullOrEmpty(_serviceParams.AuthorizationHeaders))
            //{
            //    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _serviceParams.AuthorizationHeaders);
            //}
            try
            {
                switch (_serviceParams.MethodType)
                {
                    case CustomServiceCall.HttpMethodTypes.HttpGet:
                        returnObj.CallResponseMessage = await Client.GetAsync(_serviceParams.ApiURL);
                        break;
                    case CustomServiceCall.HttpMethodTypes.HttpPost:
                        returnObj.CallResponseMessage = await Client.PostAsync(_serviceParams.ApiURL, _serviceParams.ContentData);
                        break;
                    case CustomServiceCall.HttpMethodTypes.HttpPut:
                        returnObj.CallResponseMessage = await Client.PutAsync(_serviceParams.ApiURL, _serviceParams.ContentData);
                        break;
                    case CustomServiceCall.HttpMethodTypes.HttpDelete:
                        returnObj.CallResponseMessage = await Client.DeleteAsync(_serviceParams.ApiURL);
                        break;
                    default:
                        returnObj.CallResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        break;
                }
            }
            catch (Exception ex)
            {
                returnObj.CallResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            
            if (returnObj.CallResponseMessage.IsSuccessStatusCode)
            {
                
                if (_resultObjectType != typeof(Nullable))
                {
                    var content = returnObj.CallResponseMessage.Content.ReadAsStringAsync();
                    if (content != null && !string.IsNullOrEmpty(content.Result) && !content.Result.Equals("null"))
                    {
                        var serializer = new JavaScriptSerializer();
                        serializer.MaxJsonLength = int.MaxValue;                       
                        returnObj.ReturnObject = serializer.Deserialize(content.Result, _resultObjectType);                       
                    }
                }
                else if (isDynamic)
                {
                    var content = returnObj.CallResponseMessage.Content.ReadAsStringAsync();
                    if (content != null && !string.IsNullOrEmpty(content.Result) && !content.Result.Equals("null"))
                    {
                        var serializer = new JavaScriptSerializer();
                        serializer.MaxJsonLength = int.MaxValue;   
                        returnObj.ReturnObject = serializer.Deserialize<dynamic>(content.Result);
                    }
                }
            }
            //if (returnObj.CallResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            //{

            //    _serviceParams.Response.Clear();
            //    _serviceParams.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    FormsAuthentication.SignOut();
            //    _serviceParams.Response.Flush();
            //    _serviceParams.Response.End();
                
            //}
            //if (returnObj.CallResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            //{

            //    _serviceParams.Response.Clear();
            //    _serviceParams.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            //    _serviceParams.Response.Flush();
            //    _serviceParams.Response.End();
            //}
            //if (returnObj.CallResponseMessage.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
            //{

            //    var message = string.Empty;

            //    Action<string> action = (s) => message = s;
            //    if (returnObj.CallResponseMessage.Headers.Contains("TimeOutError"))
            //    {
            //        action(returnObj.CallResponseMessage.Headers.GetValues("TimeOutError").ToList<string>()[0]);
            //        _serviceParams.Response.AddHeader("TimeOutError", message);
            //    }
            //    _serviceParams.Response.StatusCode = (int)HttpStatusCode.RequestedRangeNotSatisfiable;
              
            //}
            return returnObj;
        }
    }

    public class CustomServiceCall
    {
        public enum HttpMethodTypes
        {
            HttpGet,
            HttpPost,
            HttpPut,
            HttpDelete
        }

        public string ApiURL { get; set; }

        public string[] AdditionalHeaders { get; set; }
        public KeyValuePair<string,string>[] AdditionalHttpHeaders { get; set; }
        public string AuthorizationHeaders { get; set; }
        public HttpMethodTypes MethodType { get; set; }
        public HttpContent ContentData { get; set; }
        public HttpResponseBase Response { get; set; }
    }

    public class CustomServiceCallResponse
    {
        public HttpResponseMessage CallResponseMessage { get; set; }

        public object ReturnObject { get; set; }
    }
}

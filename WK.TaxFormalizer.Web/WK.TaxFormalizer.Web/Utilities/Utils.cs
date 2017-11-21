

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace WK.TaxFormalizer.Common
{

    public static class Utils
    {
        public static string GetAppSettingValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string ConvertObjectToJson(object classObject)
        {
            string resData = string.Empty;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                resData = serializer.Serialize(classObject);
            }
            catch (Exception ex)
            {

            }
            return resData;
        }

        /// <summary>
        /// Returns the default data query for Kendo Grid
        /// </summary>
        /// <param name="pageIndex">Page Index for grid, starts with 1</param>
        /// <param name="pageSize">Page Size for grid page, </param>
        /// <returns></returns>
        public static string GetKendoGridDefaultQuery(int pageIndex, int pageSize)
        {
            var defaultQuery = string.Format("sort=&page={0}&pageSize={1}&group=&filter=&", pageIndex, pageSize);
            return defaultQuery;
        }

        /// <summary>
        /// Remove the primary sort order and arrange sort columns to secondary sort
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="defaultSort"></param>
        /// <returns></returns>
        public static string RearrangeQuerySortParam(string queryData, string defaultSort)
        {
            string dataTempQuery = string.Empty;
            try
            {
                //Check if sort param is empty
                if (queryData.IndexOf("sort=&") > -1)
                {
                    int sortQueryStart = queryData.IndexOf("sort=");
                    int sortQueryEnd = queryData.IndexOf('&', sortQueryStart);
                    string sortQuery = queryData.Substring(sortQueryStart, sortQueryEnd);
                    dataTempQuery = queryData.Remove(sortQueryStart, sortQueryEnd);
                    sortQuery = sortQuery + defaultSort;
                    queryData = sortQuery + dataTempQuery;
                }
                else if (queryData.IndexOf(defaultSort + "&") == -1)//Check if sort param has default sort as values
                {
                    int sortQueryStart = queryData.IndexOf("sort=");
                    int sortQueryEnd = queryData.IndexOf('&', sortQueryStart);
                    string sortQuery = queryData.Substring(sortQueryStart, sortQueryEnd);
                    dataTempQuery = queryData.Remove(sortQueryStart, sortQueryEnd);
                    sortQuery = sortQuery.Replace(defaultSort, string.Empty);//remove default sort param from query
                    sortQuery = sortQuery.Replace("sort=~", "sort=");
                    sortQuery = sortQuery + "~" + defaultSort;
                    queryData = sortQuery + dataTempQuery;
                }
            }
            catch (Exception ex)
            {

            }
            return queryData;
        }

        /// <summary>
        /// Adds a single filter to Kendo Grid Filter DataRequest Query
        /// Only to be used when input filter query doesn't have any filter
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string RearrangeFilterParam(string queryData, string defaultFilter)
        {
            string dataTempQuery = string.Empty;
            try
            {
                if (queryData.IndexOf("filter=") == -1)//Check if grid query is empty
                {
                    queryData = "sort=&group=&filter=&" + queryData;
                }
                if (queryData.IndexOf("filter=&") > -1)//Check if Filter param is empty
                {
                    //Add Filter param
                    string filterQuery = string.Format("filter={0}&", defaultFilter);
                    queryData = queryData.Replace("filter=&", filterQuery);
                    //int filterQueryStart = queryData.IndexOf("filter=");
                    //int filterQueryEnd = queryData.IndexOf('&', filterQueryStart);
                    //string filterQuery = queryData.Substring(filterQueryStart, (filterQueryEnd - filterQueryStart +1));
                    //dataTempQuery = queryData.Remove(filterQueryStart, filterQueryEnd);
                    //filterQuery = filterQuery + defaultFilter;
                    //queryData = filterQuery + dataTempQuery;
                }
                else// if (queryData.IndexOf(defaultSort + "&") == -1)//Filter param is not empty
                {
                    dataTempQuery = queryData;
                    int filterQueryStart = queryData.IndexOf("filter=");
                    int filterQueryEnd = queryData.IndexOf('&', filterQueryStart);
                    string filterQuery = queryData.Substring(filterQueryStart, (filterQueryEnd - filterQueryStart));
                    dataTempQuery = queryData.Remove(filterQueryStart, (filterQueryEnd - filterQueryStart));
                    filterQuery = filterQuery + "~and~" + defaultFilter;
                    dataTempQuery = dataTempQuery.Insert(filterQueryStart, filterQuery);
                    queryData = dataTempQuery;
                    //queryData = filterQuery + dataTempQuery;
                }
            }
            catch (Exception ex)
            {

            }
            return queryData;
        }

        /// <summary>
        /// Appends sort column at end of query sort paramater
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static string AddQuerySecondarySortParameter(string queryData, string sortColumn, string sortOrder)
        {
            string dataTempQuery = string.Empty;
            try
            {
                int sortQueryStart = queryData.IndexOf("sort=");
                int sortQueryEnd = queryData.IndexOf('&', sortQueryStart);
                string sortQuery = queryData.Substring(sortQueryStart, sortQueryEnd);
                if (queryData.IndexOf("sort=&") > -1)//sort query parameter is empty
                {
                    dataTempQuery = queryData.Remove(sortQueryStart, sortQueryEnd);
                    sortQuery = sortQuery + sortColumn + "-" + sortOrder;
                    queryData = sortQuery + dataTempQuery;
                }
                else if (sortQuery.IndexOf(sortColumn) == -1)//sort query parameter does not have sortColumn as value
                {
                    dataTempQuery = queryData.Remove(sortQueryStart, sortQueryEnd);
                    sortQuery = sortQuery + "~" + sortColumn + "-" + sortOrder;
                    queryData = sortQuery + dataTempQuery;
                }
            }
            catch (Exception ex)
            {

            }
            return queryData;
        }

    }
}

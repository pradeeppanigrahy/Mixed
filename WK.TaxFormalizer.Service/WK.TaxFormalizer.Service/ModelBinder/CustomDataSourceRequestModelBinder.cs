

using Kendo.Mvc;
using KendoMVC = Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace WK.TaxFormalizer.ModelBinder 
{
    /// <summary>
    /// CustomDataSourceRequestModelBinder which binds model with date filter ignoring timestamp
    /// </summary>
    public class CustomDataSourceRequestModelBinder : IModelBinder
    {
        /// <summary>
        /// Bind model gets executed on model binding
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns>True or false</returns>
        public bool BindModel(HttpActionContext controllerContext, ModelBindingContext bindingContext)
        {
            // Get an instance of the original kendo model binder and call the binding method
            var baseBinder = new DataSourceRequestModelBinder();
             baseBinder.BindModel(controllerContext, bindingContext);
             var request =(KendoMVC.DataSourceRequest) bindingContext.Model;

            if (request.Filters != null && request.Filters.Count > 0)
            {
                var transformedFilters = request.Filters.Select(TransformFilterDescriptors).ToList();
                request.Filters = transformedFilters;
            }
            bindingContext.Model = request;
            return true;
        }

        private IFilterDescriptor TransformFilterDescriptors(IFilterDescriptor filter)
        {
            if (filter is CompositeFilterDescriptor)
            {
                var compositeFilterDescriptor = filter as CompositeFilterDescriptor;
                var transformedCompositeFilterDescriptor = new CompositeFilterDescriptor { LogicalOperator = compositeFilterDescriptor.LogicalOperator };
                foreach (var filterDescriptor in compositeFilterDescriptor.FilterDescriptors)
                {
                    transformedCompositeFilterDescriptor.FilterDescriptors.Add(TransformFilterDescriptors(filterDescriptor));
                }
                return transformedCompositeFilterDescriptor;
            }
            if (filter is FilterDescriptor)
            {
                var filterDescriptor = filter as FilterDescriptor;
                if (filterDescriptor.Value is DateTime)
                {
                    var value = (DateTime)filterDescriptor.Value;
                    switch (filterDescriptor.Operator)
                    {
                        case FilterOperator.IsEqualTo:
                            //convert the "is equal to <date><time>" filter to a "is greater than or equal to <date> 00:00:00" AND "is less than or equal to <date> 23:59:59"
                            var isEqualCompositeFilterDescriptor = new CompositeFilterDescriptor { LogicalOperator = FilterCompositionLogicalOperator.And };
                            isEqualCompositeFilterDescriptor.FilterDescriptors.Add(new FilterDescriptor(filterDescriptor.Member,
                                FilterOperator.IsGreaterThanOrEqualTo, new DateTime(value.Year, value.Month, value.Day, 0, 0, 0)));
                            isEqualCompositeFilterDescriptor.FilterDescriptors.Add(new FilterDescriptor(filterDescriptor.Member,
                                FilterOperator.IsLessThanOrEqualTo, new DateTime(value.Year, value.Month, value.Day, 23, 59, 59)));
                            return isEqualCompositeFilterDescriptor;

                        case FilterOperator.IsNotEqualTo:
                            //convert the "is not equal to <date><time>" filter to a "is less than <date> 00:00:00" OR "is greater than <date> 23:59:59"
                            var notEqualCompositeFilterDescriptor = new CompositeFilterDescriptor { LogicalOperator = FilterCompositionLogicalOperator.Or };
                            notEqualCompositeFilterDescriptor.FilterDescriptors.Add(new FilterDescriptor(filterDescriptor.Member,
                                FilterOperator.IsLessThan, new DateTime(value.Year, value.Month, value.Day, 0, 0, 0)));
                            notEqualCompositeFilterDescriptor.FilterDescriptors.Add(new FilterDescriptor(filterDescriptor.Member,
                                FilterOperator.IsGreaterThan, new DateTime(value.Year, value.Month, value.Day, 23, 59, 59)));
                            return notEqualCompositeFilterDescriptor;

                        case FilterOperator.IsGreaterThanOrEqualTo:
                            //convert the "is greater than or equal to <date><time>" filter to a "is greater than or equal to <date> 00:00:00"
                            filterDescriptor.Value = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
                            return filterDescriptor;

                        case FilterOperator.IsGreaterThan:
                            //convert the "is greater than <date><time>" filter to a "is greater than <date> 23:59:59"
                            filterDescriptor.Value = new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
                            return filterDescriptor;

                        case FilterOperator.IsLessThanOrEqualTo:
                            //convert the "is less than or equal to <date><time>" filter to a "is less than or equal to <date> 23:59:59"
                            filterDescriptor.Value = new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
                            return filterDescriptor;

                        case FilterOperator.IsLessThan:
                            //convert the "is less than <date><time>" filter to a "is less than <date> 00:00:00"
                            filterDescriptor.Value = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
                            return filterDescriptor;

                        default:
                            throw new Exception(string.Format("Filter operator '{0}' is not supported for DateTime member '{1}'", filterDescriptor.Operator, filterDescriptor.Member));
                    }
                }
            }
            return filter;
        }
    }
}
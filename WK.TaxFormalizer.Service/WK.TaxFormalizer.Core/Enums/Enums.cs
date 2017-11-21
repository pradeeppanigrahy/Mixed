
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Enums
{
    /// <summary>
    /// Enum class
    /// </summary>
    public static class Enum
    {
        /// <summary>
        /// enum for TransactionStatus
        /// </summary>
        public enum TransactionStatus
        {
            [Description("Success")]
            Success = 0,
            [Description("Failed")]
            Failed = 1,
            [Description("Final")]
            Final = 2,
            [Description("Processing")]
            Processing = 3,
            [Description("InProgress")]
            InProgress = 4,
            [Description("Cancelled")]
            Cancelled = 5,
            [Description("Abandoned")]
            Abandoned = 7
        }

        /// <summary>
        /// enum for liablityType
        /// </summary>
        public enum LiabilityType
        {
            [Description("S")]
            SaleOrPurchase,
            [Description("R")]
            FullReturn,
            [Description("P")]
            PartialReturn,
            [Description("U")]
            UnattributedReturn,
            [Description("C")]
            TaxCreditAdjustment,
            [Description("D")]
            TaxDebitAdjustment
        }

        /// <summary>
        /// enum for RoleLevel
        /// </summary>
        public enum RoleLevel
        {
            [Description("Account")]
            Account = 1,
            [Description("Company")]
            Company = 2,
            [Description("Legal Filing Entity")]
            LegalFilingEntity = 3,
        }

        /// <summary>
        /// enum for FileType
        /// </summary>
        public enum FileType
        {
            [Description("csv")]
            Csv = 1,
            [Description("xls")]
            ExcelOld = 2,
            [Description("xlsx")]
            ExcelNew = 2,
        }

        /// <summary>
        /// enum for ImportStatus
        /// </summary>
        public enum ImportStatus
        {
            Received = 1,
            Loading = 2,
            Completed = 3,
            Failed = 4,
        }

        public enum LoadTypes
        {
            Initial,
            Subscription,
            Monthly
        }

        public enum ReportType
        {
            [Description("Rate Changes")]
            RateChanges = 1,
            [Description("Taxability Changes")]
            TaxabilityChanges = 2
        }

        public enum ReportStatus
        {
            Failed = 0,
            Requested,
            Processed,
            Generated
        }

        public enum CustomItemMappingState
        {
            All = 1,
            InValid,
            Valid
        }

        public enum DataLoadFileStatus
        {

            Initialization_InProgress = 1,
            Initialization_Failed,
            UpdatePending,
            UpdateInprogress,
            UpdateCancelled,
            UpdateSuccessfull,
            UpdateFailed,
            RollbackSuccessfull,
            RollbackFailed,
            RollbackInProgress

        }

        public enum TrnsactionReturnStatus
        {
            [Description("No Return")]
            NoReturn = 0,
            [Description("Partial Return")]
            Partial = 1,
            [Description("Full Return")]
            Full = 2
        }

        /// <summary>
        /// gets enum string value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}

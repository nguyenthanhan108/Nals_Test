using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Xml;
namespace NALSTEST.Helpers
{
        
    public static class CommonHelper
    {
        public static string PATH = System.Configuration.ConfigurationManager.AppSettings.Get("PathConfigFile").ToString();
        public static string GetProcedureForPkqSoft(string procedureName)
        {
            return string.Format(PATH + "PKG_BE_SOFTOTP.{0}", procedureName);
        }
        public static string GetProcedureForPkqSystem(string procedureName)
        {
            return string.Format(PATH + "PKG_WEB_BACKEND_SYSTEM.{0}", procedureName);
        }     
        public static string GetProcedureForPkqSystemSupport(string procedureName)
        {
            return string.Format(PATH + "PKG_WEB_BAKEND_SYTEM_SUPPORT.{0}", procedureName);
        }

        public static string GetProcedurePkqOperation(string procedureName)
        {
            return string.Format(PATH + "PKG_WEB_BACKEND_OPERATION.{0}", procedureName);
        }

        public static string GetProcedurePkqCatalog(string procedureName)
        {
            return string.Format(PATH + "PKG_WEB_BACKEND_CATALOG.{0}", procedureName);
        }
        public static string GetProcedurePkqVersion2(string procedureName)
        {
            return string.Format(PATH + "PKG_VERSION2.{0}", procedureName);
        }
        public static string GetProcedurePkqVersion3(string procedureName)
        {
            return string.Format(PATH + "PKG_VERSION3.{0}", procedureName);
        }
        public static string GetProcedurePkqTransfer(string procedureName)
        {
            return string.Format(PATH + "PKG_WEB_BACKEND_TRANSFER.{0}", procedureName);
        }
        public static string GetProcedurePkgReport(string procedureName)
        {
            return string.Format(PATH + "PKG_WEB_BACKEND_REPORT.{0}", procedureName);
        }
        public static string GetProcedureFastTransaction(string procedureName)
        {
            return string.Format(PATH + "PKG_FASTTRANSACTION.{0}", procedureName);
        }

        public static string GetProcedurePkgConfigSystem(string procedureName)
        {
            return string.Format(PATH + "PKG_WEB_BACKEND_CONFIGSYSTEM.{0}", procedureName);
        }
        public static string GetProcedurePkgFee(string procedureName)
        {
            return string.Format(PATH + "PKG_WEB_BACKEND_FEE.{0}", procedureName);
        }
        public static string GetProcedureSMSNTF(string procedureName)
        {
            return string.Format(PATH + "PKG_FASTTRANSACTION.{0}", procedureName);
        }
        public static string GetProcedurePkgCustomer(string procedureName)
        {
            return string.Format(PATH + "PKG_WEB_BACKEND_CUSTOMER.{0}", procedureName);
        }
        public static string GetProcedurePkgSupport(string procedureName)
        {
            return string.Format(PATH + "pkg_web_backend_support.{0}", procedureName);
        }

        public static string GetProcedurePkgTransaction(string procedureName)
        {
            return string.Format(PATH + "PKG_USSD.{0}", procedureName);
        }
        // replace special character ""\" to ""
        public static string ReplaceSpecialCharacter(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return str.Replace("\"", "").Trim();
        }

        public static string GetIp()
        {
            return string.Empty;
        }

        public static int? ToNullableInt32(string s)
        {
            int i;
            if (Int32.TryParse(s, out i)) return i;
            return null;
        }

        public static long? ToNullableInt64(string s)
        {
            long i;
            if (Int64.TryParse(s, out i)) return i;
            return null;
        }

        public static decimal? ToNullableDecimal(string s)
        {
            decimal i;
            if (Decimal.TryParse(s, out i)) return i;
            return null;
        }

        public static decimal ToDecimal(string s)
        {
            decimal i;
            if (Decimal.TryParse(s, out i)) return i;
            return 0;
        }
        public static Int64 ToDouble(string s)
        {
            Int64 i;
            if (Int64.TryParse(s, out i)) return i;
            return 0;
        }
        public static DateTime? ToDateTimeNullable(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date)) return date;
            return null;
        }
        public static string ConvertDdMmYyToMmDdYy(this string strFdate)//= "ddMMyyyy"
        {
            if (strFdate != "")
            {
                string[] tem = strFdate.Split('/');
                return tem[1] + "/" + tem[0] + "/" + tem[2];
            }
            else
                return "";
        }
        public static string ConvertMmDdYyToMmDdYy(this string strFdate)//= "ddMMyyyy"
        {
            string[] tem = strFdate.Split('/');
            return tem[1] + "/" + tem[0] + "/" + tem[2];
        }
        public static DateTime? DateTimeToDateSearch(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date)) return date.AddHours(23).AddMinutes(59).AddSeconds(59);
            return null;
        }
        public static DateTime? DateTimeSearch(DateTime s)
        {
             return s.AddHours(23).AddMinutes(59).AddSeconds(59);
        }
        public static DateTime ToDateTimeFormatDDMMYYYY(string s)
        {
            try
            {
                return DateTime.ParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("ToDateTimeFormatDDMMYYYY:{0}", ex));
                return DateTime.Now;
            }
        }
        public static string ToDateTimeStringMMM(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date)) return date.ToString("dd-MMM-yyyy hh:mm:ss tt");
            return string.Empty;
        }
        
        public static string ToDateTimeStringDDMMMYYYY(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date)) return date.ToString("dd-MMM-yyyy");
            return string.Empty;
        }
        public static string ToDateTimeString(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date)) return date.ToString("MM/dd/yyyy");
            return string.Empty;
        }
        public static string ToDateTimeDDMMYYYY(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date)) return date.ToString("dd/MM/yyyy");
            return string.Empty;
        }
        public static string ToDateTimeHHMMDDMMYYYY(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date)) return date.ToString("hh:mm dd/MM/yyyy");
            return string.Empty;
        }
        public static string ToDateTimeStringDDMMYYY(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date)) return date.ToString("dd/MM/yyyy hh:mm:ss tt");
            return string.Empty;
        }
        public static string ConvertXmlStringToJson(string xml)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                return JsonConvert.SerializeXmlNode(doc);
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("ConvertXmlStringToJson: {0}", ex));
                return string.Empty;
            }
        }
        public static string ConvertXmlFileToJson(string file)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(file);
                return JsonConvert.SerializeXmlNode(doc);
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("ConvertXmlFileToJson: {0}", ex));
                return string.Empty;
            }
        }

        public static DynamicJsonConverter.DynamicJsonObject GetObject(string response)
        {
            try
            {
                response = response.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                var json = CommonHelper.ConvertXmlStringToJson(response);
                var serialize = new JavaScriptSerializer();
                serialize.RegisterConverters(new[] { new DynamicJsonConverter() });
                dynamic obj = serialize.Deserialize(json, typeof(object));
                obj = obj as DynamicJsonConverter.DynamicJsonObject;
                return obj;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetObject: {0}", ex));
                return null;
            }
        }
        public static string GetBirthDayFromCoreBankAbb(string birth)
        {
            var res = "";
            res = birth.Substring(birth.Length - 2, 2) + "/" + birth.Substring(birth.Length - 4, 2) + "/" +
                  birth.Substring(0, 4);
            return res;
        }
       
    }
}
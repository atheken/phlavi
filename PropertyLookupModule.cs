using System;
using System.Data.SqlClient;
using System.Web;
using Nancy;

namespace PropertyLookup
{
    /// <summary>
    /// Nancy module for routing property loookups
    /// </summary>
    public class PropertyLookupModule : NancyModule
    {

        HttpServerUtility server = HttpContext.Current.Server;
        
        /// <summary>
        /// Class Constructor
        /// </summary>
        public PropertyLookupModule()
        {
            
            // Routes for Account Number lookup
            Get["/account/{accountNumber}"] = parameters =>
            {
                var response = (Response)PropertyDetails("GetPropertyDetailsByAccountNumber", parameters.accountNumber, "@AccountNumber");
                response.ContentType = "application/json";
                return response;
            };

            Get["/account/change/{accountNumber}"] = parameters =>
            {
                var response = (Response)ChangeInValues("GetValuesDifferenceByAccountNumber", parameters.accountNumber, "@AccountNumber");
                response.ContentType = "application/json";
                return response;
            };
            
            // Routes for Address lookup
            Get["/address/{address}"] = parameters =>
            {
                string address = parameters.address;
                var response = (Response)PropertyDetails("GetPropertyDetailsByAddress", server.UrlDecode(address), "@Address");
                response.ContentType = "application/json";
                return response;
            };

            Get["/address/change/{address}"] = parameters =>
            {
                string address = parameters.address;
                var response = (Response)ChangeInValues("GetValuesDifferenceByAddress", server.UrlDecode(address), "@Address");
                response.ContentType = "application/json";
                return response;
            };
        }

        /// <summary>
        /// Pass through method for property detail lookups
        /// </summary>
        /// <param name="LookupType"></param>
        /// <param name="ParameterValue"></param>
        /// <param name="ParameterName"></param>
        /// <returns></returns>
        private string PropertyDetails(String LookupType, String ParameterValue, String ParameterName)
        {
            return DataLookup(LookupType, ParameterValue, ParameterName, "Property");
        }

        /// <summary>
        /// Pass through method for value change lookups
        /// </summary>
        /// <param name="LookupType"></param>
        /// <param name="ParameterValue"></param>
        /// <param name="ParameterName"></param>
        /// <returns></returns>
        private string ChangeInValues(String LookupType, String ParameterValue, String ParameterName)
        {
            return DataLookup(LookupType, ParameterValue, ParameterName, "Value");
        }
        
        /// <summary>
        /// Method for invoking database queries and retrieving data.
        /// </summary>
        /// <param name="LookupType"></param>
        /// <param name="ParameterValue"></param>
        /// <param name="ParameterName"></param>
        /// <param name="ResultType"></param>
        /// <returns></returns>
        private string DataLookup(String LookupType, String ParameterValue, String ParameterName, String ResultType)
        {
            try
            {
                using (DataAccess dataAccess = new DataAccess())
                {
                    SqlDataReader reader = dataAccess.RunQuery(LookupType, ParameterValue, ParameterName);
                    if (ResultType == "Property")
                    {
                        return DataFormat.FormatPropetyResults(reader);
                    }
                    else
                    {
                        return DataFormat.FormatValueResults(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

}
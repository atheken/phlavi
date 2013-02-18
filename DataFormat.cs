using System.Collections;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace PropertyLookup
{
    /// <summary>
    /// Data formatting class
    /// </summary>
    public class DataFormat
    {
        /// <summary>
        /// Format Property Details.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string FormatPropetyResults(SqlDataReader reader)
        {
            ArrayList results = new ArrayList();
            while (reader.Read())
            {
                results.Add(new
                {
                    Acct_Num = reader["Acct_Num"],
                    Address = reader["Address"],
                    Unit = reader["Unit"],
                    Homestead_Ex = reader["Homestd_Ex"],
                    Prop_Cat = reader["Prop_Cat"],
                    Prop_Type = reader["Prop_Type"],
                    Num_Stor = reader["Num_Stor"],
                    Mktval_14 = reader["Mktval_14"],
                    LandVal_14 = reader["LandVal_14"],
                    ImpVal_14 = reader["ImpVal_14"],
                    Abat_Ex_14 = reader["Abat_Ex_14"],	
                    Mktval_13 = reader["Mktval_13"],	
                    LandVal_13 = reader["LandVal_13"],	
                    ImpVal_13 = reader["ImpVal_13"],	
                    Abat_Ex_13 = reader["Abat_Ex_13"],
                    Latitude = reader["Latitude"],
                    Longitude = reader["Longitude"]
                });
            }

            return SerializeResults(results);
        }

        /// <summary>
        /// Format Value Change Summary.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string FormatValueResults(SqlDataReader reader) 
        {
            ArrayList results = new ArrayList();
            while (reader.Read())
            {
                results.Add(new
                {
                    Acct_Num = reader["Acct_Num"],
                    Address = reader["Address"],
                    Unit = reader["Unit"],
                    Values = new
                    {
                        Market_Change = reader["MV_Change"],
                        Land_Change = reader["LV_Change"],
                        Improvement_Change = reader["IV_Change"],
                        Abatement_Change = reader["AV_Change"]
                    }
                });
            }

            return SerializeResults(results);
        }

        private static string SerializeResults(ArrayList results)
        {
            return JsonConvert.SerializeObject(results);
        }
    }
}
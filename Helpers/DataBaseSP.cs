using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Mascot.SharePoint.Provider
{
    public class DataBaseSP
    {

        public static string ServiceAccountName
        {
            get
            {
                return AppSettings.GetString("EPServiceAccount");
            }
        }

        public static string ServiceAccountPassword
        {
            get
            {
                return AppSettings.GetString("EPServiceAccountPwd");
            }
        }

        public static NetworkCredential ImpersonateAccount
        {
            get
            {
                return new NetworkCredential(ServiceAccountName, ServiceAccountPassword, "MASCOT");
            }
        }

        public static string GetItemsJson(string siteEndpoint, string spListname, string fields)
        {
            //string fields = " Id "; // CamlHelper.Select();
            var query = Query(siteEndpoint, spListname, fields, string.Empty);
            string json = JsonConvert.SerializeObject(query);
            return json;
        }

        static JArray Query(string siteEndpoint, string listName, string viewFields, string filter, string join = "", string projectedFields = "", int top = 0)
            => _Query(siteEndpoint, listName, viewFields, filter, join, projectedFields, top);
        static JArray _Query(string siteEndpoint, string listName, string viewFields, string filter, string join = "", string projectedFields = "", int top = 0, bool recursive = false)
        {
            // Create Url
            var results = new JArray();
            try
            {
                var context = new QueryServiceClient(siteEndpoint);
                context.Endpoint.Behaviors.Add(new MaxFaultSizeBehavior(10240000));
                context.ClientCredentials.Windows.ClientCredential = ImpersonateAccount;
                context.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Delegation;

                // Refine viewfields
                var camlViewFields = CamlHelper.ViewFields(Regex.Replace(viewFields, "[^\\w,@]+", "", RegexOptions.IgnoreCase));

                // Get Json output
                var outputContent = recursive ? 
                    context.QueryRecursive(listName, camlViewFields, viewFields, filter, join, projectedFields, top)
                    : context.Query(listName, camlViewFields, viewFields, filter, join, projectedFields, top);

                // Parsing json
                if (!string.IsNullOrWhiteSpace(outputContent))
                {
                    var obj = JObject.Parse(outputContent);
                    var array = obj["value"] as JArray;

                    // Generate output
                    //array.ForEach(results.Add);
                    return array;
                }
            }
            catch (Exception exp)
            {
                // TODO: LOG here
                //Cs.LogError.Error(exp, "Database layer exception - Query from list {0} with viewFields {1} and filter {2} join {3} projected {4} get top {5} support recursive {6} - under account {7}",
                //    listName, viewFields, filter, join, projectedFields, top, recursive, Cs.ImpersonateAccount.UserName);

                throw exp;
            }

            return results;
        }

    }
}
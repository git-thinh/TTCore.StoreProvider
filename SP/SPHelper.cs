using System.IO;
using System.Net;

namespace TTCore.StoreProvider
{
    public class SPHelper
    {
        public static string GetString(string accessToken, string url)
        {
            string s = "";
            HttpWebRequest endpointRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            endpointRequest.Method = "GET";
            endpointRequest.Accept = "application/json;odata=verbose";
            endpointRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpWebResponse res = (HttpWebResponse)endpointRequest.GetResponse();
            s = new StreamReader(res.GetResponseStream()).ReadToEnd();
            return s;
        }

    }
}
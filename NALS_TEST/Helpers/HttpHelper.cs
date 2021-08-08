using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace NALSTEST.Helpers
{
    public class HttpHelper
    {
        public static string PATH = System.Configuration.ConfigurationManager.AppSettings.Get("PathAPI").ToString();
        public static string HttpPost(string URI, String content, ref string error)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpContent httpContent = new StringContent(content);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var uri = string.Format("{0}{1}", PATH, URI);
                    var res = client.PostAsync(uri, httpContent).Result;
                    res.EnsureSuccessStatusCode();
                    var result = res.Content.ReadAsStringAsync().Result;
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        
    }
}
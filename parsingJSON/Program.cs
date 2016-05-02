using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace parsingJSON
{
    internal class Program
    {
        public static async Task<string> httpResponse(string url)
        {
            //i'm using a proxy, you could remove this if needed.
            IWebProxy iprox = WebProxy.GetDefaultProxy();
            iprox.Credentials = CredentialCache.DefaultCredentials;

            HttpClientHandler httpHandler = new HttpClientHandler()
            {
                UseProxy = true,
                Proxy = iprox,
                PreAuthenticate = true,
                UseDefaultCredentials = true,
                Credentials = CredentialCache.DefaultCredentials
            };

            using (var httpClient = new HttpClient(httpHandler))
                return await httpClient.GetStringAsync(url);
        }

        private static void Main(string[] args)
        {
            //get some JSON data
            JObject jData = JObject.Parse(httpResponse(@"https://www.googleapis.com/customsearch/v1?q=morgan+lewis&cx=017767199019333658645%3Ahey-u5glvao&key=AIzaSyDavzc5lneutuoFy-K0Ycs7tFRu_8Hbj74").Result);

            string sampleValue = (string)jData["url"]["type"];

            Console.WriteLine(sampleValue);

            //loop through array in JSON
            foreach (var sampleItem in jData["items"])
            {
                string sampleItemValue = (string)sampleItem["link"];
                Console.WriteLine(sampleItemValue);
            }

            //filter w/ linq:

            var sampleArray =
              from p in jData["items"]
              select p;

            foreach (var sampleItem in sampleArray.Where(p => ((string)p["link"]).Contains("https")))
            {
                string sampleItemValue = (string)sampleItem["link"];
                Console.WriteLine(sampleItemValue);
            }

            Console.ReadLine();
        }
    }
}
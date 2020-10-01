using RestSharp;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace proxyscrapeProxyTool
{
    public class Proxy
    {
        private string proxyAddress { get; set; }
        private string port { get; set; }
        public Proxy(string proxyAddress, string port)
        {
            this.proxyAddress = proxyAddress;
            this.port = port;
        }
        public Proxy()
        {

        }
        public List<Proxy> proxyScrape()
        {
            string url = "https://api.proxyscrape.com/?request=getproxies&proxytype=http&timeout=10000&country=all&ssl=all&anonymity=all";

            RestClient client = new RestClient(url);

            client.AddDefaultHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:80.0) Gecko/20100101 Firefox/80.0");
            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Language", "en-GB,en;q=0.5");
            client.AddDefaultHeader("DNT", "1");
            client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
            client.ConnectionGroupName = "keep-alive";
            client.BaseHost = "api.proxyscrape.com";
            RestRequest request = new RestRequest("", Method.GET);

            IRestResponse response = client.Execute(request);
            string content = response.Content;
            string[] splitedContent = content.Split(':', '\n');

            List<Proxy> proxyList = new List<Proxy>();
            string queryForIPAddress = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}";
            Regex regex = new Regex(queryForIPAddress);
            for (int i = 0; i < splitedContent.Length; i++)
            {
                if(regex.IsMatch(splitedContent[i]))
                {
                    Proxy proxy = new Proxy(splitedContent[i].Trim(), splitedContent[i + 1].Trim());
                    proxyList.Add(proxy);
                    i = i + 1;
                }
            }
            return proxyList;
        }
    }
}

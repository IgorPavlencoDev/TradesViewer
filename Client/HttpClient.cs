using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace TradesViewer.Client
{
    //There is simple http client needed for get some basic info from server
    public class HttpClient
    {
        private static System.Net.Http.HttpClient _client;

#warning TODO remake via .ini file
        private const string _pingURL = "";
        private const string _timeURL = "";
        private const string _exchangeInfoURL = "https://data-api.binance.vision/api/v3/exchangeInfo?permissions=SPOT";

        private static void InitialiseHttpClient()
        {
            _client = new System.Net.Http.HttpClient();
        }

        public static void HttpClientThread()
        {
            InitialiseHttpClient();

#warning TODO check ping

#warning TODO check time

            HttpResponseMessage httpResponse = _client.GetAsync(_exchangeInfoURL).Result;
        }
    }
}

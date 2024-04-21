using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradesViewer.Shared;
using static System.Net.WebRequestMethods;

namespace TradesViewer.Client
{
    //There is simple http client needed for get some basic info from server
    public class HttpClient
    {
        private static System.Net.Http.HttpClient _client;

#warning TODO remake via .ini file
        private const string _pingURL = "https://data-api.binance.vision/api/v3/ping";
        private const string _timeURL = "https://data-api.binance.vision/api/v3/time";
        private const string _exchangeInfoURL = "https://data-api.binance.vision/api/v3/exchangeInfo?permissions=SPOT";

        private static HttpResponseMessage httpResponse;

        private static Task<HttpResponseMessage> httpMessageHandler;

        private static void InitialiseHttpClient()
        {
            _client = new System.Net.Http.HttpClient();
        }

        public static void HttpClientThread()
        {
            InitialiseHttpClient();

            httpMessageHandler = _client.GetAsync(_pingURL);
            httpMessageHandler.Wait();
            SignalsManager._eventPingGETDone.Set();

            httpMessageHandler = _client.GetAsync(_timeURL);
            httpMessageHandler.Wait();
            SignalsManager._eventTimeGETDone.Set();
#warning TODO time check

            httpMessageHandler = _client.GetAsync(_exchangeInfoURL);
            httpMessageHandler.Wait();
            SignalsManager._exchangeInfoGETDone.Set();

        }
    }
}

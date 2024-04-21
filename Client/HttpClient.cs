using Newtonsoft.Json;
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

        private static HttpResponseMessage _httpResponse;

        private static Task<HttpResponseMessage> _httpMessageHandler;
        private static Task<string> _readMessageHandler;

        private static void InitialiseHttpClient()
        {
            _client = new System.Net.Http.HttpClient();
        }

        public static void HttpClientThread()
        {
            try
            {
                InitialiseHttpClient();

                SignalsManager._eventStartPingGETCheck.WaitOne();
                _httpMessageHandler = _client.GetAsync(_pingURL);
                _httpMessageHandler.Wait();
                _httpResponse = _httpMessageHandler.Result;
                _httpResponse.EnsureSuccessStatusCode();
                SignalsManager._eventPingGETDone.Set();

                SignalsManager._eventStartTimeGETCheck.WaitOne();
                _httpMessageHandler = _client.GetAsync(_timeURL);
                _httpMessageHandler.Wait();
#warning TODO time check
                _httpResponse = _httpMessageHandler.Result;
                _httpResponse.EnsureSuccessStatusCode();
                SignalsManager._eventTimeGETDone.Set();

                SignalsManager._eventStartExchangeInfoGETCheck.WaitOne();
                _httpMessageHandler = _client.GetAsync(_exchangeInfoURL);
                _httpMessageHandler.Wait();
                _httpResponse = _httpMessageHandler.Result;
                _httpResponse.EnsureSuccessStatusCode();
                SignalsManager._eventExchangeInfoGETDone.Set();

                _readMessageHandler = _httpResponse.Content.ReadAsStringAsync();
                _readMessageHandler.Wait();
                string messageText = _readMessageHandler.Result;
                DataManager.ExchangeInfo = JsonConvert.DeserializeObject<ExchangeInfoCarrier>(messageText);
            }
            catch (Exception e)
            {
                Console.WriteLine("An critical error occurred while initializing the program: " + e.ToString());
                SignalsManager._eventCriticalError.Set();
            }
        }
    }
}

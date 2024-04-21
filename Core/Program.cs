using System;
using System.Collections.Generic;
using System.Threading;
using TradesViewer.Client;
using TradesViewer.Shared;
using TradesViewer.Tests;
using TradesViewer.Tools;

namespace TradesViewer.Core
{
    //There is core part of the program
    internal class Program
    {
        private static Thread _userInterface;

        private static Thread _httpClient;
        private static List<Thread> _webSocketSubscribers;

        private static Thread _dataManager;
        private static Thread _diagnostics;

        static void Main(string[] args)
        {
            try
            {
                SignalsManager.Initialisation();

                _userInterface = new Thread(UserInterface.UserInterfaceThread);
                _httpClient = new Thread(HttpClient.HttpClientThread);
                _diagnostics = new Thread(Diagnostics.DiagnosticsThread);

            } catch (Exception e)
            {
                Console.WriteLine("An critical error occurred while initializing the program: " + e.ToString());
                return;
            }

            TimeSpan timeOut = new TimeSpan(0, 1, 0);
            bool isTimeOut = false;

            _userInterface.Start();
            isTimeOut = !SignalsManager.EventUIReady.WaitOne(timeOut);
            if (!isTimeOut)
            {
                _httpClient.Start();
                _diagnostics.Start();

                SignalsManager.EventCriticalError.WaitOne();
                Console.WriteLine("App terminated.");
                return;
            }
            else
            {
                Console.WriteLine("Error: something wrong with UI");
                return;
            }
        }
    }
}

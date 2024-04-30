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
        private const string _timeOutText = "Error: Application startup timeout.";

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
                _httpClient = new Thread(TradesViewer.Client.HttpClient.HttpClientThread);
                _diagnostics = new Thread(Diagnostics.DiagnosticsThread);

            } catch (Exception e)
            {
                Console.WriteLine("An critical error occurred while initializing the program: " + e.ToString());
                return;
            }

            TimeSpan timeOut = new TimeSpan(0, 1, 0);
            bool isTimeOut = false;
#warning TODO put timeout size in .ini file


            _userInterface.Start();
            isTimeOut = !SignalsManager.EventUIReady.WaitOne(timeOut);
            if (!isTimeOut)
            {
                _httpClient.Start();
                _diagnostics.Start();

                isTimeOut = !SignalsManager.EventApplicationReadyToWork.WaitOne(timeOut);
                if (isTimeOut)
                {
                    Console.WriteLine(_timeOutText);
                }
                else 
                {
                    UserInterface.ResetUI();
                    SignalsManager.EventCriticalError.WaitOne();
                }

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

using System.Collections.Generic;
using System.Threading;
using TradesViewer.Client;
using TradesViewer.Tools;

namespace TradesViewer.Core
{
    //There is core part of the program
    internal class Program
    {
        internal static Thread _userInterface;

        internal static Thread _httpClient;
        internal static List<Thread> _webSocketSubscribers;

        internal static Thread _dataManager;

        //internal static Thread _mainThread;

        static void Main(string[] args)
        {
            _userInterface = new Thread(UserInterface.UserInterfaceThread);

            _httpClient = new Thread(HttpClient.HttpClientThread);
            _httpClient.Start();

            _userInterface.Start();

            //_mainThread = Thread.CurrentThread;
        }
    }
}

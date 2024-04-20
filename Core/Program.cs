using System.Collections.Generic;
using System.Threading;

namespace TradesViewer.Core
{
    //There is core part of the program
    internal class Program
    {
        internal static Thread _userInterface;

        internal static Thread _httpClient;
        internal static List<Thread> _webSocketSubscribers;

        internal static Thread _dataManager;

        static void Main(string[] args)
        {
        }
    }
}

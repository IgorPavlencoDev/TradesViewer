using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradesViewer.Shared;
using TradesViewer.Tools.CLI;

namespace TradesViewer.Tools
{
    //There is abstract layer of user interface
    public class UserInterface
    {

        private static EventWaitHandle _eventNewInfoTextPlaced;
        private static string infoOutputText;

#if !CLI && !GUI
#error SET DEFINITION TO SELECT USER INTERACTION TYPE
#elif CLI
        //There is a mutually exclusive choice because I want to completely replace the CLI with a GUI in the future
        private static ConsoleInterface _userInterface;
        private static void InitialiseUI()
        {
            _userInterface = new ConsoleInterface();
            _eventNewInfoTextPlaced = new EventWaitHandle(false, EventResetMode.AutoReset);
            SignalsManager.EventUIReady.Set();
        }
#elif GUI
        private static GraphicInterface _userInterface;
        private static void InitialiseUI()
        {
            _userInterface = new GraphicInterface();
        }
#endif

        public static void ChangeInfo(in string s)
        {
            infoOutputText = s;
            _eventNewInfoTextPlaced.Set();
        }

        public static void UserInterfaceThread()
        {
            InitialiseUI();
            while(true)
            {
                _eventNewInfoTextPlaced.WaitOne();
                _userInterface.ChangeInfo(infoOutputText);
                _userInterface.Refresh();
            }
        }
    }
}

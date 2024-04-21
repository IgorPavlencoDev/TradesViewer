using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradesViewer.Tools.CLI;

namespace TradesViewer.Tools
{
    internal enum StageOfUI
    {
        Start,
        DiagnosticsPing,
        DiagnosticsTime,
        DiagnosticsExchangeInfo,
        AllowedToWork
    }

    //There is abstract layer of user interface
    public class UserInterface
    {
        private static StageOfUI _currentStage = StageOfUI.Start;

#if !CLI && !GUI
#error SET DEFINITION TO SELECT USER INTERACTION TYPE
#elif CLI
        //There is a mutually exclusive choice because I want to completely replace the CLI with a GUI in the future
        private static ConsoleInterface _userInterface;
        private static void InitialiseUI()
        {
            _userInterface = new ConsoleInterface();
        }
#elif GUI
        private static GraphicInterface _userInterface;
        private static void InitialiseUI()
        {
            _userInterface = new GraphicInterface();
        }
#endif

        public static void UserInterfaceThread()
        {
            bool FlagInDebugPurposes = true;
#warning clear debug garbage
            while (FlagInDebugPurposes)
            {
                switch(_currentStage)
                {
                    case StageOfUI.Start:
                        InitialiseUI();
                        _userInterface.ChangeInfo("Initial checks, please wait...");
                        _userInterface.Refresh();
                        _currentStage = StageOfUI.DiagnosticsPing; 
                        break;

                    case StageOfUI.DiagnosticsPing:
                        _currentStage = StageOfUI.DiagnosticsTime;
                        break;

                    case StageOfUI.DiagnosticsTime:
                        _currentStage = StageOfUI.DiagnosticsExchangeInfo;
                        break;

                    case StageOfUI.DiagnosticsExchangeInfo:
                        _currentStage = StageOfUI.AllowedToWork;
                        break;

                    case StageOfUI.AllowedToWork:
                        _userInterface.ChangeInfo("All seems to be fine!");
                        _userInterface.Refresh();
                        FlagInDebugPurposes = false;
                        break;

                    default:
                        Assert.Fail("Something wrong in switch case in UserInterface.cs");
                        break;
                }
            }
        }
    }
}

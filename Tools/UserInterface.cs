using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesViewer.Shared;
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
#warning TODO remake via .ini file
        private const string _basicStartCheckText = "Initial checks, please wait...";
        private const string _pingCheckDoneText = "\nPing done succesful!";
        private const string _timeCheckDoneText = "\nServer time is correct!";
        private const string _exchangeInfoCheckDoneText = "\nData from exchange received!";
        private const string _allFineText = "\nEverything seems to be fine!";

        private const string _timeOutText = "Connection timeout. Please, check internet connection and launch app again.";

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
            TimeSpan timeOut = new TimeSpan(0, 1, 0);
            bool isTimeOut = false;
#warning TODO put timeout size in .ini file
#warning make several attempts for ping?

            bool FlagInDebugPurposes = true;
#warning TODO clear debug garbage
#warning TODO better exception handling?
            try
            {
                while (FlagInDebugPurposes)
                {
                    switch (_currentStage)
                    {
                        case StageOfUI.Start:
                            InitialiseUI();
                            _userInterface.ChangeInfo(_basicStartCheckText);
                            _userInterface.Refresh();
                            SignalsManager._eventStartPingGETCheck.Set();
                            _currentStage = StageOfUI.DiagnosticsPing;
                            break;

                        case StageOfUI.DiagnosticsPing:
                            isTimeOut = !SignalsManager._eventPingGETDone.WaitOne(timeOut);
                            if (isTimeOut)
                            {
                                throw new Exception(_timeOutText);
                            }
                            _userInterface.ChangeInfo(  _basicStartCheckText + 
                                                        _pingCheckDoneText);
                            _userInterface.Refresh();
                            SignalsManager._eventStartTimeGETCheck.Set();
                            _currentStage = StageOfUI.DiagnosticsTime;
                            break;

                        case StageOfUI.DiagnosticsTime:
                            isTimeOut = !SignalsManager._eventTimeGETDone.WaitOne(timeOut);
                            if (isTimeOut)
                            {
                                throw new Exception(_timeOutText);
                            }
                            _userInterface.ChangeInfo(  _basicStartCheckText + 
                                                        _pingCheckDoneText + 
                                                        _timeCheckDoneText);
                            _userInterface.Refresh();
                            SignalsManager._eventStartExchangeInfoGETCheck.Set();
                            _currentStage = StageOfUI.DiagnosticsExchangeInfo;
                            break;

                        case StageOfUI.DiagnosticsExchangeInfo:
                            isTimeOut = !SignalsManager._eventExchangeInfoGETDone.WaitOne(timeOut);
                            if (isTimeOut)
                            {
                                throw new Exception(_timeOutText);
                            }
                            _userInterface.ChangeInfo(  _basicStartCheckText +
                                                        _pingCheckDoneText +
                                                        _timeCheckDoneText +
                                                        _exchangeInfoCheckDoneText);
                            _userInterface.Refresh();
                            _currentStage = StageOfUI.AllowedToWork;
                            break;

                        case StageOfUI.AllowedToWork:
                            _userInterface.ChangeInfo(_basicStartCheckText +
                                                        _pingCheckDoneText +
                                                        _timeCheckDoneText +
                                                        _exchangeInfoCheckDoneText +
                                                        _allFineText);
                            _userInterface.Refresh();
                            FlagInDebugPurposes = false;
                            break;

                        default:
                            Assert.Fail("Something wrong in switch case in UserInterface.cs");
                            break;
                    }
                }
            } 
            catch (Exception e) 
            {
                Console.WriteLine("An critical error occurred while initializing the program: " + e.ToString());
                SignalsManager._eventCriticalError.Set();
            }
        }
    }
}

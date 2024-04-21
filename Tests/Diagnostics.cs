using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesViewer.Shared;
using TradesViewer.Tools;

namespace TradesViewer.Tests
{
    public static class Diagnostics
    {

#warning TODO remake via .ini file
        private const string _basicStartCheckText = "Initial checks, please wait...";
        private const string _pingCheckDoneText = "\nPing done succesful!";
        private const string _timeCheckDoneText = "\nServer time is correct!";
        private const string _exchangeInfoCheckDoneText = "\nData from exchange received!";
        private const string _allFineText = "\nEverything seems to be fine!";

        private const string _timeOutText = "Connection timeout. Please, check internet connection and launch app again.";

        internal enum StageOfApp
        {
            Start,
            DiagnosticsPing,
            DiagnosticsTime,
            DiagnosticsExchangeInfo,
            AllowedToWork
        }

        private static StageOfApp _currentStage = StageOfApp.Start;

        public static void DiagnosticsThread()
        {
            TimeSpan timeOut = new TimeSpan(0, 1, 0);
            bool isTimeOut = false;
#warning TODO put timeout size in .ini file
#warning make several attempts for ping?

            bool isDiagnosticsInProgress = true;
#warning TODO clear debug garbage
#warning TODO better exception handling?
            try
            {
                while (isDiagnosticsInProgress)
                {
                    switch (_currentStage)
                    {
                        case StageOfApp.Start:
                            UserInterface.ChangeInfo(_basicStartCheckText);
                            SignalsManager.EventStartPingGETCheck.Set();
                            _currentStage = StageOfApp.DiagnosticsPing;
                            break;

                        case StageOfApp.DiagnosticsPing:
                            isTimeOut = !SignalsManager.EventPingGETDone.WaitOne(timeOut);
                            if (isTimeOut)
                            {
                                throw new Exception(_timeOutText);
                            }
                            UserInterface.ChangeInfo(_basicStartCheckText +
                                                        _pingCheckDoneText);
                            SignalsManager.EventStartTimeGETCheck.Set();
                            _currentStage = StageOfApp.DiagnosticsTime;
                            break;

                        case StageOfApp.DiagnosticsTime:
                            isTimeOut = !SignalsManager.EventTimeGETDone.WaitOne(timeOut);
                            if (isTimeOut)
                            {
                                throw new Exception(_timeOutText);
                            }
                            UserInterface.ChangeInfo(_basicStartCheckText +
                            _pingCheckDoneText +
                                                        _timeCheckDoneText);
                            SignalsManager.EventStartExchangeInfoGET.Set();
                            _currentStage = StageOfApp.DiagnosticsExchangeInfo;
                            break;

                        case StageOfApp.DiagnosticsExchangeInfo:
                            isTimeOut = !SignalsManager.EventExchangeInfoGETDone.WaitOne(timeOut);
                            if (isTimeOut)
                            {
                                throw new Exception(_timeOutText);
                            }
                            UserInterface.ChangeInfo(_basicStartCheckText +
                                                        _pingCheckDoneText +
                                                        _timeCheckDoneText +
                                                        _exchangeInfoCheckDoneText);
                            _currentStage = StageOfApp.AllowedToWork;
                            break;

                        case StageOfApp.AllowedToWork:
                            UserInterface.ChangeInfo(_basicStartCheckText +
                                                        _pingCheckDoneText +
                                                        _timeCheckDoneText +
                                                        _exchangeInfoCheckDoneText +
                                                        _allFineText);
                            isDiagnosticsInProgress = false;
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
                SignalsManager.EventCriticalError.Set();
            }
        }
    }
}

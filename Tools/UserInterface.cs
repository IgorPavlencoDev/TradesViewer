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
#warning TODO remake via .ini file

        private static EventWaitHandle _eventNewUIInteraction;

        private static EventWaitHandle _eventNewInfoTextPlaced;
        private static string _infoOutputText;
        private const string _infoDefaultText =  "Welcome to TradesViewer 1.0.\n\n" +
                                                 "Please start entering your currency to view the app's suggestions.\n" +
                                                 "You can also enter the up arrow or down arrow to change the amount of saved trades.\n" +
                                                 "But this option does not change after you select the first trading pair\n" +
                                                 "A more flexible interface is currently in WIP. And the GUI is in WIP too.";

        private static EventWaitHandle _eventNewSecondaryInfoTextPlaced;
        private static string _secondaryInfoOutputText;
        private const string _secondaryInfoDefaultText = "Current volume of trades saved: WIP";

        private static EventWaitHandle _eventNewUserInputTextPlaced;
        private static string _userInputOutputText;
        private const string _userInputDefaultText = ">> WIP";

        private static EventWaitHandle _eventNewSuggestionsTextPlaced;
        private static string _suggestionsOutputText;
        private const string _suggestionsDefaultText = "Suggestions: (empty, WIP)";

        private static EventWaitHandle _eventNewTrackerTextPlaced;
        private static string _trackerOutputText;
        private const string _trackerDefaultText = "Tracking pairs: (empty, WIP)";

#if !CLI && !GUI
#error SET DEFINITION TO SELECT USER INTERACTION TYPE
#elif CLI
        //There is a mutually exclusive choice because I want to completely replace the CLI with a GUI in the future
        private static ConsoleInterface _userInterface;
        private static void InitialiseUI()
        {
            _userInterface = new ConsoleInterface();

            _eventNewUIInteraction = new EventWaitHandle(false, EventResetMode.AutoReset);
            _eventNewInfoTextPlaced = new EventWaitHandle(false, EventResetMode.AutoReset);
            _eventNewSecondaryInfoTextPlaced = new EventWaitHandle(false, EventResetMode.AutoReset);
            _eventNewUserInputTextPlaced = new EventWaitHandle(false, EventResetMode.AutoReset);
            _eventNewSuggestionsTextPlaced = new EventWaitHandle(false, EventResetMode.AutoReset);
            _eventNewTrackerTextPlaced = new EventWaitHandle(false, EventResetMode.AutoReset);

            SignalsManager.EventUIReady.Set();
        }
#elif GUI
        private static GraphicInterface _userInterface;
        private static void InitialiseUI()
        {
            _userInterface = new GraphicInterface();
            _eventNewInfoTextPlaced = new EventWaitHandle(false, EventResetMode.AutoReset);
            SignalsManager.EventUIReady.Set();
        }
#endif

        public static void ChangeInfo(in string s)
        {
            _infoOutputText = s;
            _eventNewInfoTextPlaced.Set();
            _eventNewUIInteraction.Set();
        }

        public static void ChangeSecondaryInfo(in string s) 
        {
            _secondaryInfoOutputText = s;
            _eventNewSecondaryInfoTextPlaced.Set();
            _eventNewUIInteraction.Set();
        }

        public static void ChangeUserInput(in string s) 
        {
            _userInputOutputText = s;
            _eventNewUserInputTextPlaced.Set();
            _eventNewUIInteraction.Set();
        }

        public static void ChangeSuggestions(in string s) 
        {
            _suggestionsOutputText = s;
            _eventNewSuggestionsTextPlaced.Set();
            _eventNewUIInteraction.Set();
        }

        public static void ChangeTracker(in string s) 
        {
            _trackerOutputText = s;
            _eventNewTrackerTextPlaced.Set();
            _eventNewUIInteraction.Set();
        }

        public static void ResetUI()
        {
            ChangeInfo(_infoDefaultText);
            ChangeSecondaryInfo(_secondaryInfoDefaultText);
            ChangeUserInput(_userInputDefaultText);
            ChangeSuggestions(_suggestionsDefaultText);
            ChangeTracker(_trackerDefaultText);
        }

        public static void UserInterfaceThread()
        {
            InitialiseUI();
            while (true)
            {
                _eventNewUIInteraction.WaitOne();
                if(SignalsManager.EventApplicationReadyToWork.WaitOne(0))
                {
                    if (_eventNewInfoTextPlaced.WaitOne(0))
                    {
                        _userInterface.ChangeInfo(_infoOutputText);
                    }
                    if (_eventNewSecondaryInfoTextPlaced.WaitOne(0))
                    {
                        _userInterface.ChangeSecondaryInfo(_secondaryInfoOutputText);
                    }
                    if (_eventNewUserInputTextPlaced.WaitOne(0))
                    {
                        _userInterface.ChangeUserInput(_userInputOutputText);
                    }
                    if (_eventNewSuggestionsTextPlaced.WaitOne(0))
                    {
                        _userInterface.ChangeSuggestions(_suggestionsOutputText);
                    }
                    if (_eventNewTrackerTextPlaced.WaitOne(0))
                    {
                        _userInterface.ChangeTracker(_trackerOutputText);
                    }

                    _userInterface.Refresh();
                }
                else
                {
                    _userInterface.ChangeInfo(_infoOutputText);
                    _userInterface.Refresh();
                }
            }
        }
    }
}

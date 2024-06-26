﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TradesViewer.Tools.CLI
{
    //There is CLI technical details
    public class ConsoleInterface
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private Thread _inputThread;

        private string _info;
        private string _secondaryInfo;
        private string _userInput;
        private string _suggestions;
        private string _tracker;

        private const string _line_Separator = "__________________________________________________________";

        //better interface in progress
        public void ChangeInfo(string s) { _info = s; }
        public void ChangeSecondaryInfo(string s) { _secondaryInfo = s; }
        public void ChangeUserInput(string s) { _userInput = s; }
        public void ChangeSuggestions(string s) { _suggestions = s; }
        public void ChangeTracker(string s) { _tracker = s; }

        public void Initialize()
        {
            _inputThread = new Thread(WaitingInputThread);
            _inputThread.Start();
        }

        private void WaitingInputThread()
        {
            while (true)
            {
            }
        }

        public void Refresh() 
        { 
            Console.Clear();

            if (_info != null)
            {
                Console.WriteLine(_info);
            }
            if (_secondaryInfo != null)
            {
                Console.WriteLine("\n" + "\n" + _secondaryInfo + "\n" + _line_Separator);
            }

            if (_userInput != null)
            {
                Console.WriteLine("\n" + _userInput);
            }

            if (_suggestions != null)
            {
                Console.WriteLine("\n" + _suggestions + "\n" + _line_Separator);
            }

            if (_tracker != null)
            {
                Console.WriteLine("\n" + _tracker);
            }
        }
    }
}

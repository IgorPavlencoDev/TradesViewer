using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesViewer.Tools.CLI
{
    //There is CLI technical details
    public class ConsoleInterface
    {
        private string _info;
        private string _secondaryInfo;
        private string _userInput;
        private string _suggestions;
        private string _tracker;

        //better interface in progress
        public void ChangeInfo(string s) { _info = s; }
        public void ChangeSecondaryInfo(string s) { _secondaryInfo = s; }
        public void ChangeUserInput(string s) { _userInput = s; }
        public void ChangeSuggestions(string s) { _suggestions = s; }
        public void ChangeTracker(string s) { _tracker = s; }

        public void Refresh() 
        { 
            Console.Clear();

            if (!string.IsNullOrEmpty(_info))
            {
                Console.WriteLine(_info);
            }

            if (!string.IsNullOrEmpty(_secondaryInfo))
            {
                Console.WriteLine(_secondaryInfo);
            }

            if (!string.IsNullOrEmpty(_userInput))
            {
                Console.WriteLine(_userInput);
            }

            if (!string.IsNullOrEmpty(_suggestions))
            {
                Console.WriteLine(_suggestions);
            }

            if (!string.IsNullOrEmpty(_tracker))
            {
                Console.WriteLine(_tracker);
            }
        }
    }
}

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
        Ready
    }

    //There is abstract layer of user interface
    public class UserInterface
    {
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
            InitialiseUI();

            _userInterface.ChangeInfo("Initial checks, please wait...");
            _userInterface.Refresh();
        }
    }
}

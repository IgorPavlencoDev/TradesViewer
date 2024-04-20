using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradesViewer.Tools.CLI;

namespace TradesViewer.Tools
{
    //There is abstract layer of user interface
    internal class UserInterface
    {
#if !CLI && !GUI
#error SET DEFINITION TO SELECT USER INTERACTION TYPE
#elif CLI
        //There is a mutually exclusive choice because I want to completely replace the CLI with a GUI in the future
        private ConsoleInterface _userInterface;
#elif GUI
        private GraphicInterface _userInterface;
#endif
    }
}

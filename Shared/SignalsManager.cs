using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradesViewer.Shared
{
    //There is interfaces for signaling about some states and etc.
    public static class SignalsManager
    {
        public static EventWaitHandle _eventPingGETDone { get; private set; }
        public static EventWaitHandle _eventTimeGETDone { get; private set; }
        public static EventWaitHandle _exchangeInfoGETDone { get; private set; }

        public static void Initialisation()
        {
            _eventPingGETDone = new EventWaitHandle(false, EventResetMode.AutoReset);
            _eventTimeGETDone = new EventWaitHandle(false, EventResetMode.AutoReset);
            _exchangeInfoGETDone = new EventWaitHandle(false, EventResetMode.AutoReset);
        }
    }
}

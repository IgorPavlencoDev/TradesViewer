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
        public static EventWaitHandle EventUIReady { get; private set; }

        public static EventWaitHandle EventPingGETDone { get; private set; }
        public static EventWaitHandle EventTimeGETDone { get; private set; }
        public static EventWaitHandle EventExchangeInfoGETDone { get; private set; }

        public static EventWaitHandle EventStartPingGETCheck { get; private set; }
        public static EventWaitHandle EventStartTimeGETCheck { get; private set; }
        public static EventWaitHandle EventStartExchangeInfoGET { get; private set; }

        public static EventWaitHandle EventCriticalError { get; private set; }

        public static void Initialisation()
        {
            EventUIReady = new EventWaitHandle(false, EventResetMode.AutoReset);

            EventPingGETDone = new EventWaitHandle(false, EventResetMode.AutoReset);
            EventTimeGETDone = new EventWaitHandle(false, EventResetMode.AutoReset);
            EventExchangeInfoGETDone = new EventWaitHandle(false, EventResetMode.AutoReset);

            EventStartPingGETCheck = new EventWaitHandle(false, EventResetMode.AutoReset);
            EventStartTimeGETCheck = new EventWaitHandle(false, EventResetMode.AutoReset);
            EventStartExchangeInfoGET = new EventWaitHandle(false, EventResetMode.AutoReset);

            EventCriticalError = new EventWaitHandle(false, EventResetMode.AutoReset);
        }
    }
}

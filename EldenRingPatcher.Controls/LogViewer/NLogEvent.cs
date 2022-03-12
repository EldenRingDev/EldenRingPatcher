using NLog;
using System;

namespace EldenRingPatcher.Controls.LogViewer
{
    public class NLogEvent : EventArgs
    {
        private readonly LogEventInfo EventInfo;

        private NLogEvent(LogEventInfo LogEventInfo)
        {
            EventInfo = LogEventInfo;
        }

        public static implicit operator LogEventInfo(NLogEvent nLogEvent)
        {
            return nLogEvent.EventInfo;
        }

        public static implicit operator NLogEvent(LogEventInfo logEventInfo)
        {
            return new NLogEvent(logEventInfo);
        }
    }
}
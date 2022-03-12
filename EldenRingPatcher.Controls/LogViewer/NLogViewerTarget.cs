using NLog.Common;
using NLog.Targets;
using System;

namespace EldenRingPatcher.Controls.LogViewer
{
    [Target("NLogViewer")]
    public class NLogViewerTarget : Target
    {
        public event Action<AsyncLogEventInfo> LogReceived;

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            base.Write(logEvent);
            LogReceived?.Invoke(logEvent);
        }
    }
}
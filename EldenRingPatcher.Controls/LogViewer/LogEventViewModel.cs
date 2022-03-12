using NLog;
using System;
using System.Globalization;
using System.Windows.Media;

namespace EldenRingPatcher.Controls.LogViewer
{
    public class LogEventViewModel
    {
        public string Level { get; }
        public string FormattedMessage { get; }
        public Exception Exception { get; }
        public string LoggerName { get; }
        public string Time { get; }

        public LogEventViewModel(LogEventInfo logEventInfo)
        {
            Level = logEventInfo.Level.ToString();
            FormattedMessage = logEventInfo.FormattedMessage;
            Exception = logEventInfo.Exception;
            LoggerName = logEventInfo.LoggerName;
            Time = logEventInfo.TimeStamp.ToString(CultureInfo.InvariantCulture);

            SetupColors(logEventInfo);
        }

        public SolidColorBrush Background { get; set; }
        public SolidColorBrush Foreground { get; set; }
        public SolidColorBrush BackgroundMouseOver { get; set; }
        public SolidColorBrush ForegroundMouseOver { get; set; }

        private void SetupColors(LogEventInfo logEventInfo)
        {
            if (logEventInfo.Level == LogLevel.Warn)
            {
                Background = Brushes.Yellow;
                BackgroundMouseOver = Brushes.GreenYellow;
            }
            else if (logEventInfo.Level == LogLevel.Error)
            {
                Background = Brushes.Tomato;
                BackgroundMouseOver = Brushes.IndianRed;
            }
            else if (logEventInfo.Level == LogLevel.Fatal)
            {
                Background = Brushes.Red;
                BackgroundMouseOver = Brushes.DarkRed;
            }
            else
            {
                Background = Brushes.White;
                BackgroundMouseOver = Brushes.LightGray;
            }

            Foreground = Brushes.Black;
            ForegroundMouseOver = Brushes.Black;
        }
    }
}
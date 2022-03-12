using NLog;
using NLog.Common;
using NLog.Config;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace EldenRingPatcher.Controls.LogViewer
{
    /// <summary>
    /// Interaction logic for NLogViewer.xaml
    /// </summary>
    public partial class NLogViewer : INotifyPropertyChanged
    {
        public ObservableCollection<LogEventViewModel> LogEntries { get; private set; }
        private bool IsTargetConfigured { get; set; }
        public object BackgroundMouseOver { get; set; }
        public object ForegroundMouseOver { get; set; }

        [Description("Width of time column in pixels"), Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double TimeWidth { get; set; }
        [Description("Width of Logger column in pixels, or auto if not specified"), Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double LoggerNameWidth { get; set; }
        [Description("Width of Level column in pixels"), Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double LevelWidth { get; set; }
        [Description("Width of Message column in pixels"), Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double MessageWidth { get; set; }
        [Description("Width of Exception column in pixels"), Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double ExceptionWidth { get; set; }
        [Description("Maximum number of log entries to show"), Category("Data")]
        public int MaximumLogEntries { get; set; }
        [Description("Automatically scrolls to the last log item in the viewer. Default is true."), Category("Data")]
        [TypeConverter(typeof(BooleanConverter))]
        public bool AutoScrollToLast { get; set; } = true;

        #region "Flags to turn show/hide levels"
        private bool showTrace = true, showInfo = true, showDebug = true, showWarn = true, showError = true, showFatal = true;

        public bool ShowTrace
        {
            get => showTrace;
            set { showTrace = value; OnPropertyChanged(nameof(ShowTrace)); }
        }
        public bool ShowInfo
        {
            get => showInfo;
            set { showInfo = value; OnPropertyChanged(nameof(ShowInfo)); }
        }
        public bool ShowDebug
        {
            get => showDebug;
            set { showDebug = value; OnPropertyChanged(nameof(ShowDebug)); }
        }
        public bool ShowWarn
        {
            get => showWarn;
            set { showWarn = value; OnPropertyChanged(nameof(ShowWarn)); }
        }
        public bool ShowError
        {
            get => showError;
            set { showError = value; OnPropertyChanged(nameof(ShowError)); }
        }
        public bool ShowFatal
        {
            get => showFatal;
            set { showFatal = value; OnPropertyChanged(nameof(ShowFatal)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public NLogViewer()
        {
            IsTargetConfigured = false;
            LogEntries = new ObservableCollection<LogEventViewModel>();

            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this)) return;

            InitLoggingConfiguration();

            foreach (var target in LogManager.Configuration.AllTargets.Where(t => t is NLogViewerTarget).Cast<NLogViewerTarget>())
            {
                IsTargetConfigured = true;
                target.LogReceived += LogReceived;
            }
        }

        private void InitLoggingConfiguration()
        {
            LoggingConfiguration conf;
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (assemblyFolder == null) return;

            try
            {
                LogManager.ThrowConfigExceptions = true;
                conf = new XmlLoggingConfiguration(Path.Combine(assemblyFolder, "NLog.config"));
            }
            catch (Exception ex)
            {
                conf = new LoggingConfiguration();
            }

            try
            {
                LogManager.Configuration = conf;
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private void LogReceived(AsyncLogEventInfo log)
        {
            var vm = new LogEventViewModel(log.LogEvent);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var addLogEvent = vm.Level.ToLower() switch
                {
                    "debug" => ShowDebug,
                    "info" => ShowInfo,
                    "trace" => ShowTrace,
                    "warn" => ShowWarn,
                    "error" => ShowError,
                    "fatal" => ShowFatal,
                    _ => false
                };
                if (!addLogEvent) return;
                if (LogEntries.Count >= MaximumLogEntries) LogEntries.RemoveAt(0);

                LogEntries.Add(vm);
                grid.ScrollIntoView(vm);
            }));
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            LogEntries.Clear();
        }
    }
}
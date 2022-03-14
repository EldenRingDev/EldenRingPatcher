using NLog;
using System;
using System.Windows;

namespace EldenRingPatcher.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly Logger MainLog = LogManager.GetLogger("Main");

        public MainWindow()
        {
            InitializeComponent();
            MainLog.Log(LogLevel.Info, "Started!");
        }

        private void AppExit(object sender, RoutedEventArgs e)
        {
            Mouse.ReleaseHook();
        }

        private void LockCursorClick(object sender, EventArgs e)
        {
            MainLog.Log(LogLevel.Info, "Locking cursor to window: {0}", Window.Title);
            MainLog.Log(LogLevel.Info, "Window borderSizes: {0}", Window.BorderSizes.ToString());
            MainLog.Log(LogLevel.Info, "Window area: {0}", Window.WindowArea.ToString());
            Mouse.InitHook();
            Window.LaunchCursorLockingThread(GameClient.WindowHandle);
        }
    }
}
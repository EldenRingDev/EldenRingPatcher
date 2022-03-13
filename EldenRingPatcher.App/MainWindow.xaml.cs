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

        private void LockCursorToWindow(object sender, EventArgs e)
        {
            var hEldenRing = Window.GetHandle("ELDEN RING™");
            if (hEldenRing == IntPtr.Zero)
                MainLog.Log(LogLevel.Error, "Failed to get Elden Ring window handle!");

            MainLog.Log(LogLevel.Info, "Obtained Elden Ring window handle: 0x{0:x}", hEldenRing);

            var windowTitle = Window.GetText(hEldenRing, WindowSettings.WindowTitleMaxLength);
            if (windowTitle == null)
                MainLog.Log(LogLevel.Error, "The Elden Ring window doesn't exists anymore!");

            MainLog.Log(LogLevel.Info, "Locking Cursor to {0}", windowTitle);
            Mouse.InitHook();
            Window.LaunchCursorLockingThread(hEldenRing);
        }
    }
}

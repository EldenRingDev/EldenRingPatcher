using System;
using System.Windows;
using NLog;

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
            MainLog.Log(LogLevel.Warn, "Started!");
            LockCursorToEldenRingProc();
        }

        private void AppExit(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LockCursorToEldenRingProc()
        {
            IntPtr eldenRingHandle = Window.GetHandle("Elden RingT");

            var windowTitle = Window.GetText(eldenRingHandle, Window.WindowTitleMaxLength);
            if (windowTitle == null)
                MainLog.Log(LogLevel.Warn, "The Elden Ring window doesn't exists anymore!");

            MainLog.Log(LogLevel.Info, "Locking Cursor to {0}", windowTitle);
            Window.LockCursor(eldenRingHandle);
        }
    }
}

using System;
using System.Windows;

namespace EldenRingPatcher.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //MainLog.Log(LogLevel.Warn, "Started!");
            //LoadSettings();
        }

        private void AppExit(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

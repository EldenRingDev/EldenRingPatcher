using System.Diagnostics;
using System.Windows;

namespace EldenRingPatcher.App.GameTab
{
    /// <summary>
    /// Interaction logic for GameClientTab.xaml
    /// </summary>
    public partial class GameClientTab
    {
        private GameClientViewModel gameClientViewModel;

        public GameClientTab()
        {
            InitializeComponent();
            gameClientViewModel = new GameClientViewModel();
            DataContext = gameClientViewModel;
        }

        private void AttachClick(object sender, RoutedEventArgs e)
        {
            var procId = 0;

            foreach (var processList in Process.GetProcesses())
                if (processList.MainWindowTitle.Contains("ELDEN RING™"))
                    procId = processList.Id;

            if (procId != 0) 
                GameClient.Attach(procId);


            gameClientViewModel.Handle = GameClient.Handle.ToString();
            gameClientViewModel.WindowHandle = GameClient.WindowHandle.ToString();
            gameClientViewModel.BaseAddress = GameClient.BaseAddress.ToString();
            gameClientViewModel.Version = GameClient.Version;
        }
    }
}

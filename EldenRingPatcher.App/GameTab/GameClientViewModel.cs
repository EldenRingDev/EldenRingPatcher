using System.ComponentModel;

namespace EldenRingPatcher.App.GameTab
{
    public class GameClientViewModel : INotifyPropertyChanged
    {
        private string _handle;
        public string Handle
        {
            get => _handle;
            set
            {
                _handle = value;
                OnPropertyChanged(nameof(Handle));
            }
        }

        private string _windowHandle;
        public string WindowHandle
        {
            get => _windowHandle;
            set
            {
                _windowHandle = value;
                OnPropertyChanged(nameof(WindowHandle));
            }
        }

        private string _baseAddress;
        public string BaseAddress
        {
            get => _baseAddress;
            set
            {
                _baseAddress = value;
                OnPropertyChanged(nameof(BaseAddress));
            }
        }

        private string _version;
        public string Version
        {
            get => _version;
            set
            {
                _version = value;
                OnPropertyChanged(nameof(Version));
            }
        }

        public GameClientViewModel()
        {
            Handle = "0x00000000";
            WindowHandle = "0x00000000";
            BaseAddress = "0x00000000";
            Version = "0.0.0";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
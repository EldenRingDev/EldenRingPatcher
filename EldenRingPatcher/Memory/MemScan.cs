using NLog;
using Reloaded.Memory.Sigscan;

namespace EldenRingPatcher.Memory
{
    public class MemScan
    {
        private static Scanner MemScanner;
        private static readonly Logger ScanLog = LogManager.GetLogger("MemScan");

        public MemScan()
        {
            MemScanner = new Scanner(GameClient.Process, GameClient.Process.MainModule);
        }
    }
}
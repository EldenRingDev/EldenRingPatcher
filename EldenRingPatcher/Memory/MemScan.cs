using NLog;
using Reloaded.Memory.Sigscan;
using System;

namespace EldenRingPatcher.Memory
{
    public class MemScan
    {
        private static Scanner MemoryScanner;
        private static readonly Logger ScanLog = LogManager.GetLogger("MemScan");

        public MemScan()
        {
            MemoryScanner = new Scanner(GameClient.Process, GameClient.Process.MainModule);
        }

        public static IntPtr FindOffsetFromAOB(string aobPattern)
        {
            IntPtr resultOffset = IntPtr.Zero;

            // First found matching array of byte pattern from base of main module
            int matchOffset = MemoryScanner.CompiledFindPattern(aobPattern).Offset;
            if (matchOffset == 0x0) return IntPtr.Zero;
#if DEBUG
            ScanLog.Info("AOB match found at: 0x{0:x}", matchOffset);
#endif

            return resultOffset;
        }
    }
}
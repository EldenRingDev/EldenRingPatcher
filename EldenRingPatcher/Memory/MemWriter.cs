using EldenRingPatcher.WIN32API;
using NLog;
using System;

namespace EldenRingPatcher.Memory
{
    class MemWriter
    {
        private static readonly Logger WriteLog = LogManager.GetLogger("MemWriter");

        public static void Write(byte[] byteData, IntPtr addressToWriteAt, int sizeOfDataWritten)
        {
            if (!NativeMethods.WriteProcessMemory(GameClient.Handle, addressToWriteAt, byteData, sizeOfDataWritten, IntPtr.Zero))
                WriteLog.Warn($"Failed to write to process: {0} at address {1:x}", GameClient.Process, addressToWriteAt);
        }
    }
}

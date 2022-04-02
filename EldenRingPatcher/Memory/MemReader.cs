using EldenRingPatcher.WIN32API;
using NLog;
using System;
using System.Runtime.InteropServices;

namespace EldenRingPatcher.Memory
{
    public class MemReader
    {
        private static readonly Logger ReadLog = LogManager.GetLogger("MemReader");

        public static T Read<T>(IntPtr address)
        {
            int sizeOfType = 1;
            if (typeof(T) != typeof(bool))
                sizeOfType = Marshal.SizeOf(typeof(T));

            byte[] arrayOfBytes = ReadBytes(address, sizeOfType);
            GCHandle gcHandle = GCHandle.Alloc(arrayOfBytes, GCHandleType.Pinned);
            T obj = (T)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T))!;

            gcHandle.Free();
            return obj;
        }

        public static byte[] ReadBytes(IntPtr addressToRead, int size)
        {
            byte[] data = new byte[size];

            if(!NativeMethods.ReadProcessMemory(GameClient.Handle, addressToRead, data, data.Length, out int bytesRead))
                ReadLog.Warn($"Failed to read from process: {0} at address {1:x}", GameClient.Process, addressToRead);

            return bytesRead == 0 ? BitConverter.GetBytes(0) : data;
        }
    }
}
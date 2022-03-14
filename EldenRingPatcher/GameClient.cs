using EldenRingPatcher.WIN32API;
using EldenRingPatcher.WIN32API.Enums;
using NLog;
using System;
using System.Diagnostics;
using System.Linq;

namespace EldenRingPatcher
{
    public static class GameClient
    {
        public static Process Process;
        public static IntPtr Handle;
        public static IntPtr WindowHandle;

        public static IntPtr BaseAddress
        {
            get
            {
                if (Process != null)
                    return Process.MainModule?.BaseAddress ?? IntPtr.Zero;
                return IntPtr.Zero;
            }
        }

        public static string Version
        {
            get
            {
                if (Process != null)
                    return Process.MainModule?.FileVersionInfo.FileVersion ?? string.Empty;
                return string.Empty;
            }
        }

        public static string Build => !string.IsNullOrEmpty(Version)
                ? Version.Split('.').LastOrDefault()
                : string.Empty;

        public static bool Attached;
        public static string AttachTime;
        private static readonly Logger GameClientLog = LogManager.GetLogger("GameClient");

        public static void Attach(int processId)
        {
            try
            {
                Process = Process.GetProcessById(processId);
                if (null == Process)
                    throw new Exception($"Failed to find process by id: {processId}");

                Handle = NativeMethods.OpenProcess(ProcessAccessFlag.PROCESS_ALL_ACCESS, false, Process.Id);
                if (Handle == IntPtr.Zero)
                    throw new Exception($"Failed to find obtain handle to process by id: {processId}");

                WindowHandle = Process.MainWindowHandle;
                if (WindowHandle == IntPtr.Zero)
                    throw new Exception($"Failed to find obtain window handle to process by id: {processId}");

                Attached = true;
                AttachTime = $"[{DateTime.Now.ToShortTimeString()}]";
                GameClientLog.Info($"Sucessfully attached at: {AttachTime}");
            }
            catch (Exception ex)
            {
                GameClientLog.Error($"{ex.Message}");
            }
        }
    }
}
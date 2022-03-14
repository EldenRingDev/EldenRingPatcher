using EldenRingPatcher.WIN32API;
using EldenRingPatcher.WIN32API.Enums;
using NLog;
using System;
using System.Diagnostics;
using Point = EldenRingPatcher.WIN32API.Structures.Point;

namespace EldenRingPatcher
{
    public static class Mouse
    {
        private static readonly Logger MouseLog = LogManager.GetLogger("Mouse");
        private static readonly Delegates.LowLevelMouseProc hookProcedure = HookCallback;
        private static IntPtr hookID = IntPtr.Zero;

        public static Point CurPosition { get; set; }

        public static void InitHook()
        {
            hookID = SetHook(hookProcedure);
            if (hookID == IntPtr.Zero)
                MouseLog.Error("Failed to init mouse hook!");
        }

        public static void ReleaseHook()
        {
            NativeMethods.UnhookWindowsHookEx(hookID);
        }

        private static IntPtr SetHook(Delegates.LowLevelMouseProc procedure)
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;

            if (curModule != null)
                return NativeMethods.SetWindowsHookEx((int)HookType.WH_MOUSE_LL, procedure,
                    NativeMethods.GetModuleHandle(curModule.ModuleName), 0);

            return IntPtr.Zero;
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var wmMouse = (MouseMessage)wParam;
                if (wmMouse == MouseMessage.WM_MOUSEMOVE)
                {
                    Point mousePoint = GetPoint(lParam);
                    CurPosition = mousePoint;
                }
            }
            
            return NativeMethods.CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        private static Point GetPoint(IntPtr _xy)
        {
            uint xy = unchecked(IntPtr.Size == 8 ? (uint)_xy.ToInt64() : (uint)_xy.ToInt32());
            int x = unchecked((short)xy);
            int y = unchecked((short)(xy >> 16));

            return new Point(x, y);
        }
    }
}
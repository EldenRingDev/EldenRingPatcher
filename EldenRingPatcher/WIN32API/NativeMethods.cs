using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using EldenRingPatcher.WIN32API.Enums;

namespace EldenRingPatcher.WIN32API
{
    public static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowText")]
        internal static extern int UnmanagedGetWindowText(IntPtr hwnd, StringBuilder lpString, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetForegroundWindow")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowRect")]
        internal static extern int GetWindowRect(IntPtr hwnd, ref Rectangle lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClientRect")]
        internal static extern int GetClientRect(IntPtr hwnd, ref Rectangle lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ClipCursor")]
        internal static extern int ClipCursor(ref Rectangle lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ClipCursor")]
        internal static extern int ClipCursor(IntPtr lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetSystemMetrics")]
        internal static extern int GetSystemMetrics(SystemMetricIndex index);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
        internal static extern WindowStyleFlag GetWindowLong(IntPtr hwnd, WindowLongIndex index);
    }
}
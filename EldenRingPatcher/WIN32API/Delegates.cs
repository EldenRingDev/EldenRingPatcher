using System;

namespace EldenRingPatcher.WIN32API
{
    public static class Delegates
    {
        internal delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}
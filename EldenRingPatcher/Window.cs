using EldenRingPatcher.WIN32API;
using EldenRingPatcher.WIN32API.Enums;
using NLog;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Rectangle = EldenRingPatcher.WIN32API.Structures.Rectangle;

namespace EldenRingPatcher
{
    public static class Window
    {
        private static readonly Logger WindowLog = LogManager.GetLogger("Window");

        public static string Title
        {
            get
            {
                if (!GameClient.Attached) 
                    return string.Empty;

                var windowTitle = GetText(GameClient.WindowHandle, WindowSettings.WindowTitleMaxLength);
                if (!string.IsNullOrEmpty(windowTitle)) return windowTitle;

                WindowLog.Log(LogLevel.Error, "The game window doesn't exists anymore!");
                return string.Empty;
            }
            set => throw new NotImplementedException();
        }

        private static Rectangle borderSizes;
        public static Rectangle BorderSizes
        {
            get
            {
                if (!GameClient.Attached) 
                    return new Rectangle();

                if (borderSizes.Top != 0 && borderSizes.Left != 0
                                         && borderSizes.Right != 0
                                         && borderSizes.Bottom != 0)
                    return borderSizes;

                return GameClient.WindowHandle != IntPtr.Zero
                    ? GetBorderSizes(GameClient.WindowHandle)
                    : new Rectangle();
            }
            private set
            {
                borderSizes.Top = value.Top;
                borderSizes.Left = value.Left;
                borderSizes.Right = value.Right;
                borderSizes.Bottom = value.Bottom;
            }
        }

        public static Rectangle WindowArea
        {
            get
            {
                if (!GameClient.Attached)
                    return new Rectangle();

                var windowArea = new Rectangle();
                if (GameClient.WindowHandle == IntPtr.Zero)
                    return windowArea;

                if (NativeMethods.GetWindowRect(GameClient.WindowHandle, ref windowArea) == 0)
                    WindowLog.Log(LogLevel.Error, $"Get window rectangle win32 error selectedWindowHandle {GameClient.WindowHandle:d}");

                return windowArea;
            }
            set => throw new NotImplementedException();
        }

        public static Rectangle TotalArea
        {
            get
            {
                var totalArea = WindowArea;

                totalArea.Top += BorderSizes.Top;
                totalArea.Left += BorderSizes.Left;
                totalArea.Right -= BorderSizes.Right;
                totalArea.Bottom -= BorderSizes.Bottom;

                return totalArea;
            }
            set => throw new NotImplementedException();
        }

        public static void LaunchCursorLockingThread(IntPtr windowHandle)
        {
            new Thread(() =>
                { LockCursor(windowHandle); }).Start();
        }

        private static void LockCursor(IntPtr windowHandle)
        {
            WindowStyleFlag previousStyle = 0;
            var selectedWindowHadFocus = false;
            var validateHandleCount = 0;
            var selectedWindowTitle = GetText(windowHandle, WindowSettings.WindowTitleMaxLength);

            while (true)
            {
                // Check if window styles changed so the program doesn't break if the window's borders style is changed
                if (previousStyle != NativeMethods.GetWindowLong(windowHandle, WindowLongIndex.GWL_STYLE))
                {
                    // Determine border sizes for the selected window
                    BorderSizes = GetBorderSizes(windowHandle);
                    previousStyle = NativeMethods.GetWindowLong(windowHandle, WindowLongIndex.GWL_STYLE);
                }

                if (NativeMethods.GetForegroundWindow() == windowHandle)
                {
                    var clipArea = ExpandAreaByOffset(TotalArea, 10);

                    // TODO: check if CurPosition outside boundaries of windowArea clip only if needed to

                    WindowLog.Log(LogLevel.Info, "Clipping cursor to area: {0}", clipArea);
                    if (NativeMethods.ClipCursor(ref clipArea) == 0)
                        throw new Win32Exception(Marshal.GetLastWin32Error(), $"Clip cursor win32 error! Clip area: {clipArea}");

                    selectedWindowHadFocus = true;
                    Thread.Sleep(300); // This is not nice :[
                }
                else if (selectedWindowHadFocus)
                {
                    // If the window lost focus remove the clipping area.
                    // Usually the clipping gets removed by default if the window loses focus. 
                    WindowLog.Log(LogLevel.Info, "The current game window is not focused!");
                    NativeMethods.ClipCursor(IntPtr.Zero);
                    selectedWindowHadFocus = false;
                }

                // Validate the window every x amount of loops 
                validateHandleCount++;
                if (validateHandleCount > WindowSettings.ValidateHandleThreshold)
                {
                    validateHandleCount = 0;
                    var tempWindowTitle = GetText(windowHandle, WindowSettings.WindowTitleMaxLength);
                    if (tempWindowTitle == null || tempWindowTitle != selectedWindowTitle)
                    {
                        WindowLog.Log(LogLevel.Info, "The current game window doesn't exists anymore!");
                        NativeMethods.ClipCursor(IntPtr.Zero);
                        break;
                    }
                }

                Thread.Sleep(WindowSettings.ClippingRefreshInterval);
            }
        }

        private static Rectangle GetBorderSizes(IntPtr hWnd)
        {
            var windowRectangle = new Rectangle();
            var windowStyle = NativeMethods.GetWindowLong(hWnd, WindowLongIndex.GWL_STYLE);

            // Window has title-bar
            if (windowStyle.HasFlag(WindowStyleFlag.WS_CAPTION))
                windowRectangle.Top += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CYCAPTION);

            // Window has re-sizable borders
            if (windowStyle.HasFlag(WindowStyleFlag.WS_THICKFRAME))
            {
                windowRectangle.Left += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CXSIZEFRAME);
                windowRectangle.Right += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CXSIZEFRAME);
                windowRectangle.Top += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CYSIZEFRAME);
                windowRectangle.Bottom += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CYSIZEFRAME);
            } // Window has normal borders
            else if (windowStyle.HasFlag(WindowStyleFlag.WS_BORDER) || windowStyle.HasFlag(WindowStyleFlag.WS_CAPTION))
            {
                windowRectangle.Left += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CXFIXEDFRAME);
                windowRectangle.Right += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CXFIXEDFRAME);
                windowRectangle.Top += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CYFIXEDFRAME);
                windowRectangle.Bottom += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CYFIXEDFRAME);
            }

            return windowRectangle;
        }

        private static string GetText(IntPtr hWnd, int maxStringLength)
        {
            var stringBuilder = new StringBuilder(maxStringLength);

            return NativeMethods.GetWindowText(hWnd, stringBuilder, maxStringLength) == 0
                ? null : stringBuilder.ToString();
        }

        private static Rectangle ExpandAreaByOffset(Rectangle area, int offset)
        {
            area.Top += offset;
            area.Left += offset;
            area.Right += offset;
            area.Bottom += offset;

            return area;
        }
    }
}
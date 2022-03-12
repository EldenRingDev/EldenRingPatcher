using EldenRingPatcher.WIN32API;
using EldenRingPatcher.WIN32API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Rectangle = EldenRingPatcher.WIN32API.Structures.Rectangle;

namespace EldenRingPatcher
{
    public static class Window
    {
        public const int WindowTitleMaxLength = 100;     // Maximum length of window title before its truncated
        private const int ValidateHandleThreshold = 10;  // How often the user selected window handle gets validated
        private const int ClippingRefreshInterval = 100; // How often the clipped area is refreshed in milliseconds
        public static bool verboseOutput = false;

        public static void LockCursor(IntPtr windowHandle)
        {
            var windowArea = new Rectangle();
            var windowBorderSize = new Rectangle();

            WindowStyleFlag previousStyle = 0;
            var selectedWindowHadFocus = false;
            var validateHandleCount = 0;
            var selectedWindowTitle = GetText(windowHandle, WindowTitleMaxLength);

            while (true)
            {
                // Check if window styles changed so the program doesn't break if the window's borders style is changed
                if (previousStyle != NativeMethods.GetWindowLong(windowHandle, WindowLongIndex.GWL_STYLE))
                {
                    // Determine border sizes for the selected window
                    windowBorderSize = GetBorderSizes(windowHandle);
                    previousStyle = NativeMethods.GetWindowLong(windowHandle, WindowLongIndex.GWL_STYLE);
                }

                if (NativeMethods.GetForegroundWindow() == windowHandle)
                {
                    if (NativeMethods.GetWindowRect(windowHandle, ref windowArea) == 0)
                        throw new Win32Exception(Marshal.GetLastWin32Error(), $"Get window rectangle win32 error. selectedWindowHandle {windowHandle:d}");

                    windowArea.Top += windowBorderSize.Top + 10;
                    windowArea.Left += windowBorderSize.Left + 10;
                    windowArea.Right -= windowBorderSize.Right + 10;
                    windowArea.Bottom -= windowBorderSize.Bottom + 10;

                    Console.WriteLine("Clipping cursor to process window!");
                    if (NativeMethods.ClipCursor(ref windowArea) == 0)
                        throw new Win32Exception(Marshal.GetLastWin32Error(), $"Clip cursor win32 error. windowArea {windowArea}");
                    
                    selectedWindowHadFocus = true;
                    Thread.Sleep(300); // This is not nice :[
                }
                else if (selectedWindowHadFocus)
                {
                    // If the window lost focus remove the clipping area.
                    // Usually the clipping gets removed by default if the window loses focus. 
                    Console.WriteLine("The selected Window is not focused!");
                    NativeMethods.ClipCursor(IntPtr.Zero);
                    selectedWindowHadFocus = false;
                }

                // Validate the window every x amount of loops 
                validateHandleCount++;
                if (validateHandleCount > ValidateHandleThreshold)
                {
                    validateHandleCount = 0;
                    var tempWindowTitle = GetText(windowHandle, WindowTitleMaxLength);
                    if (tempWindowTitle == null || tempWindowTitle != selectedWindowTitle)
                    {
                        Console.WriteLine("The selected Window doesn't exists anymore!");
                        NativeMethods.ClipCursor(IntPtr.Zero);
                        break;
                    }
                }

                Thread.Sleep(ClippingRefreshInterval);
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

        public static string GetText(IntPtr hWnd, int maxStringLength)
        {
            var stringBuilder = new StringBuilder(maxStringLength);

            return NativeMethods.GetWindowText(hWnd, stringBuilder, maxStringLength) == 0
                ? null : RemoveSpecialChars(stringBuilder.ToString());
        }

        public static List<IntPtr> GetAllHandles(bool outputWindowNames = true)
        {
            var windowHandles = new List<IntPtr>();

            var processList = Process.GetProcesses();

            windowHandles.Clear();
            var indexCounter = 1;

            foreach (var process in processList)
            {
                if (string.IsNullOrEmpty(process.MainWindowTitle)) continue;

                if(verboseOutput)
                    Console.WriteLine($"{process.ProcessName}: {process.MainWindowHandle}|{process.MainWindowTitle}");

                if (outputWindowNames)
                {
                    var windowTitle = RemoveSpecialChars(process.MainWindowTitle);
                    Console.WriteLine("({0:d}) : {1}", indexCounter, windowTitle[..Math.Min(windowTitle.Length, WindowTitleMaxLength)]);
                }

                windowHandles.Add(process.MainWindowHandle);
                indexCounter++;
            }

            return windowHandles;
        }

        public static IntPtr GetHandle(string processName)
        {
            foreach (var processList in Process.GetProcesses())
                if (processList.MainWindowTitle.Contains(processName))
                    return processList.MainWindowHandle;

            return IntPtr.Zero;
        }

        private static string RemoveSpecialChars(string str)
        {
            const char tradeMark = (char)8482;
            const char registeredTrademark = (char)174;
            const char copyRight = (char)169;

            var badChars = new[] { tradeMark, registeredTrademark, copyRight };

            foreach (var badChar in badChars)
                if (str.Contains(badChar))
                    return str.Replace(badChar, '\0');

            return str;
        }
    }
}

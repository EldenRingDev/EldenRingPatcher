using EldenRingPatcher.WIN32API;
using EldenRingPatcher.WIN32API.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Rectangle = EldenRingPatcher.WIN32API.Structures.Rectangle;

namespace EldenRingPatcher
{
    public class Window
    {
        private const int WindowTitleMaxLength = 50; // Maximum length of window title before its truncated
        private static bool verboseOutput = false;

        public static List<IntPtr> GetAllHandles(bool outputWindowNames = true)
        {
            var windowHandles = new List<IntPtr>();

            var processList = Process.GetProcesses();
            var indexCounter = 1;

            windowHandles.Clear();

            foreach (var process in processList)
            {
                if (string.IsNullOrEmpty(process.MainWindowTitle)) continue;              
                
                if (verboseOutput) 
                    Console.WriteLine($"{process.ProcessName}: {process.MainWindowHandle}|{process.MainWindowTitle}");

                if (outputWindowNames)
                {
                    var windowTitle = RemoveSpecialCharacters(process.MainWindowTitle);
                    Console.WriteLine("({0}) : {1}", indexCounter, windowTitle[..Math.Min(windowTitle.Length, WindowTitleMaxLength)]);
                }

                windowHandles.Add(process.MainWindowHandle);
                indexCounter++;
            }

            return windowHandles;
        }

        private static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_. -]+", string.Empty, RegexOptions.Compiled);
        }

        public static Rectangle GetBorderSizes(IntPtr hWnd)
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
    }
}

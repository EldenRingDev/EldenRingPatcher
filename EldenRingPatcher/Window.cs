using EldenRingPatcher.WIN32API;
using EldenRingPatcher.WIN32API.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            List<IntPtr> windowHandles = new();

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

        public static Rectangle GetBorderSizes(IntPtr window)
        {
            Rectangle windowBorderSizes = new Rectangle();

            WindowStyleFlag styles = NativeMethods.GetWindowLong(window, WindowLongIndex.GWL_STYLE);

            // Window has title-bar
            if (styles.HasFlag(WindowStyleFlag.WS_CAPTION))
                windowBorderSizes.Top += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CYCAPTION);

            // Window has re-sizable borders
            if (styles.HasFlag(WindowStyleFlag.WS_THICKFRAME))
            {
                windowBorderSizes.Left += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CXSIZEFRAME);
                windowBorderSizes.Right += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CXSIZEFRAME);
                windowBorderSizes.Top += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CYSIZEFRAME);
                windowBorderSizes.Bottom += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CYSIZEFRAME);
            } // Window has normal borders
            else if (styles.HasFlag(WindowStyleFlag.WS_BORDER) || styles.HasFlag(WindowStyleFlag.WS_CAPTION))
            {
                windowBorderSizes.Left += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CXFIXEDFRAME);
                windowBorderSizes.Right += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CXFIXEDFRAME);
                windowBorderSizes.Top += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CYFIXEDFRAME);
                windowBorderSizes.Bottom += NativeMethods.GetSystemMetrics(SystemMetricIndex.SM_CYFIXEDFRAME);
            }

            return windowBorderSizes;
        }
    }
}

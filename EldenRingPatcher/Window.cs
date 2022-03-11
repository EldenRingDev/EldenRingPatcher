using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EldenRingPatcher
{
    public class Window
    {
        private const int WindowTitleMaxLength = 50; // Maximum lenght of window title before its truncated
        private static bool verboseOutput = false;

        public static List<IntPtr> GetAllHandles(bool outputWindowNames = true)
        {
            Process[] processList;
            List<IntPtr> windowHandles = new();
            int indexCounter;

            // Print out (almost) every window title and save their handle
            processList = Process.GetProcesses();
            indexCounter = 1;

            if (windowHandles == null) 
                windowHandles = new List<IntPtr>();
            else 
                windowHandles.Clear();

            foreach (Process process in processList)
            {
                if (verboseOutput)
                    Console.WriteLine($"{process.ProcessName}: {process.MainWindowHandle}|{process.MainWindowTitle}");

                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    if (outputWindowNames)
                    {
                        string windowTitle = RemoveSpecialCharacters(process.MainWindowTitle);
                        Console.WriteLine("({0:d}) : {1:s}", indexCounter,
                            windowTitle.Substring(0, Math.Min(windowTitle.Length, WindowTitleMaxLength)));
                    }

                    windowHandles.Add(process.MainWindowHandle);
                    indexCounter++;
                }
            }

            return windowHandles;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_. -]+", string.Empty, RegexOptions.Compiled);
        }
    }
}

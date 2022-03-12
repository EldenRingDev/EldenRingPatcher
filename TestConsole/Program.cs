using EldenRingPatcher;
using System;
using System.Collections;

namespace TestConsole
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Window.verboseOutput = ((IList)args).Contains("--verbose");

            while (true)
            {
                Console.WriteLine("Available windows : ");
                var windowHandles = Window.GetAllHandles();
                Console.Write("Select a window by entering its index number : ");
                var selectedIndexStr = Console.ReadLine();

                // Validate user choice
                if (!int.TryParse(selectedIndexStr, out int selectedIndex)
                    || selectedIndex < 1 || selectedIndex > windowHandles.Count)
                {
                    Console.Clear();
                    Console.WriteLine("Only use numbers that are on the list!");
                    continue;
                }

                var selectedWindowHandle = windowHandles[selectedIndex - 1];
                var selectedWindowTitle = Window.GetText(selectedWindowHandle, Window.WindowTitleMaxLength);
                if (selectedWindowTitle == null)
                {
                    Console.WriteLine("The selected Window doesn't exists anymore!");
                    continue;
                }

                Console.WriteLine("Locking Cursor to {0}", selectedWindowTitle);
                Window.LockCursor(selectedWindowHandle);
            }
        }
    }
}

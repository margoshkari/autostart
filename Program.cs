using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace autostart
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static RegistryKey currentUser = Registry.CurrentUser;
        static RegistryKey newKey = currentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", true);
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            if (!CheckValueNames("Autostart"))
            {
                newKey.SetValue("Autostart", @$"C:\Users\{Environment.UserName}\Desktop\autostart\bin\Debug\netcoreapp3.1\autostart.exe");
            }
            ConsoleKeyInfo consoleKey;
            string answer = string.Empty;
            while (true)
            {
                ShowWindow(handle, SW_HIDE);
               
                consoleKey = Console.ReadKey();
                if (consoleKey.Key == ConsoleKey.Enter)
                {
                    ShowWindow(handle, SW_SHOW);
                }
                Console.WriteLine("Do you want to add sorting files to autostart?\nEnter yes or no");
                answer = Console.ReadLine();
                if (answer.Contains("yes"))
                {
                    if (!CheckValueNames("File sort"))
                    {
                        newKey.SetValue("Files sort", @$"C:\Users\{Environment.UserName}\Desktop\autostart\sort files\SortFiles.exe");
                        Console.WriteLine("Added!");
                    }
                }
                else if (answer.Contains("no"))
                {
                    if (CheckValueNames("File sort"))
                    {
                        newKey.DeleteValue("Files sort");
                        Console.WriteLine("Deleted!");
                    }
                }
                Console.Clear();
            }
        }
        static bool CheckValueNames(string name)
        {
            if (newKey.GetValueNames().ToList().Contains(name))
                return true;

            return false;
        }
    }
}

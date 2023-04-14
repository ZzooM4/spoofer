using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SpooferApp
{
    internal static class Program
    {
        // P/Invoke declarations
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [STAThread]
        private static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Spoofer());
        }

        public static void ToggleConsole()
        {
            const int SW_HIDE = 0;
            const int SW_SHOW = 5;

            IntPtr consoleWindow = GetConsoleWindow();
            if (consoleWindow == IntPtr.Zero)
            {
                AllocConsole();
            }
            else
            {
                bool visible = ShowWindow(consoleWindow, SW_HIDE);
                if (!visible)
                {
                    ShowWindow(consoleWindow, SW_SHOW);
                }
                else
                {
                    FreeConsole();
                }
            }
        }
    }
}

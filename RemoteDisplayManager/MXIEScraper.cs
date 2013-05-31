using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RemoteDisplayManager
{
    class MXIEScraper
    {
        private class Win32
        {
            //[DllImport("User32.dll")]
            //public static extern Int32 FindWindow(String lpClassName, String lpWindowName);
            //[DllImport("User32.dll")]
            //public static extern Int32 SetForegroundWindow(int hWnd);
            //[DllImport("User32.dll")]
            //public static extern Boolean EnumChildWindows(int hWndParent, Delegate lpEnumFunc, int lParam);
            //[DllImport("User32.dll")]
            //public static extern Int32 GetWindowText(int hWnd, StringBuilder s, int nMaxCount);
            //[DllImport("User32.dll")]
            //public static extern Int32 GetWindowTextLength(int hwnd);
            //[DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
            //public static extern int GetDesktopWindow();

            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, StringBuilder lParam);
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindowEx(IntPtr Parent, IntPtr child, string classname, string WindowTitle);

            public const int WM_SETTEXT = 0x00C;
            public const int WM_GETTEXT = 0x00D;
            public const int WM_GETTEXTLENGTH = 0x00E;
        }

        private static Process GetProcByName(string Name)
        {
            Process proc = null;
            Process[] processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++)
            {
                //System.Console.WriteLine(processes[i].ProcessName);
                if (processes[i].ProcessName == Name)
                    proc = processes[i];
            }
            return proc;
        }

        private static IntPtr GetStatusControl()
        {
            Process proc = GetProcByName("notepad");
            if (proc == null)
                throw new System.Exception("Could not find process");

            IntPtr child = Win32.FindWindowEx(proc.MainWindowHandle, IntPtr.Zero, "Edit", null);
            if (child == null)
                throw new System.Exception("Null child window");

            return child;
        }

        public static string GetStatus()
        {
            IntPtr child = GetStatusControl();

            // Get the current length
            int length = Win32.SendMessage(child, Win32.WM_GETTEXTLENGTH, 0, 0);

            // Retrieve that many characters
            StringBuilder text = new StringBuilder();
            text = new StringBuilder(length + 1);
            int retr2 = Win32.SendMessage(child, Win32.WM_GETTEXT, length + 1, text);
            if (retr2 != length)
                throw new System.Exception("WM_GETTEXT returned " + retr2);

            return text.ToString();
        }

        public static void SetStatus(string status)
        {
            IntPtr child = GetStatusControl();

            // Set the new text
            StringBuilder text = new StringBuilder(status);
            int ret = Win32.SendMessage(child, Win32.WM_SETTEXT, status.Length, text);
            if (ret != 1)
                throw new System.Exception("WM_SETTEXT returned " + ret);
        }
    }
}

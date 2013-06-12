using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace RemoteDisplayManager
{
    class MXIEScraper
    {
        private class Win32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int Left;        // x position of upper-left corner
                public int Top;         // y position of upper-left corner
                public int Right;       // x position of lower-right corner
                public int Bottom;      // y position of lower-right corner
            }

            [DllImport("User32.dll")]
            public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindowEx(IntPtr Parent, IntPtr child, string classname, string WindowTitle);
            [DllImport("User32.dll")]
            public static extern Int32 SetForegroundWindow(IntPtr hWnd);
            [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
            public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

            public const int HWND_TOPMOST = -1;
            public const int SWP_NOMOVE = 0x0002;
            public const int SWP_NOSIZE = 0x0001;

            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
            public const int SW_RESTORE = 9;

            //[DllImport("User32.dll")]
            //public static extern Boolean EnumChildWindows(int hWndParent, Delegate lpEnumFunc, int lParam);
            //[DllImport("User32.dll")]
            //public static extern Int32 GetWindowText(int hWnd, StringBuilder s, int nMaxCount);
            //[DllImport("User32.dll")]
            //public static extern Int32 GetWindowTextLength(int hwnd);
            //[DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
            //public static extern int GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);

            [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetCursorPos(int X, int Y);    

            public const int MOUSEEVENTF_ABSOULUTE = 0x8000;
            public const int MOUSEEVENTF_LEFTDOWN = 0x02;
            public const int MOUSEEVENTF_LEFTUP = 0x04;
            public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
            public const int MOUSEEVENTF_RIGHTUP = 0x10;

            [DllImport("user32.dll")]
            public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);


            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, StringBuilder lParam);
            
            [DllImport("user32.dll")]
            public static extern int GetDlgItemText(IntPtr hWnd, int nIDDlgItem, StringBuilder lpString, int maxcount);

            public const int WM_SETTEXT = 0x00C;
            public const int WM_GETTEXT = 0x00D;
            public const int WM_GETTEXTLENGTH = 0x00E;

            public const int IDC_TEXT = 1000;
        }


        private static Process GetProcByName(string Name)
        {
            foreach (var proc in Process.GetProcesses())
                if (proc.ProcessName == Name)
                    return proc;

            throw new System.Exception("Could not find process");
        }

        private static IntPtr GetMXIEMainWindow()
        {
            IntPtr win_main = GetProcByName("mxie").MainWindowHandle;
            //IntPtr win_main = Win32.FindWindow(null, "MXIE User: wkulp");

            if (win_main == IntPtr.Zero)
                throw new System.Exception("Null window");

            // Console.WriteLine("Top window = 0x" + win_main.ToString("X"));
            return win_main;
        }

        private static IntPtr GetMXIEStatusControl()
        {
            IntPtr win_main = GetMXIEMainWindow();
            IntPtr child1 = Win32.FindWindowEx(win_main, IntPtr.Zero, null, null);
            IntPtr child2 = IntPtr.Zero;

            while (child1 != IntPtr.Zero)
            {
                Console.WriteLine("Child " + child1.ToString("X"));

                child2 = IntPtr.Zero;
                child2 = Win32.FindWindowEx(child1, child2, null, "toolBarPresenceNoteEdit");
                if (child2 != IntPtr.Zero)
                    break;

                child1 = Win32.FindWindowEx(win_main, child1, null, null);
            }

            Console.WriteLine("child1 = 0x" + child1.ToString("X"));
            Console.WriteLine("child2 = 0x" + child2.ToString("X"));

            if (child2 == IntPtr.Zero)
                throw new System.Exception("Null child2");
            return child2;
        }

        public static Image GetStatusImage()
        {
            var win_main = GetMXIEMainWindow();
            var img = (Bitmap) new ScreenCapture().CaptureWindow(win_main);

            // Crop the status box
            const int x0 = 95;
            const int y0 = 105;
            const int width  = 250;
            const int height = 22;
            var crop = new Rectangle(x0, y0, width, height);

            if (img.Width < x0+width || img.Height < y0+height)
                throw new System.Exception("Image too small to crop (" + img.Width + "x" + img.Height + ")");

            img = img.Clone(crop, img.PixelFormat);

            // Resize it to help with OCR
            int newWidth  = (int)(img.Width  * 5);
            int newHeight = (int)(img.Height * 4);
            var newImage  = new Bitmap(newWidth, newHeight);
            using (var gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode     = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode   = PixelOffsetMode.HighQuality;
                gr.DrawImage(img, new Rectangle(0, 0, newWidth, newHeight));
            }
            return (Image)newImage;
        }

        public static String ExtractStatus(Image img)
        {
            img.Save(@"C:\Windows\Temp\mxieimage.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "tesseract";
            p.StartInfo.Arguments = @"C:\Windows\Temp\mxieimage.bmp C:\Windows\Temp\mxietext -l eng";
            p.Start();
            p.WaitForExit();

            return System.IO.File.ReadAllText(@"C:\Windows\Temp\mxietext.txt");
        }

        public static void SetMXIETopmost()
        {
            IntPtr win_main = GetMXIEMainWindow();
            Win32.SetForegroundWindow(win_main);
            System.Threading.Thread.Sleep(200);
            Win32.SetWindowPos(win_main, Win32.HWND_TOPMOST, 0, 0, 0, 0, Win32.SWP_NOMOVE | Win32.SWP_NOSIZE);
        }

        public static void SetStatus(string status)
        {
            int oldx = Cursor.Position.X;
            int oldy = Cursor.Position.Y;

            IntPtr win_main = GetMXIEMainWindow();
            Win32.ShowWindow(win_main, Win32.SW_RESTORE);
            Win32.SetForegroundWindow(win_main);
            System.Threading.Thread.Sleep(200);
            Win32.SetWindowPos(win_main, Win32.HWND_TOPMOST, 0, 0, 0, 0, Win32.SWP_NOMOVE | Win32.SWP_NOSIZE);

            Win32.RECT r;
            Win32.GetWindowRect(win_main, out r);
            int x = r.Left + 120;
            int y = r.Top + 112;

            Win32.SetCursorPos(x, y);
            System.Threading.Thread.Sleep(200);
            Win32.mouse_event(Win32.MOUSEEVENTF_LEFTDOWN | Win32.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            System.Threading.Thread.Sleep(200);

            System.Windows.Forms.SendKeys.Send("{BS 30}{DEL 30}" + status + "{TAB}");
            System.Threading.Thread.Sleep(200);
            Win32.SetCursorPos(oldx, oldy);
        }

        public static String GetWindowText(IntPtr hwnd)
        {
            // Get the current length
            int length = Win32.SendMessage(hwnd, Win32.WM_GETTEXTLENGTH, 0, 0);

            // Retrieve that many characters
            var text = new StringBuilder(length + 1);
            int retr2 = Win32.SendMessage(hwnd, Win32.WM_GETTEXT, length + 1, text);
            if (retr2 != length)
                throw new System.Exception("WM_GETTEXT returned " + retr2);

            return "" + text;
        }

        public static String GetDialogText(IntPtr hwnd)
        {
            // Retrieve that many characters
            var text = new StringBuilder(100);

            int ret = Win32.GetDlgItemText(hwnd, Win32.IDC_TEXT, text, 100);

            if (ret == 0)
                System.Console.WriteLine("GetDialogText returned 0: " + text);

            return "" + text;
        }

        private static IntPtr NotepadGetStatusControl()
        {
            Process proc = GetProcByName("notepad");
            if (proc == null)
                throw new System.Exception("Could not find process");

            IntPtr child = Win32.FindWindowEx(proc.MainWindowHandle, IntPtr.Zero, "Edit", null);
            if (child == null)
                throw new System.Exception("Null child window");

            return child;
        }

        public static string NotepadGetStatus()
        {
            IntPtr child = NotepadGetStatusControl();

            // Get the current length
            int length = Win32.SendMessage(child, Win32.WM_GETTEXTLENGTH, 0, 0);

            // Retrieve that many characters
            var text = new StringBuilder(length + 1);
            int retr2 = Win32.SendMessage(child, Win32.WM_GETTEXT, length + 1, text);
            if (retr2 != length)
                throw new System.Exception("WM_GETTEXT returned " + retr2);

            return text.ToString();
        }

        public static void NotepadSetStatus(string status)
        {
            IntPtr child = NotepadGetStatusControl();

            // Set the new text
            var text = new StringBuilder(status);
            int ret = Win32.SendMessage(child, Win32.WM_SETTEXT, status.Length, text);
            if (ret != 1)
                throw new System.Exception("WM_SETTEXT returned " + ret);
        }
    }
}

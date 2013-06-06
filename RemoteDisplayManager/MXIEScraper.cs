﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace RemoteDisplayManager
{
    class MXIEScraper
    {
        private class Win32
        {
            [DllImport("User32.dll")]
            public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindowEx(IntPtr Parent, IntPtr child, string classname, string WindowTitle);
            [DllImport("User32.dll")]
            public static extern Int32 SetForegroundWindow(int hWnd);
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
            public static extern int GetDlgItemText(IntPtr hWnd, int nIDDlgItem, StringBuilder lpString, int maxcount);

            public const int WM_SETTEXT = 0x00C;
            public const int WM_GETTEXT = 0x00D;
            public const int WM_GETTEXTLENGTH = 0x00E;

            public const int IDC_TEXT = 1000;
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

        private static IntPtr GetMXIEMainWindow()
        {
            Process proc = GetProcByName("mxie");
            if (proc == null)
                throw new System.Exception("Could not find process");
            IntPtr win_main = proc.MainWindowHandle;

            //IntPtr win_main = Win32.FindWindow(null, "MXIE User: wkulp");

            if (win_main == IntPtr.Zero)
                throw new System.Exception("Null window");

            return win_main;
        }

        private static IntPtr GetStatusControl(IntPtr win_main)
        {
            // Console.WriteLine("Top window = 0x" + win_main.ToString("X"));
            
            IntPtr child = Win32.FindWindowEx(win_main, IntPtr.Zero, null, null);
            IntPtr child2 = IntPtr.Zero;

            while (child != IntPtr.Zero)
            {
                Console.WriteLine("Child " + child.ToString("X"));

                child2 = IntPtr.Zero;
                child2 = Win32.FindWindowEx(child, child2, null, "toolBarPresenceNoteEdit");
                if (child2 != IntPtr.Zero)
                    break;

                child = Win32.FindWindowEx(win_main, child, null, null);
            }

            Console.WriteLine("Using: child = 0x"  + child.ToString("X"));
            Console.WriteLine("Using: child2 = 0x" + child2.ToString("X"));

            if (child2 == IntPtr.Zero)
                throw new System.Exception("Null child2");

            return child2;
        }

        public static Image GetStatusImage()
        {
            IntPtr win_main = GetMXIEMainWindow();

            ScreenCapture sc = new ScreenCapture();
            Image img = sc.CaptureWindow(win_main);

            // Crop the status box
            int width = 350;
            int height = 22;
            Rectangle croprect = new Rectangle(95, 105, width, height);
            Bitmap bmp = new Bitmap(img);
            img = bmp.Clone(croprect, bmp.PixelFormat);

            // Resize it to help with OCR
            int newWidth  = img.Width *3;
            int newHeight = img.Height *3;
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
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

            string newstatus = System.IO.File.ReadAllText(@"C:\Windows\Temp\mxietext.txt");
            return newstatus;
        }

        public static void SetStatus(string status)
        {
            IntPtr win_main = GetMXIEMainWindow();
            //System.Windows.Activa

            Win32.SetForegroundWindow(win_main);
            System.Threading.Thread.Sleep(200);

            System.Windows.Forms.SendKeys.Send("{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}");
            System.Windows.Forms.SendKeys.Send(status);
            /*
            // Set the new text
            StringBuilder text = new StringBuilder(status);
            int ret = Win32.SendMessage(child, Win32.WM_SETTEXT, status.Length, text);
            if (ret != 1)
                throw new System.Exception("WM_SETTEXT returned " + ret);
             */
        }

        public static String GetWindowText(IntPtr hwnd)
        {
            // Get the current length
            int length = Win32.SendMessage(hwnd, Win32.WM_GETTEXTLENGTH, 0, 0);

            // Retrieve that many characters
            StringBuilder text = new StringBuilder();
            text = new StringBuilder(length + 1);
            int retr2 = Win32.SendMessage(hwnd, Win32.WM_GETTEXT, length + 1, text);
            if (retr2 != length)
                throw new System.Exception("WM_GETTEXT returned " + retr2);

            return "" + text;
        }

        public static String GetDialogText(IntPtr hwnd)
        {
            // Retrieve that many characters
            StringBuilder text = new StringBuilder();
            text = new StringBuilder(100);

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
            StringBuilder text = new StringBuilder();
            text = new StringBuilder(length + 1);
            int retr2 = Win32.SendMessage(child, Win32.WM_GETTEXT, length + 1, text);
            if (retr2 != length)
                throw new System.Exception("WM_GETTEXT returned " + retr2);

            return text.ToString();
        }

        public static void NotepadSetStatus(string status)
        {
            IntPtr child = NotepadGetStatusControl();

            // Set the new text
            StringBuilder text = new StringBuilder(status);
            int ret = Win32.SendMessage(child, Win32.WM_SETTEXT, status.Length, text);
            if (ret != 1)
                throw new System.Exception("WM_SETTEXT returned " + ret);
        }
    }
}

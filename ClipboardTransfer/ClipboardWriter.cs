using NativeApi;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ClipboardTransfer
{
    internal static class ClipboardWriter
    {
        internal static void Empty()
        {
            WriteText(string.Empty);
        }

        internal static void Empty(int retryInterval, int timeout)
        {
            WriteText(string.Empty, retryInterval, timeout);
        }

        internal static void WriteBytes(byte[] bytes)
        {
            throw new Exception("The binary writing is not currently supported.");
        }

        internal static void WriteText(string text, int retryInterval = 100, int timeout = 1000)
        {
            bool opened = false;

            for (int elapsed = 0; elapsed < timeout; elapsed += retryInterval)
            {
                if (User32.OpenClipboard(IntPtr.Zero))
                {
                    opened = true;
                    break;
                }

                Thread.Sleep(retryInterval);
            }

            if (!opened)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            bool clipboardClosed = false;

            try
            {
                string nullTerminated = text + "\0";
                byte[] bytes = Encoding.Unicode.GetBytes(nullTerminated);
                IntPtr hGlobal = Marshal.AllocHGlobal(bytes.Length);

                if (!User32.EmptyClipboard())
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                if (string.IsNullOrEmpty(text)) return;

                Marshal.Copy(bytes, 0, hGlobal, bytes.Length);

                if (User32.SetClipboardData(CF.UNICODETEXT, hGlobal) == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();
                    Marshal.FreeHGlobal(hGlobal);
                    throw new Win32Exception(error);
                }

                User32.CloseClipboard();
                clipboardClosed = true;
            }
            finally
            {
                if (!clipboardClosed) User32.CloseClipboard();
            }
        }
    }
}

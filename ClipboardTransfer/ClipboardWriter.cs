using ClipboardTransfer.Properties;
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
        #region Internal Fields

        internal static readonly int RetryInterval;
        internal static readonly int RetryMax;

        #endregion

        #region Static Constructor

        static ClipboardWriter()
        {
            Settings settings = Settings.Default;
            RetryInterval = settings.RetryInterval;
            RetryMax = settings.RetryMax;
        }

        #endregion

        #region Internal Methods

        internal static void Empty()
        {
            WriteText(string.Empty, IntPtr.Zero);
        }

        internal static void WriteBytes(byte[] bytes)
        {
            throw new Exception("The binary writing is not currently supported.");
        }

        internal static void WriteText(string text, IntPtr hWndNewOwner)
        {
            bool opened = false;

            for (int i = 0; i < RetryMax; ++i)
            {
                if (i != 0)
                {
                    Thread.Sleep(RetryInterval);
                }

                if (User32.OpenClipboard(hWndNewOwner))
                {
                    opened = true;
                    break;
                }
            }

            if (!opened)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            try
            {
                if (!User32.EmptyClipboard())
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                if (string.IsNullOrEmpty(text)) return;

                byte[] bytes = Encoding.Unicode.GetBytes(text + "\0");
                IntPtr hGlobal = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, hGlobal, bytes.Length);

                if (User32.SetClipboardData(CF.UNICODETEXT, hGlobal) == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();
                    Marshal.FreeHGlobal(hGlobal);
                    throw new Win32Exception(error);
                }

                User32.CloseClipboard();
                opened = false;
            }
            finally
            {
                if (opened) User32.CloseClipboard();
            }
        }

        #endregion
    }
}

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
            WriteText(string.Empty);
        }

        internal static void WriteBytes(byte[] bytes)
        {
            throw new Exception("The binary writing is not currently supported.");
        }

        internal static void WriteText(string text)
        {
            bool opened = false;

            for (int i = 0; i < RetryMax; ++i)
            {
                if (i != 0)
                {
                    Thread.Sleep(RetryInterval);
                }

                if (User32.OpenClipboard(IntPtr.Zero))
                {
                    opened = true;
                    break;
                }
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

        #endregion
    }
}

using ClipboardTransfer.Properties;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ClipboardTransfer
{
    internal static class ClipboardReader
    {
        #region Internal Fields

        internal static readonly int RetryInterval;
        internal static readonly int RetryMax;

        #endregion

        #region Static Constructor

        static ClipboardReader()
        {
            Settings settings = Settings.Default;
            RetryInterval = settings.RetryInterval;
            RetryMax = settings.RetryMax;
        }

        #endregion

        #region Internal Methods

        internal static byte[] ReadBytes()
        {
            throw new Exception("The binary reading is not currently supported.");
        }

        internal static string ReadText()
        {
            Exception exception = null;

            for (int i = 0; i < RetryMax; ++i)
            {
                if (i != 0)
                {
                    Thread.Sleep(RetryInterval);
                }

                try
                {
                    return Clipboard.GetText();
                }
                catch (Exception exception_)
                {
                    exception = exception_;
                }
            }

            throw exception;
        }

        #endregion
    }
}

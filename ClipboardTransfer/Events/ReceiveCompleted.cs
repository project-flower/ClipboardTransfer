using System;

namespace ClipboardTransfer.Events
{
    public class ReceiveCompletedEventArgs : EventArgs
    {
        #region Public Fields

        public readonly string Md5;

        #endregion

        #region Public Methods

        public ReceiveCompletedEventArgs(string md5)
        {
            Md5 = md5;
        }

        #endregion
    }

    public delegate void ReceiveCompletedEventHandler(object sender, ReceiveCompletedEventArgs e);
}

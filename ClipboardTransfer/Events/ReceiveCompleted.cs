using System;

namespace ClipboardTransfer.Events
{
    public class ReceiveCompletedEventArgs : EventArgs
    {
        #region Public Fields

        public readonly string FileName;
        public readonly string Md5;

        #endregion

        #region Public Methods

        public ReceiveCompletedEventArgs(string fileName, string md5)
        {
            FileName = fileName;
            Md5 = md5;
        }

        #endregion
    }

    public delegate void ReceiveCompletedEventHandler(object sender, ReceiveCompletedEventArgs e);
}

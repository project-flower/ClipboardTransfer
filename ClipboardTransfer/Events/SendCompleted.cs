using System;

namespace ClipboardTransfer.Events
{
    public class SendCompletedEventArgs : EventArgs
    {
        #region Public Fields

        public readonly string FileName;

        #endregion

        #region Public Methods

        public SendCompletedEventArgs(string fileName)
        {
            FileName = fileName;
        }

        #endregion
    }

    public delegate void SendCompletedEventHandler(object sender, SendCompletedEventArgs e);
}

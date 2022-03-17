using System;

namespace ClipboardTransfer.Events
{
    public class SendCompletedEventArgs : EventArgs
    {
        #region Public Methods

        public SendCompletedEventArgs()
        {
        }

        #endregion
    }

    public delegate void SendCompletedEventHandler(object sender, SendCompletedEventArgs e);
}

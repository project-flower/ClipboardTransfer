using System;

namespace ClipboardTransfer.Events
{
    public class ErrorOccurredEventArgs : EventArgs
    {
        #region Public Fields

        public readonly string Message;

        #endregion

        #region Public Methods

        internal ErrorOccurredEventArgs(string message)
        {
            Message = message;
        }

        #endregion
    }

    public delegate void ErrorOccurredEventHandler(object sender, ErrorOccurredEventArgs e);
}

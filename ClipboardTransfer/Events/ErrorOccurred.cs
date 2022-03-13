using System;

namespace ClipboardTransfer.Events
{
    public class ErrorOccurredEventArgs : EventArgs
    {
        public readonly string Message;

        internal ErrorOccurredEventArgs(string message)
        {
            Message = message;
        }
    }

    public delegate void ErrorOccurredEventHandler(object sender, ErrorOccurredEventArgs e);
}

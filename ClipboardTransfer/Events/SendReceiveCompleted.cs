using System;

namespace ClipboardTransfer.Events
{
    public abstract class SendReceiveCompletedEventArgs : EventArgs
    {
        public readonly string FileName;
        public readonly string Md5;

        public SendReceiveCompletedEventArgs(string fileName, string md5)
        {
            FileName = fileName;
            Md5 = md5;
        }
    }
}

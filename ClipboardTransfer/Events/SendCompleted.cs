namespace ClipboardTransfer.Events
{
    public class SendCompletedEventArgs : SendReceiveCompletedEventArgs
    {
        public SendCompletedEventArgs(string fileName, string md5) : base (fileName, md5)
        {
        }
    }

    public delegate void SendCompletedEventHandler(object sender, SendCompletedEventArgs e);
}

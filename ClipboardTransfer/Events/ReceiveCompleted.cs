namespace ClipboardTransfer.Events
{
    public class ReceiveCompletedEventArgs : SendReceiveCompletedEventArgs
    {
        public ReceiveCompletedEventArgs(string fileName, string md5) : base(fileName, md5)
        {
        }
    }

    public delegate void ReceiveCompletedEventHandler(object sender, ReceiveCompletedEventArgs e);
}

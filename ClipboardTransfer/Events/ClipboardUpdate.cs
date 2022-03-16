using System;

namespace ClipboardTransfer.Events
{
    public class ClipboardUpdateEventArgs : EventArgs
    {
    }

    public delegate void ClipboardUpdateEventHandler(object sender, ClipboardUpdateEventArgs e);
}

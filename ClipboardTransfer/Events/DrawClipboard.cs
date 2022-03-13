using System;

namespace ClipboardTransfer.Events
{
    public class ClipboardEventArgs : EventArgs
    {
    }

    public delegate void ClipboardEventHandler(object sender, ClipboardEventArgs e);
}

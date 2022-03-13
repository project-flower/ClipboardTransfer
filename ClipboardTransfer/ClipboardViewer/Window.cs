using ClipboardTransfer.Events;
using NativeApi;
using System;
using System.Windows.Forms;

namespace ClipboardTransfer
{
    public abstract partial class ClipboardViewer
    {
        #region Private Classes

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        private class Window : NativeWindow
        {
            #region Private Fields

            private Form form;
            private bool messageReceived;
            private IntPtr nextHandle;

            #endregion

            #region Public Properties

            public event ClipboardEventHandler DrawClipboard = delegate { };

            #endregion

            #region Public Methods

            public Window()
            {
                form = null;
                messageReceived = false;
                nextHandle = IntPtr.Zero;
            }

            public void JoinClipboardChain(Form form)
            {
                if (form == this.form)
                {
                    return;
                }

                LeaveClipboardChain();
                this.form = form;
                form.HandleDestroyed += form_HandleDestroyed;
                AssignHandle(this.form.Handle);
                messageReceived = false;
                nextHandle = User32.SetClipboardViewer(Handle);
            }

            public void LeaveClipboardChain()
            {
                if (form == null)
                {
                    return;
                }

                User32.ChangeClipboardChain(Handle, nextHandle);
                ReleaseHandle();
                form.HandleDestroyed -= form_HandleDestroyed;
                form = null;
            }

            #endregion

            #region Protected Methods

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM.DRAWCLIPBOARD:
                        if (!messageReceived)
                        {
                            // SetClipboardViewer 直後のメッセージは、クリップボードの更新ではないので無視
                            messageReceived = true;
                            break;
                        }

                        DrawClipboard(this, new ClipboardEventArgs());

                        if (nextHandle != IntPtr.Zero)
                        {
                            User32.PostMessage(nextHandle, m.Msg, m.WParam, m.LParam);
                        }

                        break;

                    case WM.CHANGECBCHAIN:
                        if (m.WParam == nextHandle)
                        {
                            nextHandle = m.LParam;
                        }
                        else if (nextHandle != IntPtr.Zero)
                        {
                            User32.PostMessage(nextHandle, m.Msg, m.WParam, m.LParam);
                        }

                        break;
                }

                base.WndProc(ref m);
            }

            #endregion

            #region Private Methods

            private void form_HandleDestroyed(object sender, EventArgs e)
            {
                LeaveClipboardChain();
            }

            #endregion
        }

        #endregion
    }
}

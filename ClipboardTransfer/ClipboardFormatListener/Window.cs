using ClipboardTransfer.Events;
using NativeApi;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace ClipboardTransfer
{
    public abstract partial class ClipboardFormatListener
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private class Window : NativeWindow
        {
            #region Private Fields

            private Form form;
            private bool messageReceived;

            #endregion

            #region Public Properties

            public event ClipboardUpdateEventHandler ClipboardUpdate = delegate { };

            #endregion

            #region Public Methods

            public Window()
            {
                form = null;
                messageReceived = false;
            }

            public void JoinClipboardFormatListener(Form form)
            {
                if (form == this.form)
                {
                    return;
                }

                LeaveClipboardFormatListener();
                this.form = form;
                form.HandleDestroyed += form_HandleDestroyed;
                AssignHandle(this.form.Handle);
                messageReceived = false;

                if (!User32.AddClipboardFormatListener(Handle))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            public void LeaveClipboardFormatListener()
            {
                if (form == null)
                {
                    return;
                }

                User32.RemoveClipboardFormatListener(Handle);
                ReleaseHandle();
                form.HandleDestroyed -= form_HandleDestroyed;
                form = null;
            }

            #endregion

            #region Protected Methods

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM.CLIPBOARDUPDATE)
                {
                    if (!messageReceived)
                    {
                        // AddClipboardFormatListener 直後のメッセージは、クリップボードの更新ではないので無視
                        messageReceived = true;
                    }

                    ClipboardUpdate(this, new ClipboardUpdateEventArgs());
                }

                base.WndProc(ref m);
            }

            #endregion

            #region Private Methods

            private void form_HandleDestroyed(object sender, EventArgs e)
            {
                LeaveClipboardFormatListener();
            }

            #endregion
        }
    }
}

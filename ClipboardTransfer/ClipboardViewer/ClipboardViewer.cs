using ClipboardTransfer.Events;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace ClipboardTransfer
{
    #region Public Classes

    public abstract partial class ClipboardViewer : Component
    {
        #region Private Fields

        private bool enabled;
        private ContainerControl owner;
        private readonly Window window;

        #endregion

        #region Public Properties

        protected bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                if (value == enabled)
                {
                    return;
                }

                enabled = value;

                if (DesignMode)
                {
                    return;
                }

                Action action;

                if (value)
                {
                    action = delegate { JoinClipboardChain(); };
                }
                else
                {
                    action = delegate { LeaveClipboardChain(); };
                }

                owner.Invoke(action);
            }
        }

        private void JoinClipboardChain()
        {
            if (!(owner is Form form))
            {
                return;
            }

            window.JoinClipboardChain(form);
            enabled = true;
        }

        private void LeaveClipboardChain()
        {
            window.LeaveClipboardChain();
            enabled = false;
        }

        public override ISite Site
        {
            get
            {
                return base.Site;
            }

            set
            {
                base.Site = value;

                if (value == null)
                {
                    return;
                }

                if (!(value.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost))
                {
                    return;
                }

                IComponent rootComponent = designerHost.RootComponent;

                if (rootComponent is ContainerControl)
                {
                    owner = rootComponent as ContainerControl;
                }
            }
        }

        public ContainerControl Owner
        {
            get
            {
                return owner;
            }

            set
            {
                if (value == owner)
                {
                    return;
                }

                if (!(value is Form form))
                {
                    return;
                }

                owner = value;

                if (DesignMode)
                {
                    return;
                }

                // デザイナで Enabled を true にした時に、owner が null で動作しないため、
                // 再度設定する。
                window.LeaveClipboardChain();

                if (enabled)
                {
                    window.JoinClipboardChain(form);
                }
            }
        }

        #endregion

        #region Public Methods

        public ClipboardViewer()
        {
            enabled = false;
            window = new Window();
            window.DrawClipboard += DrawClipboard;
        }

        ~ClipboardViewer()
        {
            window.LeaveClipboardChain();
            window.DrawClipboard -= DrawClipboard;
        }

        #endregion

        #region Protected Methods

        protected abstract void OnDrawClipboard();

        #endregion

        #region Private Methods

        private void DrawClipboard(object sender, ClipboardEventArgs e)
        {
            OnDrawClipboard();
        }

        #endregion
    }

    #endregion
}

using ClipboardTransfer.Events;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace ClipboardTransfer
{
    #region Public Classes

    public abstract partial class ClipboardFormatListener : Component
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

            window.JoinClipboardFormatListener(form);
            enabled = true;
        }

        private void LeaveClipboardChain()
        {
            window.LeaveClipboardFormatListener();
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
                window.LeaveClipboardFormatListener();

                if (enabled)
                {
                    window.JoinClipboardFormatListener(form);
                }
            }
        }

        #endregion

        #region Public Methods

        public ClipboardFormatListener()
        {
            enabled = false;
            window = new Window();
            window.ClipboardUpdate += ClipboardUpdate;
        }

        ~ClipboardFormatListener()
        {
            window.LeaveClipboardFormatListener();
            window.ClipboardUpdate -= ClipboardUpdate;
        }

        #endregion

        #region Protected Methods

        protected abstract void OnClipboardUpdate();

        #endregion

        #region Private Methods

        private void ClipboardUpdate(object sender, ClipboardEventArgs e)
        {
            OnClipboardUpdate();
        }

        #endregion
    }

    #endregion
}

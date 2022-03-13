using ClipboardTransfer.Events;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ClipboardTransfer
{
    public class ClipboardSender : ClipboardViewer
    {
        #region Private Fields

        private int bufferSize = 1;
        private readonly string newLine = Environment.NewLine;
        private bool sending = false;
        private FileStream stream = null;
        private int timeout = 1;
        private readonly Timer timer = new Timer();
        private int wait = 100;

        #endregion

        #region Public Properties

        public event ErrorOccurredEventHandler ErrorOccurred = delegate { };
        public event SendCompletedEventHandler SendCompleted = delegate { };
        public bool Sending { get { return sending; } }
        public TransmissionMode TransmissionMode { get; set; }

        #endregion

        #region Public Methods

        public ClipboardSender()
        {
            timer.Tick += new EventHandler(timer_Tick);
        }

        public void BeginSend(string fileName, int timeout, int bufferSize, int wait)
        {
            if (sending) throw new InvalidOperationException("ClipboardSender is sending.");

            stream = new FileStream(fileName, FileMode.Open);
            this.timeout = timeout;
            this.bufferSize = bufferSize;
            this.wait = wait;
            Enabled = true;
            Send();
        }

        public void EndSending()
        {
            timer.Stop();
            Enabled = false;

            if (stream != null)
            {
                try
                {
                    stream.Dispose();
                }
                catch
                {
                }

                stream = null;
            }

            sending = false;
        }

        #endregion

        #region Protected Methods

        protected override void OnDrawClipboard()
        {
            if (!sending) return;

            timer.Stop();
            string message = string.Empty;
            bool skip = false;

            try
            {
                switch (TransmissionMode)
                {
                    case TransmissionMode.Binary:
                        if (Clipboard.GetDataObject() is DataObject dataObject)
                        {
                            skip = ((dataObject.GetData(typeof(byte[])) is byte[] data) && (data.Length > 0));
                        }

                        break;

                    case TransmissionMode.Strings:
                    default:
                        string text = Clipboard.GetText(TextDataFormat.UnicodeText);
                        skip = (!string.IsNullOrEmpty(text));
                        break;
                }
            }
            catch (Exception exception)
            {
                EndSending();
                ErrorOccurred(this, new ErrorOccurredEventArgs(string.Format("Data reception could not be confirmed.{0}{0}{1}", newLine, exception.Message)));
                return;
            }

            if (skip)
            {
                return;
            }

            Action action = delegate { Send(); };
            Owner.Invoke(action);
        }

        #endregion

        #region Private Methods

        private string Convert(byte[] bytes)
        {
            return string.Join(string.Empty, bytes.Select(n => n.ToString("X2")));
        }

        private void Send()
        {
            timer.Stop();
            System.Threading.Thread.Sleep(wait);
            var buffer = new byte[bufferSize];
            int dataLength = stream.Read(buffer, 0, buffer.Length);

            if (dataLength < 1)
            {
                string md5 = "";
                string fileName = stream.Name;
                EndSending();

                try
                {
                    ClipboardWriter.Empty();
                }
                catch (Exception exception)
                {
                    ErrorOccurred(this, new ErrorOccurredEventArgs(
                        string.Format("The data has been sent, but the clipboard could not be emptied.{0}{0}{1}", newLine, exception.Message)));
                }

                SendCompleted(this, new SendCompletedEventArgs(fileName, md5));
                return;
            }

            try
            {
                var buffer_ = new byte[dataLength];
                Array.Copy(buffer, buffer_, dataLength);
                sending = true;

                switch (TransmissionMode)
                {
                    case TransmissionMode.Binary:
                        ClipboardWriter.WriteBytes(buffer_);
                        break;
                    case TransmissionMode.Strings:
                    default:
                        ClipboardWriter.WriteText(Convert(buffer_));
                        break;
                }

                timer.Interval = timeout;
                timer.Start();
            }
            catch (Exception exception)
            {
                EndSending();
                ErrorOccurred(this, new ErrorOccurredEventArgs(string.Format("Failed to sending the data.{0}{0}{1}", newLine, exception.Message)));
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            EndSending();
            ErrorOccurred(this, new ErrorOccurredEventArgs("Data reception could not be confirmed."));
        }

        #endregion
    }
}

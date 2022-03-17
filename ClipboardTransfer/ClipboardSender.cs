using ClipboardTransfer.Events;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ClipboardTransfer
{
    public class ClipboardSender : ClipboardFormatListener
    {
        #region Private Fields

        private int bufferSize = 1;
        private readonly string newLine = Environment.NewLine;
        private bool sending = false;
        private Stream stream = null;
        private int timeout = 1;
        private readonly Timer timerSend = new Timer();
        private readonly Timer timerTimeout = new Timer();

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
            timerSend.Tick += new EventHandler(timerSend_Tick);
            timerTimeout.Tick += new EventHandler(timerTimeout_Tick);
        }

        public void BeginSend(Image image, int timeout, int bufferSize, int wait)
        {
            if (sending) throw new InvalidOperationException("ClipboardSender is sending.");

            stream = new MemoryStream();

            try
            {
                image.Save(stream, ImageFormat.Png);
                stream.Position = 0;
            }
            catch
            {
                stream.Dispose();
                stream = null;
                throw;
            }

            BeginSend(timeout, bufferSize, wait);
        }

        private void BeginSend(int timeout, int bufferSize, int wait)
        {
            this.timeout = timeout;
            this.bufferSize = bufferSize;
            timerSend.Interval = wait;
            Enabled = true;
            timerSend.Start();
        }

        public void BeginSend(string fileName, int timeout, int bufferSize, int wait)
        {
            if (sending) throw new InvalidOperationException("ClipboardSender is sending.");

            stream = new FileStream(fileName, FileMode.Open);
            BeginSend(timeout, bufferSize, wait);
        }

        public void EndSending()
        {
            timerSend.Stop();
            timerTimeout.Stop();
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

        protected override void OnClipboardUpdate()
        {
            if (!sending) return;

            timerTimeout.Stop();
            string message = string.Empty;
            bool skip = false;

            try
            {
                switch (TransmissionMode)
                {
                    case TransmissionMode.Binary:
                        byte[] data = ClipboardReader.ReadBytes();
                        skip = ((data != null) && (data.Length > 0));
                        break;

                    case TransmissionMode.Strings:
                    default:
                        string text = ClipboardReader.ReadText();
                        skip = !string.IsNullOrEmpty(text);
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
                timerTimeout.Start();
                return;
            }

            timerSend.Start();
        }

        #endregion

        #region Private Methods

        private string Convert(byte[] bytes)
        {
            return string.Join(string.Empty, bytes.Select(n => n.ToString("X2")));
        }

        private void timerSend_Tick(object sender, EventArgs e)
        {
            timerSend.Stop();
            timerTimeout.Stop();
            var buffer = new byte[bufferSize];
            int dataLength = stream.Read(buffer, 0, buffer.Length);

            if (dataLength < 1)
            {
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

                SendCompleted(this, new SendCompletedEventArgs());
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
                        ClipboardWriter.WriteText(Convert(buffer_), Owner.Handle);
                        break;
                }

                timerTimeout.Interval = timeout;
                timerTimeout.Start();
            }
            catch (Exception exception)
            {
                EndSending();
                ErrorOccurred(this, new ErrorOccurredEventArgs(string.Format("Failed to sending the data.{0}{0}{1}", newLine, exception.Message)));
            }
        }

        private void timerTimeout_Tick(object sender, EventArgs e)
        {
            EndSending();
            ErrorOccurred(this, new ErrorOccurredEventArgs("Data reception could not be confirmed."));
        }

        #endregion
    }
}

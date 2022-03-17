using ClipboardTransfer.Events;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace ClipboardTransfer
{
    public class ClipboardReceiver : ClipboardFormatListener
    {
        #region Private Fields

        private readonly string newLine = Environment.NewLine;
        private bool receiving = false;
        private bool skipNext = false;
        private Stream stream = null;
        private int timeout = 1;
        private readonly Timer timerDelay = new Timer();
        private readonly Timer timerTimeout = new Timer();

        #endregion

        #region Public Properties

        public event ErrorOccurredEventHandler ErrorOccurred = delegate { };
        public int InitialTimeout { get; set; }
        public event ReceiveCompletedEventHandler ReceiveCompleted = delegate { };
        public bool Receiving { get { return receiving; } }
        public TransmissionMode TransmissionMode { get; set; }

        #endregion

        #region Public Methods

        public ClipboardReceiver()
        {
            timerDelay.Tick += timerDelay_Clear_Tick;
            timerTimeout.Tick += new EventHandler(timerTimeout_Tick);
        }

        private void BeginReceive(int timeout, int wait)
        {
            this.timeout = timeout;
            timerDelay.Interval = wait;
            timerTimeout.Interval = InitialTimeout;
            receiving = true;
            timerTimeout.Start();
            Enabled = true;
        }

        private void CheckReceiving()
        {
            if (receiving) throw new InvalidOperationException("ClipboardReceiver is receiving.");
        }

        public void BeginReceiveImage(int timeout, int wait)
        {
            CheckReceiving();
            stream = new MemoryStream();
            BeginReceive(timeout, wait);
        }

        public void BeginReceive(string fileName, int timeout, int wait)
        {
            CheckReceiving();
            stream = new FileStream(fileName, FileMode.Create);
            BeginReceive(timeout, wait);
        }

        public void EndReceiving()
        {
            timerDelay.Stop();
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

            receiving = false;
        }

        #endregion

        #region Protected Methods

        protected override void OnClipboardUpdate()
        {
            if (!receiving) return;

            timerTimeout.Stop();
            byte[] result;

            try
            {
                switch (TransmissionMode)
                {
                    case TransmissionMode.Binary:
                        result = ClipboardReader.ReadBytes();
                        break;

                    case TransmissionMode.Strings:
                    default:
                        string text = ClipboardReader.ReadText();
                        result = Convert(text);
                        break;
                }
            }
            catch (Exception exception)
            {
                EndReceiving();
                ErrorOccurred(this, new ErrorOccurredEventArgs(string.Format("The data could not be received.{0}{0}{1}", newLine, exception.Message)));
                return;
            }

            if ((result == null) || (result.Length < 1))
            {
                if (skipNext)
                {
                    skipNext = false;
                    return;
                }

                if (!(stream is FileStream))
                {
                    timerDelay.Tick -= timerDelay_Clear_Tick;
                    timerDelay.Tick += timerDelay_SetImageToClipboard_Tick;
                    timerDelay.Start();
                    return;
                }

                EndReceiving();
                ReceiveCompleted(this, GenerateReceiveCompletedEventArgs());
                return;
            }

            try
            {
                stream.Write(result, 0, result.Length);
            }
            catch (Exception exception)
            {
                EndReceiving();
                ErrorOccurred(this, new ErrorOccurredEventArgs(string.Format("Failed to writing the file.{0}{0}{1}", newLine, exception.Message)));
                return;
            }

            timerDelay.Start();
        }

        #endregion

        #region Private Methods

        private byte[] Convert(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            int length = value.Length;

            if ((length % 2) != 0) return null;

            var result = new byte[length / 2];

            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = byte.Parse(value.Substring((i * 2), 2), NumberStyles.HexNumber);
            }

            return result;
        }

        private ReceiveCompletedEventArgs GenerateReceiveCompletedEventArgs()
        {
            stream.Position = 0;
            return new ReceiveCompletedEventArgs(HashUtility.HashFromStream(stream));
        }

        private void SetImageToClipboard()
        {
            using (Image image = Image.FromStream(stream))
            {
                Clipboard.SetImage(image);
            }
        }

        private void timerDelay_Clear_Tick(object sender, EventArgs e)
        {
            timerDelay.Stop();

            try
            {
                skipNext = true;
                ClipboardWriter.Empty();
            }
            catch (Exception exception)
            {
                EndReceiving();
                ErrorOccurred(this, new ErrorOccurredEventArgs(string.Format("Failed to clear the clipboard.{0}{0}{1}", newLine, exception.Message)));
            }

            timerTimeout.Interval = timeout;
            timerTimeout.Start();
        }

        private void timerDelay_SetImageToClipboard_Tick(object sender, EventArgs e)
        {
            timerDelay.Stop();
            timerDelay.Tick -= timerDelay_SetImageToClipboard_Tick;
            timerDelay.Tick += timerDelay_Clear_Tick;

            try
            {
                SetImageToClipboard();
            }
            catch (Exception exception)
            {
                EndReceiving();
                ErrorOccurred(this, new ErrorOccurredEventArgs(string.Format("Failed to set the image to the clipboard.{0}{0}{1}", newLine, exception.Message)));
                return;
            }

            ReceiveCompletedEventArgs eventArgs = GenerateReceiveCompletedEventArgs();
            EndReceiving();
            ReceiveCompleted(this, eventArgs);
        }

        private void timerTimeout_Tick(object sender, EventArgs e)
        {
            EndReceiving();
            ErrorOccurred(this, new ErrorOccurredEventArgs("The data could not be received."));
        }

        #endregion
    }
}

using ClipboardTransfer.Events;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace ClipboardTransfer
{
    public class ClipboardReceiver : ClipboardViewer
    {
        #region Private Fields

        private readonly string newLine = Environment.NewLine;
        private bool receiving = false;
        private bool skipNext = false;
        private FileStream stream = null;
        private int timeout = 1;
        private readonly Timer timerClear = new Timer();
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
            timerClear.Tick += new EventHandler(timerClear_Tick);
            timerTimeout.Tick += new EventHandler(timerTimeout_Tick);
        }

        public void BeginReceive(string fileName, int timeout, int wait)
        {
            if (receiving) throw new InvalidOperationException("ClipboardReceiver is receiving.");

            stream = new FileStream(fileName, FileMode.Create);
            this.timeout = timeout;
            timerClear.Interval = wait;
            timerTimeout.Interval = InitialTimeout;
            receiving = true;
            timerTimeout.Start();
            Enabled = true;
        }

        public void EndReceiving()
        {
            timerClear.Stop();
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

        protected override void OnDrawClipboard()
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

                string fileName = stream.Name;
                stream.Position = 0;
                string md5 = HashUtility.HashFromStream(stream);
                EndReceiving();
                ReceiveCompleted(this, new ReceiveCompletedEventArgs(fileName, md5));
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

            timerClear.Start();
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

        private void timerClear_Tick(object sender, EventArgs e)
        {
            timerClear.Stop();

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

        private void timerTimeout_Tick(object sender, EventArgs e)
        {
            EndReceiving();
            ErrorOccurred(this, new ErrorOccurredEventArgs("The data could not be received."));
        }

        #endregion
    }
}

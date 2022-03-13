using ClipboardTransfer.Events;
using ClipboardTransfer.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardTransfer
{
    public partial class FormMain : Form
    {
        #region Private Fields

        private bool messageShowing = false;
        private readonly string newLine = Environment.NewLine;
        private readonly string title;

        #endregion

        #region Public Methods

        public FormMain()
        {
            InitializeComponent();
            MaximumSize = new Size(int.MaxValue, Height);
            title = Text;

            if (Settings.Default.TransmissionMode == TransmissionMode.Strings)
            {
                labelBytes.Text = "Characters.";
            }
        }

        #endregion

        #region Private Methods

        private void BeginReceive()
        {
            string fileName = textBoxFileName.Text;

            if (string.IsNullOrEmpty(fileName))
            {
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                fileName = saveFileDialog.FileName;
                textBoxFileName.Text = fileName;
            }

            if (ShowMessage(
                $"Please start sending from the sender within {clipboardReceiver.InitialTimeout / 1000} seconds after clicking OK.",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;

            try
            {
                clipboardReceiver.BeginReceive(fileName, (int)(numericUpDownTimeout.Value * 1000), (int)numericUpDownWait.Value);
                SetStatus("Receiving...");
                EnableControls(false);
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception);
            }
        }

        private void BeginSend()
        {
            string fileName = textBoxFileName.Text;

            if (string.IsNullOrEmpty(fileName))
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                fileName = openFileDialog.FileName;
                textBoxFileName.Text = fileName;
            }

            if (ShowMessage(
                $"Start reception on the receiving side.{newLine}Click OK when it starts.", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                != DialogResult.OK) return;

            try
            {
                textBoxMd5.Text = HashUtility.HashFromFile(fileName);
                clipboardSender.BeginSend(fileName, (int)(numericUpDownTimeout.Value * 1000), (int)numericUpDownBufferSize.Value, (int)numericUpDownWait.Value);
                SetStatus("Sending...");
                EnableControls(false);
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception);
            }
        }

        private void EnableControls(bool value)
        {
            foreach (Control control in Controls)
            {
                control.Enabled = ((control != buttonCancel) ? value : !value);
            }

            if (value)
            {
                SetStatus(string.Empty);
            }
        }

        private void SetStatus(string status)
        {
            Text = (string.IsNullOrEmpty(status) ? title : $"{status} : {title}");
        }

        private void ShowInformation(string information)
        {
            ShowMessage(information, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowErrorMessage(Exception exception)
        {
            ShowErrorMessage(exception.Message);
        }

        private void ShowErrorMessage(string message)
        {
            ShowMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private DialogResult ShowMessage(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (messageShowing) return DialogResult.None;

            messageShowing = true;
            DialogResult result = MessageBox.Show(this, text, Text, buttons, icon);
            messageShowing = false;
            return result;
        }

        #endregion

        // Designer's Methods

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (clipboardSender.Sending)
                {
                    clipboardSender.EndSending();
                }

                if (clipboardReceiver.Receiving)
                {
                    clipboardReceiver.EndReceiving();
                }

                EnableControls(true);
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception);
            }
        }

        private void buttonCopyMd5_Click(object sender, EventArgs e)
        {
            string value = textBoxMd5.Text;

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            try
            {
                Clipboard.SetText(value);
                ShowInformation("The MD5 value has been written to the clipboard.");
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception);
            }
        }

        private void buttonReceive_Click(object sender, EventArgs e)
        {
            BeginReceive();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            BeginSend();
        }

        private void clipboardReceiver_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            textBoxMd5.Text = e.Md5;
            ShowInformation($"Data reception is complete.{newLine}Check the MD5 value.");
            EnableControls(true);
        }

        private void clipboardSender_SendCompleted(object sender, SendCompletedEventArgs e)
        {
            ShowInformation($"Data transmission is complete.{newLine}Check the MD5 value of the destination.");
            EnableControls(true);
        }

        private void clipboardViewer_ErrorOccurred(object sender, ErrorOccurredEventArgs e)
        {
            ShowErrorMessage(e.Message);
            EnableControls(true);
        }

        private void textBoxFileName_DragDrop(object sender, DragEventArgs e)
        {
            if (!(e.Data.GetData(DataFormats.FileDrop) is string[] data) || (data.Length < 1))
            {
                return;
            }

            textBoxFileName.Text = data[0];
        }

        private void textBoxFileName_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None);
        }
    }
}

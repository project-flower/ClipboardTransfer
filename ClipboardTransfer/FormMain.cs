using ClipboardTransfer.Events;
using ClipboardTransfer.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            string fileName = default;
            bool asImage = checkBoxAsImage.Checked;

            if (!asImage)
            {
                fileName = textBoxFileName.Text;

                if (string.IsNullOrEmpty(fileName))
                {
                    if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                    fileName = saveFileDialog.FileName;
                    textBoxFileName.Text = fileName;
                }

                textBoxMd5.Text = string.Empty;
            }

            if (!checkBoxNoConfirmation.Checked &&
                ShowMessage($"Please start sending from the sender within {(clipboardReceiver.InitialTimeout / 1000)} seconds after clicking OK.",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;

            int timeout = (int)(numericUpDownTimeout.Value * 1000);
            int wait = (int)numericUpDownWait.Value;

            try
            {
                if (asImage)
                {
                    clipboardReceiver.BeginReceiveImage(timeout, wait);
                }
                else
                {
                    clipboardReceiver.BeginReceive(fileName, timeout, wait);
                }

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
            bool asImage = checkBoxAsImage.Checked;
            string fileName = default;
            string message;

            if (asImage)
            {
                message = $"Set the image to the Clipboard,{newLine}and start reception on the receiving side.{newLine}Click OK when it starts.";
            }
            else
            {
                fileName = textBoxFileName.Text;

                if (string.IsNullOrEmpty(fileName))
                {
                    if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                    fileName = openFileDialog.FileName;
                    textBoxFileName.Text = fileName;
                }

                message = $"Start reception on the receiving side.{newLine}Click OK when it starts.";
            }

            if (!checkBoxNoConfirmation.Checked &&
                ShowMessage(message, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;

            Image image = null;

            try
            {
                string md5;

                if (asImage)
                {
                    image = Clipboard.GetImage();

                    using (Stream stream = new MemoryStream())
                    {
                        image.Save(stream, ImageFormat.Png);
                        stream.Position = 0;
                        md5 = HashUtility.HashFromStream(stream);
                    }
                }
                else
                {
                    md5 = HashUtility.HashFromFile(fileName);
                }

                textBoxMd5.Text = md5;
                int timeout = (int)(numericUpDownTimeout.Value * 1000);
                int bufferSize = (int)numericUpDownBufferSize.Value;
                int wait = (int)numericUpDownWait.Value;

                if (asImage)
                {
                    clipboardSender.BeginSend(image, timeout, bufferSize, wait);
                }
                else
                {
                    clipboardSender.BeginSend(fileName, timeout, bufferSize, wait);
                }

                SetStatus("Sending...");
                EnableControls(false);
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception);
            }
            finally
            {
                if (image != null) image.Dispose();
            }
        }

        private void EnableControls(bool value)
        {
            foreach (Control control in Controls)
            {
                SetEnabled(control, ((control != buttonCancel) ? value : !value));
            }

            if (value)
            {
                EnableFileOptions();
                SetStatus(string.Empty);
            }
        }

        private void EnableFileOptions()
        {
            bool enabled = !checkBoxAsImage.Checked;
            SetEnabled(labelFile, enabled);
            SetEnabled(textBoxFileName, enabled);
            SetEnabled(buttonShowFile, enabled);
        }

        private void SetEnabled(Control control, bool value)
        {
            if (control.Enabled != value) control.Enabled = value;
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

        private void buttonShowFile_Click(object sender, EventArgs e)
        {
            string fileName = textBoxFileName.Text;

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            try
            {
                Process.Start("explorer.exe", $"/select,\"{fileName}\"");
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception);
            }
        }

        private void checkBoxAsImage_CheckedChanged(object sender, EventArgs e)
        {
            EnableFileOptions();
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

namespace ClipboardTransfer
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelFile = new System.Windows.Forms.Label();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.buttonShowFile = new System.Windows.Forms.Button();
            this.labelMd5 = new System.Windows.Forms.Label();
            this.textBoxMd5 = new System.Windows.Forms.TextBox();
            this.buttonCopyMd5 = new System.Windows.Forms.Button();
            this.labelTimeout = new System.Windows.Forms.Label();
            this.numericUpDownTimeout = new System.Windows.Forms.NumericUpDown();
            this.labelSec = new System.Windows.Forms.Label();
            this.labelBuffer = new System.Windows.Forms.Label();
            this.numericUpDownBufferSize = new System.Windows.Forms.NumericUpDown();
            this.labelBytes = new System.Windows.Forms.Label();
            this.labelWait = new System.Windows.Forms.Label();
            this.numericUpDownWait = new System.Windows.Forms.NumericUpDown();
            this.labelMSec = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonReceive = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.clipboardReceiver = new ClipboardTransfer.ClipboardReceiver();
            this.clipboardSender = new ClipboardTransfer.ClipboardSender();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWait)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFile
            // 
            this.labelFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(12, 17);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(26, 12);
            this.labelFile.TabIndex = 0;
            this.labelFile.Text = "&File:";
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.AllowDrop = true;
            this.textBoxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxFileName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.textBoxFileName.Location = new System.Drawing.Point(48, 14);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(659, 19);
            this.textBoxFileName.TabIndex = 1;
            this.textBoxFileName.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxFileName_DragDrop);
            this.textBoxFileName.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxFileName_DragEnter);
            // 
            // buttonShowFile
            // 
            this.buttonShowFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowFile.Location = new System.Drawing.Point(713, 12);
            this.buttonShowFile.Name = "buttonShowFile";
            this.buttonShowFile.Size = new System.Drawing.Size(75, 23);
            this.buttonShowFile.TabIndex = 2;
            this.buttonShowFile.Text = "S&how";
            this.buttonShowFile.UseVisualStyleBackColor = true;
            this.buttonShowFile.Click += new System.EventHandler(this.buttonShowFile_Click);
            // 
            // labelMd5
            // 
            this.labelMd5.AutoSize = true;
            this.labelMd5.Location = new System.Drawing.Point(12, 46);
            this.labelMd5.Name = "labelMd5";
            this.labelMd5.Size = new System.Drawing.Size(30, 12);
            this.labelMd5.TabIndex = 3;
            this.labelMd5.Text = "MD5:";
            // 
            // textBoxMd5
            // 
            this.textBoxMd5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMd5.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxMd5.Location = new System.Drawing.Point(48, 43);
            this.textBoxMd5.Name = "textBoxMd5";
            this.textBoxMd5.ReadOnly = true;
            this.textBoxMd5.Size = new System.Drawing.Size(659, 19);
            this.textBoxMd5.TabIndex = 4;
            // 
            // buttonCopyMd5
            // 
            this.buttonCopyMd5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyMd5.Location = new System.Drawing.Point(713, 41);
            this.buttonCopyMd5.Name = "buttonCopyMd5";
            this.buttonCopyMd5.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyMd5.TabIndex = 5;
            this.buttonCopyMd5.Text = "Copy";
            this.buttonCopyMd5.UseVisualStyleBackColor = true;
            this.buttonCopyMd5.Click += new System.EventHandler(this.buttonCopyMd5_Click);
            // 
            // labelTimeout
            // 
            this.labelTimeout.AutoSize = true;
            this.labelTimeout.Location = new System.Drawing.Point(12, 70);
            this.labelTimeout.Name = "labelTimeout";
            this.labelTimeout.Size = new System.Drawing.Size(48, 12);
            this.labelTimeout.TabIndex = 6;
            this.labelTimeout.Text = "&Timeout:";
            // 
            // numericUpDownTimeout
            // 
            this.numericUpDownTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ClipboardTransfer.Properties.Settings.Default, "TimeoutSeconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownTimeout.Location = new System.Drawing.Point(101, 68);
            this.numericUpDownTimeout.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numericUpDownTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTimeout.Name = "numericUpDownTimeout";
            this.numericUpDownTimeout.Size = new System.Drawing.Size(120, 19);
            this.numericUpDownTimeout.TabIndex = 7;
            this.numericUpDownTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownTimeout.Value = global::ClipboardTransfer.Properties.Settings.Default.TimeoutSeconds;
            // 
            // labelSec
            // 
            this.labelSec.AutoSize = true;
            this.labelSec.Location = new System.Drawing.Point(227, 70);
            this.labelSec.Name = "labelSec";
            this.labelSec.Size = new System.Drawing.Size(25, 12);
            this.labelSec.TabIndex = 8;
            this.labelSec.Text = "sec.";
            // 
            // labelBuffer
            // 
            this.labelBuffer.AutoSize = true;
            this.labelBuffer.Location = new System.Drawing.Point(12, 95);
            this.labelBuffer.Name = "labelBuffer";
            this.labelBuffer.Size = new System.Drawing.Size(83, 12);
            this.labelBuffer.TabIndex = 9;
            this.labelBuffer.Text = "Sending &Buffer:";
            // 
            // numericUpDownBufferSize
            // 
            this.numericUpDownBufferSize.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ClipboardTransfer.Properties.Settings.Default, "BufferSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownBufferSize.Location = new System.Drawing.Point(101, 93);
            this.numericUpDownBufferSize.Maximum = new decimal(new int[] {
            4194304,
            0,
            0,
            0});
            this.numericUpDownBufferSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownBufferSize.Name = "numericUpDownBufferSize";
            this.numericUpDownBufferSize.Size = new System.Drawing.Size(120, 19);
            this.numericUpDownBufferSize.TabIndex = 10;
            this.numericUpDownBufferSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownBufferSize.Value = global::ClipboardTransfer.Properties.Settings.Default.BufferSize;
            // 
            // labelBytes
            // 
            this.labelBytes.AutoSize = true;
            this.labelBytes.Location = new System.Drawing.Point(226, 95);
            this.labelBytes.Name = "labelBytes";
            this.labelBytes.Size = new System.Drawing.Size(35, 12);
            this.labelBytes.TabIndex = 11;
            this.labelBytes.Text = "Bytes";
            // 
            // labelWait
            // 
            this.labelWait.AutoSize = true;
            this.labelWait.Location = new System.Drawing.Point(12, 120);
            this.labelWait.Name = "labelWait";
            this.labelWait.Size = new System.Drawing.Size(29, 12);
            this.labelWait.TabIndex = 12;
            this.labelWait.Text = "&Wait:";
            // 
            // numericUpDownWait
            // 
            this.numericUpDownWait.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ClipboardTransfer.Properties.Settings.Default, "TransmissionWait", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownWait.Location = new System.Drawing.Point(101, 118);
            this.numericUpDownWait.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownWait.Name = "numericUpDownWait";
            this.numericUpDownWait.Size = new System.Drawing.Size(120, 19);
            this.numericUpDownWait.TabIndex = 13;
            this.numericUpDownWait.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownWait.Value = global::ClipboardTransfer.Properties.Settings.Default.TransmissionWait;
            // 
            // labelMSec
            // 
            this.labelMSec.AutoSize = true;
            this.labelMSec.Location = new System.Drawing.Point(227, 120);
            this.labelMSec.Name = "labelMSec";
            this.labelMSec.Size = new System.Drawing.Size(34, 12);
            this.labelMSec.TabIndex = 14;
            this.labelMSec.Text = "msec.";
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Location = new System.Drawing.Point(551, 114);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 15;
            this.buttonSend.Text = "&Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonReceive
            // 
            this.buttonReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReceive.Location = new System.Drawing.Point(632, 114);
            this.buttonReceive.Name = "buttonReceive";
            this.buttonReceive.Size = new System.Drawing.Size(75, 23);
            this.buttonReceive.TabIndex = 16;
            this.buttonReceive.Text = "&Receive";
            this.buttonReceive.UseVisualStyleBackColor = true;
            this.buttonReceive.Click += new System.EventHandler(this.buttonReceive_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(713, 114);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // clipboardReceiver
            // 
            this.clipboardReceiver.InitialTimeout = global::ClipboardTransfer.Properties.Settings.Default.InitialTimeout;
            this.clipboardReceiver.Owner = this;
            this.clipboardReceiver.TransmissionMode = global::ClipboardTransfer.Properties.Settings.Default.TransmissionMode;
            this.clipboardReceiver.ErrorOccurred += new ClipboardTransfer.Events.ErrorOccurredEventHandler(this.clipboardViewer_ErrorOccurred);
            this.clipboardReceiver.ReceiveCompleted += new ClipboardTransfer.Events.ReceiveCompletedEventHandler(this.clipboardReceiver_ReceiveCompleted);
            // 
            // clipboardSender
            // 
            this.clipboardSender.Owner = this;
            this.clipboardSender.TransmissionMode = global::ClipboardTransfer.Properties.Settings.Default.TransmissionMode;
            this.clipboardSender.ErrorOccurred += new ClipboardTransfer.Events.ErrorOccurredEventHandler(this.clipboardViewer_ErrorOccurred);
            this.clipboardSender.SendCompleted += new ClipboardTransfer.Events.SendCompletedEventHandler(this.clipboardSender_SendCompleted);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "All Files|*.*";
            this.openFileDialog.Title = "Select the file to send.";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "All Files|*.*";
            this.saveFileDialog.Title = "Save as ...";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 149);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonReceive);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.labelMSec);
            this.Controls.Add(this.numericUpDownWait);
            this.Controls.Add(this.labelWait);
            this.Controls.Add(this.labelBytes);
            this.Controls.Add(this.numericUpDownBufferSize);
            this.Controls.Add(this.labelBuffer);
            this.Controls.Add(this.labelSec);
            this.Controls.Add(this.numericUpDownTimeout);
            this.Controls.Add(this.labelTimeout);
            this.Controls.Add(this.buttonCopyMd5);
            this.Controls.Add(this.textBoxMd5);
            this.Controls.Add(this.labelMd5);
            this.Controls.Add(this.buttonShowFile);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.labelFile);
            this.Name = "FormMain";
            this.Text = "ClipboardTransfer";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWait)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Button buttonShowFile;
        private System.Windows.Forms.Label labelMd5;
        private System.Windows.Forms.TextBox textBoxMd5;
        private System.Windows.Forms.Button buttonCopyMd5;
        private System.Windows.Forms.Label labelTimeout;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeout;
        private System.Windows.Forms.Label labelSec;
        private System.Windows.Forms.Label labelBuffer;
        private System.Windows.Forms.NumericUpDown numericUpDownBufferSize;
        private System.Windows.Forms.Label labelBytes;
        private System.Windows.Forms.Label labelWait;
        private System.Windows.Forms.NumericUpDown numericUpDownWait;
        private System.Windows.Forms.Label labelMSec;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonReceive;
        private System.Windows.Forms.Button buttonCancel;
        private ClipboardReceiver clipboardReceiver;
        private ClipboardSender clipboardSender;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}


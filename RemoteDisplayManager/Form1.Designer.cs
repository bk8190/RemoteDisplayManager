namespace RemoteDisplayManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.connectButton = new System.Windows.Forms.Button();
            this.serialPortBox = new System.Windows.Forms.ComboBox();
            this.serialTimer = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.StatusCurrentTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.rawResponsesTextBox = new System.Windows.Forms.TextBox();
            this.backlightBox = new System.Windows.Forms.ComboBox();
            this.mxieCheckBox = new System.Windows.Forms.CheckBox();
            this.connectedBox = new System.Windows.Forms.CheckBox();
            this.mxieTimer = new System.Windows.Forms.Timer(this.components);
            this.connectionTimeoutTimer = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.mxiePicture = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pingTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mxiePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(139, 13);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButtonClicked);
            // 
            // serialPortBox
            // 
            this.serialPortBox.FormattingEnabled = true;
            this.serialPortBox.Location = new System.Drawing.Point(12, 12);
            this.serialPortBox.Name = "serialPortBox";
            this.serialPortBox.Size = new System.Drawing.Size(121, 24);
            this.serialPortBox.TabIndex = 2;
            this.serialPortBox.DropDown += new System.EventHandler(this.portBoxDropDown);
            // 
            // serialTimer
            // 
            this.serialTimer.Enabled = true;
            this.serialTimer.Interval = 1000;
            this.serialTimer.Tick += new System.EventHandler(this.serialTimerCallback);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 194);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(249, 22);
            this.textBox1.TabIndex = 3;
            // 
            // StatusCurrentTextBox
            // 
            this.StatusCurrentTextBox.Enabled = false;
            this.StatusCurrentTextBox.Location = new System.Drawing.Point(12, 100);
            this.StatusCurrentTextBox.Multiline = true;
            this.StatusCurrentTextBox.Name = "StatusCurrentTextBox";
            this.StatusCurrentTextBox.Size = new System.Drawing.Size(268, 42);
            this.StatusCurrentTextBox.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(12, 165);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Set status";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.setStatusButtonClick);
            // 
            // rawResponsesTextBox
            // 
            this.rawResponsesTextBox.Location = new System.Drawing.Point(12, 268);
            this.rawResponsesTextBox.Multiline = true;
            this.rawResponsesTextBox.Name = "rawResponsesTextBox";
            this.rawResponsesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.rawResponsesTextBox.Size = new System.Drawing.Size(268, 174);
            this.rawResponsesTextBox.TabIndex = 5;
            // 
            // backlightBox
            // 
            this.backlightBox.Enabled = false;
            this.backlightBox.FormattingEnabled = true;
            this.backlightBox.Items.AddRange(new object[] {
            "off",
            "red",
            "yellow",
            "green",
            "teal",
            "blue",
            "violet",
            "white"});
            this.backlightBox.Location = new System.Drawing.Point(84, 238);
            this.backlightBox.Name = "backlightBox";
            this.backlightBox.Size = new System.Drawing.Size(121, 24);
            this.backlightBox.TabIndex = 6;
            this.backlightBox.Text = "off";
            this.backlightBox.SelectedIndexChanged += new System.EventHandler(this.changeBacklight);
            // 
            // mxieCheckBox
            // 
            this.mxieCheckBox.AutoSize = true;
            this.mxieCheckBox.Checked = true;
            this.mxieCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mxieCheckBox.Location = new System.Drawing.Point(12, 43);
            this.mxieCheckBox.Name = "mxieCheckBox";
            this.mxieCheckBox.Size = new System.Drawing.Size(104, 21);
            this.mxieCheckBox.TabIndex = 7;
            this.mxieCheckBox.Text = "MXIE status";
            this.mxieCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mxieCheckBox.UseVisualStyleBackColor = true;
            // 
            // connectedBox
            // 
            this.connectedBox.AutoSize = true;
            this.connectedBox.Enabled = false;
            this.connectedBox.Location = new System.Drawing.Point(220, 13);
            this.connectedBox.Name = "connectedBox";
            this.connectedBox.Size = new System.Drawing.Size(45, 21);
            this.connectedBox.TabIndex = 8;
            this.connectedBox.Text = "ok";
            this.connectedBox.UseVisualStyleBackColor = true;
            // 
            // mxieTimer
            // 
            this.mxieTimer.Enabled = true;
            this.mxieTimer.Interval = 10000;
            this.mxieTimer.Tick += new System.EventHandler(this.mxieTimerCallback);
            // 
            // connectionTimeoutTimer
            // 
            this.connectionTimeoutTimer.Enabled = true;
            this.connectionTimeoutTimer.Interval = 30000;
            this.connectionTimeoutTimer.Tick += new System.EventHandler(this.connectedTimerCallback);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(108, 165);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Send raw";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.sendRawButtonClick);
            // 
            // mxiePicture
            // 
            this.mxiePicture.Location = new System.Drawing.Point(12, 70);
            this.mxiePicture.Name = "mxiePicture";
            this.mxiePicture.Size = new System.Drawing.Size(268, 27);
            this.mxiePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mxiePicture.TabIndex = 9;
            this.mxiePicture.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Backlight";
            // 
            // pingTimer
            // 
            this.pingTimer.Interval = 15000;
            this.pingTimer.Tick += new System.EventHandler(this.pingTimerCallback);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 449);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mxiePicture);
            this.Controls.Add(this.connectedBox);
            this.Controls.Add(this.mxieCheckBox);
            this.Controls.Add(this.backlightBox);
            this.Controls.Add(this.rawResponsesTextBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.StatusCurrentTextBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.serialPortBox);
            this.Controls.Add(this.connectButton);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.mxiePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.ComboBox serialPortBox;
        private System.Windows.Forms.Timer serialTimer;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox StatusCurrentTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox rawResponsesTextBox;
        private System.Windows.Forms.ComboBox backlightBox;
        private System.Windows.Forms.CheckBox mxieCheckBox;
        private System.Windows.Forms.CheckBox connectedBox;
        private System.Windows.Forms.Timer mxieTimer;
        private System.Windows.Forms.Timer connectionTimeoutTimer;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox mxiePicture;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer pingTimer;
    }
}


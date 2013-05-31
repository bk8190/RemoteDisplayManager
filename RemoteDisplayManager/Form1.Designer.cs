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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.StatusCurrentTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.rawResponsesTextBox = new System.Windows.Forms.TextBox();
            this.backlightBox = new System.Windows.Forms.ComboBox();
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
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timerCallback);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 57);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(249, 22);
            this.textBox1.TabIndex = 3;
            // 
            // StatusCurrentTextBox
            // 
            this.StatusCurrentTextBox.Enabled = false;
            this.StatusCurrentTextBox.Location = new System.Drawing.Point(12, 114);
            this.StatusCurrentTextBox.Name = "StatusCurrentTextBox";
            this.StatusCurrentTextBox.Size = new System.Drawing.Size(268, 22);
            this.StatusCurrentTextBox.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(12, 85);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.copyStatus);
            // 
            // rawResponsesTextBox
            // 
            this.rawResponsesTextBox.Location = new System.Drawing.Point(12, 263);
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
            this.backlightBox.Location = new System.Drawing.Point(12, 174);
            this.backlightBox.Name = "backlightBox";
            this.backlightBox.Size = new System.Drawing.Size(121, 24);
            this.backlightBox.TabIndex = 6;
            this.backlightBox.SelectedIndexChanged += new System.EventHandler(this.changeBacklight);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 449);
            this.Controls.Add(this.backlightBox);
            this.Controls.Add(this.rawResponsesTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.StatusCurrentTextBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.serialPortBox);
            this.Controls.Add(this.connectButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.ComboBox serialPortBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox StatusCurrentTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox rawResponsesTextBox;
        private System.Windows.Forms.ComboBox backlightBox;
    }
}


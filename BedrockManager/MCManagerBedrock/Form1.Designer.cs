namespace MCManager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtOut = new TextBox();
            txtIn = new TextBox();
            btnstart = new Button();
            btnstop = new Button();
            cPlayers = new ComboBox();
            btnBackup = new Button();
            SuspendLayout();
            // 
            // txtOut
            // 
            txtOut.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtOut.Location = new Point(14, 16);
            txtOut.Margin = new Padding(3, 4, 3, 4);
            txtOut.Multiline = true;
            txtOut.Name = "txtOut";
            txtOut.ReadOnly = true;
            txtOut.ScrollBars = ScrollBars.Vertical;
            txtOut.Size = new Size(886, 435);
            txtOut.TabIndex = 99;
            txtOut.TabStop = false;
            // 
            // txtIn
            // 
            txtIn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtIn.AutoCompleteCustomSource.AddRange(new string[] { "gamemode", "time set day", "gamerule" });
            txtIn.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtIn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtIn.Location = new Point(14, 553);
            txtIn.Margin = new Padding(3, 4, 3, 4);
            txtIn.Name = "txtIn";
            txtIn.Size = new Size(667, 27);
            txtIn.TabIndex = 1;
            txtIn.KeyUp += txtIn_KeyUp;
            // 
            // btnstart
            // 
            btnstart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnstart.Location = new Point(14, 460);
            btnstart.Margin = new Padding(3, 4, 3, 4);
            btnstart.Name = "btnstart";
            btnstart.Size = new Size(151, 85);
            btnstart.TabIndex = 2;
            btnstart.TabStop = false;
            btnstart.Text = "Start";
            btnstart.UseVisualStyleBackColor = true;
            btnstart.Click += btnstart_Click;
            // 
            // btnstop
            // 
            btnstop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnstop.Location = new Point(171, 460);
            btnstop.Margin = new Padding(3, 4, 3, 4);
            btnstop.Name = "btnstop";
            btnstop.Size = new Size(151, 85);
            btnstop.TabIndex = 3;
            btnstop.TabStop = false;
            btnstop.Text = "Stop";
            btnstop.UseVisualStyleBackColor = true;
            btnstop.Click += btnstop_Click;
            // 
            // cPlayers
            // 
            cPlayers.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cPlayers.FormattingEnabled = true;
            cPlayers.Location = new Point(678, 553);
            cPlayers.Margin = new Padding(3, 4, 3, 4);
            cPlayers.Name = "cPlayers";
            cPlayers.Size = new Size(222, 28);
            cPlayers.Sorted = true;
            cPlayers.TabIndex = 4;
            cPlayers.TabStop = false;
            cPlayers.TextChanged += cPlayers_TextChanged;
            // 
            // btnBackup
            // 
            btnBackup.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnBackup.Location = new Point(328, 459);
            btnBackup.Margin = new Padding(3, 4, 3, 4);
            btnBackup.Name = "btnBackup";
            btnBackup.Size = new Size(151, 85);
            btnBackup.TabIndex = 100;
            btnBackup.TabStop = false;
            btnBackup.Text = "Backup";
            btnBackup.UseVisualStyleBackColor = true;
            btnBackup.Click += btnBackup_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(btnBackup);
            Controls.Add(cPlayers);
            Controls.Add(btnstop);
            Controls.Add(btnstart);
            Controls.Add(txtIn);
            Controls.Add(txtOut);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Bedrock";
            WindowState = FormWindowState.Maximized;
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtOut;
        private TextBox txtIn;
        private Button btnstart;
        private Button btnstop;
        private ComboBox cPlayers;
        private Button btnBackup;
    }
}
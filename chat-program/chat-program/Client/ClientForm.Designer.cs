namespace ChatProgram.Client
{
    partial class ClientForm
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
            this.gbUsers = new System.Windows.Forms.GroupBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.containerMessages = new System.Windows.Forms.GroupBox();
            this.gbMessages = new System.Windows.Forms.Panel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.pbProgressBar = new System.Windows.Forms.ProgressBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.containerMessages.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbUsers
            // 
            this.gbUsers.Location = new System.Drawing.Point(776, 14);
            this.gbUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbUsers.Name = "gbUsers";
            this.gbUsers.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbUsers.Size = new System.Drawing.Size(277, 496);
            this.gbUsers.TabIndex = 5;
            this.gbUsers.TabStop = false;
            this.gbUsers.Text = "Users";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(60, 517);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4);
            this.txtMessage.MaxLength = 256;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(707, 22);
            this.txtMessage.TabIndex = 4;
            this.txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);
            // 
            // containerMessages
            // 
            this.containerMessages.Controls.Add(this.gbMessages);
            this.containerMessages.Location = new System.Drawing.Point(13, 14);
            this.containerMessages.Margin = new System.Windows.Forms.Padding(4);
            this.containerMessages.Name = "containerMessages";
            this.containerMessages.Padding = new System.Windows.Forms.Padding(4);
            this.containerMessages.Size = new System.Drawing.Size(755, 496);
            this.containerMessages.TabIndex = 3;
            this.containerMessages.TabStop = false;
            this.containerMessages.Text = "Messages";
            // 
            // gbMessages
            // 
            this.gbMessages.AutoScroll = true;
            this.gbMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMessages.Location = new System.Drawing.Point(4, 19);
            this.gbMessages.Margin = new System.Windows.Forms.Padding(4);
            this.gbMessages.Name = "gbMessages";
            this.gbMessages.Size = new System.Drawing.Size(747, 473);
            this.gbMessages.TabIndex = 0;
            this.gbMessages.Scroll += new System.Windows.Forms.ScrollEventHandler(this.gbMessages_Scroll);
            this.gbMessages.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.gbMessages_MouseWheel);
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.Location = new System.Drawing.Point(17, 517);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(35, 25);
            this.btnUpload.TabIndex = 6;
            this.btnUpload.Text = "⇧";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // pbProgressBar
            // 
            this.pbProgressBar.Location = new System.Drawing.Point(776, 517);
            this.pbProgressBar.Name = "pbProgressBar";
            this.pbProgressBar.Size = new System.Drawing.Size(277, 22);
            this.pbProgressBar.TabIndex = 7;
            this.pbProgressBar.MouseHover += new System.EventHandler(this.pbProgressBar_MouseHover);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.pbProgressBar);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.gbUsers);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.containerMessages);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientForm_FormClosed);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.containerMessages.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbUsers;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.GroupBox containerMessages;
        private System.Windows.Forms.Panel gbMessages;
        private System.Windows.Forms.Button btnUpload;
        public System.Windows.Forms.ProgressBar pbProgressBar;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
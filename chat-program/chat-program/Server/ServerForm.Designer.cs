namespace ChatProgram.Server
{
    partial class ServerForm
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
            this.containerMessages = new System.Windows.Forms.GroupBox();
            this.gbMessages = new System.Windows.Forms.Panel();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.gbUsers = new System.Windows.Forms.GroupBox();
            this.heartBeatTimer = new System.Windows.Forms.Timer(this.components);
            this.containerMessages.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerMessages
            // 
            this.containerMessages.Controls.Add(this.gbMessages);
            this.containerMessages.Location = new System.Drawing.Point(12, 12);
            this.containerMessages.Name = "containerMessages";
            this.containerMessages.Size = new System.Drawing.Size(566, 403);
            this.containerMessages.TabIndex = 0;
            this.containerMessages.TabStop = false;
            this.containerMessages.Text = "Messages";
            // 
            // gbMessages
            // 
            this.gbMessages.AutoScroll = true;
            this.gbMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMessages.Location = new System.Drawing.Point(3, 16);
            this.gbMessages.Name = "gbMessages";
            this.gbMessages.Size = new System.Drawing.Size(560, 384);
            this.gbMessages.TabIndex = 0;
            // 
            // txtMessage
            // 
            this.txtMessage.AcceptsReturn = true;
            this.txtMessage.Location = new System.Drawing.Point(12, 421);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(566, 20);
            this.txtMessage.TabIndex = 1;
            this.txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);
            // 
            // gbUsers
            // 
            this.gbUsers.Location = new System.Drawing.Point(584, 12);
            this.gbUsers.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbUsers.Name = "gbUsers";
            this.gbUsers.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbUsers.Size = new System.Drawing.Size(208, 427);
            this.gbUsers.TabIndex = 2;
            this.gbUsers.TabStop = false;
            this.gbUsers.Text = "Users";
            // 
            // heartBeatTimer
            // 
            this.heartBeatTimer.Interval = 5000;
            this.heartBeatTimer.Tick += new System.EventHandler(this.heartBeatTimer_Tick);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gbUsers);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.containerMessages);
            this.Name = "ServerForm";
            this.Text = "ServerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerForm_FormClosed);
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.containerMessages.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox containerMessages;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.GroupBox gbUsers;
        private System.Windows.Forms.Panel gbMessages;
        public System.Windows.Forms.Timer heartBeatTimer;
    }
}
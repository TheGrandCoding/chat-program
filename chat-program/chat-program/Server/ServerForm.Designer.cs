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
            this.gbMessages = new System.Windows.Forms.GroupBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.gbUsers = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // gbMessages
            // 
            this.gbMessages.Location = new System.Drawing.Point(16, 15);
            this.gbMessages.Margin = new System.Windows.Forms.Padding(4);
            this.gbMessages.Name = "gbMessages";
            this.gbMessages.Padding = new System.Windows.Forms.Padding(4);
            this.gbMessages.Size = new System.Drawing.Size(755, 496);
            this.gbMessages.TabIndex = 0;
            this.gbMessages.TabStop = false;
            this.gbMessages.Text = "Messages";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(16, 518);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(753, 22);
            this.txtMessage.TabIndex = 1;
            this.txtMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyUp);
            // 
            // gbUsers
            // 
            this.gbUsers.Location = new System.Drawing.Point(778, 15);
            this.gbUsers.Name = "gbUsers";
            this.gbUsers.Size = new System.Drawing.Size(277, 525);
            this.gbUsers.TabIndex = 2;
            this.gbUsers.TabStop = false;
            this.gbUsers.Text = "Users";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.gbUsers);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.gbMessages);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ServerForm";
            this.Text = "ServerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMessages;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.GroupBox gbUsers;
    }
}
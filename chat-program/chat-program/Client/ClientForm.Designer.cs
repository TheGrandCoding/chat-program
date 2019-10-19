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
            this.gbUsers = new System.Windows.Forms.GroupBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.containerMessages = new System.Windows.Forms.GroupBox();
            this.gbMessages = new System.Windows.Forms.Panel();
            this.containerMessages.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbUsers
            // 
            this.gbUsers.Location = new System.Drawing.Point(776, 14);
            this.gbUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbUsers.Name = "gbUsers";
            this.gbUsers.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbUsers.Size = new System.Drawing.Size(277, 526);
            this.gbUsers.TabIndex = 5;
            this.gbUsers.TabStop = false;
            this.gbUsers.Text = "Users";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(13, 517);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(753, 22);
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
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
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
    }
}
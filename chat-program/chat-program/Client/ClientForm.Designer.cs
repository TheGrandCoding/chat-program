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
            this.gbUsers.Location = new System.Drawing.Point(582, 11);
            this.gbUsers.Margin = new System.Windows.Forms.Padding(2);
            this.gbUsers.Name = "gbUsers";
            this.gbUsers.Padding = new System.Windows.Forms.Padding(2);
            this.gbUsers.Size = new System.Drawing.Size(208, 427);
            this.gbUsers.TabIndex = 5;
            this.gbUsers.TabStop = false;
            this.gbUsers.Text = "Users";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(10, 420);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(566, 20);
            this.txtMessage.TabIndex = 4;
            this.txtMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyUp);
            // 
            // containerMessages
            // 
            this.containerMessages.Controls.Add(this.gbMessages);
            this.containerMessages.Location = new System.Drawing.Point(10, 11);
            this.containerMessages.Name = "containerMessages";
            this.containerMessages.Size = new System.Drawing.Size(566, 403);
            this.containerMessages.TabIndex = 3;
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
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gbUsers);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.containerMessages);
            this.Name = "ClientForm";
            this.Text = "ClientForm";
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
namespace ChatProgram.Client
{
    partial class NotificationForm
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
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtReply = new System.Windows.Forms.TextBox();
            this.timeoutTimer = new System.Windows.Forms.Timer(this.components);
            this.lblTimeout = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFrom
            // 
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(100, 11);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(303, 18);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(16, 30);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(387, 169);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "From:";
            // 
            // txtReply
            // 
            this.txtReply.Location = new System.Drawing.Point(20, 203);
            this.txtReply.Margin = new System.Windows.Forms.Padding(4);
            this.txtReply.Name = "txtReply";
            this.txtReply.Size = new System.Drawing.Size(385, 22);
            this.txtReply.TabIndex = 2;
            this.txtReply.TextChanged += new System.EventHandler(this.txtReply_TextChanged);
            this.txtReply.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtReply_KeyUp);
            // 
            // timeoutTimer
            // 
            this.timeoutTimer.Interval = 1000;
            this.timeoutTimer.Tick += new System.EventHandler(this.timeoutTimer_Tick);
            // 
            // lblTimeout
            // 
            this.lblTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeout.Location = new System.Drawing.Point(16, 11);
            this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(55, 18);
            this.lblTimeout.TabIndex = 3;
            this.lblTimeout.Text = "30";
            this.lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 232);
            this.ControlBox = false;
            this.Controls.Add(this.lblTimeout);
            this.Controls.Add(this.txtReply);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblFrom);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(437, 279);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(437, 279);
            this.Name = "NotificationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "New Message";
            this.Activated += new System.EventHandler(this.NotificationForm_Activated);
            this.LocationChanged += new System.EventHandler(this.NotificationForm_LocationChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NotificationForm_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtReply;
        private System.Windows.Forms.Timer timeoutTimer;
        private System.Windows.Forms.Label lblTimeout;
    }
}
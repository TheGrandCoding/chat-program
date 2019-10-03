namespace ChatProgram
{
    partial class Menu
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
            this.btnJoin = new System.Windows.Forms.Button();
            this.btnHost = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(16, 15);
            this.btnJoin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(236, 85);
            this.btnJoin.TabIndex = 0;
            this.btnJoin.Text = "Join a Text Server";
            this.btnJoin.UseVisualStyleBackColor = true;
            // 
            // btnHost
            // 
            this.btnHost.Location = new System.Drawing.Point(16, 107);
            this.btnHost.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHost.Name = "btnHost";
            this.btnHost.Size = new System.Drawing.Size(236, 85);
            this.btnHost.TabIndex = 1;
            this.btnHost.Text = "Host a Server";
            this.btnHost.UseVisualStyleBackColor = true;
            this.btnHost.Click += new System.EventHandler(this.btnHost_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 206);
            this.Controls.Add(this.btnHost);
            this.Controls.Add(this.btnJoin);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Menu";
            this.Text = "Chat Program";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnHost;
    }
}


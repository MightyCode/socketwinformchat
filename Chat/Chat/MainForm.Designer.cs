namespace Chat
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.labelMode = new System.Windows.Forms.Label();
            this.buttonServer = new System.Windows.Forms.Button();
            this.buttonConnection = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox
            // 
            this.richTextBox.Location = new System.Drawing.Point(51, 99);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(700, 354);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(51, 474);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(529, 20);
            this.textBoxMessage.TabIndex = 1;
            this.textBoxMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxMessage_KeyDown);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(632, 474);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(119, 21);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.Location = new System.Drawing.Point(374, 63);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(41, 13);
            this.labelMode.TabIndex = 3;
            this.labelMode.Text = "#Mode";
            this.labelMode.Visible = false;
            // 
            // buttonServer
            // 
            this.buttonServer.Location = new System.Drawing.Point(632, 12);
            this.buttonServer.Name = "buttonServer";
            this.buttonServer.Size = new System.Drawing.Size(119, 30);
            this.buttonServer.TabIndex = 4;
            this.buttonServer.Text = "Server";
            this.buttonServer.UseVisualStyleBackColor = true;
            this.buttonServer.Click += new System.EventHandler(this.buttonServer_Click);
            // 
            // buttonConnection
            // 
            this.buttonConnection.Location = new System.Drawing.Point(632, 54);
            this.buttonConnection.Name = "buttonConnection";
            this.buttonConnection.Size = new System.Drawing.Size(119, 30);
            this.buttonConnection.TabIndex = 5;
            this.buttonConnection.Text = "Connection";
            this.buttonConnection.UseVisualStyleBackColor = true;
            this.buttonConnection.Click += new System.EventHandler(this.buttonConnection_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 521);
            this.Controls.Add(this.buttonConnection);
            this.Controls.Add(this.buttonServer);
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.richTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label labelMode;
        private System.Windows.Forms.Button buttonServer;
        private System.Windows.Forms.Button buttonConnection;
    }
}


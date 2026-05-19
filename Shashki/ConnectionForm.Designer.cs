namespace Shashki
{
    partial class ConnectionForm
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
            this.StartOwnBtn = new System.Windows.Forms.Button();
            this.JoinGameBtn = new System.Windows.Forms.Button();
            this.IPmaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StartOwnBtn
            // 
            this.StartOwnBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartOwnBtn.Location = new System.Drawing.Point(33, 118);
            this.StartOwnBtn.Name = "StartOwnBtn";
            this.StartOwnBtn.Size = new System.Drawing.Size(135, 23);
            this.StartOwnBtn.TabIndex = 0;
            this.StartOwnBtn.Text = "Начать свою игру";
            this.StartOwnBtn.UseVisualStyleBackColor = true;
            this.StartOwnBtn.Click += new System.EventHandler(this.StartOwnBtn_Click);
            // 
            // JoinGameBtn
            // 
            this.JoinGameBtn.Enabled = false;
            this.JoinGameBtn.Location = new System.Drawing.Point(32, 70);
            this.JoinGameBtn.Name = "JoinGameBtn";
            this.JoinGameBtn.Size = new System.Drawing.Size(137, 23);
            this.JoinGameBtn.TabIndex = 1;
            this.JoinGameBtn.Text = "Присоединиться к игре";
            this.JoinGameBtn.UseVisualStyleBackColor = true;
            this.JoinGameBtn.Click += new System.EventHandler(this.JoinGameBtn_Click);
            // 
            // IPmaskedTextBox
            // 
            this.IPmaskedTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.IPmaskedTextBox.Location = new System.Drawing.Point(30, 35);
            this.IPmaskedTextBox.Mask = "990\\.990\\.990\\.990";
            this.IPmaskedTextBox.Name = "IPmaskedTextBox";
            this.IPmaskedTextBox.Size = new System.Drawing.Size(143, 29);
            this.IPmaskedTextBox.TabIndex = 4;
            this.IPmaskedTextBox.TextChanged += new System.EventHandler(this.IPmaskedTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Введите IP-адрес оппонента";
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point(43, 99);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(0, 13);
            this.InfoLabel.TabIndex = 6;
            // 
            // ConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 162);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IPmaskedTextBox);
            this.Controls.Add(this.JoinGameBtn);
            this.Controls.Add(this.StartOwnBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Соединение";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConnectionForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartOwnBtn;
        private System.Windows.Forms.Button JoinGameBtn;
        private System.Windows.Forms.MaskedTextBox IPmaskedTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label InfoLabel;
    }
}
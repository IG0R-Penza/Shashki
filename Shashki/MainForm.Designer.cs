namespace Shashki
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.BoardTable = new System.Windows.Forms.TableLayoutPanel();
            this.GameControlBtn = new System.Windows.Forms.Button();
            this.HodBtn = new System.Windows.Forms.Button();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BoardTable
            // 
            this.BoardTable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoardTable.ColumnCount = 8;
            this.BoardTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.Location = new System.Drawing.Point(52, 20);
            this.BoardTable.Margin = new System.Windows.Forms.Padding(0);
            this.BoardTable.MinimumSize = new System.Drawing.Size(400, 400);
            this.BoardTable.Name = "BoardTable";
            this.BoardTable.RowCount = 8;
            this.BoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.BoardTable.Size = new System.Drawing.Size(400, 400);
            this.BoardTable.TabIndex = 0;
            // 
            // GameControlBtn
            // 
            this.GameControlBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.GameControlBtn.Location = new System.Drawing.Point(52, 435);
            this.GameControlBtn.Name = "GameControlBtn";
            this.GameControlBtn.Size = new System.Drawing.Size(117, 34);
            this.GameControlBtn.TabIndex = 1;
            this.GameControlBtn.Text = "Начать игру";
            this.GameControlBtn.UseVisualStyleBackColor = true;
            // 
            // HodBtn
            // 
            this.HodBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.HodBtn.Location = new System.Drawing.Point(335, 435);
            this.HodBtn.Name = "HodBtn";
            this.HodBtn.Size = new System.Drawing.Size(117, 34);
            this.HodBtn.TabIndex = 2;
            this.HodBtn.Text = "Завершить ход";
            this.HodBtn.UseVisualStyleBackColor = true;
            // 
            // InfoLabel
            // 
            this.InfoLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point(190, 446);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(0, 13);
            this.InfoLabel.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 481);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.HodBtn);
            this.Controls.Add(this.GameControlBtn);
            this.Controls.Add(this.BoardTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(520, 520);
            this.Name = "MainForm";
            this.Text = "Шашки";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel BoardTable;
        private System.Windows.Forms.Button GameControlBtn;
        private System.Windows.Forms.Button HodBtn;
        private System.Windows.Forms.Label InfoLabel;
    }
}


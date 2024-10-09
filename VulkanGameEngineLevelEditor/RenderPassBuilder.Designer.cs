namespace VulkanGameEngineLevelEditor
{
    partial class RenderPassBuilder
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ImageType = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.ImageTypeBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(276, 1004);
            this.listBox1.TabIndex = 0;
            // 
            // ImageType
            // 
            this.ImageType.AutoSize = true;
            this.ImageType.Location = new System.Drawing.Point(332, 44);
            this.ImageType.Name = "ImageType";
            this.ImageType.Size = new System.Drawing.Size(58, 20);
            this.ImageType.TabIndex = 1;
            this.ImageType.Text = "Image:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 28);
            this.comboBox1.TabIndex = 2;
            // 
            // ImageTypeBox
            // 
            this.ImageTypeBox.FormattingEnabled = true;
            this.ImageTypeBox.Location = new System.Drawing.Point(520, 35);
            this.ImageTypeBox.Name = "ImageTypeBox";
            this.ImageTypeBox.Size = new System.Drawing.Size(265, 28);
            this.ImageTypeBox.TabIndex = 3;
            this.ImageTypeBox.SelectedIndexChanged += new System.EventHandler(this.ImageTypeBox_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(615, 323);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // RenderPassBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2225, 1038);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.ImageTypeBox);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.ImageType);
            this.Controls.Add(this.listBox1);
            this.Name = "RenderPassBuilder";
            this.Text = "RenderPassBuilder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label ImageType;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox ImageTypeBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
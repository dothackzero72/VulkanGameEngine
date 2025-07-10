using System.Windows.Forms;
using VulkanGameEngineLevelEditor.LevelEditor.EditorEnhancements;

namespace VulkanGameEngineLevelEditor.RenderPassWindows
{
    partial class RendererEditorForm
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RendererEditorForm));
            panel1 = new Panel();
            objectDataGridView1 = new LevelEditor.EditorEnhancements.ObjectDataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            contextMenuStrip1 = new ContextMenuStrip(components);
            AddRenderPassAttachment = new ToolStripMenuItem();
            addSubpassDependency = new ToolStripMenuItem();
            addGraphicsPipeline = new ToolStripMenuItem();
            toolStripTextBox1 = new ToolStripTextBox();
            pictureBox1 = new PictureBox();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            addRenderPassAttachmentToolStripMenuItem = new ToolStripMenuItem();
            addSubpassDependencyToolStripMenuItem = new ToolStripMenuItem();
            addGraphicsPipelineToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton2 = new ToolStripDropDownButton();
            saveComponentsToolStripMenuItem = new ToolStripMenuItem();
            saveTempleteToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton3 = new ToolStripDropDownButton();
            buildToolStripMenuItem = new ToolStripMenuItem();
            treeView1 = new TreeView();
            levelEditorTreeView1 = new LevelEditor.EditorEnhancements.LevelEditorTreeView();
            menuStrip1 = new MenuStrip();
            richTextBox1 = new RichTextBox();
            panel2 = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)objectDataGridView1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            toolStrip1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(objectDataGridView1);
            panel1.Location = new System.Drawing.Point(2215, 77);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(586, 1080);
            panel1.TabIndex = 1;
            // 
            // objectDataGridView1
            // 
            objectDataGridView1.AllowUserToAddRows = false;
            objectDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            objectDataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(30, 30, 30);
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            objectDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            objectDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            objectDataGridView1.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2 });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            objectDataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            objectDataGridView1.Dock = DockStyle.Fill;
            objectDataGridView1.Location = new System.Drawing.Point(0, 0);
            objectDataGridView1.Name = "objectDataGridView1";
            objectDataGridView1.RowHeadersVisible = false;
            objectDataGridView1.RowHeadersWidth = 62;
            objectDataGridView1.SelectedObject = null;
            objectDataGridView1.Size = new System.Drawing.Size(586, 1080);
            objectDataGridView1.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Property";
            dataGridViewTextBoxColumn1.MinimumWidth = 8;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Value";
            dataGridViewTextBoxColumn2.MinimumWidth = 8;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { AddRenderPassAttachment, addSubpassDependency, addGraphicsPipeline, toolStripTextBox1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(315, 135);
            // 
            // AddRenderPassAttachment
            // 
            AddRenderPassAttachment.Name = "AddRenderPassAttachment";
            AddRenderPassAttachment.Size = new System.Drawing.Size(314, 32);
            AddRenderPassAttachment.Text = "Add Render Pass Attachment";
            AddRenderPassAttachment.Click += addToolStripMenuItem_Click;
            // 
            // addSubpassDependency
            // 
            addSubpassDependency.Name = "addSubpassDependency";
            addSubpassDependency.Size = new System.Drawing.Size(314, 32);
            addSubpassDependency.Text = "Add Subpass Dependency";
            addSubpassDependency.Click += addSubpassDependency_Click;
            // 
            // addGraphicsPipeline
            // 
            addGraphicsPipeline.Name = "addGraphicsPipeline";
            addGraphicsPipeline.Size = new System.Drawing.Size(314, 32);
            addGraphicsPipeline.Text = "Add Graphics Pipeline";
            addGraphicsPipeline.Click += addGraphicsPipeline_Click;
            // 
            // toolStripTextBox1
            // 
            toolStripTextBox1.Name = "toolStripTextBox1";
            toolStripTextBox1.Size = new System.Drawing.Size(100, 31);
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new System.Drawing.Point(289, 41);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(1920, 1080);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton2, toolStripDropDownButton3 });
            toolStrip1.Location = new System.Drawing.Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(2806, 33);
            toolStrip1.TabIndex = 7;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { addRenderPassAttachmentToolStripMenuItem, addSubpassDependencyToolStripMenuItem, addGraphicsPipelineToolStripMenuItem });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(42, 28);
            toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // addRenderPassAttachmentToolStripMenuItem
            // 
            addRenderPassAttachmentToolStripMenuItem.Name = "addRenderPassAttachmentToolStripMenuItem";
            addRenderPassAttachmentToolStripMenuItem.Size = new System.Drawing.Size(344, 34);
            addRenderPassAttachmentToolStripMenuItem.Text = "Add Render Pass Attachment";
            // 
            // addSubpassDependencyToolStripMenuItem
            // 
            addSubpassDependencyToolStripMenuItem.Name = "addSubpassDependencyToolStripMenuItem";
            addSubpassDependencyToolStripMenuItem.Size = new System.Drawing.Size(344, 34);
            addSubpassDependencyToolStripMenuItem.Text = "Add Subpass Dependency";
            // 
            // addGraphicsPipelineToolStripMenuItem
            // 
            addGraphicsPipelineToolStripMenuItem.Name = "addGraphicsPipelineToolStripMenuItem";
            addGraphicsPipelineToolStripMenuItem.Size = new System.Drawing.Size(344, 34);
            addGraphicsPipelineToolStripMenuItem.Text = "Add Graphics Pipeline";
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { saveComponentsToolStripMenuItem, saveTempleteToolStripMenuItem });
            toolStripDropDownButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new System.Drawing.Size(42, 28);
            toolStripDropDownButton2.Text = "toolStripDropDownButton2";
            // 
            // saveComponentsToolStripMenuItem
            // 
            saveComponentsToolStripMenuItem.Name = "saveComponentsToolStripMenuItem";
            saveComponentsToolStripMenuItem.Size = new System.Drawing.Size(254, 34);
            saveComponentsToolStripMenuItem.Text = "SaveComponents";
            saveComponentsToolStripMenuItem.Click += saveComponentsToolStripMenuItem_Click;
            // 
            // saveTempleteToolStripMenuItem
            // 
            saveTempleteToolStripMenuItem.Name = "saveTempleteToolStripMenuItem";
            saveTempleteToolStripMenuItem.Size = new System.Drawing.Size(254, 34);
            saveTempleteToolStripMenuItem.Text = "SaveTemplete";
            saveTempleteToolStripMenuItem.Click += saveTempleteToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton3
            // 
            toolStripDropDownButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton3.DropDownItems.AddRange(new ToolStripItem[] { buildToolStripMenuItem });
            toolStripDropDownButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton3.Image");
            toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            toolStripDropDownButton3.Size = new System.Drawing.Size(42, 28);
            toolStripDropDownButton3.Text = "toolStripDropDownButton3";
            // 
            // buildToolStripMenuItem
            // 
            buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            buildToolStripMenuItem.Size = new System.Drawing.Size(153, 34);
            buildToolStripMenuItem.Text = "Build";
            buildToolStripMenuItem.Click += buildToolStripMenuItem_Click;
            // 
            // treeView1
            // 
            treeView1.Location = new System.Drawing.Point(165, 374);
            treeView1.Name = "treeView1";
            treeView1.Size = new System.Drawing.Size(1920, 1080);
            treeView1.TabIndex = 8;
            treeView1.AfterSelect += treeView1_AfterSelect_1;
            // 
            // levelEditorTreeView1
            // 
            levelEditorTreeView1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            levelEditorTreeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            levelEditorTreeView1.ForeColor = System.Drawing.Color.White;
            levelEditorTreeView1.Location = new System.Drawing.Point(9, 61);
            levelEditorTreeView1.Margin = new Padding(3, 4, 3, 4);
            levelEditorTreeView1.Name = "levelEditorTreeView1";
            levelEditorTreeView1.RootObject = null;
            levelEditorTreeView1.Size = new System.Drawing.Size(270, 1080);
            levelEditorTreeView1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(2806, 24);
            menuStrip1.TabIndex = 9;
            menuStrip1.Text = "menuStrip1";
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            richTextBox1.ForeColor = System.Drawing.Color.White;
            richTextBox1.Location = new System.Drawing.Point(-4, 36);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(2789, 260);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // panel2
            // 
            panel2.Controls.Add(richTextBox1);
            panel2.Location = new System.Drawing.Point(13, 1128);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(2788, 259);
            panel2.TabIndex = 3;
            // 
            // RendererEditorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            ClientSize = new System.Drawing.Size(2806, 1436);
            Controls.Add(levelEditorTreeView1);
            Controls.Add(treeView1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(pictureBox1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
            Name = "RendererEditorForm";
            Text = "RenderPassBuilder";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)objectDataGridView1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            contextMenuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel1;
        private RichTextBox RenderPassBuilderDebug;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem AddRenderPassAttachment;
        private ToolStripMenuItem addSubpassDependency;
        private ToolStripMenuItem addGraphicsPipeline;
        private PictureBox pictureBox1;
        private ObjectDataGridView objectDataGridView1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem addRenderPassAttachmentToolStripMenuItem;
        private ToolStripMenuItem addSubpassDependencyToolStripMenuItem;
        private ToolStripMenuItem addGraphicsPipelineToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripMenuItem saveComponentsToolStripMenuItem;
        private ToolStripMenuItem saveTempleteToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton3;
        private ToolStripMenuItem buildToolStripMenuItem;
        private TreeView treeView1;
        private LevelEditorTreeView levelEditorTreeView1;
        private MenuStrip menuStrip1;
        private RichTextBox richTextBox1;
        private Panel panel2;
    }
}
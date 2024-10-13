using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor.RenderPassWindows
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
            this.components = new System.ComponentModel.Container();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.RenderPassBuilderDebug = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BuildButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddRenderPassAttachment = new System.Windows.Forms.ToolStripMenuItem();
            this.addSubpassDependency = new System.Windows.Forms.ToolStripMenuItem();
            this.addGraphicsPipeline = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveTemplete = new System.Windows.Forms.Button();
            this.SaveComponents = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(13, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(243, 864);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.propertyGrid1);
            this.panel1.Location = new System.Drawing.Point(1142, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(527, 821);
            this.panel1.TabIndex = 1;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(527, 821);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.Click += new System.EventHandler(this.propertyGrid1_Click_1);
            // 
            // RenderPassBuilderDebug
            // 
            this.RenderPassBuilderDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPassBuilderDebug.Location = new System.Drawing.Point(0, 0);
            this.RenderPassBuilderDebug.Name = "RenderPassBuilderDebug";
            this.RenderPassBuilderDebug.Size = new System.Drawing.Size(1657, 207);
            this.RenderPassBuilderDebug.TabIndex = 2;
            this.RenderPassBuilderDebug.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.RenderPassBuilderDebug);
            this.panel2.Location = new System.Drawing.Point(12, 902);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1657, 207);
            this.panel2.TabIndex = 3;
            // 
            // BuildButton
            // 
            this.BuildButton.Location = new System.Drawing.Point(1585, 850);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(75, 27);
            this.BuildButton.TabIndex = 3;
            this.BuildButton.Text = "Build";
            this.BuildButton.UseVisualStyleBackColor = true;
            this.BuildButton.Click += new System.EventHandler(this.BuildButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddRenderPassAttachment,
            this.addSubpassDependency,
            this.addGraphicsPipeline});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(315, 100);
            // 
            // AddRenderPassAttachment
            // 
            this.AddRenderPassAttachment.Name = "AddRenderPassAttachment";
            this.AddRenderPassAttachment.Size = new System.Drawing.Size(314, 32);
            this.AddRenderPassAttachment.Text = "Add Render Pass Attachment";
            this.AddRenderPassAttachment.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // addSubpassDependency
            // 
            this.addSubpassDependency.Name = "addSubpassDependency";
            this.addSubpassDependency.Size = new System.Drawing.Size(314, 32);
            this.addSubpassDependency.Text = "Add Subpass Dependency";
            this.addSubpassDependency.Click += new System.EventHandler(this.addSubpassDependency_Click);
            // 
            // addGraphicsPipeline
            // 
            this.addGraphicsPipeline.Name = "addGraphicsPipeline";
            this.addGraphicsPipeline.Size = new System.Drawing.Size(314, 32);
            this.addGraphicsPipeline.Text = "Add Graphics Pipeline";
            this.addGraphicsPipeline.Click += new System.EventHandler(this.addGraphicsPipeline_Click);
            // 
            // SaveTemplete
            // 
            this.SaveTemplete.Location = new System.Drawing.Point(1430, 850);
            this.SaveTemplete.Name = "SaveTemplete";
            this.SaveTemplete.Size = new System.Drawing.Size(123, 27);
            this.SaveTemplete.TabIndex = 4;
            this.SaveTemplete.Text = "Save Templete";
            this.SaveTemplete.UseVisualStyleBackColor = true;
            this.SaveTemplete.Click += new System.EventHandler(this.SaveTemplete_Click);
            // 
            // SaveComponents
            // 
            this.SaveComponents.Location = new System.Drawing.Point(1228, 850);
            this.SaveComponents.Name = "SaveComponents";
            this.SaveComponents.Size = new System.Drawing.Size(172, 27);
            this.SaveComponents.TabIndex = 5;
            this.SaveComponents.Text = "Save Components";
            this.SaveComponents.UseVisualStyleBackColor = true;
            this.SaveComponents.Click += new System.EventHandler(this.SaveComponents_Click);
            // 
            // RenderPassBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1681, 1110);
            this.Controls.Add(this.SaveComponents);
            this.Controls.Add(this.SaveTemplete);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listBox1);
            this.Name = "RenderPassBuilder";
            this.Text = "RenderPassBuilder";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox listBox1;
        private Panel panel1;
        private PropertyGrid propertyGrid1;
        private RichTextBox RenderPassBuilderDebug;
        private Panel panel2;
        private Button BuildButton;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem AddRenderPassAttachment;
        private ToolStripMenuItem addSubpassDependency;
        private ToolStripMenuItem addGraphicsPipeline;
        private Button SaveTemplete;
        private Button SaveComponents;
    }
}
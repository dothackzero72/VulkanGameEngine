using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.LevelEditor.EditorEnhancements;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.RenderPassWindows
{
    public partial class RendererEditorForm : Form
    {
        private const string filePath = "..\\..\\..\\RenderPass\\";
        public ivec2 SwapChainResuloution { get; set; } = new ivec2();
        public MessengerModel RenderPassMessager { get; set; }
        public RenderPassBuildInfoModel RenderPassModels { get; set; } = new RenderPassBuildInfoModel();
        private ObjectDataGridView propertyGrid;
        private LevelEditorTreeView levelEditorTreeView;
        public RendererEditorForm()
        {
            InitializeComponent();

            // Initialize TreeView
            levelEditorTreeView = new LevelEditorTreeView
            {
                Location = new System.Drawing.Point(13, 41),
                Size = new System.Drawing.Size(270, this.ClientSize.Height - 51) // Dynamic height
            };

            // Initialize PropertyGrid
            propertyGrid = new ObjectDataGridView
            {
                Location = new Point(290, 41),
                Size = new Size(this.ClientSize.Width - 303, this.ClientSize.Height - 51) // Dynamic width
            };

            // Buttons
            var saveButton = new System.Windows.Forms.Button { Text = "Save", Location = new Point(13, 10), Size = new Size(80, 30) };
            var undoButton = new System.Windows.Forms.Button { Text = "Undo", Location = new Point(100, 10), Size = new Size(80, 30) };
            var redoButton = new System.Windows.Forms.Button { Text = "Redo", Location = new Point(190, 10), Size = new Size(80, 30) };
            var addPipelineButton = new System.Windows.Forms.Button { Text = "Add Pipeline", Location = new Point(290, 10), Size = new Size(80, 30) };
            saveButton.Click += (s, e) => propertyGrid.SaveToJson();
            undoButton.Click += (s, e) => propertyGrid.Undo();
            redoButton.Click += (s, e) => propertyGrid.Redo();
            addPipelineButton.Click += (s, e) => levelEditorTreeView.AddRenderPipeline();

            // Connect TreeView to Grid
            var renderPassBuildInfo = new RenderPassBuildInfoModel(); // Load or create your model
            levelEditorTreeView.RootObject = renderPassBuildInfo;
            levelEditorTreeView.SelectionChanged += obj =>
            {
                if (obj != null)
                {
                    propertyGrid.SelectedObject = obj;
                }
            };

            // Add controls to form
            Controls.AddRange(new Control[] { levelEditorTreeView, propertyGrid, saveButton, undoButton, redoButton, addPipelineButton });

            // Load data (example)
            try
            {
                propertyGrid.LoadFromJson<RenderPassBuildInfoModel>("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\RenderPass\\LevelShader2DRenderPass.json");
                levelEditorTreeView.RootObject = propertyGrid.SelectedObject; // Sync initial state
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load JSON: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Resize event to handle form resizing
            this.Resize += (s, e) =>
            {
                levelEditorTreeView.Size = new Size(270, this.ClientSize.Height - 51);
                propertyGrid.Size = new Size(this.ClientSize.Width - 303, this.ClientSize.Height - 51);
            };
        }
        

        public void ShowRenderPassBuilder()
        {
            using (RendererEditorForm popup = new RendererEditorForm())
            {
                DialogResult result = popup.ShowDialog();

                if (result == DialogResult.OK)
                {
                    MessageBox.Show("Popup closed.");
                }
            }
        }
        private void ImageTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  var asdf = (ImageType)Enum.Parse(typeof(ImageType), ImageTypeBox.SelectedItem.ToString());
            //   var asd = ImageTypeBox.SelectedItem.ToString();
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void Accept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            GlobalMessenger.messenger.Remove(RenderPassMessager);
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (objectTreeView.SelectedItem is VkImageCreateInfo selectedPerson2)
            //{
            //    objectDataGridView1.SelectedObject = selectedPerson2;
            //}
            //if (objectTreeView.SelectedItem is VkSubpassDependency subpass)
            //{
            //    objectDataGridView1.SelectedObject = subpass;
            //}
            //if (objectTreeView.SelectedItem is VkSamplerCreateInfo samplerCreateInfo)
            //{
            //    objectDataGridView1.SelectedObject = samplerCreateInfo;
            //}
            //if (objectTreeView.SelectedItem is VkAttachmentDescription attachmentDescription)
            //{
            //    objectDataGridView1.SelectedObject = attachmentDescription;
            //}
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            // buildRenderPass = new JsonRenderPass<Vertex2D>();
            // buildRenderPass.CreateRenderPass();
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            GlobalMessenger.messenger.Remove(RenderPassMessager);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var attachment = new ImageCreateInfoModel($"TextureAttachment{AttachmentList.Count}", SwapChainResuloution, Format.R8G8B8A8Unorm);
            //AttachmentList.Add(attachment);
            //listBox1.Items.Add(attachment);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void addSubpassDependency_Click(object sender, EventArgs e)
        {
            //var subpassDependency = new SubpassDependencyModel();
            //SubpassDependencyList.Add(subpassDependency);
            //listBox1.Items.Add(subpassDependency);
        }

        private void addGraphicsPipeline_Click(object sender, EventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void propertyGrid1_Click_1(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {


        }

        private void SaveTemplete_Click(object sender, EventArgs e)
        {
            RenderPassEditorBaseModel obj = (RenderPassEditorBaseModel)objectDataGridView1.SelectedObject;

            string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\DefaultRenderPass.json";
            string jsonString = JsonConvert.SerializeObject(RenderPassModels, Formatting.Indented);
            File.WriteAllText(finalfilePath, jsonString);

        }

        private void SaveComponents_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void buildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // buildRenderPass = new JsonRenderPass<Vertex2D>();
            // buildRenderPass.CreateRenderPass();
        }

        private void saveComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //object obj = objectDataGridView1.SelectedObject;
            //if (objectTreeView.SelectedItem is VkSubpassDependencyModel subpass)
            //{
            //    subpass.SaveJsonComponent();
            //}
            //if (objectTreeView.SelectedItem is VkImageCreateInfoModel imageCreateInfo)
            //{
            //    imageCreateInfo.SaveJsonComponent();
            //}
            //if (objectTreeView.SelectedItem is VkSamplerCreateInfoModel samplerCreateInfo)
            //{
            //    samplerCreateInfo.SaveJsonComponent();
            //}
            //if (objectTreeView.SelectedItem is VkAttachmentDescriptionModel attachment)
            //{
            //    attachment.SaveJsonComponent();
            //}
        }

        private void saveTempleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderPassEditorBaseModel obj = (RenderPassEditorBaseModel)objectDataGridView1.SelectedObject;

            string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\DefaultRenderPass.json";
            string jsonString = JsonConvert.SerializeObject(RenderPassModels, Formatting.Indented);
            File.WriteAllText(finalfilePath, jsonString);
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }
    }
}

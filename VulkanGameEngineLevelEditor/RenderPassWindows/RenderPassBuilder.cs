using GlmSharp;
using Silk.NET.Core;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.RenderPassWindows
{
    public partial class RenderPassBuilder : Form
    {
        public ivec2 SwapChainResuloution { get; set; } = new ivec2();
        public MessengerModel RenderPassMessager { get; set; }
        public List<RenderPipeline> RenderPipelineList { get; set; } = new List<RenderPipeline>();
        public List<RenderedTextureInfoModel> AttachmentList { get; set; } = new List<RenderedTextureInfoModel>();
        public List<SubpassDependencyModel> SubpassDependencyList { get; set; } = new List<SubpassDependencyModel>();
        public BuildRenderPass buildRenderPass { get; set; } = new BuildRenderPass();
        public RenderPassBuilder()
        {
            InitializeComponent();
            this.FormClosing += OnClose;

            RenderPassMessager = new MessengerModel()
            {
                IsActive = true,
                richTextBox = RenderPassBuilderDebug,
                TextBoxName = RenderPassBuilderDebug.Name
            };

            GlobalMessenger.AddMessenger(RenderPassMessager);
            listBox1.Items.Add(SwapChainResuloution);

            AttachmentList.Add(new RenderedTextureInfoModel("a"));
            AttachmentList.Add(new RenderedTextureInfoModel("b"));
            SubpassDependencyList.Add(new SubpassDependencyModel("C"));
            SubpassDependencyList.Add(new SubpassDependencyModel("D"));

            listBox1.Items.Add(AttachmentList[0]);
            listBox1.Items.Add(AttachmentList[1]);
            listBox1.Items.Add(SubpassDependencyList[0]);
            listBox1.Items.Add(SubpassDependencyList[1]);



            //// Add columns to the ListView
            //listView1.Columns.Add("Item Name", 100);
            //listView1.Columns.Add("Quantity", 70);
            //listView1.Columns.Add("Price", 70);

            //// Add items to the ListView
            //ListViewItem item1 = new ListViewItem("Apple");
            //item1.SubItems.Add("10");
            //item1.SubItems.Add("$1.00");
            //listView1.Items.Add(item1);

            //ListViewItem item2 = new ListViewItem("Banana");
            //item2.SubItems.Add("5");
            //item2.SubItems.Add("$0.50");
            //listView1.Items.Add(item2);

            //ListViewItem item3 = new ListViewItem("Orange");
            //item3.SubItems.Add("20");
            //item3.SubItems.Add("$0.75");
            //listView1.Items.Add(item3);
        }

        public void ShowRenderPassBuilder()
        {
            using (RenderPassBuilder popup = new RenderPassBuilder())
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
            if (listBox1.SelectedItem is RenderedTextureInfoModel selectedPerson)
            {
                // Get type information
                Type type = selectedPerson.GetType();
                var a = listBox1.SelectedIndex;
                propertyGrid1.SelectedObject = selectedPerson;
            }
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            RenderPassMessager.IsActive = true;
            var otherMessangers = GlobalMessenger.messenger.Where(p => p.richTextBox != RenderPassMessager.richTextBox);
            foreach(var messengers in otherMessangers)
            {
                messengers.IsActive = false;
            }

            buildRenderPass.CreateRenderPass(new RenderPassModel()
            {
                AttachmentList = AttachmentList,
                RenderPipelineList = RenderPipelineList,
                SubpassDependencyList = SubpassDependencyList,
                SwapChainResuloution = SwapChainResuloution
            });

            foreach (var messengers in otherMessangers)
            {
                messengers.IsActive = true;
            }
            RenderPassMessager.IsActive = false;
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            GlobalMessenger.messenger.Remove(RenderPassMessager);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var attachment = new RenderedTextureInfoModel($"TextureAttachment{AttachmentList.Count}");
            AttachmentList.Add(attachment);
            listBox1.Items.Add(attachment);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void addSubpassDependency_Click(object sender, EventArgs e)
        {
            var subpassDependency = new SubpassDependencyModel($"SubpassDependency{AttachmentList.Count}");
            SubpassDependencyList.Add(subpassDependency);
            listBox1.Items.Add(subpassDependency);
        }

        private void addGraphicsPipeline_Click(object sender, EventArgs e)
        {
            //var subpassDependency = new RenderPipeline($"SubpassDependency{AttachmentList.Count}");
            //SubpassDependencyList.Add(subpassDependency);
            //listBox1.Items.Add(subpassDependency);
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
    }
}

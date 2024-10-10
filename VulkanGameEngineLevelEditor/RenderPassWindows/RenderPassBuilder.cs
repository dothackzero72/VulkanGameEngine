using GlmSharp;
using Silk.NET.Core;
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
            var a = listBox1.SelectedIndex;
            propertyGrid1.SelectedObject = AttachmentList;
     
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            RenderPassMessager.IsActive = true;
            var otherMessangers = GlobalMessenger.messenger.Where(p => p.richTextBox != RenderPassMessager.richTextBox);
            foreach(var messengers in otherMessangers)
            {
                messengers.IsActive = false;
            }

            RenderPassModel renderPass = new RenderPassModel()
            {
                AttachmentList = AttachmentList,
                RenderPipelineList = RenderPipelineList,
                SubpassDependencyList = SubpassDependencyList,
                SwapChainResuloution = SwapChainResuloution
            };

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
    }
}

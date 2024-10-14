using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.Core;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.RenderPassWindows
{
    public partial class RenderPassBuilder : Form
    {
        private const string filePath = "..\\..\\..\\RenderPass\\";
        public ivec2 SwapChainResuloution { get; set; } = new ivec2();
        public MessengerModel RenderPassMessager { get; set; }
        public List<RenderPipeline> RenderPipelineList { get; set; } = new List<RenderPipeline>();
        public List<ImageCreateInfoModel> AttachmentList { get; set; } = new List<ImageCreateInfoModel>();
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

            SwapChainResuloution = new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height);

            GlobalMessenger.AddMessenger(RenderPassMessager);
            listBox1.Items.Add(SwapChainResuloution);

            AttachmentList.Add(new ImageCreateInfoModel("a", SwapChainResuloution, Format.R8G8B8A8Unorm));
            AttachmentList.Add(new ImageCreateInfoModel("b", SwapChainResuloution, Format.R8G8B8A8Unorm));
            SubpassDependencyList.Add(new SubpassDependencyModel("C"));
            SubpassDependencyList.Add(new SubpassDependencyModel("D"));

            listBox1.Items.Add(AttachmentList[0]);
            listBox1.Items.Add(AttachmentList[1]);
            listBox1.Items.Add(SubpassDependencyList[0]);
            listBox1.Items.Add(SubpassDependencyList[1]);
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
            if (listBox1.SelectedItem is ImageCreateInfoModel selectedPerson2)
            {
                Type type = selectedPerson2.GetType();
                var a = listBox1.SelectedIndex;
                propertyGrid1.SelectedObject = selectedPerson2;
            }
            if (listBox1.SelectedItem is SubpassDependencyModel subpass)
            {
                Type type = subpass.GetType();
                var a = listBox1.SelectedIndex;
                propertyGrid1.SelectedObject = subpass;
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

            //buildRenderPass.CreateRenderPass(new RenderPassModel()
            //{
            //    AttachmentList = AttachmentList,
            //    RenderPipelineList = RenderPipelineList,
            //    SubpassDependencyList = SubpassDependencyList,
            //    SwapChainResuloution = SwapChainResuloution
            //});

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
            var attachment = new ImageCreateInfoModel($"TextureAttachment{AttachmentList.Count}", SwapChainResuloution, Format.R8G8B8A8Unorm);
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
            if (listBox1.SelectedItem is RenderedTextureInfoModel selectedPerson)
            {
                // Get type information
                Type type = selectedPerson.GetType();
                var a = listBox1.SelectedIndex;
                propertyGrid1.SelectedObject = selectedPerson;
            }


        }

        private void SaveTemplete_Click(object sender, EventArgs e)
        {
            RenderPassEditorBaseModel obj = (RenderPassEditorBaseModel)propertyGrid1.SelectedObject;
            //if (listBox1.SelectedItem is RenderedTextureInfoModel selectedPerson)
            //{
            //    string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\RenderedTextureInfoModel\aaaaa.json";
            //    string jsonString = JsonConvert.SerializeObject(AttachmentList[0].AttachmentDescription.ConvertToVulkan(), Formatting.Indented);
            //    File.WriteAllText(finalfilePath, jsonString);
            //}
        
        }

        private void SaveComponents_Click(object sender, EventArgs e)
        {
            object obj = propertyGrid1.SelectedObject;
            if (listBox1.SelectedItem is RenderedTextureInfoModel renderedTextureInfo)
            {
                string finalfilePath = @$"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\RenderedTextureInfoModel\{renderedTextureInfo.ImageCreateInfo._name}.json";
                string jsonString = JsonConvert.SerializeObject(renderedTextureInfo.ImageCreateInfo, Formatting.Indented);
                File.WriteAllText(finalfilePath, jsonString);
            }
            if (listBox1.SelectedItem is ImageCreateInfoModel imageCreateInfo)
            {
                string finalfilePath = @$"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\RenderedTextureInfoModel\{imageCreateInfo._name}.json";
                string jsonString = JsonConvert.SerializeObject(imageCreateInfo, Formatting.Indented);
                File.WriteAllText(finalfilePath, jsonString);
            }
            if (listBox1.SelectedItem is SubpassDependencyModel subpass)
            {
                string finalfilePath = @$"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\RenderedTextureInfoModel\{subpass._name}.json";
                string jsonString = JsonConvert.SerializeObject(subpass, Formatting.Indented);
                File.WriteAllText(finalfilePath, jsonString);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

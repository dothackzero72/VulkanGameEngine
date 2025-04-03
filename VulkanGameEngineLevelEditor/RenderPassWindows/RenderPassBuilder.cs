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
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.RenderPassWindows
{
    public partial class RenderPassBuilder : Form
    {
        private const string filePath = "..\\..\\..\\RenderPass\\";
        public ivec2 SwapChainResuloution { get; set; } = new ivec2();
        public MessengerModel RenderPassMessager { get; set; }
        public RenderPassBuildInfoModel RenderPassModels { get; set; } = new RenderPassBuildInfoModel();
        public JsonRenderPass<Vertex2D> buildRenderPass { get; set; }
        public RenderPassBuilder()
        {
            InitializeComponent();
            this.FormClosing += OnClose;

            SwapChainResuloution = new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height);

            RenderPassMessager = new MessengerModel()
            {
                IsActive = true,
                richTextBox = RenderPassBuilderDebug,
                TextBoxName = RenderPassBuilderDebug.Name,
                ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId,
            };
           // GlobalMessenger.AddMessenger(RenderPassMessager);

            RenderPassModels = new RenderPassBuildInfoModel
            {
                _name = "BasicRenderPass",
                //RenderedTextureInfoModelList = new List<RenderedTextureInfoModel>
                //{
                //    new RenderedTextureInfoModel()
                //    {
                //        IsRenderedToSwapchain = true,
                //        RenderedTextureInfoName = "ColorRenderTexture",
                //        AttachmentDescription = new VkAttachmentDescriptionModel(ConstConfig.DefaultColorAttachmentDescriptionModel),
                //        ImageCreateInfo = new VkImageCreateInfoModel(ConstConfig.DefaultCreateColorImageInfo, SwapChainResuloution, VkFormat.VK_FORMAT_R8G8B8A8_UNORM),
                //        SamplerCreateInfo = new VkSamplerCreateInfoModel(ConstConfig.DefaultColorSamplerCreateInfo),
                //        TextureType = RenderedTextureType.ColorRenderedTexture
                //    },
                //    new RenderedTextureInfoModel()
                //    {
                //        IsRenderedToSwapchain = false,
                //        RenderedTextureInfoName = "DepthRenderedTexture",
                //        AttachmentDescription = new VkAttachmentDescriptionModel(ConstConfig.DefaultDepthAttachmentDescriptionModel),
                //        ImageCreateInfo = new VkImageCreateInfoModel(ConstConfig.DefaultCreateDepthImageInfo, SwapChainResuloution, VkFormat.VK_FORMAT_D32_SFLOAT),
                //        SamplerCreateInfo = new VkSamplerCreateInfoModel(ConstConfig.DefaultDepthSamplerCreateInfo),
                //        TextureType = RenderedTextureType.DepthRenderedTexture
                //    }
                //},
                //SubpassDependencyList = new List<VkSubpassDependencyModel>() { new VkSubpassDependencyModel(ConstConfig.DefaultSubpassDependencyModel) },
            };

            //listBox1.Items.Add(RenderPassModels.RenderedTextureInfoModelList[0].AttachmentDescription);
            ////listBox1.Items.Add(RenderPassModels.RenderedTextureInfoModelList[1].AttachmentDescription);
            //listBox1.Items.Add(RenderPassModels.RenderedTextureInfoModelList[0].ImageCreateInfo);
            ////listBox1.Items.Add(RenderPassModels.RenderedTextureInfoModelList[1].ImageCreateInfo);
            //listBox1.Items.Add(RenderPassModels.RenderedTextureInfoModelList[0].SamplerCreateInfo);
            ////listBox1.Items.Add(RenderPassModels.RenderedTextureInfoModelList[1].SamplerCreateInfo);
            //listBox1.Items.Add(RenderPassModels.SubpassDependencyList[0]);
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
            if (listBox1.SelectedItem is VkImageCreateInfo selectedPerson2)
            {
                propertyGrid1.SelectedObject = selectedPerson2;
            }
            if (listBox1.SelectedItem is VkSubpassDependency subpass)
            {
                propertyGrid1.SelectedObject = subpass;
            }
            if (listBox1.SelectedItem is VkSamplerCreateInfo samplerCreateInfo)
            {
                propertyGrid1.SelectedObject = samplerCreateInfo;
            }
            if (listBox1.SelectedItem is VkAttachmentDescription attachmentDescription)
            {
                propertyGrid1.SelectedObject = attachmentDescription;
            }
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            buildRenderPass = new JsonRenderPass<Vertex2D>();
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
            RenderPassEditorBaseModel obj = (RenderPassEditorBaseModel)propertyGrid1.SelectedObject;
           
                string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\DefaultRenderPass.json";
                string jsonString = JsonConvert.SerializeObject(RenderPassModels, Formatting.Indented);
                File.WriteAllText(finalfilePath, jsonString);
           
        }

        private void SaveComponents_Click(object sender, EventArgs e)
        {
            object obj = propertyGrid1.SelectedObject;
            if (listBox1.SelectedItem is VkSubpassDependencyModel subpass)
            {
                subpass.SaveJsonComponent();
            }
            if (listBox1.SelectedItem is VkImageCreateInfoModel imageCreateInfo)
            {
                imageCreateInfo.SaveJsonComponent();
            }
            if (listBox1.SelectedItem is VkSamplerCreateInfoModel samplerCreateInfo)
            {
                samplerCreateInfo.SaveJsonComponent();
            }
            if (listBox1.SelectedItem is VkAttachmentDescriptionModel attachment)
            {
                attachment.SaveJsonComponent();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

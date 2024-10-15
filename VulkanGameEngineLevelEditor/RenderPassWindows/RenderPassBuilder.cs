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
        public List<RenderPipeline> RenderPipelineList { get; set; } = new List<RenderPipeline>();
        public List<SamplerCreateInfoModel> SamplerCreateInfoList { get; set; } = new List<SamplerCreateInfoModel>();
        public List<AttachmentDescriptionModel> AttachmentList { get; set; } = new List<AttachmentDescriptionModel>();
        public List<ImageCreateInfoModel> ImageCreateInfoList { get; set; } = new List<ImageCreateInfoModel>();
        public List<SubpassDependencyModel> SubpassDependencyList { get; set; } = new List<SubpassDependencyModel>();
        public List<RenderedTextureInfoModel> renderedTextureInfoModelList { get; set; } = new List<RenderedTextureInfoModel> { };
        public RenderPassModel RenderPassModels { get; set; } = new RenderPassModel();
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

            AttachmentList.Add(new AttachmentDescriptionModel(RenderPassEditorConsts.DefaultColorAttachmentDescriptionModel));
            AttachmentList.Add(new AttachmentDescriptionModel(RenderPassEditorConsts.DefaultDepthAttachmentDescriptionModel));
            ImageCreateInfoList.Add(new ImageCreateInfoModel(RenderPassEditorConsts.DefaultCreateColorImageInfo, SwapChainResuloution, Format.R8G8B8A8Unorm));
            ImageCreateInfoList.Add(new ImageCreateInfoModel(RenderPassEditorConsts.DefaultCreateDepthImageInfo, SwapChainResuloution, Format.R8G8B8A8Unorm));
            SamplerCreateInfoList.Add(new SamplerCreateInfoModel(RenderPassEditorConsts.DefaultColorSamplerCreateInfo));
            SamplerCreateInfoList.Add(new SamplerCreateInfoModel(RenderPassEditorConsts.DefaultDepthSamplerCreateInfo));
            SubpassDependencyList.Add(new SubpassDependencyModel(RenderPassEditorConsts.DefaultSubpassDependencyModel));

            listBox1.Items.Add(AttachmentList[0]);
            listBox1.Items.Add(AttachmentList[1]);
            listBox1.Items.Add(SamplerCreateInfoList[0]);
            listBox1.Items.Add(SamplerCreateInfoList[1]);
            listBox1.Items.Add(SubpassDependencyList[0]);

            renderedTextureInfoModelList = new List<RenderedTextureInfoModel>
            {
                new RenderedTextureInfoModel()
                {
                    RenderedTextureInfoName = "ColorRenderTexture",
                    AttachmentDescription = new AttachmentDescriptionModel(RenderPassEditorConsts.DefaultColorAttachmentDescriptionModel),
                    ImageCreateInfo = new ImageCreateInfoModel(RenderPassEditorConsts.DefaultCreateColorImageInfo, SwapChainResuloution, Format.R8G8B8A8Unorm),
                    SamplerCreateInfo = new SamplerCreateInfoModel(RenderPassEditorConsts.DefaultColorSamplerCreateInfo),
                    TextureType = RenderedTextureType.ColorRenderedTexture
                },
                new RenderedTextureInfoModel()
                {
                    RenderedTextureInfoName = "DepthRenderedTexture",
                    AttachmentDescription = new AttachmentDescriptionModel(RenderPassEditorConsts.DefaultColorAttachmentDescriptionModel),
                    ImageCreateInfo = new ImageCreateInfoModel(RenderPassEditorConsts.DefaultCreateColorImageInfo, SwapChainResuloution, Format.R8G8B8A8Unorm),
                    SamplerCreateInfo = new SamplerCreateInfoModel(RenderPassEditorConsts.DefaultColorSamplerCreateInfo),
                    TextureType = RenderedTextureType.DepthRenderedTexture
                } 
            };

            RenderPassModels = new RenderPassModel
            {
                _name = "BasicRenderPass",
                RenderedTextureInfoModelList = { renderedTextureInfoModelList[0], renderedTextureInfoModelList[1] },
                SubpassDependencyList = SubpassDependencyList,
                SwapChainResuloution = SwapChainResuloution
            };
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
            if (listBox1.SelectedItem is ImageCreateInfoModel selectedPerson2)
            {
                propertyGrid1.SelectedObject = selectedPerson2;
            }
            if (listBox1.SelectedItem is SubpassDependencyModel subpass)
            {
                propertyGrid1.SelectedObject = subpass;
            }
            if (listBox1.SelectedItem is SamplerCreateInfoModel samplerCreateInfo)
            {
                propertyGrid1.SelectedObject = samplerCreateInfo;
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

            buildRenderPass.CreateRenderPass(RenderPassModels);

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
            //var attachment = new ImageCreateInfoModel($"TextureAttachment{AttachmentList.Count}", SwapChainResuloution, Format.R8G8B8A8Unorm);
            //AttachmentList.Add(attachment);
            //listBox1.Items.Add(attachment);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void addSubpassDependency_Click(object sender, EventArgs e)
        {
            var subpassDependency = new SubpassDependencyModel();
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
            if (listBox1.SelectedItem is SubpassDependencyModel subpass)
            {
                string finalfilePath = @$"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\RenderedTextureInfoModel\{subpass._name}.json";
                string jsonString = JsonConvert.SerializeObject(subpass, Formatting.Indented);
                File.WriteAllText(finalfilePath, jsonString);
            }
            if (listBox1.SelectedItem is ImageCreateInfoModel imageCreateInfo)
            {
                string finalfilePath = @$"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\RenderedTextureInfoModel\{imageCreateInfo._name}.json";
                string jsonString = JsonConvert.SerializeObject(imageCreateInfo, Formatting.Indented);
                File.WriteAllText(finalfilePath, jsonString);
            }
            if (listBox1.SelectedItem is SamplerCreateInfoModel samplerCreateInfo)
            {
                string finalfilePath = @$"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\RenderedTextureInfoModel\{samplerCreateInfo._name}.json";
                string jsonString = JsonConvert.SerializeObject(samplerCreateInfo, Formatting.Indented);
                File.WriteAllText(finalfilePath, jsonString);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

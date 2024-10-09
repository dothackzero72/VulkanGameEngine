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
        public RenderedTextureInfoModel RenderedTexture { get; set; } = new RenderedTextureInfoModel();
        public RenderPassBuilder()
        {
            InitializeComponent();
            var imageCreateInfo = new RenderedTextureInfoModel
            {
                TextureType = RenderedTextureType.ColorRenderedTexture,
                ImageCreateInfo = new ImageCreateInfoModel()
                {
                    Flags = ImageCreateFlags.None,
                    ImageType = Silk.NET.Vulkan.ImageType.Type2D,
                    Format = Format.Undefined,
                    Extent = new Extent3DModel { Width = 256, Height = 256, Depth = 1 },
                    MipLevels = 1,
                    ArrayLayers = 1,
                    Samples = SampleCountFlags.SampleCount1Bit,
                    Tiling = ImageTiling.Linear,
                    Usage = ImageUsageFlags.None,
                    SharingMode = SharingMode.Exclusive,
                    InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined
                },
                SamplerCreateInfo = new SamplerCreateInfoModel()
                {
                    Flags = 0,
                    MagFilter = Filter.Linear,
                    MinFilter = Filter.Linear,
                    MipmapMode = SamplerMipmapMode.Linear,
                    AddressModeU = SamplerAddressMode.Repeat,
                    AddressModeV = SamplerAddressMode.Repeat,
                    AddressModeW = SamplerAddressMode.Repeat,
                    MipLodBias = 0.0f,
                    AnisotropyEnable = Vk.False,
                    MaxAnisotropy = 1.0f,
                    CompareEnable = Vk.False,
                    CompareOp = CompareOp.Always,
                    MinLod = 0.0f,
                    MaxLod = float.MaxValue,
                    BorderColor = BorderColor.FloatTransparentBlack,
                    UnnormalizedCoordinates = Vk.False
                }
            };
            propertyGrid1.SelectedObject = imageCreateInfo;
            
        }

        public void ShowRenderPassBuilder()
        {
            using (RenderPassBuilder popup = new RenderPassBuilder())
            {
                DialogResult result = popup.ShowDialog();
               
                if (result == DialogResult.OK)
                {
                    // Handle the close result if necessary
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
            this.Close();
        }
    }
}

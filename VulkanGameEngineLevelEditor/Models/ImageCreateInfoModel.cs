using Silk.NET.Core.Attributes;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public unsafe class ImageCreateInfoModel
    {

        [Category("Image Properties")]
        public ImageCreateFlags Flags { get; set; }

        [Category("Image Properties")]
        public ImageType ImageType { get; set; }

        [Category("Image Properties")]
        public Format Format { get; set; }

        [Category("Image Properties")]
        public Extent3DModel Extent { get; set; } = new Extent3DModel();

        [Category("Image Properties")]
        public uint MipLevels { get; set; }

        [Category("Image Properties")]
        public uint ArrayLayers { get; set; }

        [Category("Image Properties")]
        public SampleCountFlags Samples { get; set; }

        [Category("Image Properties")]
        public ImageTiling Tiling { get; set; }

        [Category("Image Properties")]
        public ImageUsageFlags Usage { get; set; }

        [Category("Image Properties")]
        public SharingMode SharingMode { get; set; }

        [Category("Queue Family")]
        public uint QueueFamilyIndexCount { get; set; }

        [Browsable(false)]
        public unsafe uint* PQueueFamilyIndices { get; set; }

        [Category("Image Layout")]
        public ImageLayout InitialLayout { get; set; }

        public RenderedTextureInfoModel RenderedTextureInfoModel { get; set; }
        public ImageCreateInfoModel()
        {

        }

        public ImageCreateInfo ConvertToVulkan()
        {
            return new ImageCreateInfo()
            {
                SType = StructureType.ImageCreateInfo,
                PNext = null,
                Flags = Flags,
                ImageType = ImageType,
                Format = Format,
                Extent = Extent.ConvertToVulkan(),
                MipLevels = MipLevels,
                ArrayLayers = ArrayLayers,
                Samples = Samples,
                Tiling = Tiling,
                Usage = Usage,
                SharingMode = SharingMode,
                QueueFamilyIndexCount = QueueFamilyIndexCount,
                PQueueFamilyIndices = PQueueFamilyIndices,
                InitialLayout = InitialLayout
            };
        }
    }
}

using GlmSharp;
using Silk.NET.Vulkan;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using VulkanGameEngineLevelEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class RenderedTexture : Texture
    {
        public RenderedTexture() : base()
        {

        }

        public RenderedTexture(ivec2 TextureResolution) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = Format.R8G8B8A8Unorm;
            TextureImageLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
            SampleCount = SampleCountFlags.SampleCount1Bit;


            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();

        }

        public RenderedTexture(ivec2 TextureResolution, SampleCountFlags sampleCount) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = Format.R8G8B8A8Unorm;
            TextureImageLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
            SampleCount = sampleCount;

            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();
        }

        public RenderedTexture(ivec2 TextureResolution, SampleCountFlags sampleCount, Format format) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = format;
            TextureImageLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
            SampleCount = sampleCount;

            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();

        }

        protected Result CreateTextureImage()
        {
            Image textureImage;
            DeviceMemory textureMemory;

            var imageInfo = new ImageCreateInfo
            {
                SType = StructureType.ImageCreateInfo,
                ImageType = ImageType.ImageType2D,
                Format = TextureByteFormat,
                Extent = new Extent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 },
                MipLevels = MipMapLevels,
                ArrayLayers = 1,
                Samples = SampleCountFlags.Count1Bit,
                Tiling = ImageTiling.Optimal,
                Usage = ImageUsageFlags.ImageUsageTransferSrcBit |
                    ImageUsageFlags.SampledBit |
                    ImageUsageFlags.ColorAttachmentBit |
                    ImageUsageFlags.ImageUsageTransferDstBit,
                SharingMode = SharingMode.Exclusive,
                InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined
            };

            var result = VKConst.vulkan.CreateImage(SilkVulkanRenderer.device, &imageInfo, null, &textureImage);
            VKConst.vulkan.GetImageMemoryRequirements(SilkVulkanRenderer.device, textureImage, out MemoryRequirements memRequirements);

            var allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, MemoryPropertyFlags.DeviceLocalBit)
            };
            result = VKConst.vulkan.AllocateMemory(SilkVulkanRenderer.device, &allocInfo, null, &textureMemory);
            result = VKConst.vulkan.BindImageMemory(SilkVulkanRenderer.device, textureImage, textureMemory, 0);

            Image = textureImage;
            Memory = textureMemory;

            return result;
        }

        public uint GetMemoryType(uint typeBits, MemoryPropertyFlags properties)
        {
            PhysicalDeviceMemoryProperties memoryProperties;
            vk.GetPhysicalDeviceMemoryProperties(SilkVulkanRenderer.physicalDevice, out memoryProperties);

            for (int i = 0; i < memoryProperties.MemoryTypeCount; i++)
            {
                if ((typeBits & (1U << i)) != 0 // Check if the memory type is supported
                    && (memoryProperties.MemoryTypes[i].PropertyFlags & properties) == properties) // Check if the properties match
                {
                    return (uint)i; // Return the valid index
                }
            }

            // No suitable memory type was found, throw an exception
            throw new InvalidOperationException("Failed to find a suitable memory type!");
        }

        public override Result CreateTextureView()
        {
            ImageSubresourceRange imageSubresourceRange = new ImageSubresourceRange()
            {
                AspectMask = ImageAspectFlags.ColorBit,
                BaseMipLevel = 0,
                LevelCount = MipMapLevels,
                BaseArrayLayer = 0,
                LayerCount = 1,
            };

            ImageViewCreateInfo TextureImageViewInfo = new ImageViewCreateInfo()
            {
                SType = StructureType.ImageViewCreateInfo,
                Image = Image,
                ViewType = ImageViewType.ImageViewType2D,
                Format = TextureByteFormat,
                SubresourceRange = imageSubresourceRange
            };
            Result result = VKConst.vulkan.CreateImageView(SilkVulkanRenderer.device, &TextureImageViewInfo, null, out ImageView textureView);
            View = textureView;
            return result;
        }

        protected override void CreateTextureSampler()
        {
            SamplerCreateInfo textureImageSamplerInfo = new SamplerCreateInfo
            {
                SType = StructureType.SamplerCreateInfo,
                MagFilter = Filter.Nearest,
                MinFilter = Filter.Nearest,
                MipmapMode = SamplerMipmapMode.Linear,
                AddressModeU = SamplerAddressMode.Repeat,
                AddressModeV = SamplerAddressMode.Repeat,
                AddressModeW = SamplerAddressMode.Repeat,
                MipLodBias = 0,
                AnisotropyEnable = Vk.True,
                MaxAnisotropy = 16.0f,
                CompareEnable = Vk.False,
                CompareOp = CompareOp.Always,
                MinLod = 0,
                MaxLod = MipMapLevels,
                BorderColor = BorderColor.IntOpaqueBlack,
                UnnormalizedCoordinates = Vk.False,
            };

            Sampler sampler = new Sampler();
            Result result = VKConst.vulkan.CreateSampler(SilkVulkanRenderer.device, ref textureImageSamplerInfo, null, out sampler);
            Sampler = sampler;
        }

        public void RecreateRendererTexture(vec2 TextureResolution)
        {
            Width = (int)TextureResolution.x;
            Height = (int)TextureResolution.y;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();

        }
    }
}

using GlmSharp;
using Silk.NET.Vulkan;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using VulkanGameEngineLevelEditor;
using ImageLayout = Silk.NET.Vulkan.ImageLayout;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class RenderedTexture : Texture
    {
        Vk vk = Vk.GetApi();
        public RenderedTexture() : base()
        {

        }

        public RenderedTexture(ivec2 TextureResolution) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = (VkFormat)Format.R8G8B8A8Unorm;
            TextureImageLayout = (VkImageLayout)Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
            SampleCount = (VkSampleCountFlagBits)SampleCountFlags.SampleCount1Bit;


            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();

        }

        public RenderedTexture(ivec2 TextureResolution, SampleCountFlags sampleCount) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = (VkFormat)Format.R8G8B8A8Unorm;
            TextureImageLayout = (VkImageLayout)Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
            SampleCount = (VkSampleCountFlagBits)sampleCount;

            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();
        }

        public RenderedTexture(ivec2 TextureResolution, SampleCountFlags sampleCount, Format format) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = (VkFormat)format;
            TextureImageLayout = (VkImageLayout)Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
            SampleCount = (VkSampleCountFlagBits)sampleCount;

            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();

        }

        public RenderedTexture(string filePath, Format textureByteFormat, TextureTypeEnum textureType)
        {
            TextureBufferIndex = 0;
            Width = 1;
            Height = 1;
            Depth = 1;
            MipMapLevels = 1;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = textureType;
            TextureByteFormat = (VkFormat)textureByteFormat;
            TextureImageLayout = (VkImageLayout)Silk.NET.Vulkan.ImageLayout.Undefined;
            SampleCount = (VkSampleCountFlagBits)SampleCountFlags.Count1Bit;

            CreateImageTexture(filePath);
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
                Format = (Format)TextureByteFormat,
                Extent = new Extent3D((uint)Width, (uint)Height, (uint)1),
                MipLevels = MipMapLevels,
                ArrayLayers = 1,
                Samples = SampleCountFlags.Count1Bit,
                Tiling = ImageTiling.Linear,
                Usage = ImageUsageFlags.ImageUsageTransferSrcBit |
                        ImageUsageFlags.ImageUsageSampledBit |
                        ImageUsageFlags.ImageUsageTransferDstBit,
                SharingMode = SharingMode.Exclusive,
                InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined
            };

            var result = vk.CreateImage(new Device(VulkanRenderer.Device), &imageInfo, null, &textureImage);
            if (result != Result.Success)
            {
            }

            vk.GetImageMemoryRequirements(new Device(VulkanRenderer.Device), textureImage, out MemoryRequirements memRequirements);

            var allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, (VkMemoryPropertyFlagBits)MemoryPropertyFlags.MemoryPropertyHostVisibleBit)
            };

            result = vk.AllocateMemory(new Device(VulkanRenderer.Device), &allocInfo, null, &textureMemory);
            if (result != Result.Success)
            {
            }

            result = vk.BindImageMemory(new Device(VulkanRenderer.Device), textureImage, textureMemory, 0);
            if (result != Result.Success)
            {
            }

            Image = new IntPtr((long)textureImage.Handle);
            Memory = new IntPtr((long)textureMemory.Handle);

            return result;
        }

        virtual protected void CreateImageTexture(string FilePath)
        {
            ColorChannels = ColorComponents.RedGreenBlueAlpha;
            Image textureImage;
            DeviceMemory textureMemory;

            var imageInfo = new ImageCreateInfo
            {
                SType = StructureType.ImageCreateInfo,
                ImageType = ImageType.ImageType2D,
                Format = (Format)TextureByteFormat,
                Extent = new Extent3D((uint)Width, (uint)Height, (uint)1),
                MipLevels = MipMapLevels,
                ArrayLayers = 1,
                Samples = SampleCountFlags.Count1Bit,
                Tiling = ImageTiling.Linear,
                Usage = ImageUsageFlags.ImageUsageTransferSrcBit |
                        ImageUsageFlags.ImageUsageSampledBit |
                        ImageUsageFlags.ImageUsageTransferDstBit,
                SharingMode = SharingMode.Exclusive,
                InitialLayout = ImageLayout.Undefined
            };

            var result = vk.CreateImage(new Device(VulkanRenderer.Device), &imageInfo, null, &textureImage);
            if (result != Result.Success)
            {
            }

            vk.GetImageMemoryRequirements(new Device(VulkanRenderer.Device), textureImage, out MemoryRequirements memRequirements);

            var allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, (VkMemoryPropertyFlagBits)(MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.DeviceLocalBit))
            };

            result = vk.AllocateMemory(new Device(VulkanRenderer.Device), &allocInfo, null, &textureMemory);
            if (result != Result.Success)
            {
            }

            result = vk.BindImageMemory(new Device(VulkanRenderer.Device), textureImage, textureMemory, 0);
            if (result != Result.Success)
            {
            }

            Image = new IntPtr((long)textureImage.Handle);
            Memory = new IntPtr((long)textureMemory.Handle);
        }

        public uint GetMemoryType(uint typeBits, MemoryPropertyFlags properties)
        {
            PhysicalDeviceMemoryProperties memoryProperties;
            vk.GetPhysicalDeviceMemoryProperties(new PhysicalDevice(VulkanRenderer.PhysicalDevice), out memoryProperties);

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

        public Result CreateTextureView()
        {
            var textureImageViewInfo = new ImageViewCreateInfo
            {
                SType = StructureType.ImageViewCreateInfo,
                ViewType = ImageViewType.ImageViewType2D,
                Image = new Image((ulong)Image.ToInt64()),
                Format = (Format)TextureByteFormat,
                SubresourceRange = new ImageSubresourceRange
                {
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                    AspectMask = ImageAspectFlags.ImageAspectColorBit
                }
            };

            Result result = vk.CreateImageView(new Device(VulkanRenderer.Device), &textureImageViewInfo, null, out var view);
            if (result != Result.Success)
            {
            }

            View = new IntPtr((long)view.Handle);

            return result;
        }

        protected override void CreateTextureSampler()
        {
            var textureImageSamplerInfo = new SamplerCreateInfo
            {
                SType = StructureType.SamplerCreateInfo,
                MagFilter = Filter.Linear,
                MinFilter = Filter.Linear,
                MipmapMode = SamplerMipmapMode.Linear,
                AddressModeU = SamplerAddressMode.ClampToEdge,
                AddressModeV = SamplerAddressMode.ClampToEdge,
                AddressModeW = SamplerAddressMode.ClampToEdge,
                MipLodBias = 0.0f,
                MaxAnisotropy = 1.0f,
                MinLod = 0.0f,
                MaxLod = 1.0f,
                BorderColor = BorderColor.FloatOpaqueWhite,
            };

            vk.CreateSampler(new Device(VulkanRenderer.Device), &textureImageSamplerInfo, null, out var sampler);
            Sampler = new IntPtr((long)sampler.Handle);
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
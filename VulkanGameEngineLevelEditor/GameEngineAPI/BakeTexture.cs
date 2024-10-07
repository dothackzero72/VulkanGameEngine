using GlmSharp;
using StbImageSharp;
using Silk.NET.Vulkan;
using System;
using System.Runtime.InteropServices;
using Silk.NET.Core.Native;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using ImageLayout = Silk.NET.Vulkan.ImageLayout;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class BakeTexture : Texture
    {
        Vk vk = Vk.GetApi();
        public BakeTexture() : base()
        {
        }

        public BakeTexture(string filePath, VkFormat textureByteFormat, TextureTypeEnum textureType)
        {
            TextureBufferIndex = 0;
            Width = 1;
            Height = 1;
            Depth = 1;
            MipMapLevels = 1;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = textureType;
            TextureByteFormat = textureByteFormat;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

            CreateImageTexture(filePath);
            CreateTextureView();
            CreateTextureSampler();
        }

        public BakeTexture(Pixel clearColor, ivec2 textureResolution, Format textureFormat) : base()
        {
            Width = textureResolution.x;
            Height = textureResolution.y;
            Depth = 1;
            TextureImageLayout = (VkImageLayout)ImageLayout.Undefined;
            SampleCount = (VkSampleCountFlagBits)(VkSampleCountFlags)SampleCountFlags.Count1Bit;
            TextureByteFormat = (VkFormat)textureFormat;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
        }
        protected override void CreateImageTexture()
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

        protected Result CreateTextureView()
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

        public void RecreateRendererTexture(vec2 textureResolution)
        {
            Width = (int)textureResolution.x;
            Height = (int)textureResolution.y;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
        }
    }
}
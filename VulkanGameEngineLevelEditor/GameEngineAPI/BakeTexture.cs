using GlmSharp;
using StbImageSharp;
using Silk.NET.Vulkan;
using System;
using System.Runtime.InteropServices;
using Silk.NET.Core.Native;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using ImageLayout = Silk.NET.Vulkan.ImageLayout;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class BakeTexture : Texture
    {
        Vk vk = Vk.GetApi();
        public BakeTexture() : base()
        {
        }

        public BakeTexture(ivec2 TextureResolution) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = Format.R8G8B8A8Unorm;
            TextureImageLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
            SampleCount = SampleCountFlags.SampleCount1Bit;


            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();

        }

        public BakeTexture(string filePath, Format textureByteFormat, TextureTypeEnum textureType)
        {
            TextureBufferIndex = 0;
            Width = 1;
            Height = 1;
            Depth = 1;
            MipMapLevels = 1;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = textureType;
            TextureByteFormat = textureByteFormat;
            TextureImageLayout = ImageLayout.Undefined;
            SampleCount = SampleCountFlags.Count1Bit;

            CreateImageTexture(filePath);
            CreateTextureView();
            CreateTextureSampler();
        }

        public BakeTexture(Pixel clearColor, ivec2 textureResolution, Format textureFormat) : base()
        {
            Width = textureResolution.x;
            Height = textureResolution.y;
            Depth = 1;
            TextureImageLayout = ImageLayout.Undefined;
            SampleCount = SampleCountFlags.Count1Bit;
            TextureByteFormat = textureFormat;

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

            var result = vk.CreateImage(VulkanRenderer.device, &imageInfo, null, &textureImage);
            if (result != Result.Success)
            {
            }

            vk.GetImageMemoryRequirements(VulkanRenderer.device, textureImage, out MemoryRequirements memRequirements);

            var allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.DeviceLocalBit)
            };

            result = vk.AllocateMemory(VulkanRenderer.device, &allocInfo, null, &textureMemory);
            if (result != Result.Success)
            {
            }

            result = vk.BindImageMemory(VulkanRenderer.device, textureImage, textureMemory, 0);
            if (result != Result.Success)
            {
            }

            Image = textureImage;
            Memory = textureMemory;
        }

        protected Result CreateTextureView()
        {
            var textureImageViewInfo = new ImageViewCreateInfo
            {
                SType = StructureType.ImageViewCreateInfo,
                ViewType = ImageViewType.ImageViewType2D,
                Image = Image,
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

            Result result = vk.CreateImageView(VulkanRenderer.device, &textureImageViewInfo, null, out var view);
            if (result != Result.Success)
            {
            }

            View = view;

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

            vk.CreateSampler(VulkanRenderer.device, &textureImageSamplerInfo, null, out var sampler);
            Sampler = sampler;
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
using GlmSharp;
using Silk.NET.Vulkan;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class DepthTexture : Texture
    {
        public DepthTexture() : base()
        {
        }

        public DepthTexture(ivec2 textureResolution) : base()
        {
            Width = textureResolution.x;
            Height = textureResolution.y;
            Depth = 1;
            TextureImageLayout = ImageLayout.Undefined;
            SampleCount = SampleCountFlags.Count1Bit;
            TextureByteFormat = Format.D32Sfloat;

            CreateImageTexture();
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
                Extent = new Extent3D((uint)Width, (uint)Height, (uint)1),
                MipLevels = MipMapLevels,
                ArrayLayers = 1,
                Samples = SampleCountFlags.Count1Bit,
                Tiling = ImageTiling.Optimal,
                Usage = ImageUsageFlags.ImageUsageTransferSrcBit |
                        ImageUsageFlags.ImageUsageSampledBit |
                        ImageUsageFlags.ImageUsageTransferDstBit |
                        ImageUsageFlags.DepthStencilAttachmentBit,
                SharingMode = SharingMode.Exclusive,
                InitialLayout = ImageLayout.Undefined
            };

            var result = VKConst.vulkan.CreateImage(SilkVulkanRenderer.device, &imageInfo, null, &textureImage);
            if (result != Result.Success)
            {
            }

            VKConst.vulkan.GetImageMemoryRequirements(SilkVulkanRenderer.device, textureImage, out MemoryRequirements memRequirements);

            var allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits,
                    MemoryPropertyFlags.MemoryPropertyDeviceLocalBit | MemoryPropertyFlags.MemoryPropertyHostVisibleBit)
            };

            result = VKConst.vulkan.AllocateMemory(SilkVulkanRenderer.device, &allocInfo, null, &textureMemory);
            if (result != Result.Success)
            {
            }

            result = VKConst.vulkan.BindImageMemory(SilkVulkanRenderer.device, textureImage, textureMemory, 0);
            if (result != Result.Success)
            {
            }

            Image = textureImage;
            Memory = textureMemory;

            return result;
        }

        public override Result CreateTextureView()
        {
            var textureImageViewInfo = new ImageViewCreateInfo
            {
                SType = StructureType.ImageViewCreateInfo,
                ViewType = ImageViewType.ImageViewType2D,
                Image = Image,
                Format = TextureByteFormat,
                SubresourceRange = new ImageSubresourceRange
                {
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                    AspectMask = ImageAspectFlags.DepthBit
                }
            };

            Result result = VKConst.vulkan.CreateImageView(SilkVulkanRenderer.device, &textureImageViewInfo, null, out var view);
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

            VKConst.vulkan.CreateSampler(SilkVulkanRenderer.device, &textureImageSamplerInfo, null, out var sampler);
            Sampler = sampler;
        }

        public void RecreateRendererTexture(vec2 textureResolution)
        {
            Width = (int)textureResolution.x;
            Height = (int)textureResolution.y;

            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();
        }
    }
}

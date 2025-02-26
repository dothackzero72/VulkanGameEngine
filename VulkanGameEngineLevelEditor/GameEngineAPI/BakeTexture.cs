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
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Models;

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
            TextureByteFormat = VkFormat.VK_FORMAT_R8G8B8A8_UNORM;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_READ_ONLY_OPTIMAL;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;


            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();

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

        public BakeTexture(Pixel clearColor, ivec2 textureResolution, VkFormat textureFormat) : base()
        {
            Width = textureResolution.x;
            Height = textureResolution.y;
            Depth = 1;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
            TextureByteFormat = textureFormat;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
        }
        protected override void CreateImageTexture()
        {
            ColorChannels = ColorComponents.RedGreenBlueAlpha;
            var imageInfo = new VkImageCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
                imageType = VkImageType.VK_IMAGE_TYPE_2D,
                format = TextureByteFormat,
                extent = new VkExtent3D((uint)Width, (uint)Height, (uint)1),
                mipLevels = MipMapLevels,
                arrayLayers = 1,
                samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                tiling = VkImageTiling.VK_IMAGE_TILING_LINEAR,
                usage = VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED
            };

            var result = VkFunc.vkCreateImage(VulkanRenderer.device, &imageInfo, null, out VkImage textureImage);
            if (result != VkResult.VK_SUCCESS)
            {
            }

            VkFunc.vkGetImageMemoryRequirements(VulkanRenderer.device, textureImage, out VkMemoryRequirements memRequirements);

            var allocInfo = new VkMemoryAllocateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_EXPORT_MEMORY_ALLOCATE_INFO,
                allocationSize = memRequirements.size,
                memoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.memoryTypeBits, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
            };

            result = VkFunc.vkAllocateMemory(VulkanRenderer.device, &allocInfo, null, out VkDeviceMemory textureMemory);
            if (result != VkResult.VK_SUCCESS)
            {
            }

            result = VkFunc.vkBindImageMemory(VulkanRenderer.device, textureImage, textureMemory, 0);
            if (result != VkResult.VK_SUCCESS)
            {
            }

            Image = textureImage;
            Memory = textureMemory;
        }

        protected VkResult CreateTextureView()
        {
            var textureImageViewInfo = new VkImageViewCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                viewType = VkImageViewType.VK_IMAGE_VIEW_TYPE_2D,
                image = Image,
                format = TextureByteFormat,
                subresourceRange = new VkImageSubresourceRange
                {
                    baseMipLevel = 0,
                    levelCount = 1,
                    baseArrayLayer = 0,
                    layerCount = 1,
                    aspectMask = VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT
                }
            };

            VkResult result = VkFunc.vkCreateImageView(VulkanRenderer.device, &textureImageViewInfo, null, out var view);
            if (result != VkResult.VK_SUCCESS)
            {
            }

            View = view;

            return result;
        }

        protected override void CreateTextureSampler()
        {
            var textureImageSamplerInfo = new VkSamplerCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                magFilter = VkFilter.VK_FILTER_LINEAR,
                minFilter = VkFilter.VK_FILTER_LINEAR,
                mipmapMode = VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_LINEAR,
                addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
                addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
                addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
                mipLodBias = 0.0f,
                maxAnisotropy = 1.0f,
                minLod = 0.0f,
                maxLod = 1.0f,
                borderColor = VkBorderColor.VK_BORDER_COLOR_FLOAT_OPAQUE_WHITE,
            };

            VkFunc.vkCreateSampler(VulkanRenderer.device, &textureImageSamplerInfo, null, out var sampler);
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
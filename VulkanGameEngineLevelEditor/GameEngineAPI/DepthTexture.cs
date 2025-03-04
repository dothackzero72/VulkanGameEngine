using GlmSharp;
using Silk.NET.Vulkan;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Models;
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
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
            TextureByteFormat = VkFormat.VK_FORMAT_D32_SFLOAT;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
        }

        public DepthTexture(ivec2 TextureResolution, VkImageCreateInfo createInfo, VkSamplerCreateInfo samplerCreateInfo) : base()
        {
            createInfo.extent.width = (uint)TextureResolution.x;
            createInfo.extent.height = (uint)TextureResolution.y;
            createInfo.extent.depth = 1;
            Width = (int)createInfo.extent.width;
            Height = (int)createInfo.extent.height;
            Depth = 1;
            TextureByteFormat = createInfo.format;
            TextureImageLayout = createInfo.initialLayout;
            SampleCount = createInfo.samples;

            CreateTextureImage(createInfo);
            CreateTextureView();
            CreateTextureSampler(samplerCreateInfo);
        }

        protected override void CreateImageTexture()
        {
           VkImageCreateInfo imageInfo = new VkImageCreateInfo
           {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                imageType = VkImageType.VK_IMAGE_TYPE_2D,
                format = TextureByteFormat,
                extent = new VkExtent3D { width = (uint)Width, height = (uint)Height, depth = 1 },
                mipLevels = MipMapLevels,
                arrayLayers = 1,
                samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                tiling = VkImageTiling.VK_IMAGE_TILING_OPTIMAL,
                usage = VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED
            };

            VkImage tempImage = new VkImage();
            VkDeviceMemory memory = new VkDeviceMemory();
            CTexture.CreateImage(imageInfo, ref tempImage, ref memory, imageInfo);
            Memory = memory;
            Image = tempImage;
        }

        public override VkResult CreateTextureView()
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
                    aspectMask = VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT
                }
            };

            VkResult result = VkFunc.vkCreateImageView(VulkanRenderer.device, &textureImageViewInfo, null, out VkImageView view);
            View = view;

            return result;
        }

        protected override void CreateTextureSampler()
        {
            var textureImageSamplerInfo = new VkSamplerCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                magFilter = VkFilter.VK_FILTER_NEAREST,              
                minFilter = VkFilter.VK_FILTER_NEAREST,              
                mipmapMode = VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_LINEAR, 
                addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_MIRROR_CLAMP_TO_EDGE,
                addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_MIRROR_CLAMP_TO_EDGE,
                addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
                mipLodBias = 0.0f,
                maxAnisotropy = 1.0f,                     
                minLod = 0.0f,
                maxLod = 0.0f,                           
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

using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using VulkanGameEngineLevelEditor;

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
            TextureByteFormat = VkFormat.VK_FORMAT_R8G8B8A8_UNORM;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;


            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();

        }

        public RenderedTexture(ivec2 TextureResolution, VkSampleCountFlagBits sampleCount) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = VkFormat.VK_FORMAT_R8G8B8A8_UNORM;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
            SampleCount = sampleCount;

            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();
        }

        public RenderedTexture(ivec2 TextureResolution, VkSampleCountFlagBits sampleCount, VkFormat format) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = format;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
            SampleCount = sampleCount;

            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();

        }
        public void CreateTextureImage()
        {
            VkImageCreateInfo TextureInfo = new VkImageCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
                imageType = VkImageType.VK_IMAGE_TYPE_2D,
                mipLevels = 1, // Adjust if using mipmaps
                arrayLayers = 1,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED, // Changed here
                samples = SampleCount,
                tiling = VkImageTiling.VK_IMAGE_TILING_OPTIMAL,
                usage = VkImageUsageFlags.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT | // Ensure this is supported for your format
                        VkImageUsageFlags.VK_IMAGE_USAGE_SAMPLED_BIT |
                        VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                        VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                format = TextureByteFormat,
                extent = new VkExtent3D
                {
                    Width = (uint)Width,
                    Height = (uint)Height,
                    Depth = 1,
                }
            };

            var image = Image;
            VulkanAPI.vkCreateImage(VulkanRenderer.Device, &TextureInfo, null, &image);
            Image = image;

            VkMemoryRequirements memRequirements;
            VulkanAPI.vkGetImageMemoryRequirements(VulkanRenderer.Device, Image, &memRequirements);

            VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                allocationSize = memRequirements.size,
                memoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.memoryTypeBits, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
            };

            var Alloc = allocInfo;
            var memory = Memory;
            VulkanAPI.vkAllocateMemory(VulkanRenderer.Device, &Alloc, null, &memory);
            Memory = memory;

            VulkanAPI.vkBindImageMemory(VulkanRenderer.Device, Image, Memory, 0);
        }

        protected override VkResult CreateTextureView()
        {
            VkImageViewCreateInfo TextureImageViewInfo = new VkImageViewCreateInfo()
            {
                sType =  VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                viewType =  VkImageViewType.VK_IMAGE_VIEW_TYPE_2D,
                image = Image,
                format = TextureByteFormat,
                subresourceRange = new VkImageSubresourceRange()
                {
                    baseMipLevel = 0,
                    levelCount = 1,
                    baseArrayLayer = 0,
                    layerCount = 1,
                    aspectMask =  VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT
                }
            };
            var view = View;
            var textureImageViewInfo = TextureImageViewInfo;
            VulkanAPI.vkCreateImageView(VulkanRenderer.Device, &TextureImageViewInfo, null, &view);
            TextureImageViewInfo = textureImageViewInfo;
            View = view;

            return VkResult.VK_SUCCESS;
        }

        protected override void CreateTextureSampler()
        {
            VkSamplerCreateInfo TextureImageSamplerInfo = new VkSamplerCreateInfo();
            TextureImageSamplerInfo.sType =  VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO;
            TextureImageSamplerInfo.magFilter =  VkFilter.VK_FILTER_LINEAR;
            TextureImageSamplerInfo.minFilter = VkFilter.VK_FILTER_LINEAR;
            TextureImageSamplerInfo.mipmapMode =  VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_LINEAR;
            TextureImageSamplerInfo.addressModeU =  VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
            TextureImageSamplerInfo.addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
            TextureImageSamplerInfo.addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
            TextureImageSamplerInfo.mipLodBias = 0.0f;
            TextureImageSamplerInfo.maxAnisotropy = 1.0f;
            TextureImageSamplerInfo.minLod = 0.0f;
            TextureImageSamplerInfo.maxLod = 1.0f;
            TextureImageSamplerInfo.borderColor =  VkBorderColor.VK_BORDER_COLOR_FLOAT_OPAQUE_WHITE;

            var textureImageSamplerInfo = TextureImageSamplerInfo;
            var sampler = Sampler;
            VulkanAPI.vkCreateSampler(VulkanRenderer.Device, &textureImageSamplerInfo, null, &sampler);
            Sampler = sampler;
            TextureImageSamplerInfo = textureImageSamplerInfo;
            
        }

        public void RecreateRendererTexture(vec2 TextureResolution)
        {
            Width = (int)TextureResolution.x;
            Height = (int)TextureResolution.y;

            CreateTextureImage();
            CreateTextureView();
            CreateTextureSampler();

        }
    }
}

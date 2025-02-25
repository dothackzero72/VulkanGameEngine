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
using VulkanGameEngineLevelEditor.Models;
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

        public RenderedTexture(ivec2 TextureResolution, VkImageCreateInfo createInfo, VkSamplerCreateInfo samplerCreateInfo) : base()
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

        protected VkResult CreateTextureImage()
        {
            VkImage textureImage;
            VkDeviceMemory textureMemory;

            var imageInfo = new VkImageCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
                imageType = VkImageType.VK_IMAGE_TYPE_2D,
                format = TextureByteFormat,
                extent = new VkExtent3D { width = (uint)Width, height = (uint)Height, depth = 1 },
                mipLevels = MipMapLevels,
                arrayLayers = 1,
                samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                tiling = VkImageTiling.VK_IMAGE_TILING_OPTIMAL,
                usage = VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED
            };

            var result = VkFunc.vkCreateImage(VulkanRenderer.device, &imageInfo, null, &textureImage);
            VkFunc.vkGetImageMemoryRequirements(VulkanRenderer.device, textureImage, out VkMemoryRequirements memRequirements);

            var allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.size,
                MemoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.memoryTypeBits, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
            };
            result = VkFunc.vkAllocateMemory(VulkanRenderer.device, &allocInfo, null, &textureMemory);
            result = VkFunc.vkBindImageMemory(VulkanRenderer.device, textureImage, textureMemory, 0);

            Image = textureImage;
            Memory = textureMemory;

            return result;
        }

        public uint GetMemoryType(uint typeBits, VkMemoryPropertyFlagBits properties)
        {
            VkFunc.vkGetPhysicalDeviceMemoryProperties(VulkanRenderer.physicalDevice, out VkPhysicalDeviceMemoryProperties memoryProperties);

            for (int i = 0; i < memoryProperties.memoryTypeCount; i++)
            {
                if ((typeBits & (1U << i)) != 0 // Check if the memory type is supported
                    && (memoryProperties.memoryTypes[i].propertyFlags & properties) == properties) // Check if the properties match
                {
                    return (uint)i; // Return the valid index
                }
            }

            // No suitable memory type was found, throw an exception
            throw new InvalidOperationException("Failed to find a suitable memory type!");
        }

        public override VkResult CreateTextureView()
        {
            VkImageSubresourceRange imageSubresourceRange = new VkImageSubresourceRange()
            {
                aspectMask = VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT,
                baseMipLevel = 0,
                levelCount = MipMapLevels,
                baseArrayLayer = 0,
                layerCount = 1,
            };

            VkImageViewCreateInfo TextureImageViewInfo = new VkImageViewCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                image = Image,
                viewType = VkImageViewType.VK_IMAGE_VIEW_TYPE_2D,
                format = TextureByteFormat,
                subresourceRange = imageSubresourceRange
            };
            VkResult result = VkFunc.vkCreateImageView(VulkanRenderer.device, &TextureImageViewInfo, null, out ImageView textureView);
            View = textureView;
            return result;
        }

        protected override void CreateTextureSampler()
        {
            VkSamplerCreateInfo textureImageSamplerInfo = new VkSamplerCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                magFilter = VkFilter.VK_FILTER_NEAREST,
                minFilter = VkFilter.VK_FILTER_NEAREST,
                mipmapMode = VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_LINEAR,
                addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                mipLodBias = 0,
                anisotropyEnable = true,
                maxAnisotropy = 16.0f,
                compareEnable =false,
                compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS,
                minLod = 0,
                maxLod = MipMapLevels,
                borderColor = VkBorderColor.VK_BORDER_COLOR_FLOAT_OPAQUE_BLACK,
                unnormalizedCoordinates = false,
            };

            VkSampler sampler = new VkSampler();
            VkResult result = VkFunc.vkCreateSampler(VulkanRenderer.device, ref textureImageSamplerInfo, null, out sampler);
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

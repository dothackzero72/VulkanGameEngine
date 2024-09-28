using GlmSharp;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class BakeTexture : Texture
    {
        public BakeTexture() : base()
        {

        }

        public BakeTexture(string filePath, VkFormat textureByteFormat, TextureTypeEnum textureType) : base()
        {
            //Width = TextureResolution.x;
            //Height = TextureResolution.y;
            //Depth = 1;
            //TextureByteFormat = VkFormat.VK_FORMAT_R8G8B8A8_UNORM;
            //TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
            //SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;


            //CreateTextureImage();
            //CreateTextureView();
            //CreateTextureSampler();

        }

        public BakeTexture(Pixel ClearColor, ivec2 TextureResolution, VkFormat TextureFormat) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;

            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
            TextureByteFormat = TextureFormat;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
        }

        protected override void CreateImageTexture()
        {
            ColorChannels = ColorComponents.RedGreenBlueAlpha;
            uint size = (uint)Width * (uint)Height * (uint)ColorChannels;

            Pixel[] pixels = new Pixel[Width * Height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Pixel(0x00, 0x00, 0xFF,  0xFF); 
            }

            GCHandle pixelHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            IntPtr dataPtr;
            dataPtr = pixelHandle.AddrOfPinnedObject();

            VulkanBuffer<Pixel> stagingBuffer = new VulkanBuffer<Pixel>(
                dataPtr,
                size,
                VkBufferUsageFlags.VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT
            );
            var bHandle = stagingBuffer.Buffer;

            CreateTextureImage();
            Texture_QuickTransitionImageLayout(TextureImageLayout, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
            CopyBufferToTexture(ref bHandle);

            pixelHandle.Free();
            stagingBuffer.DestroyBuffer();
        }

        protected override VkResult CreateTextureImage()
        {
            VkImage textureImage = new VkImage();
            VkDeviceMemory textureMemory;

            VkImageCreateInfo imageInfo = new VkImageCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
                imageType = VkImageType.VK_IMAGE_TYPE_2D,
                format = TextureByteFormat,
                extent = new VkExtent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 },
                mipLevels = MipMapLevels,
                arrayLayers = 1,
                samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                tiling = VkImageTiling.VK_IMAGE_TILING_LINEAR,
                usage = VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                        VkImageUsageFlags.VK_IMAGE_USAGE_SAMPLED_BIT |
                        VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED
            };

            var ImageInfo = imageInfo;

            VkResult result = VulkanAPI.vkCreateImage(VulkanRenderer.Device, &imageInfo, null, &textureImage);
            if (result != VkResult.VK_SUCCESS)
            {

            }

            VkMemoryRequirements memRequirements;
            VulkanAPI.vkGetImageMemoryRequirements(VulkanRenderer.Device, textureImage, &memRequirements);

            VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                allocationSize = memRequirements.size,
                memoryTypeIndex = GameEngineDLL.DLL_Renderer_GetMemoryType(VulkanRenderer.PhysicalDevice, memRequirements.memoryTypeBits, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT)
            };

            result = VulkanAPI.vkAllocateMemory(VulkanRenderer.Device, &allocInfo, null, &textureMemory);
            if (result != VkResult.VK_SUCCESS)
            {
            }

            result = VulkanAPI.vkBindImageMemory(VulkanRenderer.Device, textureImage, textureMemory, 0);
            if (result != VkResult.VK_SUCCESS)
            {
            }

            Image = textureImage;
            Memory = textureMemory;

            return result;
        }


        protected override VkResult CreateTextureView()
        {
            VkImageViewCreateInfo TextureImageViewInfo = new VkImageViewCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                viewType = VkImageViewType.VK_IMAGE_VIEW_TYPE_2D,
                image = Image,
                format = TextureByteFormat,
                subresourceRange = new VkImageSubresourceRange()
                {
                    baseMipLevel = 0,
                    levelCount = 1,
                    baseArrayLayer = 0,
                    layerCount = 1,
                    aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT
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
            TextureImageSamplerInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO;
            TextureImageSamplerInfo.magFilter = VkFilter.VK_FILTER_LINEAR;
            TextureImageSamplerInfo.minFilter = VkFilter.VK_FILTER_LINEAR;
            TextureImageSamplerInfo.mipmapMode = VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_LINEAR;
            TextureImageSamplerInfo.addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
            TextureImageSamplerInfo.addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
            TextureImageSamplerInfo.addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
            TextureImageSamplerInfo.mipLodBias = 0.0f;
            TextureImageSamplerInfo.maxAnisotropy = 1.0f;
            TextureImageSamplerInfo.minLod = 0.0f;
            TextureImageSamplerInfo.maxLod = 1.0f;
            TextureImageSamplerInfo.borderColor = VkBorderColor.VK_BORDER_COLOR_FLOAT_OPAQUE_WHITE;

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

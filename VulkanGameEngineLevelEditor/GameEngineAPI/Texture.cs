using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor;
using StbImageSharp;
using System.IO;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Texture
    {
        public UInt64 TextureBufferIndex { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; private set; }
        public ColorComponents ColorChannels { get; private set; }
        public UInt32 MipMapLevels { get; private set; }
        public TextureUsageEnum TextureUsage { get; private set; }
        public TextureTypeEnum TextureType { get; private set; }
        public VkFormat TextureByteFormat { get; private set; }
        public VkImageLayout TextureImageLayout { get; private set; }
        public VkSampleCountFlagBits SampleCount { get; private set; }
        public VkImage Image { get; private set; }
        public VkDeviceMemory Memory { get; private set; }
        public VkImageView View { get; private set; }
        public VkSampler Sampler { get; private set; }
        public VkDescriptorImageInfo textureBuffer { get; private set; }

        public Texture()
        {
            TextureBufferIndex = 0;
            Width = 1;
            Height = 1;
            Depth = 1;
            MipMapLevels = 1;

            Image = VulkanConsts.VK_NULL_HANDLE;
            Memory = VulkanConsts.VK_NULL_HANDLE;
            View = VulkanConsts.VK_NULL_HANDLE;
            Sampler = VulkanConsts.VK_NULL_HANDLE;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = TextureTypeEnum.kType_UndefinedTexture;
            TextureByteFormat = VkFormat.VK_FORMAT_UNDEFINED;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
        }

        public Texture(string filePath, VkFormat textureByteFormat, TextureTypeEnum textureType)
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

        virtual protected void CreateImageTexture(string FilePath)
        {
            MipMapLevels = (uint)Math.Floor(Math.Log(Math.Max(Width, Height)) / Math.Log(2)) + 1;

            using (var stream = File.OpenRead(FilePath))
            {
                ImageResult image = ImageResult.FromStream(stream);
                Width = image.Width;
                Height = image.Height;
                ColorChannels = image.Comp;

                GCHandle handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                IntPtr dataPtr = handle.AddrOfPinnedObject();

                var buffer = new VulkanBuffer<byte>(dataPtr, (uint)(Width * Height * (int)ColorChannels), VkBufferUsageFlags.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT);
                var bHandle = buffer.Buffer;

                CreateTextureImage();
                TransitionImageLayout(VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
                CopyBufferToTexture(ref bHandle);
                GenerateMipmaps();

                handle.Free();
                buffer.DestroyBuffer();
            }
        }

        virtual protected void CreateTextureSampler()
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
                compareEnable = false,
                compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS,
                minLod = 0,
                maxLod = MipMapLevels,
                borderColor = VkBorderColor.VK_BORDER_COLOR_INT_OPAQUE_BLACK,
                unnormalizedCoordinates = false,
            };

            VkSampler sampler = new VkSampler();
            VkResult result = GameEngineDLL.DLL_Texture_CreateTextureSampler(VulkanRenderer.Device, ref textureImageSamplerInfo, out sampler);
            Sampler = sampler;
        }

        public VkDescriptorImageInfo GetTextureBuffer()
        {
            return new VkDescriptorImageInfo
            {
        		sampler = Sampler,
        		imageView = View,
        		imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
            };
        }
        virtual public void UpdateTextureSize(vec2 TextureResolution)
        {
            //GameEngineDLL.DLL_Renderer_DestroyImageView(&View);
            //GameEngineDLL.DLL_Renderer_DestroySampler(&Sampler);
            //GameEngineDLL.DLL_Renderer_DestroyImage(&Image);
            //GameEngineDLL.DLL_Renderer_FreeMemory(&Memory);
        }

        virtual public void Destroy()
        {

        }

        private VkResult CreateTextureImage()
        {
            VkImage textureImage;
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
                tiling = VkImageTiling.VK_IMAGE_TILING_OPTIMAL,
                usage = VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VkImageUsageFlags.VK_IMAGE_USAGE_SAMPLED_BIT,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED
            };

              VkResult result = VulkanAPI.vkCreateImage(VulkanRenderer.Device, ref imageInfo, IntPtr.Zero, out textureImage);
            if (result != VkResult.VK_SUCCESS)
            {
                
            }

            VkMemoryRequirements memRequirements;
            VulkanAPI.vkGetImageMemoryRequirements(VulkanRenderer.Device, textureImage, out memRequirements);

            VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                allocationSize = memRequirements.size,
                memoryTypeIndex = GameEngineDLL.DLL_Renderer_GetMemoryType(VulkanRenderer.PhysicalDevice, memRequirements.memoryTypeBits, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
            };

            result = VulkanAPI.vkAllocateMemory(VulkanRenderer.Device, ref allocInfo, IntPtr.Zero, out textureMemory);
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

        private VkResult CreateTextureView()
        {
            var textureView = new IntPtr();
            VkResult result = GameEngineDLL.DLL_Texture_CreateTextureView(VulkanRenderer.Device, out textureView, Image, TextureByteFormat, MipMapLevels);
            View = textureView;
            return result;
        }

        private VkResult TransitionImageLayout(VkImageLayout newLayout)
        {
            VkImageLayout oldLayout = TextureImageLayout;
            var result = GameEngineDLL.DLL_Texture_QuickTransitionImageLayout(VulkanRenderer.Device, VulkanRenderer.CommandPool, VulkanRenderer.GraphicsQueue, Image, MipMapLevels, ref oldLayout, ref newLayout);
            TextureImageLayout = newLayout;
            return result;
        }

        private VkResult CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newLayout)
        {
            return GameEngineDLL.DLL_Texture_CommandBufferTransitionImageLayout(commandBuffer, Image, MipMapLevels, TextureImageLayout, newLayout);
        }

        private VkResult CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout oldLayout, VkImageLayout newLayout)
        {
            return GameEngineDLL.DLL_Texture_CommandBufferTransitionImageLayout(commandBuffer, Image, MipMapLevels, oldLayout, newLayout);
        }

        private VkResult CopyBufferToTexture(ref VkBuffer buffer)
        {
            // Ensure buffer is initialized correctly
            if (buffer == IntPtr.Zero)
            {
                throw new InvalidOperationException("Buffer has not been initialized.");
            }

            // Call the DLL function to copy the buffer to the texture
            VkResult result = GameEngineDLL.DLL_Texture_CopyBufferToTexture(
                (IntPtr)VulkanRenderer.Device,
                (IntPtr)VulkanRenderer.CommandPool,
                (IntPtr)VulkanRenderer.GraphicsQueue,
                (IntPtr)Image,
                (IntPtr)buffer, // Use .Handle if VkBuffer is a struct with a Handle field
                TextureUsage,
                Width,
                Height,
                Depth
            );

            // Additional post-call checks can be done based on the result
            if (result != VkResult.VK_SUCCESS)
            {
                // Handle error here
                throw new InvalidOperationException($"Failed to copy buffer to texture: {result}");
            }

            return result;
        }

        private VkResult CopyBufferToTexture(VkImage image, ref VkBuffer buffer, TextureUsageEnum textureType, vec3 textureSize)
        {
            return GameEngineDLL.DLL_Texture_CopyBufferToTexture(VulkanRenderer.Device, VulkanRenderer.CommandPool, VulkanRenderer.GraphicsQueue, image, buffer, textureType, (int)textureSize.x, (int)textureSize.y, (int)textureSize.z);
        }

        private VkResult GenerateMipmaps()
        {
            var textureByteFormat = TextureByteFormat;
            VkResult result = GameEngineDLL.DLL_Texture_GenerateMipmaps(VulkanRenderer.Device, VulkanRenderer.PhysicalDevice, VulkanRenderer.CommandPool, VulkanRenderer.GraphicsQueue, Image, ref textureByteFormat, MipMapLevels, Width, Height);
            TextureByteFormat = textureByteFormat;
            return result;
        }
    }
}
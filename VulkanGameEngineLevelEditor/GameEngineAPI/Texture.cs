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

        public Texture(string filePath, VkFormat textureByteFormat, TextureTypeEnum TextureType)
        {
            TextureBufferIndex = 0;
            Width = 1;
            Height = 1;
            Depth = 1;
            MipMapLevels = 1;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = TextureTypeEnum.kType_UndefinedTexture;
            TextureByteFormat = VkFormat.VK_FORMAT_UNDEFINED;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

            CreateImageTexture(filePath);
            Texture_CreateTextureView(this);
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

                var buffer = new VulkanBuffer<byte>((IntPtr)image.Data[0], (uint)(Width * Height * (int)ColorChannels), VkBufferUsageFlags.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT);
                var bHandle = buffer.BufferHandle;

                Texture_CreateTextureImage(this);
                Texture_TransitionImageLayout(this, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
                Texture_CopyBufferToTexture(this, out bHandle);
                Texture_GenerateMipmaps(this);
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

            VkSampler sampler = Sampler;
            VkResult result = GameEngineDLL.DLL_Texture_CreateTextureSampler(VulkanRenderer.Device, out textureImageSamplerInfo);
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

        private VkResult Texture_CreateTextureImage(Texture texture)
        {
            var textureImage = texture.View;
            VkResult result = GameEngineDLL.DLL_Texture_CreateTextureView(VulkanRenderer.Device, out textureImage, texture.Image, texture.TextureByteFormat, texture.MipMapLevels);
            texture.Image = textureImage;
            return result;
        }

        private VkResult Texture_CreateTextureView(Texture texture)
        {
            var textureView = texture.View;
            VkResult result = GameEngineDLL.DLL_Texture_CreateTextureView(VulkanRenderer.Device, out textureView, texture.Image, texture.TextureByteFormat, texture.MipMapLevels);
            texture.View = textureView;
            return result;
        }

        private VkResult Texture_TransitionImageLayout(Texture texture, VkImageLayout newLayout)
        {
            VkImageLayout oldLayout = TextureImageLayout;
            var result = GameEngineDLL.DLL_Texture_QuickTransitionImageLayout(texture.Image, texture.MipMapLevels, out oldLayout, out newLayout);
            texture.TextureImageLayout = newLayout;
            return result;
        }

        private VkResult Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, Texture texture, VkImageLayout newLayout)
        {
            return GameEngineDLL.DLL_Texture_CommandBufferTransitionImageLayout(commandBuffer, texture.Image, texture.MipMapLevels, texture.TextureImageLayout, newLayout);
        }

        private VkResult Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, Texture texture, VkImageLayout oldLayout, VkImageLayout newLayout)
        {
            return GameEngineDLL.DLL_Texture_CommandBufferTransitionImageLayout(commandBuffer, texture.Image, texture.MipMapLevels, oldLayout, newLayout);
        }

        private VkResult Texture_CopyBufferToTexture(Texture texture, out VkBuffer buffer)
        {
            return GameEngineDLL.DLL_Texture_CopyBufferToTexture(texture.Image, out buffer, texture.TextureUsage, texture.Width, texture.Height, texture.Depth);
        }

        private VkResult Texture_CopyBufferToTexture(VkImage image, out VkBuffer buffer, TextureUsageEnum textureType, vec3 textureSize)
        {
            return GameEngineDLL.DLL_Texture_CopyBufferToTexture(image, out buffer, textureType, (int)textureSize.x, (int)textureSize.y, (int)textureSize.z);
        }

        private VkResult Texture_GenerateMipmaps(Texture texture)
        {
            var textureByteFormat = texture.TextureByteFormat;
            VkResult result = GameEngineDLL.DLL_Texture_GenerateMipmaps(VulkanRenderer.PhysicalDevice, texture.Image, out textureByteFormat, texture.MipMapLevels, texture.Width, texture.Height);
            texture.TextureByteFormat = textureByteFormat;
            return result;
        }
    }
}
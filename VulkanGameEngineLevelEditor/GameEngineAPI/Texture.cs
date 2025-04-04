using System;
using StbImageSharp;
using VulkanGameEngineLevelEditor.Vulkan;
using VulkanGameEngineGameObjectScripts;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Texture
    {
        [JsonIgnore]
        static private uint NextTextureId;
        [JsonIgnore]
        public uint TextureId { get; private set; } = 0;
        [JsonIgnore]
        public uint TextureBufferIndex { get; private set; } = 0;
        [JsonIgnore]
        public string Name { get; private set; } = "Texture";
        [JsonIgnore]
        public int Width { get; protected set; } = 1;
        [JsonIgnore]
        public int Height { get; protected set; } = 1;
        [JsonIgnore]
        public int Depth { get; protected set; } = 1;
        [JsonIgnore]
        public ColorComponents ColorChannels { get; protected set; } = ColorComponents.RedGreenBlueAlpha;
        public uint MipMapLevels { get; protected set; } = 1;
        [JsonIgnore]
        public TextureUsageEnum TextureUsage { get; protected set; } = TextureUsageEnum.kUse_Undefined;
        [JsonIgnore]
        public TextureTypeEnum TextureType { get; protected set; } = TextureTypeEnum.kType_UndefinedTexture;
        [JsonIgnore]
        public VkFormat TextureByteFormat { get; protected set; } = VkFormat.VK_FORMAT_UNDEFINED;
        [JsonIgnore]
        public VkImageLayout TextureImageLayout { get; protected set; } = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
        [JsonIgnore]
        public VkSampleCountFlagBits SampleCount { get; protected set; } = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
        [JsonIgnore]
        public VkImage Image { get; protected set; } = VulkanConst.VK_NULL_HANDLE;
        [JsonIgnore]
        public VkDeviceMemory Memory { get; protected set; } = VulkanConst.VK_NULL_HANDLE;
        [JsonIgnore]
        public VkImageView View { get; protected set; } = VulkanConst.VK_NULL_HANDLE;
        [JsonIgnore]
        public VkSampler Sampler { get; protected set; } = VulkanConst.VK_NULL_HANDLE;
        public string TexturePath { get; private set; } = string.Empty;

        public Texture()
        {

        }

        public Texture(Pixel clearColor, int width, int height, VkFormat textureByteFormat, VkImageAspectFlagBits imageType, TextureTypeEnum textureType, bool useMipMaps)
        {
            Width = width;
            Height = height;
            TextureType = textureType;
            TextureByteFormat = textureByteFormat;

            if (useMipMaps)
            {
                MipMapLevels = (uint)(Math.Floor(Math.Log2(Math.Max(Width, Height)))) + 1;
            }

            CreateImageTexture(clearColor, useMipMaps);
            CreateTextureView(imageType);
            CreateTextureSampler();

        }

        public Texture(string filePath, VkFormat textureByteFormat, VkImageAspectFlagBits imageType, TextureTypeEnum textureType, bool useMipMaps)
        {
            TexturePath = filePath;
            TextureType = textureType;
            TextureByteFormat = textureByteFormat;

            if (useMipMaps)
            {
                MipMapLevels = (uint)(Math.Floor(Math.Log2(Math.Max(Width, Height)))) + 1;
            }

            CreateImageTexture(filePath, useMipMaps);
            CreateTextureView(imageType);
            CreateTextureSampler();
        }


        protected virtual void CreateImageTexture(Pixel clearColor, bool useMipMaps)
        {
            int width = Width;
            int height = Height;
            int depth = Depth;
            int colorChannels = 0;
            MipMapLevels = 1;

            VkDeviceMemory textureMemory = Memory;
            VkFormat textureByteFormat = TextureByteFormat;
            VkImage textureImage = Image;
            VkImageLayout textureImageLayout = TextureImageLayout;
            ColorComponents colorChannelUsed = ColorChannels;

            GameEngineImport.DLL_Texture_CreateImageTextureFromClearColor(
                VulkanRenderer.device,
                VulkanRenderer.physicalDevice,
                VulkanRenderer.commandPool,
                VulkanRenderer.graphicsQueue,
                ref width,
                ref height,
                ref depth,
                textureByteFormat,
                MipMapLevels,
                ref textureImage,
                ref textureMemory,
                ref textureImageLayout,
                ref colorChannelUsed,
                TextureUsageEnum.kUse_2DImageTexture,
                clearColor,
                useMipMaps);

            Width = width;
            Height = height;
            Depth = depth;
            ColorChannels = 0;
            MipMapLevels = 1;

            Memory = textureMemory;
            Image = textureImage;
            TextureImageLayout = textureImageLayout;
            ColorChannels = colorChannelUsed;
        }

        virtual protected void CreateImageTexture(string filePath, bool useMipMaps)
        {
            int width = Width;
            int height = Height;
            int depth = Depth;
            int colorChannels = 0;
            MipMapLevels = 1;

            VkDeviceMemory textureMemory = Memory;
            VkFormat textureByteFormat = TextureByteFormat;
            VkImage textureImage = Image;
            VkImageLayout textureImageLayout = TextureImageLayout;
            ColorComponents colorChannelUsed = ColorChannels;

            GameEngineImport.DLL_Texture_CreateImageTextureFromFile(
                VulkanRenderer.device,
                VulkanRenderer.physicalDevice,
                VulkanRenderer.commandPool,
                VulkanRenderer.graphicsQueue,
                ref width,
                ref height,
                ref depth,
                textureByteFormat,
                MipMapLevels,
                ref textureImage,
                ref textureMemory,
                ref textureImageLayout,
                ref colorChannelUsed,
                TextureUsageEnum.kUse_2DImageTexture,
                filePath,
                useMipMaps);

            Width = width;
            Height = height;
            Depth = depth;
            ColorChannels = 0;
            MipMapLevels = 1;

            Memory = textureMemory;
            Image = textureImage;
            TextureImageLayout = textureImageLayout;
            ColorChannels = colorChannelUsed;
        }

        public virtual VkResult CreateTextureView(VkImageAspectFlagBits imageType)
        {
            var result = GameEngineImport.DLL_Texture_CreateTextureView(VulkanRenderer.device, out VkImageView view, Image, TextureByteFormat, imageType, MipMapLevels);
            View = view;

            return result;
        }

        virtual protected void CreateTextureSampler()
        {
            VkSamplerCreateInfo textureImageSamplerInfo = new VkSamplerCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                magFilter = VkFilter.VK_FILTER_NEAREST,
                minFilter = VkFilter.VK_FILTER_NEAREST,
                mipmapMode = VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_NEAREST,
                addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
                addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
                addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
                mipLodBias = 0,
                anisotropyEnable = true,
                maxAnisotropy = 1.0f,
                compareEnable = false,
                compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS,
                minLod = 0,
                maxLod = MipMapLevels,
                borderColor = VkBorderColor.VK_BORDER_COLOR_INT_OPAQUE_BLACK,
                unnormalizedCoordinates = false,
            };

            CreateTextureSampler(textureImageSamplerInfo);
        }

        virtual protected void CreateTextureSampler(VkSamplerCreateInfo samplerCreateInfo)
        {
            GameEngineImport.DLL_Texture_CreateTextureSampler(VulkanRenderer.device, samplerCreateInfo, out VkSampler sampler);
            Sampler = sampler;
        }

        protected VkResult CreateImage(VkImageCreateInfo imageCreateInfo)
        {
            var result =  GameEngineImport.DLL_Texture_CreateImage(VulkanRenderer.device, VulkanRenderer.physicalDevice, out VkImage image, out VkDeviceMemory memory, imageCreateInfo);
            Image = image;
            Memory = memory;

            return result;
        }

        public VkDescriptorImageInfo GetTexturePropertiesBuffer()
        {
            return new VkDescriptorImageInfo
            {
                sampler = Sampler,
                imageView = View,
                imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
            };
        }

        public void SaveTexture(string filename, ExportTextureFormat textureFormat)
        {
            //Texture_SaveTexture(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, filename, SharedPtr<Texture>(this), textureFormat, ColorChannels);
        }

        public void UpdateTextureBufferIndex(uint bufferIndex)
        {
            TextureBufferIndex = bufferIndex;
        }

        public void Destroy()
        {

        }
    }
}
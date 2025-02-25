using GlmSharp;
using System;
using System.Linq;
using StbImageSharp;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using Silk.NET.Vulkan;
using VulkanGameEngineLevelEditor.Vulkan;
using Image = Silk.NET.Vulkan.Image;
using System.Collections.Generic;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Texture
    {
        public Vk vk = Vk.GetApi();
        public UInt64 TextureBufferIndex { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Depth { get; protected set; }
        public VkColorComponents ColorChannels { get; protected set; }
        public UInt32 MipMapLevels { get; protected set; }
        public TextureUsageEnum TextureUsage { get; protected set; }
        public TextureTypeEnum TextureType { get; protected set; }
        public VkFormat TextureByteFormat { get; protected set; }
        public VkImageLayout TextureImageLayout { get; protected set; }
        public VkSampleCountFlagBits SampleCount { get; protected set; }
        public VkImage Image { get; protected set; }
        public VkDeviceMemory Memory { get; protected set; }
        public VkImageView View { get; protected set; }
        public VkSampler Sampler { get; protected set; }
        public VkDescriptorImageInfo textureBuffer { get; protected set; }

        public Texture()
        {
            TextureBufferIndex = 0;
            Width = 1;
            Height = 1;
            Depth = 1;
            MipMapLevels = 1;

            //Image = Vk.ha;
            //Memory = VulkanConsts.VK_NULL_HANDLE;
            //View = VulkanConsts.VK_NULL_HANDLE;
            //Sampler = VulkanConsts.VK_NULL_HANDLE;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = TextureTypeEnum.kType_UndefinedTexture;
            TextureByteFormat = VkFormat.VK_FORMAT_UNDEFINED;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.Count1Bit;
        }

        public Texture(ivec2 TextureResolution)
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = VkFormat.R8G8B8A8Unorm;
            TextureImageLayout = VkImageLayout.ShaderReadOnlyOptimal;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
            MipMapLevels = 1;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();

        }

        public Texture(Pixel ClearColor, ivec2 TextureResolution, VkFormat TextureFormat)
        {
            TextureBufferIndex = 0;
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            MipMapLevels = 1;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = TextureTypeEnum.kType_UndefinedTexture;
            TextureByteFormat = TextureFormat;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
        }

        public Texture(ivec2 textureSize, VkFormat textureFormat)
        {
            TextureBufferIndex = 0;
            Width = textureSize.x;
            Height = textureSize.y;
            Depth = 1;
            MipMapLevels = 1;

            //Image = VulkanConsts.VK_NULL_HANDLE;
            //Memory = VulkanConsts.VK_NULL_HANDLE;
            //View = VulkanConsts.VK_NULL_HANDLE;
            //Sampler = VulkanConsts.VK_NULL_HANDLE;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = TextureTypeEnum.kType_UndefinedTexture;
            TextureByteFormat = textureFormat;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
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
            SampleCount = VkSampleCountFlagBits.Count1Bit;

            CreateImageTexture(filePath);
            CreateTextureView();
            CreateTextureSampler();
        }

        public static Texture CreateTexture(ivec2 TextureResolution)
        {
            Texture texture = MemoryManager.AllocateTexture();
            texture.Initialize(TextureResolution);
            return texture;

        }

        public static Texture CreateTexture(Pixel ClearColor, ivec2 TextureResolution, Format TextureFormat)
        {
            Texture texture = MemoryManager.AllocateTexture();
            texture.Initialize(ClearColor, TextureResolution, TextureFormat);
            return texture;
        }

        public static Texture CreateTexture(ivec2 textureSize, VkFormat textureFormat)
        {
            Texture texture = MemoryManager.AllocateTexture();
            texture.Initialize(textureSize, textureFormat);
            return texture;
        }

        public static Texture CreateTexture(string filePath, VkFormat textureByteFormat, VkTextureTypeEnum textureType)
        {
            Texture texture = MemoryManager.AllocateTexture();
            texture.Initialize(filePath, textureByteFormat, textureType);
            return texture;
        }

        public void Initialize(ivec2 TextureResolution)
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = VkFormat.R8G8B8A8Unorm;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
            MipMapLevels = 1;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();

        }

        public void Initialize(Pixel ClearColor, ivec2 TextureResolution, VkFormat TextureFormat)
        {
            TextureBufferIndex = 0;
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            MipMapLevels = 1;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = TextureTypeEnum.kType_UndefinedTexture;
            TextureByteFormat = TextureFormat;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
        }

        public void Initialize(ivec2 textureSize, Format textureFormat)
        {
            TextureBufferIndex = 0;
            Width = textureSize.x;
            Height = textureSize.y;
            Depth = 1;
            MipMapLevels = 1;

            //Image = VulkanConsts.VK_NULL_HANDLE;
            //Memory = VulkanConsts.VK_NULL_HANDLE;
            //View = VulkanConsts.VK_NULL_HANDLE;
            //Sampler = VulkanConsts.VK_NULL_HANDLE;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = TextureTypeEnum.kType_UndefinedTexture;
            TextureByteFormat = textureFormat;
            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
        }

        public void Initialize(string filePath, Format textureByteFormat, TextureTypeEnum textureType)
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

        protected virtual void CreateImageTexture()
        {
            ColorChannels = ColorComponents.RedGreenBlueAlpha;
            var size = (ulong)(Width * Height * (uint)ColorChannels);
            byte[] pixels = new byte[size];

            GCHandle pixelHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            try
            {
                for (int i = 0; i < pixels.Length; i += 4)
                {
                    pixels[i] = 255;
                    pixels[i + 1] = 0;
                    pixels[i + 2] = 0;
                    pixels[i + 3] = 255;
                }

                GCHandle handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                IntPtr dataPtr = handle.AddrOfPinnedObject();
                VulkanBuffer<byte> buffer = new VulkanBuffer<byte>((void*)dataPtr, (uint)size, BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, false);
                var tempBuffer = buffer.Buffer;

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
                    usage = VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                    sharingMode = VkSharingMode.Exclusive,
                    initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED
                };

                CTexture.CreateTextureImage(imageInfo, out VkImage tempImage, out VkDeviceMemory memory, Width, Height, TextureByteFormat, MipMapLevels);
                CTexture.QuickTransitionImageLayout(tempImage, VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, MipMapLevels, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT);
                CTexture.CopyBufferToTexture(ref tempBuffer, tempImage, new Extent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 }, TextureUsage, ImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT);

                Memory = memory;
                Image = tempImage;

                handle.Free();
            }
            finally
            {
                pixelHandle.Free();
            }
        }

        virtual protected void CreateImageTexture(string FilePath)
        {
            using (var stream = File.OpenRead(FilePath))
            {
                ImageResult image = ImageResult.FromStream(stream);
                Width = image.Width;
                Height = image.Height;
                ColorChannels = image.Comp;

                var size = (ulong)(Width * Height * (uint)ColorChannels);

                GCHandle handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                IntPtr dataPtr = handle.AddrOfPinnedObject();
                VulkanBuffer<byte> buffer = new VulkanBuffer<byte>((void*)dataPtr, (uint)size, VkBufferUsageFlags.BufferUsageTransferSrcBit | VkBufferUsageFlags.BufferUsageTransferDstBit, VkMemoryPropertyFlags.MemoryPropertyHostVisibleBit | VkMemoryPropertyFlags.MemoryPropertyHostCoherentBit, false);
                var tempBuffer = buffer.Buffer;

                VkImageCreateInfo imageInfo = new VkImageCreateInfo
                {
                    sType = StructureType.ImageCreateInfo,
                    imageType = ImageType.ImageType2D,
                    format = TextureByteFormat,
                    extent = new Extent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 },
                    mipLevels = MipMapLevels,
                    arrayLayers = 1,
                    samples = SampleCountFlags.Count1Bit,
                    tiling = ImageTiling.Optimal,
                    usage = ImageUsageFlags.ImageUsageTransferSrcBit |
                    ImageUsageFlags.SampledBit |
                    ImageUsageFlags.ColorAttachmentBit |
                    ImageUsageFlags.ImageUsageTransferDstBit,
                    sharingMode = SharingMode.Exclusive,
                    initialLayout = VkImageLayout.Undefined
                };

                CTexture.CreateTextureImage(imageInfo, out VkImage tempImage, out VkDeviceMemory memory, Width, Height, TextureByteFormat, MipMapLevels);
                CTexture.QuickTransitionImageLayout(tempImage, TextureImageLayout, VkImageLayout.TransferDstOptimal, MipMapLevels, VkImageAspectFlags.ColorBit);
                CTexture.CopyBufferToTexture(ref tempBuffer, tempImage, new VkExtent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 }, TextureUsage, VkImageAspectFlags.ColorBit);

                Memory = memory;
                Image = tempImage;
                handle.Free();
            }
        }

        virtual protected Result CreateTextureImage(ImageCreateInfo createInfo)
        {
            Image textureImage;
            DeviceMemory textureMemory;

            var info = createInfo;
            var result = vk.CreateImage(VulkanRenderer.device, &info, null, &textureImage);
            vk.GetImageMemoryRequirements(VulkanRenderer.device, textureImage, out VkMemoryRequirements memRequirements);

            var allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, VkMemoryPropertyFlags.DeviceLocalBit)
            };
            result = vk.AllocateMemory(VulkanRenderer.device, &allocInfo, null, &textureMemory);
            result = vk.BindImageMemory(VulkanRenderer.device, textureImage, textureMemory, 0);

            Image = textureImage;
            Memory = textureMemory;

            return result;
        }

        public virtual VkResult CreateTextureView()
        {
            VkImageSubresourceRange imageSubresourceRange = new VkImageSubresourceRange()
            {
                AspectMask = ImageAspectFlags.ColorBit,
                BaseMipLevel = 0,
                LevelCount = MipMapLevels,
                BaseArrayLayer = 0,
                LayerCount = 1,
            };

            VkImageViewCreateInfo TextureImageViewInfo = new VkImageViewCreateInfo()
            {
                SType = VkStructureType.ImageViewCreateInfo,
                Image = Image,
                ViewType = VkImageViewType.ImageViewType2D,
                Format = TextureByteFormat,
                SubresourceRange = imageSubresourceRange
            };
            VkResult result = vk.CreateImageView(VulkanRenderer.device, &TextureImageViewInfo, null, out VkImageView textureView);
            View = textureView;
            return result;
        }

        protected void CreateTextureSampler(VkSamplerCreateInfo samplerInfo)
        {
            VkSampler sampler = new SVkampler();
            VkSamplerCreateInfo info = samplerInfo;
            Result result = vk.CreateSampler(VulkanRenderer.device, ref info, null, out sampler);
            Sampler = sampler;
        }

        virtual protected void CreateTextureSampler()
        {
            VkSamplerCreateInfo textureImageSamplerInfo = new VkSamplerCreateInfo
            {
                sType = VkStructureType.SamplerCreateInfo,
                magFilter = VkFilter.Nearest,
                minFilter = VkFilter.Nearest,
                mipmapMode = VkSamplerMipmapMode.Linear,
                addressModeU = VkSamplerAddressMode.Repeat,
                addressModeV = VkSamplerAddressMode.Repeat,
                addressModeW = VkSamplerAddressMode.Repeat,
                mipLodBias = 0,
                anisotropyEnable = true,
                maxAnisotropy = 16.0f,
                compareEnable = false,
                compareOp = CompareOp.Always,
                minLod = 0,
                maxLod = MipMapLevels,
                borderColor = BorderColor.IntOpaqueBlack,
                unnormalizedCoordinates = false,
            };

            VkSampler sampler = new VkSampler();
            VkResult result = vk.CreateSampler(VulkanRenderer.device, ref textureImageSamplerInfo, null, out sampler);
            Sampler = sampler;
        }

        public void UpdateImageLayout(VkCommandBuffer cmdBuffer, VkImageLayout newImageLayout, VkImageAspectFlagBits imageAspectFlags)
        {
            var oldImageLayout = TextureImageLayout;
            CTexture.UpdateImageLayout(cmdBuffer, Image, ref oldImageLayout, newImageLayout, MipMapLevels, imageAspectFlags);
            TextureImageLayout = oldImageLayout;
        }

        public void UpdateImageLayout(VkCommandBuffer cmdBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout, VkImageAspectFlagBits imageAspectFlags)
        {
            CTexture.UpdateImageLayout(cmdBuffer, Image, ref oldImageLayout, newImageLayout, MipMapLevels, imageAspectFlags);
            TextureImageLayout = oldImageLayout;
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

        virtual public void Destroy()
        {

        }
    }
}
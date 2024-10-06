using GlmSharp;
using StbImageSharp;
using Silk.NET.Vulkan;
using System;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class BakeTexture : Texture
    {
        public BakeTexture() : base()
        {
        }

        public BakeTexture(string filePath, Format textureByteFormat, TextureTypeEnum textureType) : base()
        {
        }

        public BakeTexture(Pixel clearColor, ivec2 textureResolution, Format textureFormat) : base()
        {
            Width = textureResolution.x;
            Height = textureResolution.y;
            Depth = 1;
            TextureImageLayout = ImageLayout.Undefined;
            SampleCount = SampleCountFlags.Count1Bit;
            TextureByteFormat = textureFormat;

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
                pixels[i] = new Pixel(0x00, 0x00, 0xFF, 0xFF);
            }

            GCHandle pixelHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            IntPtr dataPtr = pixelHandle.AddrOfPinnedObject();

            VulkanBuffer <byte> stagingBuffer = new VulkanBuffer<byte>(
                (void*)dataPtr,
                size,
                BufferUsageFlags.BufferUsageTransferSrcBit,
                MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, false
            );
            var bHandle = stagingBuffer.Buffer;

            
            CreateTextureImage();
            QuickTransitionImageLayout(Image, TextureImageLayout, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal);
            CTexture.CopyBufferToTexture(ref bHandle, Image, new Extent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 }, TextureUsage);
            // GenerateMipmaps();

            pixelHandle.Free();
            //stagingBuffer.DestroyBuffer();
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
                Tiling = ImageTiling.Linear,
                Usage = ImageUsageFlags.ImageUsageTransferSrcBit |
                        ImageUsageFlags.ImageUsageSampledBit |
                        ImageUsageFlags.ImageUsageTransferDstBit,
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

        protected Result CreateTextureView()
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
                    AspectMask = ImageAspectFlags.ImageAspectColorBit
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
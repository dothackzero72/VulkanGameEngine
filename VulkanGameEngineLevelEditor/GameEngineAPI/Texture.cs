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

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Texture
    {
        public Vk vk = Vk.GetApi();
        public UInt64 TextureBufferIndex { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Depth { get; protected set; }
        public ColorComponents ColorChannels { get; protected set; }
        public UInt32 MipMapLevels { get; protected set; }
        public TextureUsageEnum TextureUsage { get; protected set; }
        public TextureTypeEnum TextureType { get; protected set; }
        public Format TextureByteFormat { get; protected set; }
        public Silk.NET.Vulkan.ImageLayout TextureImageLayout { get; protected set; }
        public SampleCountFlags SampleCount { get; protected set; }
        public Image Image { get; protected set; }
        public DeviceMemory Memory { get; protected set; }
        public ImageView View { get; protected set; }
        public Sampler Sampler { get; protected set; }
        public DescriptorImageInfo textureBuffer { get; protected set; }
        public byte[] Data { get; set; }

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
            TextureByteFormat = Format.Undefined;
            TextureImageLayout = Silk.NET.Vulkan.ImageLayout.Undefined;
            SampleCount = SampleCountFlags.Count1Bit;
        }

        public Texture(ivec2 TextureResolution) : base()
        {
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            TextureByteFormat = Format.R8G8B8A8Unorm;
            TextureImageLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
            SampleCount = SampleCountFlags.SampleCount1Bit;
            MipMapLevels = 1;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();

        }

        public Texture(Pixel ClearColor, ivec2 TextureResolution, Format TextureFormat)
        {
            TextureBufferIndex = 0;
            Width = TextureResolution.x;
            Height = TextureResolution.y;
            Depth = 1;
            MipMapLevels = 1;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = TextureTypeEnum.kType_UndefinedTexture;
            TextureByteFormat = TextureFormat;
            TextureImageLayout = Silk.NET.Vulkan.ImageLayout.Undefined;
            SampleCount = SampleCountFlags.Count1Bit;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
        }

        public Texture(ivec2 textureSize, Format textureFormat)
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
            TextureImageLayout = Silk.NET.Vulkan.ImageLayout.Undefined;
            SampleCount = SampleCountFlags.Count1Bit;

            CreateImageTexture();
            CreateTextureView();
            CreateTextureSampler();
        }

        public Texture(string filePath, Format textureByteFormat, TextureTypeEnum textureType)
        {
            TextureBufferIndex = 0;
            Width = 1;
            Height = 1;
            Depth = 1;
            MipMapLevels = 1;

            TextureUsage = TextureUsageEnum.kUse_Undefined;
            TextureType = textureType;
            TextureByteFormat = textureByteFormat;
            TextureImageLayout = Silk.NET.Vulkan.ImageLayout.Undefined;
            SampleCount = SampleCountFlags.Count1Bit;

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
                    pixels[i] = 255;   // R
                    pixels[i + 1] = 0; // G
                    pixels[i + 2] = 0; // B
                    pixels[i + 3] = 255; // A
                }

                GCHandle handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                IntPtr dataPtr = handle.AddrOfPinnedObject();
                VulkanBuffer<byte> buffer = new VulkanBuffer<byte>((void*)dataPtr, (uint)size, BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, false);
                var tempBuffer = buffer.Buffer;

                ImageCreateInfo imageInfo = new ImageCreateInfo
                {
                    SType = StructureType.ImageCreateInfo,
                    ImageType = ImageType.ImageType2D,
                    Format = TextureByteFormat,
                    Extent = new Extent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 },
                    MipLevels = MipMapLevels,
                    ArrayLayers = 1,
                    Samples = SampleCountFlags.Count1Bit,
                    Tiling = ImageTiling.Optimal,
                    Usage = ImageUsageFlags.ImageUsageTransferSrcBit |
                            ImageUsageFlags.SampledBit |
                            ImageUsageFlags.ColorAttachmentBit |
                            ImageUsageFlags.ImageUsageTransferDstBit,
                    SharingMode = SharingMode.Exclusive,
                    InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined
                };

                CTexture.CreateTextureImage(imageInfo, out Silk.NET.Vulkan.Image tempImage, out DeviceMemory memory, Width, Height, TextureByteFormat, MipMapLevels);
                CTexture.QuickTransitionImageLayout(tempImage, Silk.NET.Vulkan.ImageLayout.Undefined, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, MipMapLevels, ImageAspectFlags.ColorBit);
                CTexture.CopyBufferToTexture(ref tempBuffer, tempImage, new Extent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 }, TextureUsage, ImageAspectFlags.ColorBit);

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
                Data = image.Data.ToArray();

                var size = (ulong)(Width * Height * (uint)ColorChannels);

                GCHandle handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                IntPtr dataPtr = handle.AddrOfPinnedObject();
                VulkanBuffer<byte> buffer = new VulkanBuffer<byte>((void*)dataPtr, (uint)size, BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, false);
                var tempBuffer = buffer.Buffer;

                ImageCreateInfo imageInfo = new ImageCreateInfo
                {
                    SType = StructureType.ImageCreateInfo,
                    ImageType = ImageType.ImageType2D,
                    Format = TextureByteFormat,
                    Extent = new Extent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 },
                    MipLevels = MipMapLevels,
                    ArrayLayers = 1,
                    Samples = SampleCountFlags.Count1Bit,
                    Tiling = ImageTiling.Optimal,
                    Usage = ImageUsageFlags.ImageUsageTransferSrcBit |
                    ImageUsageFlags.SampledBit |
                    ImageUsageFlags.ColorAttachmentBit |
                    ImageUsageFlags.ImageUsageTransferDstBit,
                    SharingMode = SharingMode.Exclusive,
                    InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined
                };

                CTexture.CreateTextureImage(imageInfo,out Silk.NET.Vulkan.Image tempImage, out DeviceMemory memory, Width, Height, TextureByteFormat, MipMapLevels);
                CTexture.QuickTransitionImageLayout(tempImage, TextureImageLayout, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, MipMapLevels, ImageAspectFlags.ColorBit);
                CTexture.CopyBufferToTexture(ref tempBuffer, tempImage, new Extent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1}, TextureUsage, ImageAspectFlags.ColorBit);

                Memory = memory;
                Image = tempImage;
                handle.Free();
            }
        }

        public virtual Result CreateTextureView()
        {
            ImageSubresourceRange imageSubresourceRange = new ImageSubresourceRange()
            {
                AspectMask = ImageAspectFlags.ColorBit,
                BaseMipLevel = 0,
                LevelCount = MipMapLevels,
                BaseArrayLayer = 0,
                LayerCount = 1,
            };

            ImageViewCreateInfo TextureImageViewInfo = new ImageViewCreateInfo()
            {
                SType = StructureType.ImageViewCreateInfo,
                Image = Image,
                ViewType = ImageViewType.ImageViewType2D,
                Format = TextureByteFormat,
                SubresourceRange = imageSubresourceRange
            };
            Result result = VKConst.vulkan.CreateImageView(SilkVulkanRenderer.device, &TextureImageViewInfo, null, out ImageView textureView);
            View = textureView;
            return result;
        }


        virtual protected void CreateTextureSampler()
        {
            SamplerCreateInfo textureImageSamplerInfo = new SamplerCreateInfo
            {
                SType = StructureType.SamplerCreateInfo,
                MagFilter = Filter.Nearest,
                MinFilter = Filter.Nearest,
                MipmapMode = SamplerMipmapMode.Linear,
                AddressModeU = SamplerAddressMode.Repeat,
                AddressModeV = SamplerAddressMode.Repeat,
                AddressModeW = SamplerAddressMode.Repeat,
                MipLodBias = 0,
                AnisotropyEnable = Vk.True,
                MaxAnisotropy = 16.0f,
                CompareEnable = Vk.False,
                CompareOp = CompareOp.Always,
                MinLod = 0,
                MaxLod = MipMapLevels,
                BorderColor = BorderColor.IntOpaqueBlack,
                UnnormalizedCoordinates = Vk.False,
            };

            Sampler sampler = new Sampler();
            Result result = VKConst.vulkan.CreateSampler(SilkVulkanRenderer.device, ref textureImageSamplerInfo, null, out sampler);
            Sampler = sampler;
        }

        public void UpdateImageLayout(CommandBuffer cmdBuffer, ImageLayout newImageLayout, ImageAspectFlags imageAspectFlags)
        {
            var oldImageLayout = TextureImageLayout;
            CTexture.UpdateImageLayout(cmdBuffer, Image, ref oldImageLayout, newImageLayout, MipMapLevels, imageAspectFlags);
            TextureImageLayout = oldImageLayout;
        }

        public void UpdateImageLayout(CommandBuffer cmdBuffer, ImageLayout oldImageLayout, ImageLayout newImageLayout, ImageAspectFlags imageAspectFlags)
        {
            CTexture.UpdateImageLayout(cmdBuffer, Image, ref oldImageLayout, newImageLayout, MipMapLevels, imageAspectFlags);
            TextureImageLayout = oldImageLayout;
        }

        public DescriptorImageInfo GetTextureBuffer()
        {
            return new DescriptorImageInfo
            {
                Sampler = Sampler,
                ImageView = View,
                ImageLayout = Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal
            };
        }

        virtual public void Destroy()
        {

        }
    }
}
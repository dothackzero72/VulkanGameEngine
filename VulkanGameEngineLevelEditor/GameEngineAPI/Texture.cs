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

            // Fill pixel data (example) here if needed

            GCHandle pixelHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            try
            {
                void* dataPtr = (void*)pixelHandle.AddrOfPinnedObject();

                VulkanBuffer<byte> stagingBuffer = new VulkanBuffer<byte>(
                    dataPtr,
                    (uint)size,
                     BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit,
                    MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit | MemoryPropertyFlags.DeviceLocalBit, false
                );

                // Initialize and transition the texture image
                if (CTexture.CreateTextureImage(out Image tempImage, out DeviceMemory memory, Width, Height, TextureByteFormat, MipMapLevels) == Result.Success)
                {
                    // Perform the copy here
                    QuickTransitionImageLayout(tempImage, TextureImageLayout, ImageLayout.TransferDstOptimal);
                    CTexture.CopyBufferToTexture(ref stagingBuffer.Buffer, tempImage, new Extent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 }, TextureUsage);

                    // Set class properties
                    Image = tempImage;
                    Memory = memory;
                }
            }
            finally
            {
                pixelHandle.Free();
            }
        }

        protected Result QuickTransitionImageLayout(Image image, Silk.NET.Vulkan.ImageLayout oldLayout, Silk.NET.Vulkan.ImageLayout newLayout)
        {
            CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
            CTexture.TransitionImageLayout(commandBuffer, image, MipMapLevels, ref oldLayout, newLayout);
            Result result = SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            return result;
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
                // MipMapLevels = (uint)Math.Floor(Math.Log(Math.Max(Width, Height)) / Math.Log(2)) + 1;

                 GCHandle handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                IntPtr dataPtr = handle.AddrOfPinnedObject();

                //var buffer = new VulkanBuffer<byte>((void*)dataPtr, (uint)(Width * Height * (int)ColorChannels), BufferUsageFlags.TransferSrcBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit);
                VulkanBuffer<byte> buffer = new VulkanBuffer<byte>((void*)dataPtr, (uint)size, BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, false);
                var bHandle = buffer.Buffer;

                var tempBuffer = buffer.Buffer;
                CTexture.CreateTextureImage(out Silk.NET.Vulkan.Image tempImage, out DeviceMemory memory, Width, Height, TextureByteFormat, MipMapLevels);
                QuickTransitionImageLayout(tempImage, TextureImageLayout, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal);
                CTexture.CopyBufferToTexture(ref tempBuffer, tempImage, new Extent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1}, TextureUsage);
                // GenerateMipmaps();

                Memory = memory;
                Image = tempImage;
                handle.Free();
               // buffer.DestroyBuffer();
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

        public void UpdateImageLayout(CommandBuffer cmdBuffer, ImageLayout newImageLayout)
        {
            var oldImageLayout = TextureImageLayout;
            CTexture.UpdateImageLayout(cmdBuffer, Image, ref oldImageLayout, newImageLayout, MipMapLevels);
            TextureImageLayout = oldImageLayout;
        }

        public void UpdateImageLayout(CommandBuffer cmdBuffer, ImageLayout oldImageLayout, ImageLayout newImageLayout)
        {
            CTexture.UpdateImageLayout(cmdBuffer, Image, ref oldImageLayout, newImageLayout, MipMapLevels);
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

        [DllImport("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DLL_stbi_write_bmp(string filename, int w, int h, int comp, void* data);
        protected void WriteImage(byte[] imageData, int width, int height, int channels)
        {
            int result = DLL_stbi_write_bmp("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\asdfa", width, height, channels, (void*)imageData.ToArray()[0]);
            if (result == 0)
            {
                Console.WriteLine("Failed to write image.");
            }
            else
            {
                Console.WriteLine("Image written successfully.");
            }
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
    }
}
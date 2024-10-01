using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor;
using StbImageSharp;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;
using Silk.NET.Vulkan;
using Image = Silk.NET.Vulkan.Image;
using Silk.NET.Core.Native;
using Silk.NET.GLFW;
using VulkanGameEngineLevelEditor.Tests;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Texture
    {
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
            uint size = (uint)Width * (uint)Height * (uint)ColorChannels;

            // Create an array of Pixel initialized with clear color (black with alpha 255)
            Pixel[] pixels = new Pixel[Width * Height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Pixel(0x00, 0x00, 0x00, 0xFF); // Black color with full alpha
            }

            GCHandle pixelHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            IntPtr dataPtr;
            dataPtr = pixelHandle.AddrOfPinnedObject();

            // Create the staging buffer and upload pixel data to it
            VulkanBuffer<Pixel> stagingBuffer = new VulkanBuffer<Pixel>(
                (void*)dataPtr,
                size,
                BufferUsageFlags.BufferUsageTransferSrcBit,
                MemoryPropertyFlags.MemoryPropertyHostVisibleBit |
                MemoryPropertyFlags.HostCoherentBit
            );
            var bHandle = stagingBuffer.Buffer;

            CreateTextureImage();
            Texture_QuickTransitionImageLayout(TextureImageLayout, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal);
            CopyBufferToTexture(ref bHandle);
            // GenerateMipmaps();

            pixelHandle.Free();
           // stagingBuffer.DestroyBuffer();
        }

        protected Result Texture_QuickTransitionImageLayout(Silk.NET.Vulkan.ImageLayout oldLayout, Silk.NET.Vulkan.ImageLayout newLayout)
        {
            Image image = Image;
            CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
            TransitionImageLayout(commandBuffer, oldLayout, newLayout);
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
                // MipMapLevels = (uint)Math.Floor(Math.Log(Math.Max(Width, Height)) / Math.Log(2)) + 1;

                GCHandle handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                IntPtr dataPtr = handle.AddrOfPinnedObject();

                var buffer = new VulkanBuffer<byte>((void*)dataPtr, (uint)(Width * Height * (int)ColorChannels), BufferUsageFlags.TransferSrcBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit);
                var bHandle = buffer.Buffer;

                //var a = buffer.CheckBufferContents();

                CreateTextureImage();
                Texture_QuickTransitionImageLayout(TextureImageLayout, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal);
                CopyBufferToTexture(ref bHandle);
                // GenerateMipmaps();

                handle.Free();
               // buffer.DestroyBuffer();
            }
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

        public DescriptorImageInfo GetTextureBuffer()
        {
            return new DescriptorImageInfo
            {
                Sampler = Sampler,
                ImageView = View,
                ImageLayout = Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal
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

        private void CreateBuffer(uint size, BufferUsageFlags usage, MemoryPropertyFlags properties, Silk.NET.Vulkan.Buffer* buffer, DeviceMemory* bufferMemory)
        {
            BufferCreateInfo bufferInfo = new BufferCreateInfo
            {
                SType = StructureType.BufferCreateInfo,
                Size = size,
                Usage = usage,
                SharingMode = SharingMode.Exclusive
            };
            var BufferInfo = bufferInfo;
            VKConst.vulkan.CreateBuffer(SilkVulkanRenderer.device, &BufferInfo, null, buffer);

            MemoryRequirements memRequirements;
            VKConst.vulkan.GetBufferMemoryRequirements(SilkVulkanRenderer.device, *buffer, &memRequirements);

            MemoryAllocateInfo allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, properties)
            };

            VKConst.vulkan.AllocateMemory(SilkVulkanRenderer.device, &allocInfo, null, bufferMemory);
            VKConst.vulkan.BindBufferMemory(SilkVulkanRenderer.device, *buffer, *bufferMemory, 0);
        }

        public void UpdateImageLayout(CommandBuffer commandBuffer, Silk.NET.Vulkan.ImageLayout newImageLayout)
        {
            UpdateImageLayout(commandBuffer, newImageLayout, MipMapLevels);
        }

        public void UpdateImageLayout(CommandBuffer commandBuffer, Silk.NET.Vulkan.ImageLayout newImageLayout, uint MipLevel)
        {
            ImageSubresourceRange ImageSubresourceRange = new ImageSubresourceRange();
            ImageSubresourceRange.AspectMask = ImageAspectFlags.ColorBit;
            ImageSubresourceRange.LevelCount = 1;
            ImageSubresourceRange.LayerCount = 1;

            ImageMemoryBarrier barrier = new ImageMemoryBarrier();
            barrier.SType = StructureType.ImageMemoryBarrier;
            barrier.OldLayout = TextureImageLayout;
            barrier.NewLayout = newImageLayout;
            barrier.Image = Image;
            barrier.SubresourceRange = ImageSubresourceRange;
            barrier.SrcAccessMask = 0;
            barrier.DstAccessMask = AccessFlags.TransferWriteBit;

            VKConst.vulkan.CmdPipelineBarrier(commandBuffer, PipelineStageFlags.AllCommandsBit, PipelineStageFlags.AllCommandsBit, 0, 0, null, 0, null, 1, &barrier);
            TextureImageLayout = newImageLayout;
        }

        public void UpdateImageLayout(CommandBuffer commandBuffer, Silk.NET.Vulkan.ImageLayout oldImageLayout, Silk.NET.Vulkan.ImageLayout newImageLayout, uint MipLevel)
        {
            ImageSubresourceRange ImageSubresourceRange = new ImageSubresourceRange();
            ImageSubresourceRange.AspectMask = ImageAspectFlags.ColorBit;
            ImageSubresourceRange.BaseMipLevel = MipLevel;
            ImageSubresourceRange.LevelCount = Vk.RemainingMipLevels;
            ImageSubresourceRange.LayerCount = 1;

            ImageMemoryBarrier barrier = new ImageMemoryBarrier();
            barrier.SType = StructureType.ImageMemoryBarrier;
            barrier.OldLayout = oldImageLayout;
            barrier.NewLayout = newImageLayout;
            barrier.Image = Image;
            barrier.SubresourceRange = ImageSubresourceRange;
            barrier.SrcAccessMask = 0;
            barrier.DstAccessMask = AccessFlags.TransferWriteBit;

            VKConst.vulkan.CmdPipelineBarrier(commandBuffer, PipelineStageFlags.AllCommandsBit, PipelineStageFlags.AllCommandsBit, 0, 0, null, 0, null, 1, &barrier);
            TextureImageLayout = newImageLayout;
        }


        public byte[] GetTextureData()
        {
            if (Data == null || Width <= 0 || Height <= 0)
            {
                throw new InvalidOperationException("Invalid texture data.");
            }

            var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            byte* ptr = (byte*)bmpData.Scan0.ToPointer();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int dataIndex = (y * Width + x) * 4;
                    ptr[dataIndex + 0] = Data[dataIndex + 2];
                    ptr[dataIndex + 1] = Data[dataIndex + 1];
                    ptr[dataIndex + 2] = Data[dataIndex + 0];
                    ptr[dataIndex + 3] = Data[dataIndex + 3];
                }
            }

            byte[] data = new byte[Width * Height * 4];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {

                    int index = (y * Width + x) * 4;
                    data[index + 0] = 255;
                    data[index + 1] = 255;
                    data[index + 2] = 255;
                    data[index + 3] = 255;
                }
            }

            return data;
        }

        virtual protected Result CreateTextureImage()
        {
            Image textureImage;
            DeviceMemory textureMemory;

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
                        ImageUsageFlags.ImageUsageTransferDstBit,
                SharingMode = SharingMode.Exclusive,
                InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined
            };

            Result result = VKConst.vulkan.CreateImage(SilkVulkanRenderer.device, &imageInfo, null, &textureImage);
            if (result != Result.Success)
            {

            }

            MemoryRequirements memRequirements;
            VKConst.vulkan.GetImageMemoryRequirements(SilkVulkanRenderer.device, textureImage, &memRequirements);

            MemoryAllocateInfo allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, MemoryPropertyFlags.DeviceLocalBit)
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

        protected virtual Result CreateTextureView()
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

        public Result TransitionImageLayout(CommandBuffer commandBuffer, Silk.NET.Vulkan.ImageLayout oldLayout, Silk.NET.Vulkan.ImageLayout newLayout)
        {
            // VkImageLayout oldLayout = TextureImageLayout;
            var image = Image;
            var mipmapLevels = MipMapLevels;

            PipelineStageFlags sourceStage = PipelineStageFlags.AllCommandsBit;
            PipelineStageFlags destinationStage = PipelineStageFlags.AllCommandsBit;
            ImageMemoryBarrier barrier = new ImageMemoryBarrier()
            {
                SType = StructureType.ImageMemoryBarrier,
                OldLayout = TextureImageLayout,
                NewLayout = newLayout,
                SrcQueueFamilyIndex = Vk.QueueFamilyIgnored,
                DstQueueFamilyIndex = Vk.QueueFamilyIgnored,
                Image = image,

                SubresourceRange = new ImageSubresourceRange()
                {
                    AspectMask = ImageAspectFlags.ColorBit,
                    LevelCount = mipmapLevels,
                    BaseArrayLayer = 0,
                    BaseMipLevel = 0,
                    LayerCount = Vk.RemainingArrayLayers,
                }
            };
            if (TextureImageLayout == Silk.NET.Vulkan.ImageLayout.Undefined &&
                newLayout == Silk.NET.Vulkan.ImageLayout.TransferDstOptimal)
            {
                barrier.SrcAccessMask = 0;
                barrier.DstAccessMask = AccessFlags.TransferWriteBit;

                sourceStage = PipelineStageFlags.TopOfPipeBit;
                destinationStage = PipelineStageFlags.TransferBit;
            }
            else if (TextureImageLayout == Silk.NET.Vulkan.ImageLayout.TransferDstOptimal &&
                     newLayout == Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal)
            {
                barrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                barrier.DstAccessMask = AccessFlags.AccessMemoryReadBit;

                sourceStage = PipelineStageFlags.TransferBit;
                destinationStage = PipelineStageFlags.FragmentShaderBit;
            }

            VKConst.vulkan.CmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, null, 0, null, 1, &barrier);
            TextureImageLayout = newLayout;
            return Result.Success;
        }

        public Result TransitionImageLayout(Silk.NET.Vulkan.ImageLayout oldLayout, Silk.NET.Vulkan.ImageLayout newLayout)
        {
            var image = Image;
            var mipmapLevels = MipMapLevels;
            PipelineStageFlags sourceStage = PipelineStageFlags.AllCommandsBit;
            PipelineStageFlags destinationStage = PipelineStageFlags.AllCommandsBit;

            CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();

            ImageMemoryBarrier barrier = new ImageMemoryBarrier()
            {
                SType = StructureType.ImageMemoryBarrier,
                OldLayout = oldLayout,
                NewLayout = newLayout,
                SrcQueueFamilyIndex = Vk.QueueFamilyIgnored,
                DstQueueFamilyIndex = Vk.QueueFamilyIgnored,
                Image = image,

                SubresourceRange = new ImageSubresourceRange()
                {
                    AspectMask = ImageAspectFlags.ColorBit,
                    LevelCount = mipmapLevels,
                    BaseArrayLayer = 0,
                    BaseMipLevel = 0,
                    LayerCount = Vk.RemainingArrayLayers,
                }
            };
            if (TextureImageLayout == Silk.NET.Vulkan.ImageLayout.Undefined &&
                newLayout == Silk.NET.Vulkan.ImageLayout.TransferDstOptimal)
            {
                barrier.SrcAccessMask = 0;
                barrier.DstAccessMask = AccessFlags.TransferWriteBit;

                sourceStage = PipelineStageFlags.TopOfPipeBit;
                destinationStage = PipelineStageFlags.TransferBit;
            }
            else if (TextureImageLayout == Silk.NET.Vulkan.ImageLayout.TransferDstOptimal &&
                     newLayout == Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal)
            {
                barrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                barrier.DstAccessMask = AccessFlags.AccessMemoryReadBit;

                sourceStage = PipelineStageFlags.TransferBit;
                destinationStage = PipelineStageFlags.FragmentShaderBit;
            }

            VKConst.vulkan.CmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, null, 0, null, 1, &barrier);
            return SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        private Result CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, Silk.NET.Vulkan.ImageLayout newLayout)
        {
            var image = Image;
            var mipmapLevels = MipMapLevels;
            PipelineStageFlags sourceStage = PipelineStageFlags.AllCommandsBit;
            PipelineStageFlags destinationStage = PipelineStageFlags.AllCommandsBit;

            ImageMemoryBarrier barrier = new ImageMemoryBarrier()
            {
                SType = StructureType.ImageMemoryBarrier,
                OldLayout = TextureImageLayout,
                NewLayout = newLayout,
                SrcQueueFamilyIndex = Vk.QueueFamilyIgnored,
                DstQueueFamilyIndex = Vk.QueueFamilyIgnored,
                Image = image,

                SubresourceRange = new ImageSubresourceRange()
                {
                    AspectMask = ImageAspectFlags.ColorBit,
                    LevelCount = mipmapLevels,
                    BaseArrayLayer = 0,
                    BaseMipLevel = 0,
                    LayerCount = Vk.RemainingArrayLayers,
                }
            };
            if (TextureImageLayout == Silk.NET.Vulkan.ImageLayout.Undefined &&
                newLayout == Silk.NET.Vulkan.ImageLayout.TransferDstOptimal)
            {
                barrier.SrcAccessMask = 0;
                barrier.DstAccessMask = AccessFlags.TransferWriteBit;

                sourceStage = PipelineStageFlags.TopOfPipeBit;
                destinationStage = PipelineStageFlags.TransferBit;
            }
            else if (TextureImageLayout == Silk.NET.Vulkan.ImageLayout.TransferDstOptimal &&
                     newLayout == Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal)
            {
                barrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                barrier.DstAccessMask = AccessFlags.AccessMemoryReadBit;

                sourceStage = PipelineStageFlags.TransferBit;
                destinationStage = PipelineStageFlags.FragmentShaderBit;
            }

            VKConst.vulkan.CmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, null, 0, null, 1, &barrier);
            return Result.Success;
        }

        private Result CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, Silk.NET.Vulkan.ImageLayout oldLayout, Silk.NET.Vulkan.ImageLayout newLayout)
        {
            var image = Image;
            var mipmapLevels = MipMapLevels;
            PipelineStageFlags sourceStage = PipelineStageFlags.AllCommandsBit;
            PipelineStageFlags destinationStage = PipelineStageFlags.AllCommandsBit;

            ImageMemoryBarrier barrier = new ImageMemoryBarrier()
            {
                SType = StructureType.ImageMemoryBarrier,
                OldLayout = oldLayout,
                NewLayout = newLayout,
                SrcQueueFamilyIndex = Vk.QueueFamilyIgnored,
                DstQueueFamilyIndex = Vk.QueueFamilyIgnored,
                Image = image,

                SubresourceRange = new ImageSubresourceRange()
                {
                    AspectMask = ImageAspectFlags.ColorBit,
                    LevelCount = mipmapLevels,
                    BaseArrayLayer = 0,
                    BaseMipLevel = 0,
                    LayerCount = Vk.RemainingArrayLayers,
                }
            };
            if (TextureImageLayout == Silk.NET.Vulkan.ImageLayout.Undefined &&
                newLayout == Silk.NET.Vulkan.ImageLayout.TransferDstOptimal)
            {
                barrier.SrcAccessMask = 0;
                barrier.DstAccessMask = AccessFlags.TransferWriteBit;

                sourceStage = PipelineStageFlags.TopOfPipeBit;
                destinationStage = PipelineStageFlags.TransferBit;
            }
            else if (TextureImageLayout == Silk.NET.Vulkan.ImageLayout.TransferDstOptimal &&
                     newLayout == Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal)
            {
                barrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                barrier.DstAccessMask = AccessFlags.AccessMemoryReadBit;

                sourceStage = PipelineStageFlags.TransferBit;
                destinationStage = PipelineStageFlags.FragmentShaderBit;
            }

            VKConst.vulkan.CmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, null, 0, null, 1, &barrier);
            return Result.Success;
        }

        public Result CopyBufferToTexture(ref Silk.NET.Vulkan.Buffer buffer)
        {
            //if (buffer == Vk.null)
            //{
            //    throw new InvalidOperationException("Buffer has not been initialized.");
            //}


            BufferImageCopy BufferImage = new BufferImageCopy()
            {
                BufferOffset = 0,
                BufferRowLength = 0,
                BufferImageHeight = 0,
                ImageSubresource = new ImageSubresourceLayers
                {
                    AspectMask = ImageAspectFlags.ColorBit,
                    MipLevel = 0,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                },
                ImageOffset = new Offset3D
                {

                    X = 0,
                    Y = 0,
                    Z = 0
                },
                ImageExtent = new Extent3D
                {
                    Width = (uint)Width,
                    Height = (uint)Height,
                    Depth = (uint)Depth,
                }

            };
            if (TextureUsage == TextureUsageEnum.kUse_CubeMapTexture)
            {
                BufferImage.ImageSubresource.LayerCount = 6;
            }
            var bufferImage = BufferImage;
            CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
            VKConst.vulkan.CmdCopyBufferToImage(commandBuffer, buffer, Image, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, 1, &bufferImage);
            SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
            return Result.Success;
        }

        private Result CopyBufferToTexture(Image image, ref VkBuffer buffer, TextureUsageEnum textureType, vec3 textureSize)
        {
            BufferImageCopy BufferImage = new BufferImageCopy()
            {
                BufferOffset = 0,
                BufferRowLength = 0,
                BufferImageHeight = 0,
                ImageExtent = new Extent3D()
                {
                    Width = (uint)Width,
                    Height = (uint)Height,
                    Depth = (uint)Depth,
                },
                ImageOffset = new Offset3D()
                {
                    X = 0,
                    Y = 0,
                    Z = 0,
                },
                ImageSubresource = new ImageSubresourceLayers()
                {
                    AspectMask = ImageAspectFlags.ColorBit,
                    MipLevel = 0,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                }

            };
            if (textureType == TextureUsageEnum.kUse_CubeMapTexture)
            {
                BufferImage.ImageSubresource.LayerCount = 6;
            }
            CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
            VKConst.vulkan.CmdCopyBufferToImage(commandBuffer, buffer, image, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, 1, &BufferImage);
            return SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        private Result GenerateMipmaps()
        {
            uint mipWidth = (uint)Width;
            uint mipHeight = (uint)Height;

            FormatProperties formatProperties;
            SilkVulkanRenderer.vulkan.GetPhysicalDeviceFormatProperties(SilkVulkanRenderer.physicalDevice, TextureByteFormat, out formatProperties);
            if ((formatProperties.OptimalTilingFeatures & FormatFeatureFlags.SampledImageFilterLinearBit) == 0)
            {
                // Handle error if needed
            }

            CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
            ImageMemoryBarrier imageMemoryBarrier = new ImageMemoryBarrier
            {
                SType = StructureType.ImageMemoryBarrier,
                Image = Image,
                SrcQueueFamilyIndex = uint.MaxValue,
                DstQueueFamilyIndex = uint.MaxValue,
                SubresourceRange = new ImageSubresourceRange
                {
                    AspectMask = ImageAspectFlags.ColorBit,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                    LevelCount = 1
                }
            };

            for (uint x = 1; x < MipMapLevels; x++)
            {
                imageMemoryBarrier.SubresourceRange.BaseMipLevel = x - 1;
                imageMemoryBarrier.OldLayout = Silk.NET.Vulkan.ImageLayout.TransferDstOptimal;
                imageMemoryBarrier.NewLayout = Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal;
                imageMemoryBarrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                imageMemoryBarrier.DstAccessMask = AccessFlags.TransferReadBit;
                SilkVulkanRenderer.vulkan.CmdPipelineBarrier(commandBuffer, PipelineStageFlags.TransferBit, PipelineStageFlags.TransferBit, 0, 0, null, 0, null, 1, ref imageMemoryBarrier);

                ImageBlit imageBlit = new ImageBlit
                {
                    SrcOffsets = new ImageBlit.SrcOffsetsBuffer()
                    {
                       Element0 = new Offset3D(0, 0, 0),
                       Element1 = new Offset3D((int)mipWidth, (int)mipHeight, 1)
                    },
                    DstOffsets = new ImageBlit.DstOffsetsBuffer()
                    {
                        Element0 = new Offset3D(0, 0, 0),
                        Element1 = new Offset3D((int)mipWidth > 1 ? (int)mipWidth / 2 : 1, (int)mipHeight > 1 ? (int)mipHeight / 2 : 1, 1)
                    },
                    SrcSubresource = new ImageSubresourceLayers
                    {
                        AspectMask = ImageAspectFlags.ColorBit,
                        MipLevel = x - 1,
                        BaseArrayLayer = 0,
                        LayerCount = 1
                    },
                    DstSubresource = new ImageSubresourceLayers
                    {
                        AspectMask = ImageAspectFlags.ColorBit,
                        MipLevel = x,
                        BaseArrayLayer = 0,
                        LayerCount = 1
                    }
                };
                SilkVulkanRenderer.vulkan.CmdBlitImage(commandBuffer, Image, Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal, Image, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, 1, ref imageBlit, Filter.Linear);

                imageMemoryBarrier.OldLayout = Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal;
                imageMemoryBarrier.NewLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
                imageMemoryBarrier.SrcAccessMask = AccessFlags.TransferReadBit;
                imageMemoryBarrier.DstAccessMask = AccessFlags.ShaderReadBit;

                SilkVulkanRenderer.vulkan.CmdPipelineBarrier(commandBuffer, PipelineStageFlags.TransferBit, PipelineStageFlags.FragmentShaderBit, 0, 0, null, 0, null, 1, ref imageMemoryBarrier);

                if (mipWidth > 1)
                {
                    mipWidth /= 2;
                }
                if (mipHeight > 1)
                {
                    mipHeight /= 2;
                }
            }

            imageMemoryBarrier.SubresourceRange.BaseMipLevel = MipMapLevels - 1;
            imageMemoryBarrier.OldLayout = Silk.NET.Vulkan.ImageLayout.TransferDstOptimal;
            imageMemoryBarrier.NewLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
            imageMemoryBarrier.SrcAccessMask = AccessFlags.TransferWriteBit;
            imageMemoryBarrier.DstAccessMask = AccessFlags.ShaderReadBit;

            SilkVulkanRenderer.vulkan.CmdPipelineBarrier(commandBuffer, PipelineStageFlags.TransferBit, PipelineStageFlags.FragmentShaderBit, 0, 0, null, 0, null, 1, ref imageMemoryBarrier);
            return SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }
    }
}
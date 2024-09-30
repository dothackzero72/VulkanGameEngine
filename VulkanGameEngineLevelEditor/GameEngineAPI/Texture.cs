//using GlmSharp;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using VulkanGameEngineLevelEditor;
//using StbImageSharp;
//using System.IO;
//using System.Runtime.InteropServices;
//using System.Collections;
//using System.Runtime.InteropServices.ComTypes;
//using System.Drawing;
//using System.Drawing.Imaging;
//using static System.Net.Mime.MediaTypeNames;

//using static VulkanGameEngineLevelEditor.GameEngineAPI.VulkanRenderer;
//using static VulkanGameEngineLevelEditor.VulkanAPI;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Texture
    {
    }
}
//        public UInt64 TextureBufferIndex { get; protected set; }
//        public int Width { get; protected set; }
//        public int Height { get; protected set; }
//        public int Depth { get; protected set; }
//        public ColorComponents ColorChannels { get; protected set; }
//        public UInt32 MipMapLevels { get; protected set; }
//        public TextureUsageEnum TextureUsage { get; protected set; }
//        public TextureTypeEnum TextureType { get; protected set; }
//        public VkFormat TextureByteFormat { get; protected set; }
//        public VkImageLayout TextureImageLayout { get; protected set; }
//        public VkSampleCountFlagBits SampleCount { get; protected set; }
//        public VkImage Image { get; protected set; }
//        public VkDeviceMemory Memory { get; protected set; }
//        public VkImageView View { get; protected set; }
//        public VkSampler Sampler { get; protected set; }
//        public VkDescriptorImageInfo textureBuffer { get; protected set; }
//        public byte[] Data { get; set; }

//        public Texture()
//        {
//            TextureBufferIndex = 0;
//            Width = 1;
//            Height = 1;
//            Depth = 1;
//            MipMapLevels = 1;

//            Image = VulkanConsts.VK_NULL_HANDLE;
//            Memory = VulkanConsts.VK_NULL_HANDLE;
//            View = VulkanConsts.VK_NULL_HANDLE;
//            Sampler = VulkanConsts.VK_NULL_HANDLE;

//            TextureUsage = TextureUsageEnum.kUse_Undefined;
//            TextureType = TextureTypeEnum.kType_UndefinedTexture;
//            TextureByteFormat = VkFormat.VK_FORMAT_UNDEFINED;
//            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
//            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
//        }

//        public Texture(Pixel ClearColor, ivec2 TextureResolution, VkFormat TextureFormat)
//        {
//            TextureBufferIndex = 0;
//            Width = TextureResolution.x;
//            Height = TextureResolution.y;
//            Depth = 1;
//            MipMapLevels = 1;

//            TextureUsage = TextureUsageEnum.kUse_Undefined;
//            TextureType = TextureTypeEnum.kType_UndefinedTexture;
//            TextureByteFormat = TextureFormat;
//            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
//            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

//            CreateImageTexture();
//            CreateTextureView();
//            CreateTextureSampler();
//        }

//        public Texture(ivec2 textureSize, VkFormat textureFormat)
//        {
//            TextureBufferIndex = 0;
//            Width = textureSize.x;
//            Height = textureSize.y;
//            Depth = 1;
//            MipMapLevels = 1;

//            Image = VulkanConsts.VK_NULL_HANDLE;
//            Memory = VulkanConsts.VK_NULL_HANDLE;
//            View = VulkanConsts.VK_NULL_HANDLE;
//            Sampler = VulkanConsts.VK_NULL_HANDLE;

//            TextureUsage = TextureUsageEnum.kUse_Undefined;
//            TextureType = TextureTypeEnum.kType_UndefinedTexture;
//            TextureByteFormat = textureFormat;
//            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
//            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

//            CreateImageTexture();
//            CreateTextureView();
//            CreateTextureSampler();
//        }

//        public Texture(string filePath, VkFormat textureByteFormat, TextureTypeEnum textureType)
//        {
//            TextureBufferIndex = 0;
//            Width = 1;
//            Height = 1;
//            Depth = 1;
//            MipMapLevels = 1;

//            TextureUsage = TextureUsageEnum.kUse_Undefined;
//            TextureType = textureType;
//            TextureByteFormat = textureByteFormat;
//            TextureImageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
//            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

//            CreateImageTexture(filePath);
//            CreateTextureView();
//            CreateTextureSampler();
//        }

//        protected virtual void CreateImageTexture()
//        {
//            ColorChannels = ColorComponents.RedGreenBlueAlpha;
//            uint size = (uint)Width * (uint)Height * (uint)ColorChannels;

//            // Create an array of Pixel initialized with clear color (black with alpha 255)
//            Pixel[] pixels = new Pixel[Width * Height];
//            for (int i = 0; i < pixels.Length; i++)
//            {
//                pixels[i] = new Pixel(0x00, 0x00, 0x00, 0xFF); // Black color with full alpha
//            }

//            GCHandle pixelHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
//            IntPtr dataPtr;
//            dataPtr = pixelHandle.AddrOfPinnedObject();

//            // Create the staging buffer and upload pixel data to it
//            VulkanBuffer<Pixel> stagingBuffer = new VulkanBuffer<Pixel>(
//                dataPtr,
//                size,
//                VkBufferUsageFlags.VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
//                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
//                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT
//            );
//            var bHandle = stagingBuffer.Buffer;

//            CreateTextureImage();
//            Texture_QuickTransitionImageLayout(TextureImageLayout, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
//            CopyBufferToTexture(ref bHandle);
//            // GenerateMipmaps();

//            pixelHandle.Free();
//            stagingBuffer.DestroyBuffer();
//        }

//        protected VkResult Texture_QuickTransitionImageLayout(VkImageLayout oldLayout, VkImageLayout newLayout)
//        {
//            VkImage image = Image;
//            VkCommandBuffer commandBuffer = VulkanRenderer.BeginCommandBuffer();
//            TransitionImageLayout(commandBuffer, oldLayout, newLayout);
//            VkResult result = VulkanRenderer.EndCommandBuffer(commandBuffer);

//            return result;
//        }

//        virtual protected void CreateImageTexture(string FilePath)
//        {

//            using (var stream = File.OpenRead(FilePath))
//            {
//                ImageResult image = ImageResult.FromStream(stream);
//                Width = image.Width;
//                Height = image.Height;
//                ColorChannels = image.Comp;
//                Data = image.Data.ToArray();
//                // MipMapLevels = (uint)Math.Floor(Math.Log(Math.Max(Width, Height)) / Math.Log(2)) + 1;

//                GCHandle handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
//                IntPtr dataPtr = handle.AddrOfPinnedObject();

//                var buffer = new VulkanBuffer<byte>(dataPtr, (uint)(Width * Height * (int)ColorChannels), VkBufferUsageFlags.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT);
//                var bHandle = buffer.Buffer;

//                CreateTextureImage();
//                Texture_QuickTransitionImageLayout(TextureImageLayout, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
//                CopyBufferToTexture(ref bHandle);
//               // GenerateMipmaps();

//                handle.Free();
//                buffer.DestroyBuffer();
//            }
//        }

//        virtual protected void CreateTextureSampler()
//        {
//            VkSamplerCreateInfo textureImageSamplerInfo = new VkSamplerCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
//                magFilter = VkFilter.VK_FILTER_NEAREST,
//                minFilter = VkFilter.VK_FILTER_NEAREST,
//                mipmapMode = VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_LINEAR,
//                addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
//                addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
//                addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
//                mipLodBias = 0,
//                anisotropyEnable = VulkanConsts.VK_TRUE,
//                maxAnisotropy = 16.0f,
//                compareEnable = VulkanConsts.VK_FALSE,
//                compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS,
//                minLod = 0,
//                maxLod = MipMapLevels,
//                borderColor = VkBorderColor.VK_BORDER_COLOR_INT_OPAQUE_BLACK,
//                unnormalizedCoordinates = VulkanConsts.VK_FALSE,
//            };

//            VkSampler sampler = new VkSampler();
//            VkResult result = GameEngineDLL.DLL_Texture_CreateTextureSampler(VulkanRenderer.Device, ref textureImageSamplerInfo, out sampler);
//            Sampler = sampler;
//        }

//        public VkDescriptorImageInfo GetTextureBuffer()
//        {
//            return new VkDescriptorImageInfo
//            {
//        		sampler = Sampler,
//        		imageView = View,
//        		imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
//            };
//        }
//        virtual public void UpdateTextureSize(vec2 TextureResolution)
//        {
//            //GameEngineDLL.DLL_Renderer_DestroyImageView(&View);
//            //GameEngineDLL.DLL_Renderer_DestroySampler(&Sampler);
//            //GameEngineDLL.DLL_Renderer_DestroyImage(&Image);
//            //GameEngineDLL.DLL_Renderer_FreeMemory(&Memory);
//        }

//        virtual public void Destroy()
//        {

//        }

//        private void CreateBuffer(uint size, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties, VkBuffer* buffer, VkDeviceMemory* bufferMemory)
//        {
//            VkBufferCreateInfo bufferInfo = new VkBufferCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
//                size = size,
//                usage = usage,
//                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE
//            };
//            var BufferInfo = bufferInfo;
//            VulkanAPI.vkCreateBuffer(VulkanRenderer.Device, &BufferInfo, null,  buffer);

//            VkMemoryRequirements memRequirements;

//            VulkanAPI.vkGetBufferMemoryRequirements(VulkanRenderer.Device, *buffer, &memRequirements);

//            VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
//                allocationSize = memRequirements.size,
//                memoryTypeIndex = GameEngineDLL.DLL_Renderer_GetMemoryType(VulkanRenderer.PhysicalDevice, memRequirements.memoryTypeBits, properties)
//            };

//            var AllocInfo = allocInfo;
//            var BufferMemory = bufferMemory;
//            var bufferMemory2 = &BufferMemory;
//            VulkanAPI.vkAllocateMemory(VulkanRenderer.Device, &AllocInfo, null, BufferMemory);
//            VulkanAPI.vkBindBufferMemory(VulkanRenderer.Device, *buffer, BufferMemory, 0);
//        }

//        public void UpdateImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newImageLayout)
//        {
//            UpdateImageLayout(commandBuffer, newImageLayout, MipMapLevels);
//        }

//        public void UpdateImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newImageLayout, uint MipLevel)
//        {
//            VkImageSubresourceRange ImageSubresourceRange = new VkImageSubresourceRange();
//            ImageSubresourceRange.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
//            ImageSubresourceRange.levelCount = 1;
//            ImageSubresourceRange.layerCount = 1;

//            VkImageMemoryBarrier barrier = new VkImageMemoryBarrier();
//            barrier.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
//            barrier.oldLayout = TextureImageLayout;
//            barrier.newLayout = newImageLayout;
//            barrier.image = Image;
//            barrier.subresourceRange = ImageSubresourceRange;
//            barrier.srcAccessMask = 0;
//            barrier.dstAccessMask = VkAccessFlags.VK_ACCESS_TRANSFER_WRITE_BIT;

//            VulkanAPI.vkCmdPipelineBarrier(commandBuffer, VkPipelineStageFlags.VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VkPipelineStageFlags.VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, null, 0, null, 1, &barrier);
//            TextureImageLayout = newImageLayout;
//        }

//        public void UpdateImageLayout(VkCommandBuffer commandBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout, uint MipLevel)
//        {
//            VkImageSubresourceRange ImageSubresourceRange = new VkImageSubresourceRange();
//            ImageSubresourceRange.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
//            ImageSubresourceRange.baseMipLevel = MipLevel;
//            ImageSubresourceRange.levelCount = VulkanConsts.VK_REMAINING_MIP_LEVELS;
//            ImageSubresourceRange.layerCount = 1;

//            VkImageMemoryBarrier barrier = new VkImageMemoryBarrier();
//            barrier.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
//            barrier.oldLayout = oldImageLayout;
//            barrier.newLayout = newImageLayout;
//            barrier.image = Image;
//            barrier.subresourceRange = ImageSubresourceRange;
//            barrier.srcAccessMask = 0;
//            barrier.dstAccessMask = VkAccessFlags.VK_ACCESS_TRANSFER_WRITE_BIT;

//            VulkanAPI.vkCmdPipelineBarrier(commandBuffer, VkPipelineStageFlags.VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VkPipelineStageFlags.VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, null, 0, null, 1, &barrier);
//            TextureImageLayout = newImageLayout;
//        }


//        public byte[] GetTextureData()
//        {
//            if (Data == null || Width <= 0 || Height <= 0)
//            {
//                throw new InvalidOperationException("Invalid texture data.");
//            }

//            // Assume Data is in RGBA format
//            var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

//            // Lock the bitmap's bits.
//            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

//            // Copy data from the byte array to the bitmap
//            unsafe
//            {
//                // This assumes the Data array has enough data (Width * Height * 4)
//                // Bitmap lock will give us an IntPtr to the memory array
//                byte* ptr = (byte*)bmpData.Scan0.ToPointer();

//                for (int y = 0; y < Height; y++)
//                {
//                    for (int x = 0; x < Width; x++)
//                    {
//                        // Index in byte array
//                        int dataIndex = (y * Width + x) * 4; // 4 for RGBA
//                                                                     // Setting the pixel value in the bitmap
//                        ptr[dataIndex + 0] = Data[dataIndex + 2]; // R
//                        ptr[dataIndex + 1] = Data[dataIndex + 1]; // G
//                        ptr[dataIndex + 2] = Data[dataIndex + 0]; // B
//                        ptr[dataIndex + 3] = Data[dataIndex + 3]; // A
//                    }
//                }
//            }

//            byte[] data = new byte[Width * Height * 4];
//            for (int y = 0; y < Height; y++)
//            {
//                for (int x = 0; x < Width; x++)
//                {

//                    int index = (y * Width + x) * 4;
//                    data[index + 0] = 255;
//                    data[index + 1] = 255;
//                    data[index + 2] = 255;
//                    data[index + 3] = 255;
//                }
//            }

//            return data;
//        }

//        virtual protected VkResult CreateTextureImage()
//        {
//            VkImage textureImage;
//            VkDeviceMemory textureMemory;

//            VkImageCreateInfo imageInfo = new VkImageCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
//                imageType = VkImageType.VK_IMAGE_TYPE_2D,
//                format = TextureByteFormat,
//                extent = new VkExtent3D { Width = (uint)Width, Height = (uint)Height, Depth = 1 },
//                mipLevels = MipMapLevels,
//                arrayLayers = 1,
//                samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
//                tiling = VkImageTiling.VK_IMAGE_TILING_OPTIMAL,
//                usage = VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
//                        VkImageUsageFlags.VK_IMAGE_USAGE_SAMPLED_BIT |
//                        VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
//                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE,
//                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED
//            };

//              VkResult result = VulkanAPI.vkCreateImage(VulkanRenderer.Device, &imageInfo, null, &textureImage);
//            if (result != VkResult.VK_SUCCESS)
//            {

//            }

//            VkMemoryRequirements memRequirements;
//            VulkanAPI.vkGetImageMemoryRequirements(VulkanRenderer.Device, textureImage, &memRequirements);

//            VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
//                allocationSize = memRequirements.size,
//                memoryTypeIndex = GameEngineDLL.DLL_Renderer_GetMemoryType(VulkanRenderer.PhysicalDevice, memRequirements.memoryTypeBits, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
//            };

//            result = VulkanAPI.vkAllocateMemory(VulkanRenderer.Device, &allocInfo, null, &textureMemory);
//            if (result != VkResult.VK_SUCCESS)
//            {
//            }

//            result = VulkanAPI.vkBindImageMemory(VulkanRenderer.Device, textureImage, textureMemory, 0);
//            if (result != VkResult.VK_SUCCESS)
//            {
//            }

//            Image = textureImage;
//            Memory = textureMemory;

//            return result;
//        }

//        protected virtual VkResult CreateTextureView()
//        {
//            var textureView = new IntPtr();
//            VkResult result = GameEngineDLL.DLL_Texture_CreateTextureView(VulkanRenderer.Device, out textureView, Image, TextureByteFormat, MipMapLevels);
//            View = textureView;
//            return result;
//        }

//        public VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout oldLayout, VkImageLayout newLayout)
//        {
//            // VkImageLayout oldLayout = TextureImageLayout;
//            var image = Image;
//            var mipmapLevels = MipMapLevels;

//            VkPipelineStageFlags sourceStage = VkPipelineStageFlags.VK_PIPELINE_STAGE_MAX_ENUM;
//            VkPipelineStageFlags destinationStage = VkPipelineStageFlags.VK_PIPELINE_STAGE_MAX_ENUM;
//            VkImageMemoryBarrier barrier = new VkImageMemoryBarrier()
//            {
//		        sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
//		        oldLayout = TextureImageLayout,
//		        newLayout = newLayout,
//		        srcQueueFamilyIndex = VulkanConsts.VK_QUEUE_FAMILY_IGNORED,
//		        dstQueueFamilyIndex = VulkanConsts.VK_QUEUE_FAMILY_IGNORED,
//		        image = image,

//                subresourceRange = new VkImageSubresourceRange()
//                {
//                    aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT,
//                    levelCount = mipmapLevels,
//                    baseArrayLayer = 0,
//                    baseMipLevel = 0,
//                    layerCount = VulkanConsts.VK_REMAINING_ARRAY_LAYERS,
//                }
//            };
//            if (TextureImageLayout == VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED &&
//                newLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL)
//            {
//                barrier.srcAccessMask = 0;
//                barrier.dstAccessMask = VkAccessFlags.VK_ACCESS_TRANSFER_WRITE_BIT;

//                sourceStage = VkPipelineStageFlags.VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
//                destinationStage = VkPipelineStageFlags.VK_PIPELINE_STAGE_TRANSFER_BIT;
//            }
//            else if (TextureImageLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL &&
//                     newLayout == VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL)
//            {
//                barrier.srcAccessMask = VkAccessFlags.VK_ACCESS_TRANSFER_WRITE_BIT;
//                barrier.dstAccessMask = VkAccessFlags.VK_ACCESS_SHADER_READ_BIT;

//                sourceStage = VkPipelineStageFlags.VK_PIPELINE_STAGE_TRANSFER_BIT;
//                destinationStage = VkPipelineStageFlags.VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
//            }

//            VulkanAPI.vkCmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, null, 0, null, 1, &barrier);
//            TextureImageLayout = newLayout;
//            return VkResult.VK_SUCCESS;
//        }

//        public VkResult TransitionImageLayout(VkImageLayout oldLayout, VkImageLayout newLayout)
//        {
//            var result = GameEngineDLL.DLL_Texture_QuickTransitionImageLayout(VulkanRenderer.Device, VulkanRenderer.CommandPool, VulkanRenderer.GraphicsQueue, Image, MipMapLevels, ref oldLayout, ref newLayout);
//            TextureImageLayout = newLayout;
//            return result;
//        }

//        private VkResult CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newLayout)
//        {
//            return GameEngineDLL.DLL_Texture_CommandBufferTransitionImageLayout(commandBuffer, Image, MipMapLevels, TextureImageLayout, newLayout);
//        }

//        private VkResult CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout oldLayout, VkImageLayout newLayout)
//        {
//            return GameEngineDLL.DLL_Texture_CommandBufferTransitionImageLayout(commandBuffer, Image, MipMapLevels, oldLayout, newLayout);
//        }

//        public VkResult CopyBufferToTexture(ref VkBuffer buffer)
//        {
//            if (buffer == null)
//            {
//                throw new InvalidOperationException("Buffer has not been initialized.");
//            }


//            VkBufferImageCopy BufferImage = new VkBufferImageCopy()
//            {
//		        bufferOffset = 0,
//		        bufferRowLength = 0,
//		        bufferImageHeight = 0,
//                imageSubresource = new VkImageSubresourceLayers
//                {
//                    aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT,
//                    mipLevel = 0,
//                     baseArrayLayer = 0,
//                    layerCount = 1,
//                },
//                 imageOffset = new VkOffset3D
//                {

//                    X = 0,
//                    Y = 0,
//                    Z = 0
//                },
//                 imageExtent = new VkExtent3D
//                {
//                    Width = (uint)Width,
//                    Height = (uint)Height,
//                    Depth = (uint)Depth,
//                }

//            };
//            if (TextureUsage ==  TextureUsageEnum.kUse_CubeMapTexture)
//            {
//                BufferImage.imageSubresource.layerCount = 6;
//            }
//            var bufferImage = BufferImage;
//            VkCommandBuffer commandBuffer = VulkanRenderer.BeginCommandBuffer();
//            VulkanAPI.vkCmdCopyBufferToImage(commandBuffer, buffer, Image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &bufferImage);
//            VulkanRenderer.EndCommandBuffer(commandBuffer);



//            return VkResult.VK_SUCCESS;
//        }

//        private VkResult CopyBufferToTexture(VkImage image, ref VkBuffer buffer, TextureUsageEnum textureType, vec3 textureSize)
//        {
//            return GameEngineDLL.DLL_Texture_CopyBufferToTexture(VulkanRenderer.Device, VulkanRenderer.CommandPool, VulkanRenderer.GraphicsQueue, image, buffer, textureType, (int)textureSize.x, (int)textureSize.y, (int)textureSize.z);
//        }

//        private VkResult GenerateMipmaps()
//        {
//            var textureByteFormat = TextureByteFormat;
//            VkResult result = GameEngineDLL.DLL_Texture_GenerateMipmaps(VulkanRenderer.Device, VulkanRenderer.PhysicalDevice, VulkanRenderer.CommandPool, VulkanRenderer.GraphicsQueue, Image, ref textureByteFormat, MipMapLevels, Width, Height);
//            TextureByteFormat = textureByteFormat;
//            return result;
//        }
//    }
//}
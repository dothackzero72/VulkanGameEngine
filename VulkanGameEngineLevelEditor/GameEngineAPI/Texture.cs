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
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;

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
        public byte[] Data { get; set; }

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

        public Texture(ivec2 textureSize, VkFormat textureFormat)
        {
            TextureBufferIndex = 0;
            Width = textureSize.x;
            Height = textureSize.y;
            Depth = 1;
            MipMapLevels = 1;

            Image = VulkanConsts.VK_NULL_HANDLE;
            Memory = VulkanConsts.VK_NULL_HANDLE;
            View = VulkanConsts.VK_NULL_HANDLE;
            Sampler = VulkanConsts.VK_NULL_HANDLE;

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
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

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
            IntPtr dataPtr = IntPtr.Zero;
            dataPtr = pixelHandle.AddrOfPinnedObject();

            // Create the staging buffer and upload pixel data to it
            VulkanBuffer<Pixel> stagingBuffer = new VulkanBuffer<Pixel>(
                dataPtr,
                size,
                VkBufferUsageFlags.VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT
            );
            var bHandle = stagingBuffer.Buffer;

            try
            {
                // Assuming you have your Vulkan commands to create the texture image
                asfd();
                TransitionImageLayout(VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
                CopyBufferToTexture(ref bHandle);
            }
            finally
            {
                // Always free pinned handles to avoid memory leaks
             
            }

            // Destroy the staging buffer after use
            stagingBuffer.DestroyBuffer();
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

                var buffer = new VulkanBuffer<byte>(dataPtr, (uint)(Width * Height * (int)ColorChannels), VkBufferUsageFlags.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT);
                var bHandle = buffer.Buffer;

                CreateTextureImage();
                TransitionImageLayout(VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
                CopyBufferToTexture(ref bHandle);
               // GenerateMipmaps();

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
                anisotropyEnable = VulkanConsts.VK_TRUE,
                maxAnisotropy = 16.0f,
                compareEnable = VulkanConsts.VK_FALSE,
                compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS,
                minLod = 0,
                maxLod = MipMapLevels,
                borderColor = VkBorderColor.VK_BORDER_COLOR_INT_OPAQUE_BLACK,
                unnormalizedCoordinates = VulkanConsts.VK_FALSE,
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

        private void CreateBuffer(uint size, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties, out VkBuffer buffer, out VkDeviceMemory bufferMemory)
        {
            VkBufferCreateInfo bufferInfo = new VkBufferCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
                size = size,
                usage = usage,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE
            };

            VulkanAPI.vkCreateBuffer(VulkanRenderer.Device, ref bufferInfo, IntPtr.Zero, out buffer);

            VkMemoryRequirements memRequirements;
            VulkanAPI.vkGetBufferMemoryRequirements(VulkanRenderer.Device, buffer, out memRequirements);

            VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                allocationSize = memRequirements.size,
                memoryTypeIndex = GameEngineDLL.DLL_Renderer_GetMemoryType(VulkanRenderer.PhysicalDevice, memRequirements.memoryTypeBits, properties)
            };

            VulkanAPI.vkAllocateMemory(VulkanRenderer.Device, ref allocInfo, IntPtr.Zero, out bufferMemory);
            VulkanAPI.vkBindBufferMemory(VulkanRenderer.Device, buffer, bufferMemory, 0);
        }

        public void UpdateImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newImageLayout)
        {
            UpdateImageLayout(commandBuffer, newImageLayout, MipMapLevels);
        }

        public void UpdateImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newImageLayout, uint MipLevel)
        {
            VkImageSubresourceRange ImageSubresourceRange = new VkImageSubresourceRange();
            ImageSubresourceRange.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
            ImageSubresourceRange.levelCount = 1;
            ImageSubresourceRange.layerCount = 1;

            VkImageMemoryBarrier barrier = new VkImageMemoryBarrier();
            barrier.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
            barrier.oldLayout = TextureImageLayout;
            barrier.newLayout = newImageLayout;
            barrier.image = Image;
            barrier.subresourceRange = ImageSubresourceRange;
            barrier.srcAccessMask = 0;
            barrier.dstAccessMask = VkAccessFlags.VK_ACCESS_TRANSFER_WRITE_BIT;

            VulkanAPI.vkCmdPipelineBarrier(commandBuffer, VkPipelineStageFlags.VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VkPipelineStageFlags.VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, null, 0, null, 1, &barrier);
            TextureImageLayout = newImageLayout;
        }

        public void UpdateImageLayout(VkCommandBuffer commandBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout, uint MipLevel)
        {
            VkImageSubresourceRange ImageSubresourceRange = new VkImageSubresourceRange();
            ImageSubresourceRange.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
            ImageSubresourceRange.baseMipLevel = MipLevel;
            ImageSubresourceRange.levelCount = VulkanConsts.VK_REMAINING_MIP_LEVELS;
            ImageSubresourceRange.layerCount = 1;

            VkImageMemoryBarrier barrier = new VkImageMemoryBarrier();
            barrier.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
            barrier.oldLayout = oldImageLayout;
            barrier.newLayout = newImageLayout;
            barrier.image = Image;
            barrier.subresourceRange = ImageSubresourceRange;
            barrier.srcAccessMask = 0;
            barrier.dstAccessMask = VkAccessFlags.VK_ACCESS_TRANSFER_WRITE_BIT;

            VulkanAPI.vkCmdPipelineBarrier(commandBuffer, VkPipelineStageFlags.VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VkPipelineStageFlags.VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, null, 0, null, 1, &barrier);
            TextureImageLayout = newImageLayout;
        }

        public static byte[] UpdateBitmapData(Texture texture)
        {
            Texture BakeTexture = new Texture(new ivec2(texture.Width, texture.Height), VkFormat.VK_FORMAT_R8G8B8A8_UNORM);

            VkCommandBuffer commandBuffer = VulkanRenderer.BeginCommandBuffer();

            BakeTexture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
            texture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);

            VkImageCopy copyImage = new VkImageCopy
            {
                srcSubresource = new VkImageSubresourceLayers
                {
                    aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT,
                    mipLevel = 0,
                    baseArrayLayer = 0,
                    layerCount = 1,
                },
                dstSubresource = new VkImageSubresourceLayers
                {
                    aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT,
                    mipLevel = 0,
                    baseArrayLayer = 0,
                    layerCount = 1,
                },
                dstOffset = new VkOffset3D { X = 0, Y = 0, Z = 0 },
                extent = new VkExtent3D
                {
                    Width = (uint)BakeTexture.Width,
                    Height = (uint)BakeTexture.Height,
                    Depth = 1
                }
            };

            VulkanAPI.vkCmdCopyImage(commandBuffer, texture.Image,
                VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL,
                BakeTexture.Image,
                VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                1, &copyImage);

            BakeTexture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_GENERAL, BakeTexture.MipMapLevels);
            texture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR, texture.MipMapLevels);

            VulkanRenderer.EndCommandBuffer(commandBuffer);

            VkImageSubresource subResource = new VkImageSubresource
            {
                aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT,
                arrayLayer = 0,
                mipLevel = 0
            };

            VkSubresourceLayout subResourceLayout;
            VulkanAPI.vkGetImageSubresourceLayout(VulkanRenderer.Device, BakeTexture.Image, &subResource, &subResourceLayout);
            Console.WriteLine($"Row Pitch: {subResourceLayout.rowPitch}, Width: {BakeTexture.Width}, Height: {BakeTexture.Height}");

            byte* data;
            VulkanAPI.vkMapMemory(VulkanRenderer.Device, BakeTexture.Memory, 0, VulkanConsts.VK_WHOLE_SIZE, 0, (void**)&data);

            byte[] pixelData = new byte[BakeTexture.Width * BakeTexture.Height * 4];

            unsafe
            {
                fixed (byte* destPtr = pixelData)
                {
                    for (int y = 0; y < BakeTexture.Height; y++)
                    {
                        for (int x = 0; x < BakeTexture.Width; x++)
                        {
                            int srcIndex = (y * (int)subResourceLayout.rowPitch) + (x * 4);
                            if ((ulong)(srcIndex + 3) < VulkanConsts.VK_WHOLE_SIZE)
                            {
                                byte r = data[srcIndex + 0];
                                byte g = data[srcIndex + 1];
                                byte b = data[srcIndex + 2];
                                byte a = data[srcIndex + 3];

                                int destIndex = (y * BakeTexture.Width + x) * 4;
                                Console.WriteLine($"Pixel ({x}, {y}): R={r}, G={g}, B={b}, A={a}");

                                destPtr[destIndex + 0] = a;
                                destPtr[destIndex + 1] = r;
                                destPtr[destIndex + 2] = g; 
                                destPtr[destIndex + 3] = b; 
                            }
                        }
                    }
                }
            }
            VulkanAPI.vkUnmapMemory(VulkanRenderer.Device, BakeTexture.Memory);

            return pixelData; 
        }

        private VkResult asfd()
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
                tiling = VkImageTiling.VK_IMAGE_TILING_LINEAR,
                usage = VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                        VkImageUsageFlags.VK_IMAGE_USAGE_SAMPLED_BIT |
                        VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
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
                memoryTypeIndex = GameEngineDLL.DLL_Renderer_GetMemoryType(VulkanRenderer.PhysicalDevice, memRequirements.memoryTypeBits, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT)
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
                usage = VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                        VkImageUsageFlags.VK_IMAGE_USAGE_SAMPLED_BIT |
                        VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
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

        public VkResult TransitionImageLayout(VkImageLayout newLayout)
        {
            VkImageLayout oldLayout = TextureImageLayout;
            var result = GameEngineDLL.DLL_Texture_QuickTransitionImageLayout(VulkanRenderer.Device, VulkanRenderer.CommandPool, VulkanRenderer.GraphicsQueue, Image, MipMapLevels, ref oldLayout, ref newLayout);
            TextureImageLayout = newLayout;
            return result;
        }

        public VkResult TransitionImageLayout(VkImageLayout oldLayout, VkImageLayout newLayout)
        {
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
            if (buffer == IntPtr.Zero)
            {
                throw new InvalidOperationException("Buffer has not been initialized.");
            }

            VkResult result = GameEngineDLL.DLL_Texture_CopyBufferToTexture(
                (IntPtr)VulkanRenderer.Device,
                (IntPtr)VulkanRenderer.CommandPool,
                (IntPtr)VulkanRenderer.GraphicsQueue,
                (IntPtr)Image,
                (IntPtr)buffer,
                TextureUsage,
                Width,
                Height,
                Depth
            );

            if (result != VkResult.VK_SUCCESS)
            {
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
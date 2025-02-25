using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.Vulkan;
using Image = Silk.NET.Vulkan.Image;
using VulkanGameEngineLevelEditor.Models;
using StbImageSharp;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public static unsafe class CTexture
    {
        static Vk vk = Vk.GetApi();
        public static void CreateBuffer(uint size, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, VkBuffer* buffer, VkDeviceMemory* bufferMemory)
        {
            VkBufferCreateInfo bufferInfo = new VkBufferCreateInfo
            {
                SType = StructureType.BufferCreateInfo,
                Size = size,
                Usage = usage,
                SharingMode = SharingMode.Exclusive
            };
            var BufferInfo = bufferInfo;
            VkFunc.vkCreateBuffer(VulkanRenderer.device, &BufferInfo, null, buffer);

            MemoryRequirements memRequirements;
            VkFunc.vkGetBufferMemoryRequirements(VulkanRenderer.device, *buffer, &memRequirements);

            MemoryAllocateInfo allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, properties)
            };

            VkFunc.vkAllocateMemory(VulkanRenderer.device, &allocInfo, null, bufferMemory);
            VkFunc.vkBindBufferMemory(VulkanRenderer.device, *buffer, *bufferMemory, 0);
        }


        public static void UpdateImageLayout(VkCommandBuffer commandBuffer, Silk.NET.Vulkan.Image image, ref VkImageLayout oldImageLayout, VkImageLayout newImageLayout, uint MipLevel, VkImageAspectFlags imageAspectFlags)
        {
            VkImageSubresourceRange ImageSubresourceRange = new VkImageSubresourceRange();
            ImageSubresourceRange.AspectMask = imageAspectFlags;
            ImageSubresourceRange.LevelCount = Vk.RemainingMipLevels;
            ImageSubresourceRange.LayerCount = 1;

            VkImageMemoryBarrier barrier = new VkImageMemoryBarrier();
            barrier.SType = StructureType.ImageMemoryBarrier;
            barrier.OldLayout = oldImageLayout;
            barrier.NewLayout = newImageLayout;
            barrier.Image = image;
            barrier.SubresourceRange = ImageSubresourceRange;
            barrier.SrcAccessMask = 0;
            barrier.DstAccessMask = AccessFlags.TransferWriteBit;

            VkFunc.vkCmdPipelineBarrier(commandBuffer, VkPipelineStageFlags.AllCommandsBit, VkPipelineStageFlags.AllCommandsBit, 0, 0, null, 0, null, 1, &barrier);
            oldImageLayout = newImageLayout;
        }

        public static byte[] GetTextureData(byte[] Data, uint Width, uint Height)
        {
            if (Data == null || Width <= 0 || Height <= 0)
            {
                throw new InvalidOperationException("Invalid texture data.");
            }

            var bitmap = new Bitmap((int)Width, (int)Height, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bitmap.LockBits(new VkRectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            byte* ptr = (byte*)bmpData.Scan0.ToPointer();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int dataIndex = (y * (int)Width + x) * 4;
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

                    int index = (y * (int)Width + x) * 4;
                    data[index + 0] = 255;
                    data[index + 1] = 255;
                    data[index + 2] = 255;
                    data[index + 3] = 255;
                }
            }

            return data;
        }

        public static VkResult CreateTextureImage(VkImageCreateInfo createInfo, out VkImage image, out VkDeviceMemory textureMemory, int Width, int Height, VkFormat format, uint MipLevels)
        {
            image = new VkImage();
            textureMemory = new VkDeviceMemory();


            VkImage tempImagePtr = new VkImage();
            VkResult result = VkFunc.vkCreateImage(VulkanRenderer.device, &createInfo, null, &tempImagePtr);


            VkMemoryRequirements memRequirements;
            VkFunc.vkGetImageMemoryRequirements(VulkanRenderer.device, tempImagePtr, &memRequirements);

            VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo
            {
                SType = VkStructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, VkMemoryPropertyFlags.DeviceLocalBit)
            };

            VkDeviceMemory textureMemoryPtr = new VkDeviceMemory();
            result = VkFunc.vkAllocateMemory(VulkanRenderer.device, &allocInfo, null, &textureMemoryPtr);
            if (result != VkResult.Success)
            {
            }

            image = tempImagePtr;
            textureMemory = textureMemoryPtr;
            result = VkFunc.vkBindImageMemory(VulkanRenderer.device, image, textureMemory, 0);
            if (result != VkResult.Success)
            {
            }

            

            return result;
        }

        public static Result TransitionImageLayout(CommandBuffer commandBuffer, Silk.NET.Vulkan.Image image, uint mipmapLevels, ref Silk.NET.Vulkan.ImageLayout oldLayout, Silk.NET.Vulkan.ImageLayout newLayout, ImageAspectFlags colorFlags)
        {
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
                    AspectMask = colorFlags,
                    LevelCount = mipmapLevels,
                    BaseArrayLayer = 0,
                    BaseMipLevel = 0,
                    LayerCount = Vk.RemainingArrayLayers,
                }
            };
            if (oldLayout == Silk.NET.Vulkan.ImageLayout.Undefined &&
                newLayout == Silk.NET.Vulkan.ImageLayout.TransferDstOptimal)
            {
                barrier.SrcAccessMask = 0;
                barrier.DstAccessMask = AccessFlags.TransferWriteBit;

                sourceStage = PipelineStageFlags.TopOfPipeBit;
                destinationStage = PipelineStageFlags.TransferBit;
            }
            else if (oldLayout == Silk.NET.Vulkan.ImageLayout.TransferDstOptimal &&
                     newLayout == Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal)
            {
                barrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                barrier.DstAccessMask = AccessFlags.AccessMemoryReadBit;

                sourceStage = PipelineStageFlags.TransferBit;
                destinationStage = PipelineStageFlags.FragmentShaderBit;
            }

            VkFunc.vkCmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, null, 0, null, 1, &barrier);
            oldLayout = newLayout;

            return Result.Success;
        }

        public static VkResult CopyBufferToTexture(ref VkBuffer buffer, VkImage image, VkExtent3D extent, VkTextureUsageEnum textureUsage, VkImageAspectFlags imageAspectFlags)
        {
            VkBufferImageCopy bufferImage = new VkBufferImageCopy()
            {
                BufferOffset = 0,
                BufferRowLength = 0,
                BufferImageHeight = 0,
                ImageSubresource = new VkImageSubresourceLayers
                {
                    AspectMask = imageAspectFlags,
                    MipLevel = 0,
                    BaseArrayLayer = 0,
                    LayerCount = (uint)(textureUsage == TextureUsageEnum.kUse_CubeMapTexture ? 6 : 1),
                },
                ImageOffset = new Offset3D { X = 0, Y = 0, Z = 0 },
                ImageExtent = extent
            };

            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            VkFunc.vkCmdCopyBufferToImage(commandBuffer, buffer, image, VkImageLayout.TransferDstOptimal, 1, &bufferImage);
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            return VkResult.Success;
        }

        public static Result QuickTransitionImageLayout(Image image, VkImageLayout oldLayout, VkImageLayout newLayout, uint MipMapLevels, VkImageAspectFlags imageAspectFlags)
        {
            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            CTexture.TransitionImageLayout(commandBuffer, image, MipMapLevels, ref oldLayout, newLayout, imageAspectFlags);
            VkResult result = VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            return result;
        }

        public static Result CopyBufferToTexture(ref VkBuffer buffer, VkImage image, VkTextureUsageEnum textureType, vec3 textureSize, VkExtent3D extent, VkImageAspectFlags imageAspectFlags)
        {
            VkBufferImageCopy BufferImage = new VkBufferImageCopy()
            {
                BufferOffset = 0,
                BufferRowLength = 0,
                BufferImageHeight = 0,
                ImageExtent = new VkExtent3D()
                {
                    Width = (uint)extent.Width,
                    Height = (uint)extent.Height,
                    Depth = (uint)extent.Depth,
                },
                ImageOffset = new VkOffset3D()
                {
                    X = 0,
                    Y = 0,
                    Z = 0,
                },
                ImageSubresource = new VkImageSubresourceLayers()
                {
                    AspectMask = imageAspectFlags,
                    MipLevel = 0,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                }

            };
            if (textureType == TextureUsageEnum.kUse_CubeMapTexture)
            {
                BufferImage.ImageSubresource.LayerCount = 6;
            }
            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            VkFunc.vkCmdCopyBufferToImage(commandBuffer, buffer, image, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, 1, &BufferImage);
            return VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        public static VkResult GenerateMipmaps(VkImage image, int Width, int Height, VkFormat format, uint mipLevels, VkImageAspectFlags imageAspectFlags)
        {
            uint mipWidth = (uint)Width;
            uint mipHeight = (uint)Height;

            VkFunc.vkGetPhysicalDeviceFormatProperties(VulkanRenderer.physicalDevice, format, out VkFormatProperties formatProperties);
            if ((formatProperties.OptimalTilingFeatures & FormatFeatureFlags.SampledImageFilterLinearBit) == 0)
            {
                // Handle error if needed
            }

            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            Vk ImageMemoryBarrier imageMemoryBarrier = new ImageMemoryBarrier
            {
                SType = StructureType.ImageMemoryBarrier,
                Image = image,
                SrcQueueFamilyIndex = uint.MaxValue,
                DstQueueFamilyIndex = uint.MaxValue,
                SubresourceRange = new ImageSubresourceRange
                {
                    AspectMask = imageAspectFlags,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                    LevelCount = 1
                }
            };

            for (uint x = 1; x < mipLevels; x++)
            {
                imageMemoryBarrier.SubresourceRange.BaseMipLevel = x - 1;
                imageMemoryBarrier.OldLayout = Silk.NET.Vulkan.ImageLayout.TransferDstOptimal;
                imageMemoryBarrier.NewLayout = Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal;
                imageMemoryBarrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                imageMemoryBarrier.DstAccessMask = AccessFlags.TransferReadBit;
                vk.CmdPipelineBarrier(commandBuffer, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.TransferBit, 0, 0, null, 0, null, 1, ref imageMemoryBarrier);

                VkImageBlit imageBlit = new VkImageBlit
                {
                    SrcOffsets = new VkImageBlit.SrcOffsetsBuffer()
                    {
                        Element0 = new VkOffset3D(0, 0, 0),
                        Element1 = new VkOffset3D((int)mipWidth, (int)mipHeight, 1)
                    },
                    DstOffsets = new VkImageBlit.DstOffsetsBuffer()
                    {
                        Element0 = new VkOffset3D(0, 0, 0),
                        Element1 = new VkOffset3D((int)mipWidth > 1 ? (int)mipWidth / 2 : 1, (int)mipHeight > 1 ? (int)mipHeight / 2 : 1, 1)
                    },
                    SrcSubresource = new VkImageSubresourceLayers
                    {
                        AspectMask = imageAspectFlags,
                        MipLevel = x - 1,
                        BaseArrayLayer = 0,
                        LayerCount = 1
                    },
                    DstSubresource = new VkImageSubresourceLayers
                    {
                        AspectMask = imageAspectFlags,
                        MipLevel = x,
                        BaseArrayLayer = 0,
                        LayerCount = 1
                    }
                };
                VkFunc.vkCmdBlitImage(commandBuffer, image, Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal, image, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, 1, ref imageBlit, Filter.Linear);

                imageMemoryBarrier.OldLayout = Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal;
                imageMemoryBarrier.NewLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
                imageMemoryBarrier.SrcAccessMask = AccessFlags.TransferReadBit;
                imageMemoryBarrier.DstAccessMask = AccessFlags.ShaderReadBit;

                VkFunc.vkCmdPipelineBarrier(commandBuffer, PipelineStageFlags.TransferBit, PipelineStageFlags.FragmentShaderBit, 0, 0, null, 0, null, 1, ref imageMemoryBarrier);

                if (mipWidth > 1)
                {
                    mipWidth /= 2;
                }
                if (mipHeight > 1)
                {
                    mipHeight /= 2;
                }
            }

            imageMemoryBarrier.SubresourceRange.BaseMipLevel = mipLevels - 1;
            imageMemoryBarrier.OldLayout = Silk.NET.Vulkan.ImageLayout.TransferDstOptimal;
            imageMemoryBarrier.NewLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal;
            imageMemoryBarrier.SrcAccessMask = AccessFlags.TransferWriteBit;
            imageMemoryBarrier.DstAccessMask = AccessFlags.ShaderReadBit;

            VkFunc.vkCmdPipelineBarrier(commandBuffer, PipelineStageFlags.TransferBit, PipelineStageFlags.FragmentShaderBit, 0, 0, null, 0, null, 1, ref imageMemoryBarrier);
            return VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }
    }
}

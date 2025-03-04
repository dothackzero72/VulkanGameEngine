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
        public static void CreateBuffer(uint size, VkBufferUsageFlagBits usage, VkMemoryPropertyFlagBits properties, VkBuffer* buffer, VkDeviceMemory* bufferMemory)
        {
            VkBufferCreateInfo bufferInfo = new VkBufferCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
                size = size,
                usage = usage,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE
            };

            var BufferInfo = bufferInfo;
            VkFunc.vkCreateBuffer(VulkanRenderer.device, &BufferInfo, null, out *buffer);
            VkFunc.vkGetBufferMemoryRequirements(VulkanRenderer.device, *buffer, out VkMemoryRequirements memRequirements);

            VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_EXPORT_MEMORY_ALLOCATE_INFO,
                allocationSize = memRequirements.size,
                memoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.memoryTypeBits, properties)
            };

            VkFunc.vkAllocateMemory(VulkanRenderer.device, &allocInfo, null, out VkDeviceMemory tempBufferMemory);
            bufferMemory = &tempBufferMemory;

            VkFunc.vkBindBufferMemory(VulkanRenderer.device, *buffer, *bufferMemory, 0);
        }


        public static void UpdateImageLayout(VkCommandBuffer commandBuffer, VkImage image, ref VkImageLayout oldImageLayout, VkImageLayout newImageLayout, uint MipLevel, VkImageAspectFlagBits imageAspectFlags)
        {
            VkImageSubresourceRange ImageSubresourceRange = new VkImageSubresourceRange
            {
                aspectMask = imageAspectFlags,
                levelCount = Vk.RemainingMipLevels,
                layerCount = 1
            };


            VkImageMemoryBarrier barrier = new VkImageMemoryBarrier
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                oldLayout = oldImageLayout,
                newLayout = newImageLayout,
                image = image,
                subresourceRange = ImageSubresourceRange,
                srcAccessMask = 0,
                dstAccessMask = VkAccessFlagBits.VK_ACCESS_TRANSFER_READ_BIT
            };

            VkFunc.vkCmdPipelineBarrier(commandBuffer, VkPipelineStageFlagBits.ALL_COMMANDS_BIT, VkPipelineStageFlagBits.ALL_COMMANDS_BIT, 0, 0, null, 0, null, 1, &barrier);
            oldImageLayout = newImageLayout;
        }

        public static byte[] GetTextureData(byte[] Data, uint Width, uint Height)
        {
            if (Data == null || Width <= 0 || Height <= 0)
            {
                throw new InvalidOperationException("Invalid texture data.");
            }

            var bitmap = new Bitmap((int)Width, (int)Height, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

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

        public static VkResult CreateImage(VkImageCreateInfo createInfo, ref VkImage image, ref VkDeviceMemory textureMemory, VkImageCreateInfo imageCreateInfo)
        {
            return GameEngineImport.DLL_Texture_CreateImage(VulkanRenderer.device, VulkanRenderer.physicalDevice, ref image, ref textureMemory, imageCreateInfo);
        }

        public static VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint mipmapLevels, ref VkImageLayout oldLayout, VkImageLayout newLayout, VkImageAspectFlagBits colorFlags)
        {
            VkPipelineStageFlagBits sourceStage = VkPipelineStageFlagBits.ALL_COMMANDS_BIT;
            VkPipelineStageFlagBits destinationStage = VkPipelineStageFlagBits.ALL_COMMANDS_BIT;
            VkImageMemoryBarrier barrier = new VkImageMemoryBarrier()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                oldLayout = oldLayout,
                newLayout = newLayout,
                srcQueueFamilyIndex = Vk.QueueFamilyIgnored,
                dstQueueFamilyIndex = Vk.QueueFamilyIgnored,
                image = image,
                subresourceRange = new VkImageSubresourceRange()
                {
                    aspectMask = colorFlags,
                    levelCount = mipmapLevels,
                    baseArrayLayer = 0,
                    baseMipLevel = 0,
                    layerCount = Vk.RemainingArrayLayers,
                }
            };
            if (oldLayout == VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED &&
                newLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL)
            {
                barrier.srcAccessMask = 0;
                barrier.dstAccessMask = VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;

                sourceStage = VkPipelineStageFlagBits.TOP_OF_PIPE_BIT;
                destinationStage = VkPipelineStageFlagBits.TRANSFER_BIT;
            }
            else if (oldLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL &&
                     newLayout == VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_READ_ONLY_OPTIMAL)
            {
                barrier.srcAccessMask = VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;
                barrier.dstAccessMask = VkAccessFlagBits.VK_ACCESS_MEMORY_READ_BIT;

                sourceStage = VkPipelineStageFlagBits.TRANSFER_BIT;
                destinationStage = VkPipelineStageFlagBits.FRAGMENT_SHADER_BIT;
            }

            VkFunc.vkCmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, null, 0, null, 1, &barrier);
            oldLayout = newLayout;

            return VkResult.VK_SUCCESS;
        }

        public static VkResult CopyBufferToTexture(ref VkBuffer buffer, VkImage image, VkExtent3D extent, TextureUsageEnum textureUsage, VkImageAspectFlagBits imageAspectFlags)
        {
            VkBufferImageCopy bufferImage = new VkBufferImageCopy()
            {
                bufferOffset = 0,
                bufferRowLength = 0,
                bufferImageHeight = 0,
                imageSubresource = new VkImageSubresourceLayers
                {
                    aspectMask = imageAspectFlags,
                    mipLevel = 0,
                    baseArrayLayer = 0,
                    layerCount = (uint)(textureUsage == TextureUsageEnum.kUse_CubeMapTexture ? 6 : 1),
                },
                imageOffset = new VkOffset3D { x = 0, y = 0, z = 0 },
                imageExtent = extent
            };

            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            VkFunc.vkCmdCopyBufferToImage(commandBuffer, buffer, image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &bufferImage);
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            return VkResult.VK_SUCCESS;
        }

        public static VkResult QuickTransitionImageLayout(VkImage image, ref VkImageLayout oldLayout, ref VkImageLayout newLayout, uint mipMapLevels)
        {
            return GameEngineImport.DLL_Texture_QuickTransitionImageLayout(VulkanRenderer.device, VulkanRenderer.commandPool, VulkanRenderer.graphicsQueue, image, mipMapLevels, ref oldLayout, ref newLayout);
        }

        public static VkResult CopyBufferToTexture(ref VkBuffer buffer, VkImage image, TextureUsageEnum textureType, vec3 textureSize, VkExtent3D extent, VkImageAspectFlagBits imageAspectFlags)
        {
            VkBufferImageCopy BufferImage = new VkBufferImageCopy()
            {
                bufferOffset = 0,
                bufferRowLength = 0,
                bufferImageHeight = 0,
                imageExtent = new VkExtent3D()
                {
                    width = (uint)extent.width,
                    height = (uint)extent.height,
                    depth = (uint)extent.depth,
                },
                imageOffset = new VkOffset3D()
                {
                    x = 0,
                    y = 0,
                    z = 0,
                },
                imageSubresource = new VkImageSubresourceLayers()
                {
                    aspectMask = imageAspectFlags,
                    mipLevel = 0,
                    baseArrayLayer = 0,
                    layerCount = 1,
                }

            };
            if (textureType == TextureUsageEnum.kUse_CubeMapTexture)
            {
                BufferImage.imageSubresource.layerCount = 6;
            }
            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            VkFunc.vkCmdCopyBufferToImage(commandBuffer, buffer, image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &BufferImage);
            return VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        public static VkResult GenerateMipmaps(VkImage image, int Width, int Height, VkFormat format, uint mipLevels, VkImageAspectFlagBits imageAspectFlags)
        {
            uint mipWidth = (uint)Width;
            uint mipHeight = (uint)Height;

            VkFormatProperties* formatProperties = null;
            VkFunc.vkGetPhysicalDeviceFormatProperties(VulkanRenderer.physicalDevice, format, formatProperties);
            if ((formatProperties->optimalTilingFeatures & VkFormatFeatureFlagBits.VK_FORMAT_FEATURE_SAMPLED_IMAGE_FILTER_LINEAR_BIT) == 0)
            {
                // Handle error if needed
            }

            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            VkImageMemoryBarrier imageMemoryBarrier = new VkImageMemoryBarrier
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                image = image,
                srcQueueFamilyIndex = uint.MaxValue,
                dstQueueFamilyIndex = uint.MaxValue,
                subresourceRange = new VkImageSubresourceRange
                {
                    aspectMask = imageAspectFlags,
                    baseArrayLayer = 0,
                    layerCount = 1,
                    levelCount = 1
                }
            };

            for (uint x = 1; x < mipLevels; x++)
            {
                imageMemoryBarrier.subresourceRange.baseMipLevel = x - 1;
                imageMemoryBarrier.oldLayout = VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
                imageMemoryBarrier.newLayout = VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL;
                imageMemoryBarrier.srcAccessMask = VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;
                imageMemoryBarrier.dstAccessMask = VkAccessFlagBits.VK_ACCESS_TRANSFER_READ_BIT;
                VkFunc.vkCmdPipelineBarrier(commandBuffer, VkPipelineStageFlagBits.TRANSFER_BIT, VkPipelineStageFlagBits.FRAGMENT_SHADER_BIT, VkDependencyFlagBits.VK_DEPENDENCY_FLAG_BITS_MAX_ENUM, 0, null, 0, null, 1, &imageMemoryBarrier);

                VkImageBlit imageBlit = new VkImageBlit()
                {
                    srcOffsets = new VkOffset3D[2]
                   {
                    new VkOffset3D() { x = 0, y = 0, z = 0 },
                    new VkOffset3D() { x = (int)mipWidth, y = (int)mipHeight, z = 1 }
                   },

                    dstOffsets = new VkOffset3D[2]
                   {
                    new VkOffset3D() { x = 0, y = 0, z = 0 },
                    new VkOffset3D() {
                        x = (int)(mipWidth > 1 ? mipWidth / 2 : 1),
                        y = (int)(mipHeight > 1 ? mipHeight / 2 : 1),
                        z = 1
                    }
               },
                    srcSubresource = new VkImageSubresourceLayers
                    {
                        aspectMask = imageAspectFlags,
                        mipLevel = x - 1,
                        baseArrayLayer = 0,
                        layerCount = 1
                    },
                    dstSubresource = new VkImageSubresourceLayers
                    {
                        aspectMask = imageAspectFlags,
                        mipLevel = x,
                        baseArrayLayer = 0,
                        layerCount = 1
                    }
                };
                VkFunc.vkCmdBlitImage(commandBuffer, image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &imageBlit, VkFilter.VK_FILTER_LINEAR);

                imageMemoryBarrier.oldLayout = VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL;
                imageMemoryBarrier.newLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
                imageMemoryBarrier.srcAccessMask = VkAccessFlagBits.VK_ACCESS_TRANSFER_READ_BIT;
                imageMemoryBarrier.dstAccessMask = VkAccessFlagBits.VK_ACCESS_SHADER_READ_BIT;

                VkFunc.vkCmdPipelineBarrier(commandBuffer, VkPipelineStageFlagBits.TRANSFER_BIT, VkPipelineStageFlagBits.FRAGMENT_SHADER_BIT, 0, 0, null, 0, null, 1, &imageMemoryBarrier);

                if (mipWidth > 1)
                {
                    mipWidth /= 2;
                }
                if (mipHeight > 1)
                {
                    mipHeight /= 2;
                }
            }

            imageMemoryBarrier.subresourceRange.baseMipLevel = mipLevels - 1;
            imageMemoryBarrier.oldLayout = VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
            imageMemoryBarrier.newLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
            imageMemoryBarrier.dstAccessMask = VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;
            imageMemoryBarrier.srcAccessMask = VkAccessFlagBits.VK_ACCESS_SHADER_READ_BIT;

            VkFunc.vkCmdPipelineBarrier(commandBuffer, VkPipelineStageFlagBits.TRANSFER_BIT, VkPipelineStageFlagBits.FRAGMENT_SHADER_BIT, 0, 0, null, 0, null, 1, &imageMemoryBarrier);
            return VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }
    }
}
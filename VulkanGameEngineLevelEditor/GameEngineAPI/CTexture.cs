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

        public static VkResult CreateImage(VkImageCreateInfo createInfo, ref VkImage image, ref VkDeviceMemory textureMemory, VkImageCreateInfo imageCreateInfo)
        {
            return GameEngineImport.DLL_Texture_CreateImage(VulkanRenderer.device, VulkanRenderer.physicalDevice, ref image, ref textureMemory, imageCreateInfo);
        }

        public static VkResult QuickTransitionImageLayout(VkImage image, ref VkImageLayout oldLayout, ref VkImageLayout newLayout, uint mipMapLevels)
        {
            return GameEngineImport.DLL_Texture_QuickTransitionImageLayout(VulkanRenderer.device, VulkanRenderer.commandPool, VulkanRenderer.graphicsQueue, image, mipMapLevels, ref oldLayout, ref newLayout);
        }

      
    }
}
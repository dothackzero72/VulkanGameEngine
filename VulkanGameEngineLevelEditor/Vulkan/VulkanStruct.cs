using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public struct VkSurfaceCapabilitiesKHR
    {
       public uint minImageCount;
        public uint maxImageCount;
        public VkExtent2D currentExtent;
        public VkExtent2D minImageExtent;
        public VkExtent2D maxImageExtent;
        public uint maxImageArrayLayers;
        public VkSurfaceTransformFlagsKHR supportedTransforms;
        public VkSurfaceTransformFlagBitsKHR currentTransform;
        public VkCompositeAlphaFlagsKHR supportedCompositeAlpha;
        public VkImageUsageFlags supportedUsageFlags;
    };

    public unsafe struct VkFramebufferCreateInfo 
    {
        public VkStructureType sType;
        public void* pNext;
        public VkFramebufferCreateFlags flags;
        public VkRenderPass renderPass;
        public  uint attachmentCount;
        public VkImageView* pAttachments;
        public uint width;
        public uint height;
        public uint layers;
    };
}

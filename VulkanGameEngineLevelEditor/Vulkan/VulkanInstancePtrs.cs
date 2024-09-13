using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor
{
    public unsafe static class VulkanInstance
    {
        static public VkInstance Instance { get; set; }
        static public VkDevice Device { get; set; }
        static public VkPhysicalDevice PhysicalDevice { get; set; }
        static public VkSurfaceKHR Surface { get; set; }
        static public VkCommandPool CommandPool { get; set; }
        static public UInt32 ImageIndex { get; set; }
        static public UInt32 CommandIndex { get; set; }
        static public VkDebugUtilsMessengerEXT DebugMessenger { get; set; }
        static public VkPhysicalDeviceFeatures PhysicalDeviceFeatures { get; set; }
        static public UInt32 SwapChainImageCount;
        static public UInt32 GraphicsFamily;
        static public UInt32 PresentFamily;
        static public VkQueue GraphicsQueue;
        static public VkQueue PresentQueue;
        static public VkFormat Format;
        static public VkColorSpaceKHR ColorSpace;
        static public VkPresentModeKHR PresentMode;
        static public List<VkImage> SwapChainImages;
        static public List<VkImageView> SwapChainImageViews;
        static public VkExtent2D SwapChainResolution;
        static public VkSwapchainKHR Swapchain;
        static public List<VkFence> InFlightFences;
        static public List<VkSemaphore> AcquireImageSemaphores;
        static public List<VkSemaphore> PresentImageSemaphores;
    }
}

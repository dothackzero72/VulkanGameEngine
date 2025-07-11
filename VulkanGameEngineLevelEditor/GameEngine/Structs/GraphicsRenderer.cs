using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct GraphicsRenderer
    {
        public VkInstance Instance { get; set; }
        public VkDevice Device { get; set; }
        public VkPhysicalDevice PhysicalDevice { get; set; }
        public VkSurfaceKHR Surface { get; set; }
        public VkCommandPool CommandPool { get; set; }
        public VkDebugUtilsMessengerEXT DebugMessenger { get; set; }

        public VkFence* InFlightFences { get; set; }
        public VkSemaphore* AcquireImageSemaphores { get; set; }
        public VkSemaphore* PresentImageSemaphores { get; set; }
        public VkImage* SwapChainImages { get; set; }
        public VkImageView* SwapChainImageViews { get; set; }
        public VkExtent2D SwapChainResolution { get; set; }
        public VkSwapchainKHR Swapchain { get; set; }

        public size_t SwapChainImageCount { get; set; }
        public size_t ImageIndex { get; set; }
        public size_t CommandIndex { get; set; }
        public uint GraphicsFamily { get; set; }
        public uint PresentFamily { get; set; }

        public VkQueue GraphicsQueue { get; set; }
        public VkQueue PresentQueue { get; set; }
        public VkFormat Format { get; set; }
        public VkColorSpaceKHR ColorSpace { get; set; }
        public VkPresentModeKHR PresentMode { get; set; }

        public bool RebuildRendererFlag { get; set; }
    }
}

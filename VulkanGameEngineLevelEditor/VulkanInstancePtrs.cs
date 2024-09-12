using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor
{
    public static class VulkanInstance
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
    }
}

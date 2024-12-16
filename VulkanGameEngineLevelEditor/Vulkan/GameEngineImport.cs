using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class GameEngineImport
    {
        private const string DLLPath = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanEngineDLL.dll";

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkInstance DLL_Renderer_CreateVulkanInstance();

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkPhysicalDevice DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint graphicsFamily, uint presentFamily);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint graphicsFamily, uint presentFamily);

        //[DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        //VkExtensionProperties_C* DLL_Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice, int* count);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkSurfaceFormatKHR[] DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, out VkSurfaceKHR[] surface, out int count);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkPresentModeKHR DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, out VkSurfaceKHR[] surface, out int count);
    }
}

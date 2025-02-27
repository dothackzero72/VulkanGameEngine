using Silk.NET.Vulkan;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class GameEngineImport
    {
        private const string DLLPath = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanEngineDLL.dll";

        ///Renderer
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkInstance DLL_Renderer_CreateVulkanInstance();
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint graphicsFamily, uint presentFamily, out VkQueue graphicsQueue, out VkQueue presentQueue);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_CreateSemaphores(VkDevice device, out IntPtr inFlightFences, out IntPtr acquireImageSemaphores, out IntPtr presentImageSemaphores, int maxFramesInFlight);

        //SwapChain
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSurfaceCapabilitiesKHR DLL_SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out uint graphicsFamily, out uint presentFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSurfaceFormatKHR DLL_SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR[] availableFormats);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkPresentModeKHR DLL_SwapChain_FindSwapPresentMode(VkPresentModeKHR[] availablePresentModes);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSwapchainKHR DLL_SwapChain_SetUpSwapChain(VkDevice device, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint graphicsFamily, uint presentFamily, uint width, uint height, out uint swapChainImageCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkImage[] DLL_SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkImageView[] DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, VkImage[] swapChainImageList, out VkSurfaceFormatKHR swapChainImageFormat);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSurfaceFormatKHR* DLL_SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out uint count);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_SwapChain_DeletePhysicalDeviceFormats(VkSurfaceFormatKHR* ptr);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkPresentModeKHR* DLL_SwapChain_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out uint count);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_SwapChain_DeleteSurfacePresentModes(VkPresentModeKHR* ptr);


        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkPhysicalDevice DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint graphicsFamily, uint presentFamily);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint graphicsFamily, uint presentFamily);

        //[DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        //VkExtensionProperties_C* DLL_Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice, int* count);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkSurfaceFormatKHR[] DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, out VkSurfaceKHR[] surface, out int count);


    }
}

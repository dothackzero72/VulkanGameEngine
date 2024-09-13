using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class GameEngineDLL
    {
        private const string DLLPath = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll";
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkInstance DLL_Renderer_CreateVulkanInstance();
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, ref VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref VkPhysicalDeviceFeatures physicalDeviceFeatures, out uint graphicsFamily, out uint presentFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint graphicsFamily, uint presentFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkCommandPool DLL_Renderer_SetUpCommandPool(VkDevice device, uint graphicsFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, out VkFence[] inFlightFences, out VkSemaphore[] acquireImageSemaphores, out VkSemaphore[] presentImageSemaphores, int maxFramesInFlight);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint graphicsFamily, uint presentFamily, out VkQueue graphicsQueue, out VkQueue presentQueue);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR[] surfaceFormats, ref uint surfaceFormatCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_GetPresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR presentModes, ref int presentModeCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSurfaceFormatKHR DLL_SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR[] availableFormats, uint availableFormatsCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkPresentModeKHR DLL_SwapChain_FindSwapPresentMode(VkPresentModeKHR[] availablePresentModes, uint availablePresentModesCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out uint graphicsFamily, out uint presentFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR surfaceCapabilities);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out VkSurfaceFormatKHR[] compatibleSwapChainFormatList, out uint surfaceFormatCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out VkPresentModeKHR[] compatiblePresentModesList, out uint presentModeCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSwapchainKHR DLL_SwapChain_SetUpSwapChain(VkDevice device, VkSurfaceKHR surface, IntPtr surfaceCapabilities, VkSurfaceFormatKHR swapChainImageFormat, VkPresentModeKHR swapChainPresentMode, uint graphicsFamily, uint presentFamily, uint width, uint height, out uint swapChainImageCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkImage DLL_SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR[] swapChain, out uint swapChainImageCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkImageView DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, VkImage[] swapChainImages, VkSurfaceFormatKHR swapChainImageFormat, uint swapChainImageCount);
    }
}

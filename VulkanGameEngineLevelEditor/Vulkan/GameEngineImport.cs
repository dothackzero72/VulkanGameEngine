using Silk.NET.Vulkan;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class GameEngineImport
    {
        private const string DLLPath = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanEngineDLL.dll";

        ///Renderer
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkInstance DLL_Renderer_CreateVulkanInstance();
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint graphicsFamily, uint presentFamily, out VkQueue graphicsQueue, out VkQueue presentQueue);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, out IntPtr inFlightFences, out IntPtr acquireImageSemaphores, out IntPtr presentImageSemaphores, int maxFramesInFlight);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkPhysicalDevice DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint graphicsFamily, uint presentFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint graphicsFamily, uint presentFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkPresentModeKHR* DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out uint count);

        //SwapChain
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_Texture_UpdateTextureLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkImageLayout* oldImageLayout, VkImageLayout* newImageLayout, uint mipLevel);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_Texture_UpdateCmdTextureLayout(VkCommandBuffer* commandBuffer, VkImage image, VkImageLayout oldImageLayout, VkImageLayout* newImageLayout, uint mipLevel);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSurfaceCapabilitiesKHR DLL_SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out uint width, out uint height);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out uint graphicsFamily, out uint presentFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSurfaceFormatKHR DLL_SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR* availableFormats, uint count);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkPresentModeKHR DLL_SwapChain_FindSwapPresentMode(VkPresentModeKHR* availablePresentModes, uint count);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSwapchainKHR DLL_SwapChain_SetUpSwapChain(VkDevice device, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint graphicsFamily, uint presentFamily, uint width, uint height, out uint swapChainImageCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkImage* DLL_SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint swapChainImageCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkImageView* DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, VkImage* swapChainImageList, VkSurfaceFormatKHR swapChainImageFormat, uint swapChainImageCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSurfaceFormatKHR* DLL_SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out uint count);

        //Texture
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_BaseCreateTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, ref VkImage image, ref VkDeviceMemory memory, VkImageCreateInfo imageCreateInfo);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_NewTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint mipmapLevels, VkFormat textureByteFormat);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CreateTextureImage(VkDevice Device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint mipmapLevels, VkFormat textureByteFormat);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, uint mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkBuffer buffer, TextureUsageEnum textureType, int width, int height, int depth);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkFormat* textureByteFormat, uint mipmapLevels, int width, int height);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, uint mipmapLevels);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler);

        //Invoke Tools
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_DeleteAllocatedPtr(void* ptr);

    }
}

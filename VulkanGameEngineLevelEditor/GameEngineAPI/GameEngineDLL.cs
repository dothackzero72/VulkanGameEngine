using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class GameEngineDLL
    {
        private const string DLLPath = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll";

        //Renderer functions:
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkInstance DLL_Renderer_CreateVulkanInstance();
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, ref VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref VkPhysicalDeviceFeatures physicalDeviceFeatures, out uint graphicsFamily, out uint presentFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint graphicsFamily, uint presentFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkCommandPool DLL_Renderer_SetUpCommandPool(VkDevice device, uint graphicsFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, out IntPtr inFlightFences, out IntPtr acquireImageSemaphores, out IntPtr presentImageSemaphores, int maxFramesInFlight);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint graphicsFamily, uint presentFamily, out VkQueue graphicsQueue, out VkQueue presentQueue);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR[] surfaceFormatList, ref uint surfaceFormatCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_GetPresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR[] presentModeList, ref uint presentModeCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSurfaceFormatKHR DLL_SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR[] availableFormats, uint availableFormatsCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkPresentModeKHR DLL_SwapChain_FindSwapPresentMode(VkPresentModeKHR[] availablePresentModes, uint availablePresentModesCount);
        
        //SwapChain functions:
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out uint graphicsFamily, out uint presentFamily);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out VkSurfaceCapabilitiesKHR surfaceCapabilities);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, IntPtr compatibleSwapChainFormatList, ref uint surfaceFormatCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, IntPtr compatiblePresentModesList, out uint presentModeCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkSwapchainKHR DLL_SwapChain_SetUpSwapChain(VkDevice device, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR surfaceCapabilities, VkSurfaceFormatKHR swapChainImageFormat, VkPresentModeKHR swapChainPresentMode, uint graphicsFamily, uint presentFamily, uint width, uint height,  out uint swapChainImageCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern IntPtr DLL_SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint swapChainImageCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkImageView[] DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, VkImage[] swapChainImages, VkSurfaceFormatKHR swapChainImageFormat, uint swapChainImageCount);

        //Buffer functions:
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Buffer_CreateBuffer(
            IntPtr bufferInfo,         // Pointer to VulkanBufferInfo (not ref)
            IntPtr bufferData,         // Pointer to data (void*)
            ulong bufferSize,          // Size of the buffer
            VkBufferUsageFlags bufferUsage,
            VkMemoryPropertyFlagBits properties);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Buffer_CreateStagingBuffer(ref VulkanBufferInfo bufferInfo, VkBuffer bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlagBits properties);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Buffer_CopyBuffer(VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize size);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Buffer_CopyStagingBuffer(ref VulkanBufferInfo bufferInfo, VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize size);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Buffer_UpdateBufferSize(ref VulkanBufferInfo bufferInfo, VkDeviceSize bufferSize);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Buffer_UnmapBufferMemory(ref VulkanBufferInfo bufferInfo);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Buffer_UpdateBufferMemory(ref VulkanBufferInfo bufferInfo, IntPtr dataToCopy, VkDeviceSize bufferSize);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Buffer_UpdateStagingBufferMemory(ref VulkanBufferInfo bufferInfo, IntPtr dataToCopy, VkDeviceSize bufferSize);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern IntPtr DLL_Buffer_MapBufferMemory(ref VulkanBufferInfo bufferInfo);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_Buffer_DestroyBuffer(ref VulkanBufferInfo bufferInfo);

        //Texture Functions:
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, UInt32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CreateTextureImage(VkDevice device, ref VkImage image, VkDeviceMemory memory, int width, int height, UInt32 mipmapLevels, VkFormat textureByteFormat);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_QuickTransitionImageLayout(VkImage image, UInt32 mipmapLevels, out VkImageLayout oldLayout, out VkImageLayout newLayout);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, UInt32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CopyBufferToTexture(VkImage image, out VkBuffer buffer, TextureUsageEnum textureType, int width, int height, int depth);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_GenerateMipmaps(VkPhysicalDevice physicalDevice, VkImage image, out VkFormat textureByteFormat, UInt32 mipmapLevels, int width, int height);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CreateTextureView(VkDevice device, out VkImageView view, VkImage image, VkFormat format, UInt32 mipmapLevels);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CreateTextureSampler(VkDevice device, out VkSamplerCreateInfo samplerCreateInfo);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_BUFFER_BufferTest();
    }
}

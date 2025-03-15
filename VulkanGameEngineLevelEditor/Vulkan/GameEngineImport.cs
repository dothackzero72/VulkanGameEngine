using GlmSharp;
using Silk.NET.Vulkan;
using StbImageSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts.Vulkan;
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
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, VkFence* inFlightFences, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, uint swapChainImageCount);
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
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CreateImage(VkDevice device, VkPhysicalDevice physicalDevice, ref VkImage image, ref VkDeviceMemory memory, VkImageCreateInfo ImageCreateInfo);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, uint mipmapLevels, ref VkImageLayout oldLayout, ref VkImageLayout newLayout);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkBuffer buffer, TextureUsageEnum textureType, int width, int height, int depth);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkFormat* textureByteFormat, uint mipmapLevels, int width, int height);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, uint mipmapLevels);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler);
        [DllImport(DLLPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)] public static extern void DLL_Texture_CreateImageTexture(VkDevice device,
                                                                                                                                                            VkPhysicalDevice physicalDevice,
                                                                                                                                                            VkCommandPool commandPool,
                                                                                                                                                            VkQueue graphicsQueue,
                                                                                                                                                            ref int width,
                                                                                                                                                            ref int height,
                                                                                                                                                            ref int depth,
                                                                                                                                                            VkFormat textureByteFormat,
                                                                                                                                                            uint mipmapLevels,
                                                                                                                                                            ref VkImage textureImage,
                                                                                                                                                            ref VkDeviceMemory textureMemory,
                                                                                                                                                            ref VkImageLayout textureImageLayout,
                                                                                                                                                            ref ColorComponents colorChannelUsed,
                                                                                                                                                            TextureUsageEnum textureUsage,
                                                                                                                                                            string filePath);

        //RenderPass
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_RenderPass_BuildRenderPass(VkDevice device, VkRenderPass* renderPass, RenderPassBuildInfoModel* renderPassBuildInfo, RenderedTexture* renderedColorTextureList, DepthTexture depthTexture);
	    [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, RenderPassBuildInfoModel renderPassBuildInfo, RenderedTexture* renderedColorTextureList, VkFramebuffer* frameBufferList, DepthTexture* depthTexture, ivec2 renderPassResolution);

        //Pipeline
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern unsafe VkDescriptorPool DLL_Pipeline_CreateDescriptorPool(
     VkDevice device,
     RenderPipelineDLL* renderPipelineModel,
     GPUIncludes* includes);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_Pipeline_CreateDescriptorSetLayout(VkDevice device, RenderPipelineModel model, GPUIncludes includes, VkDescriptorSetLayout* descriptorSetLayoutList, uint descriptorSetCount);
	    [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkDescriptorSet* DLL_Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, VkDescriptorSetLayout* descriptorSetLayoutList, out size_t outCount);
	    [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_Pipeline_UpdateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSetList, RenderPipelineModel model, GPUIncludes includes);
    	[DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_Pipeline_CreatePipelineLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayoutList, uint constBufferSize, VkPipelineLayout* pipelineLayout);
    	[DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_Pipeline_CreatePipeline(VkDevice device,
                                                                                                                                  VkRenderPass renderpass,
                                                                                                                                  VkPipelineLayout pipelineLayout,
                                                                                                                                  VkPipelineCache pipelineCache,
                                                                                                                                  RenderPipelineModel model,
                                                                                                                                  VkVertexInputBindingDescription* vertexBindingList,
                                                                                                                                  VkVertexInputAttributeDescription* vertexAttributeList,
                                                                                                                                  VkPipeline pipeline);
        //Invoke Tools
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern uint DLL_Tools_GetMemoryType(VkPhysicalDevice physicalDevice, uint typeFilter, VkMemoryPropertyFlagBits properties);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void DLL_Tools_DeleteAllocatedPtr(void* ptr);

    }
}

using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class GameEngineDLL
    {
        private const string DLLPath = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll";

        // Texture functions
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Texture_CreateTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint mipmapLevels, VkFormat textureByteFormat);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, uint mipmapLevels, ref VkImageLayout oldLayout, ref VkImageLayout newLayout);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkBuffer buffer, TextureUsageEnum textureType, int width, int height, int depth);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, ref VkFormat textureByteFormat, uint mipmapLevels, int width, int height);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Texture_CreateTextureView(VkDevice device, out VkImageView view, VkImage image, VkFormat format, uint mipmapLevels);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Texture_CreateTextureSampler(VkDevice device, ref VkSamplerCreateInfo samplerCreateInfo, out VkSampler sampler);

        // Buffer functions
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Buffer_AllocateMemory(VkDevice device, VkPhysicalDevice physicalDevice, out VkBuffer bufferData, out VkDeviceMemory bufferMemory, VkMemoryPropertyFlagBits properties);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Buffer_CreateBuffer(VkDevice device, VkPhysicalDevice physicalDevice, out VkBuffer buffer, out VkDeviceMemory bufferMemory, IntPtr bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlagBits properties);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Buffer_CreateStagingBuffer(VkDevice device, VkPhysicalDevice physicalDevice, out VkBuffer stagingBuffer, out VkDeviceMemory stagingBufferMemory, IntPtr bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlagBits properties);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Buffer_CopyBuffer(VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize size);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Buffer_CopyStagingBuffer(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize size);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Buffer_UpdateBufferSize(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer buffer, out VkDeviceMemory bufferMemory, IntPtr bufferData, out VkDeviceSize oldBufferSize, VkDeviceSize newBufferSize, VkBufferUsageFlags bufferUsageFlags, VkMemoryPropertyFlagBits propertyFlags);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Buffer_UpdateBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, IntPtr dataToCopy, VkDeviceSize bufferSize);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Buffer_UpdateStagingBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, IntPtr dataToCopy, VkDeviceSize bufferSize);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Buffer_UnmapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, out bool isMapped);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DLL_Buffer_MapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, VkDeviceSize bufferSize, out bool isMapped);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Buffer_DestroyBuffer(VkDevice device, ref VkBuffer buffer, ref VkBuffer stagingBuffer, ref VkDeviceMemory bufferMemory, ref VkDeviceMemory stagingBufferMemory, IntPtr bufferData, ref VkDeviceSize bufferSize, ref VkBufferUsageFlags bufferUsageFlags, ref VkMemoryPropertyFlagBits propertyFlags);
      
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern int DLL_BUFFER_BufferTest();

        // Renderer functions
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkInstance DLL_Renderer_CreateVulkanInstance();
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, ref VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref VkPhysicalDeviceFeatures physicalDeviceFeatures, out uint graphicsFamily, out uint presentFamily);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint graphicsFamily, uint presentFamily);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkCommandPool DLL_Renderer_SetUpCommandPool(VkDevice device, uint graphicsFamily);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, out IntPtr inFlightFences, out IntPtr acquireImageSemaphores, out IntPtr presentImageSemaphores, int maxFramesInFlight);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint graphicsFamily, uint presentFamily, out VkQueue graphicsQueue, out VkQueue presentQueue);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out IntPtr presentModes, out uint presentModeCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_GetPresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR[] presentMode, out uint presentModeCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern bool DLL_Renderer_GetRayTracingSupport();
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_GetRendererFeatures(VkPhysicalDeviceVulkan11Features* physicalDeviceVulkan11Features);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_GetWin32Extensions(out uint extensionCount, out IntPtr enabledExtensions);

        // Debug functions
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DLL_Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice, out uint deviceExtensionCountPtr);

        // Command and Rendering functions
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_CreateCommandBuffers(VkDevice device, VkCommandPool commandPool, VkCommandBuffer* commandBufferList, uint commandBufferCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_CreateFrameBuffer(VkDevice device, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_CreateRenderPass(VkDevice device, IntPtr renderPassCreateInfo);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_CreateDescriptorPool(VkDevice device, out VkDescriptorPool descriptorPool, IntPtr descriptorPoolCreateInfo);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_CreateDescriptorSetLayout(VkDevice device, out VkDescriptorSetLayout descriptorSetLayout, IntPtr descriptorSetLayoutCreateInfo);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_CreatePipelineLayout(VkDevice device, out VkPipelineLayout pipelineLayout, IntPtr pipelineLayoutCreateInfo);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_AllocateDescriptorSets(VkDevice device, out VkDescriptorSet descriptorSet, IntPtr descriptorSetAllocateInfo);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_AllocateCommandBuffers(VkDevice device, out VkCommandBuffer commandBuffer, IntPtr commandBufferAllocateInfo);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_CreateGraphicsPipelines(VkDevice device, out VkPipeline graphicPipeline, IntPtr createGraphicPipelines, uint createGraphicPipelinesCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_CreateCommandPool(VkDevice device, out VkCommandPool commandPool, VkCommandPoolCreateInfo* commandPoolInfo);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_UpdateDescriptorSet(VkDevice device, List<VkWriteDescriptorSet> writeDescriptorSet, uint count);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_GetSurfaceFormats(
        VkPhysicalDevice physicalDevice,
        VkSurfaceKHR surface,
        IntPtr surfaceFormats, // We will handle this with an IntPtr to manage properly.
        ref uint surfaceFormatCount);

        // Frame and Submission functions
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, out uint pImageIndex, out uint pCommandIndex, out bool pRebuildRendererFlag);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint commandIndex, uint imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint commandBufferCount, out bool rebuildRendererFlag);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_SubmitDraw(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint commandIndex, uint imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint commandBufferCount, out bool rebuildRendererFlag);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern uint DLL_Renderer_GetMemoryType(VkPhysicalDevice physicalDevice, uint typeFilter, VkMemoryPropertyFlagBits properties);

        // Command Buffer management functions
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkCommandBuffer DLL_Renderer_BeginSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_Renderer_EndSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer);

        // Destruction Functions
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyFences(VkDevice device, ref VkSemaphore acquireImageSemaphores, ref VkSemaphore presentImageSemaphores, ref VkFence fences, uint semaphoreCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyCommandPool(VkDevice device, ref VkCommandPool commandPool);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyDevice(VkDevice device);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroySurface(VkInstance instance, ref VkSurfaceKHR surface);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyDebugger(ref VkInstance instance);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyInstance(ref VkInstance instance);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyRenderPass(VkDevice device, ref VkRenderPass renderPass);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyFrameBuffers(VkDevice device, ref VkFramebuffer frameBufferList, uint count);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyDescriptorPool(VkDevice device, ref VkDescriptorPool descriptorPool);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyDescriptorSetLayout(VkDevice device, ref VkDescriptorSetLayout descriptorSetLayout);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyCommandBuffers(VkDevice device, ref VkCommandPool commandPool, ref VkCommandBuffer commandBufferList, uint count);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyBuffer(VkDevice device, ref VkBuffer buffer);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_FreeDeviceMemory(VkDevice device, ref VkDeviceMemory memory);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroySwapChainImageView(VkDevice device, ref VkImageView pSwapChainImageViewList, uint count);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroySwapChain(VkDevice device, ref VkSwapchainKHR swapChain);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyImageView(VkDevice device, ref VkImageView imageView);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyImage(VkDevice device, ref VkImage image);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroySampler(VkDevice device, ref VkSampler sampler);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyPipeline(VkDevice device, ref VkPipeline pipeline);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyPipelineLayout(VkDevice device, ref VkPipelineLayout pipelineLayout);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Renderer_DestroyPipelineCache(VkDevice device, ref VkPipelineCache pipelineCache);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern int DLL_Renderer_SimpleTestLIB();

        // SwapChain functions
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkSurfaceFormatKHR DLL_SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR[] availableFormats, uint availableFormatsCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkPresentModeKHR DLL_SwapChain_FindSwapPresentMode(VkPresentModeKHR[] availablePresentModes, uint availablePresentModesCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out uint graphicsFamily, out uint presentFamily);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out VkSurfaceCapabilitiesKHR surfaceCapabilities);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out IntPtr compatibleSwapChainFormatList, out uint surfaceFormatCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkResult DLL_SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out IntPtr compatiblePresentModesList, out uint presentModeCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern VkSwapchainKHR DLL_SwapChain_SetUpSwapChain(VkDevice device, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR surfaceCapabilities, VkSurfaceFormatKHR swapChainImageFormat, VkPresentModeKHR swapChainPresentMode, uint graphicsFamily, uint presentFamily, uint width, uint height, out uint swapChainImageCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DLL_SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint swapChainImageCount);
        
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, VkImage[] swapChainImages, ref VkSurfaceFormatKHR swapChainImageFormat, uint swapChainImageCount);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DLL_Mesh_CreateMesh();

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DLL_Mesh_CreateMesh2D();

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Mesh_DestroyMesh(IntPtr mesh);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Mesh_Update(IntPtr mesh, float deltaTime);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Mesh_BufferUpdate(IntPtr mesh, IntPtr commandBuffer, float deltaTime);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Mesh_Draw(
            IntPtr mesh,
            IntPtr commandBuffer,
            IntPtr pipeline,
            IntPtr shaderPipelineLayout,
            IntPtr descriptorSet,
            MeshProperitiesStruct sceneProperties);

        // shader stuff
        [DllImport(DLLPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern VkShaderModule DLL_Shader_BuildGLSLShaderFile(string path);

        [DllImport(DLLPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern VkPipelineShaderStageCreateInfo DLL_Shader_CreateShader(
            string shaderModule,
            VkShaderStageFlagBits shaderStages);
    }
}
//#include "VulkanImp.h"
//
//void GameEngineAPI_GetWin32Extensions(uint32_t* extensionCount, const char*** enabledExtensions) 
//{
//    Renderer_GetWin32Extensions(extensionCount, enabledExtensions);
//}
//
// VkInstance GameEngineAPI_CreateVulkanInstance(RichTextBoxCallback callback)
// {
//     VkInstance instance = Renderer_CreateVulkanInstance();
//     return instance;
//}
//
// VkDebugUtilsMessengerEXT GameEngineAPI_SetupDebugMessenger(RichTextBoxCallback callback, VkInstance instance)
// {
//     return Renderer_SetupDebugMessenger(instance);
// }
//
// VkResult GameEngineAPI_Renderer_SetUpPhysicalDevice(VkPhysicalDevice* physicalDevice, VkSurfaceKHR surface, VkPhysicalDeviceFeatures* physicalDeviceFeatures, uint32_t* graphicsFamily, uint32_t* presentFamily)
// {
//     return Renderer_SetUpPhysicalDevice(physicalDevice, surface, physicalDeviceFeatures, graphicsFamily, presentFamily);
// }
//
// VkDevice GameEngineAPI_Render_SetUpDevice(VkPhysicalDevice physicalDevice)
// {
//     return Render_SetUpDevice(physicalDevice);
//  }
//
// VkCommandPool GameEngineAPI_Renderer_SetUpCommandPool(VkDevice device)
// {
//     return Renderer_SetUpCommandPool(device);
// }
//
// VkResult GameEngineAPI_Renderer_SetUpSemaphores(VkDevice device, VkFence** inFlightFences, VkSemaphore** acquireImageSemaphores, VkSemaphore** presentImageSemaphores, int maxFramesInFlight)
// {
//     return Renderer_SetUpSemaphores(device, inFlightFences, acquireImageSemaphores, presentImageSemaphores,  maxFramesInFlight);
// }
//
// VkResult GameEngineAPI_Renderer_GetDeviceQueue(VkDevice device, uint32_t graphicsFamily, uint32_t presentFamily, VkQueue* graphicsQueue, VkQueue* presentQueue)
// {
//     return Renderer_GetDeviceQueue(device, graphicsFamily, presentFamily, graphicsQueue, presentQueue);
// }
//
// VkSurfaceFormatKHR GameEngineAPI_FindSwapSurfaceFormat(VkSurfaceFormatKHR* availableFormats, uint32_t availableFormatsCount)
// {
//     return SwapChain_FindSwapSurfaceFormat(availableFormats, availableFormatsCount);
// }
//
// VkPresentModeKHR GameEngineAPI_FindSwapPresentMode(VkPresentModeKHR* availablePresentModes, uint32_t availablePresentModesCount)
// {
//     return SwapChain_FindSwapPresentMode(availablePresentModes, availablePresentModesCount);
// }
//
// VkResult GameEngineAPI_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32_t* graphicsFamily, uint32_t* presentFamily)
// {
//    return SwapChain_GetQueueFamilies(physicalDevice, surface, graphicsFamily, presentFamily);
// }
//
// VkResult GameEngineAPI_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR* surfaceCapabilities)
// {
//     return SwapChain_GetSurfaceCapabilities(physicalDevice, surface, surfaceCapabilities);
// }
//
// VkResult GameEngineAPI_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR** compatibleSwapChainFormatList, uint32_t* surfaceFormatCount)
// {
//     return SwapChain_GetPhysicalDeviceFormats(physicalDevice, surface, compatibleSwapChainFormatList, surfaceFormatCount);
// }
//
// VkResult GameEngineAPI_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR** compatiblePresentModesList, uint32_t* presentModeCount)
// {
//     return GetPhysicalDevicePresentModes(physicalDevice, surface, compatiblePresentModesList, presentModeCount);
// }
//
// VkSwapchainKHR GameEngineAPI_SetUpSwapChain(VkDevice device, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR surfaceCapabilities, VkSurfaceFormatKHR SwapChainImageFormat, VkPresentModeKHR SwapChainPresentMode, uint32_t width, uint32_t height, uint32_t* swapChainImageCount)
// {
//     return SwapChain_SetUpSwapChain( device,  surface,  surfaceCapabilities,  SwapChainImageFormat,  SwapChainPresentMode,  width,  height,   swapChainImageCount);
// }
//
// VkImage* GameEngineAPI_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint32_t* swapChainImageCount)
// {
//     return SwapChain_SetUpSwapChainImages(device, swapChain, swapChainImageCount);
// }
//
// VkImageView* GameEngineAPI_SetUpSwapChainImageViews(VkDevice device, VkImage* swapChainImages, VkSurfaceFormatKHR* swapChainImageFormat, uint32_t swapChainImageCount)
// {
//     return SwapChain_SetUpSwapChainImageViews(device, swapChainImages, swapChainImageFormat, swapChainImageCount);
// }
//
// void GameEngineAPI_DestroyFences(VkDevice* device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount, int bufferCount)
// {
//     Renderer_CSDestroyFences(device, acquireImageSemaphores, presentImageSemaphores, fences, semaphoreCount, bufferCount);
// }
//
// void GameEngineAPI_DestroyCommandPool(VkDevice* device, VkCommandPool* commandPool)
// {
//     Renderer_CSDestroyCommandPool(device, commandPool);
// }
//
// void GameEngineAPI_DestroyDevice(VkDevice* device)
// {
//     Renderer_CSDestroyDevice(device);
// }
//
// void GameEngineAPI_DestroySurface(VkInstance* instance, VkSurfaceKHR* surface)
// {
//     Renderer_CSDestroySurface(instance, surface);
// }
//
// void GameEngineAPI_DestroyDebugger(VkInstance* instance)
// {
//     Renderer_CSDestroyDebugger(instance);
// }
//
// void GameEngineAPI_DestroyInstance(VkInstance* instance)
// {
//     Renderer_CSDestroyInstance(instance);
// }
//
// void GameEngineAPI_DestroyRenderPass(VkRenderPass* renderPass)
// {
//     Renderer_DestroyRenderPass(renderPass);
// }
//
// void GameEngineAPI_DestroyFrameBuffers(VkFramebuffer* frameBufferList)
// {
//     Renderer_DestroyFrameBuffers(frameBufferList);
// }
//
// void GameEngineAPI_DestroyDescriptorPool(VkDescriptorPool* descriptorPool)
// {
//     Renderer_DestroyDescriptorPool(descriptorPool);
// }
//
// void GameEngineAPI_DestroyDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout)
// {
//     Renderer_DestroyDescriptorSetLayout(descriptorSetLayout);
// }
//
// void GameEngineAPI_DestroyCommandBuffers(VkCommandPool* commandPool, VkCommandBuffer* commandBufferList)
// {
//     Renderer_DestroyCommandBuffers(commandPool, commandBufferList);
// }
//
//
// void GameEngineAPI_DestroyBuffer(VkBuffer* buffer)
// {
//     Renderer_DestroyBuffer(buffer);
// }
//
// void GameEngineAPI_FreeMemory(VkDeviceMemory* memory)
// {
//     Renderer_FreeMemory(memory);
// }
//
// void GameEngineAPI_DestroyImageView(VkImageView* imageView)
// {
//     Renderer_DestroyImageView(imageView);
// }
//
// void GameEngineAPI_DestroyImage(VkImage* image)
// {
//     Renderer_DestroyImage(image);
// }
//
// void GameEngineAPI_DestroySampler(VkSampler* sampler)
// {
//     Renderer_DestroySampler(sampler);
// }
//
// void GameEngineAPI_DestroyPipeline(VkPipeline* pipeline)
// {
//     Renderer_DestroyPipeline(pipeline);
// }
//
// void GameEngineAPI_DestroyPipelineLayout(VkPipelineLayout* pipelineLayout)
// {
//     Renderer_DestroyPipelineLayout(pipelineLayout);
// }
//
// void GameEngineAPI_DestroyPipelineCache(VkPipelineCache* pipelineCache)
// {
//     Renderer_DestroyPipelineCache(pipelineCache);
// }
//
// int SimpleTestLIB()
// {
//     return Renderer_SimpleTestLIB();
// }
//
// int SimpleTestDLL()
//{
//    return 42;
//}
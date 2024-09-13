//#include <windows.h>
//#include <vulkan/vulkan.h>
//#include <VulkanRenderer.h>
//#include <VulkanSwapChain.h>
//
//
//DLL_EXPORT void GameEngineAPI_GetWin32Extensions(uint32_t* extensionCount, const char*** enabledExtensions);
//DLL_EXPORT VkInstance GameEngineAPI_CreateVulkanInstance(RichTextBoxCallback callback);
//DLL_EXPORT VkDebugUtilsMessengerEXT GameEngineAPI_SetupDebugMessenger(RichTextBoxCallback callback, VkInstance instance);
//DLL_EXPORT VkResult GameEngineAPI_Renderer_SetUpPhysicalDevice(VkPhysicalDevice* physicalDevice, VkSurfaceKHR surface, VkPhysicalDeviceFeatures* physicalDeviceFeatures, uint32_t* graphicsFamily, uint32_t* presentFamily);
//DLL_EXPORT VkDevice GameEngineAPI_Render_SetUpDevice(VkPhysicalDevice physicalDevice);
//DLL_EXPORT VkCommandPool GameEngineAPI_Renderer_SetUpCommandPool(VkDevice device);
//DLL_EXPORT VkResult GameEngineAPI_Renderer_SetUpSemaphores(VkDevice device, VkFence** inFlightFences, VkSemaphore** acquireImageSemaphores, VkSemaphore** presentImageSemaphores, int maxFramesInFlight);
//DLL_EXPORT VkResult GameEngineAPI_Renderer_GetDeviceQueue(VkDevice device, uint32_t graphicsFamily, uint32_t presentFamily, VkQueue* graphicsQueue, VkQueue* presentQueue);
//DLL_EXPORT VkSurfaceFormatKHR GameEngineAPI_FindSwapSurfaceFormat(VkSurfaceFormatKHR* availableFormats, uint32_t availableFormatsCount);
//DLL_EXPORT VkPresentModeKHR GameEngineAPI_FindSwapPresentMode(VkPresentModeKHR* availablePresentModes, uint32_t availablePresentModesCount);
//DLL_EXPORT VkResult GameEngineAPI_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32_t* graphicsFamily, uint32_t* presentFamily);
//DLL_EXPORT VkResult GameEngineAPI_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR* surfaceCapabilities);
//DLL_EXPORT VkResult GameEngineAPI_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR** compatibleSwapChainFormatList, uint32_t* surfaceFormatCount);
//DLL_EXPORT VkResult GameEngineAPI_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR** compatiblePresentModesList, uint32_t* presentModeCount);
//DLL_EXPORT VkSwapchainKHR GameEngineAPI_SetUpSwapChain(VkDevice device, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR surfaceCapabilities, VkSurfaceFormatKHR SwapChainImageFormat, VkPresentModeKHR SwapChainPresentMode, uint32_t width, uint32_t height, uint32_t* swapChainImageCount);
//DLL_EXPORT VkImage* GameEngineAPI_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint32_t* swapChainImageCount);
//DLL_EXPORT VkImageView* GameEngineAPI_SetUpSwapChainImageViews(VkDevice device, VkImage* swapChainImages, VkSurfaceFormatKHR* swapChainImageFormat, uint32_t swapChainImageCount);
//
//DLL_EXPORT void GameEngineAPI_DestroyPipelineCache(VkPipelineCache* pipelineCache);
//DLL_EXPORT void GameEngineAPI_DestroyFences(VkDevice* device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount, int bufferCount);
//DLL_EXPORT void GameEngineAPI_DestroyCommandPool(VkDevice* device, VkCommandPool* commandPool);
//DLL_EXPORT void GameEngineAPI_DestroyDevice(VkDevice* device);
//DLL_EXPORT void GameEngineAPI_DestroySurface(VkInstance* instance, VkSurfaceKHR* surface);
//DLL_EXPORT void GameEngineAPI_DestroyDebugger(VkInstance* instance);
//DLL_EXPORT void GameEngineAPI_DestroyInstance(VkInstance* instance);
//DLL_EXPORT void GameEngineAPI_DestroyRenderPass(VkRenderPass* renderPass);
//DLL_EXPORT void GameEngineAPI_DestroyFrameBuffers(VkFramebuffer* frameBufferList);
//DLL_EXPORT void GameEngineAPI_DestroyDescriptorPool(VkDescriptorPool* descriptorPool);
//DLL_EXPORT void GameEngineAPI_DestroyDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout);
//DLL_EXPORT void GameEngineAPI_DestroyCommandBuffers(VkCommandPool* commandPool, VkCommandBuffer* commandBufferList);
//DLL_EXPORT void GameEngineAPI_DestroyBuffer(VkBuffer* buffer);
//DLL_EXPORT void GameEngineAPI_FreeMemory(VkDeviceMemory* memory);
//DLL_EXPORT void GameEngineAPI_DestroyImageView(VkImageView* imageView);
//DLL_EXPORT void GameEngineAPI_DestroyImage(VkImage* image);
//DLL_EXPORT void GameEngineAPI_DestroySampler(VkSampler* sampler);
//DLL_EXPORT void GameEngineAPI_DestroyPipeline(VkPipeline* pipeline);
//DLL_EXPORT void GameEngineAPI_DestroyPipelineLayout(VkPipelineLayout* pipelineLayout);
//DLL_EXPORT void GameEngineAPI_DestroyPipelineCache(VkPipelineCache* pipelineCache);
//
//DLL_EXPORT int SimpleTestLIB();
//DLL_EXPORT int SimpleTestDLL();
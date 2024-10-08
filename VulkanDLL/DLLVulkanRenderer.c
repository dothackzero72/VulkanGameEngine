#include "DLLVulkanRenderer.h"

VkInstance DLL_Renderer_CreateVulkanInstance() {
    return Renderer_CreateVulkanInstance();
}

VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance) {
    return Renderer_SetupDebugMessenger(instance);
}

VkResult DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkPhysicalDevice* physicalDevice,
    VkSurfaceKHR surface, VkPhysicalDeviceFeatures* physicalDeviceFeatures,
    uint32_t* graphicsFamily, uint32_t* presentFamily) {
    return Renderer_SetUpPhysicalDevice(instance, physicalDevice, surface, physicalDeviceFeatures, graphicsFamily, presentFamily);
}

VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32_t graphicsFamily,
    uint32_t presentFamily) {
    return Renderer_SetUpDevice(physicalDevice, graphicsFamily, presentFamily);
}

VkCommandPool DLL_Renderer_SetUpCommandPool(VkDevice device, uint32_t graphicsFamily) {
    return Renderer_SetUpCommandPool(device, graphicsFamily);
}

VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, VkFence** inFlightFences,
    VkSemaphore** acquireImageSemaphores, VkSemaphore** presentImageSemaphores,
    int maxFramesInFlight) {
    return Renderer_SetUpSemaphores(device, inFlightFences, acquireImageSemaphores, presentImageSemaphores, maxFramesInFlight);
}

VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint32_t graphicsFamily, uint32_t presentFamily,
    VkQueue* graphicsQueue, VkQueue* presentQueue) {
    return Renderer_GetDeviceQueue(device, graphicsFamily, presentFamily, graphicsQueue, presentQueue);
}

VkResult DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface,
    VkPresentModeKHR** presentModes, uint32_t* presentModeCount) {
    return Renderer_GetSurfacePresentModes(physicalDevice, surface, presentModes, presentModeCount);
}

bool DLL_Renderer_GetRayTracingSupport() {
    return Renderer_GetRayTracingSupport();
}

VkResult DLL_Renderer_CreateRenderPass(VkDevice device, RenderPassCreateInfoStruct* renderPassCreateInfo)
{
    return Renderer_CreateRenderPass(device, renderPassCreateInfo);
}

void DLL_Renderer_GetRendererFeatures(VkPhysicalDeviceVulkan11Features* physicalDeviceVulkan11Features) {
    Renderer_GetRendererFeatures(physicalDeviceVulkan11Features);
}

VkResult DLL_Renderer_GetWin32Extensions(uint32_t* extensionCount, char*** enabledExtensions) {
    return Renderer_GetWin32Extensions(extensionCount, enabledExtensions);
}

VkResult DLL_Renderer_CreateCommandBuffers(VkDevice device, VkCommandPool commandPool,
    VkCommandBuffer* commandBufferList, uint32_t commandBufferCount) {
    return Renderer_CreateCommandBuffers(device, commandPool, commandBufferList, commandBufferCount);
}

VkResult DLL_Renderer_CreateFrameBuffer(VkDevice device, VkFramebuffer* pFrameBuffer,
    VkFramebufferCreateInfo* frameBufferCreateInfo) {
    return Renderer_CreateFrameBuffer(device, pFrameBuffer, frameBufferCreateInfo);
}

VkResult DLL_Renderer_CreateDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool,
    VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo) {
    return Renderer_CreateDescriptorPool(device, descriptorPool, descriptorPoolCreateInfo);
}

VkResult DLL_Renderer_CreateDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout,
    VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo) {
    return Renderer_CreateDescriptorSetLayout(device, descriptorSetLayout, descriptorSetLayoutCreateInfo);
}

VkResult DLL_Renderer_CreatePipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout,
    VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo) {
    return Renderer_CreatePipelineLayout(device, pipelineLayout, pipelineLayoutCreateInfo);
}

VkResult DLL_Renderer_AllocateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSet,
    VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo) {
    return Renderer_AllocateDescriptorSets(device, descriptorSet, descriptorSetAllocateInfo);
}

VkResult DLL_Renderer_AllocateCommandBuffers(VkDevice device, VkCommandBuffer* commandBuffer,
    VkCommandBufferAllocateInfo* commandBufferAllocateInfo) {
    return Renderer_AllocateCommandBuffers(device, commandBuffer, commandBufferAllocateInfo);
}

VkResult DLL_Renderer_CreateGraphicsPipelines(VkDevice device, VkPipeline* graphicPipeline,
    VkGraphicsPipelineCreateInfo* createGraphicPipelines,
    uint32_t createGraphicPipelinesCount) {
    return Renderer_CreateGraphicsPipelines(device, graphicPipeline, createGraphicPipelines, createGraphicPipelinesCount);
}

VkResult DLL_Renderer_CreateCommandPool(VkDevice device, VkCommandPool* commandPool,
    VkCommandPoolCreateInfo* commandPoolInfo) {
    return Renderer_CreateCommandPool(device, commandPool, commandPoolInfo);
}

void DLL_Renderer_UpdateDescriptorSet(VkDevice device, VkWriteDescriptorSet* writeDescriptorSet,
    uint32_t count) {
    Renderer_UpdateDescriptorSet(device, writeDescriptorSet, count);
}

//VkResult DLL_Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList,
//    VkSemaphore* acquireImageSemaphoreList, uint32_t* pImageIndex,
//    uint32_t* pCommandIndex, bool* pRebuildRendererFlag) {
//    return Renderer_StartFrame(device, swapChain, fenceList, acquireImageSemaphoreList, pImageIndex, pCommandIndex, pRebuildRendererFlag);
//}

VkResult DLL_Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList,
    VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue,
    VkQueue presentQueue, uint32_t commandIndex, uint32_t imageIndex,
    VkCommandBuffer* pCommandBufferSubmitList, uint32_t commandBufferCount,
    bool* rebuildRendererFlag) {
    return Renderer_EndFrame(swapChain, acquireImageSemaphoreList, presentImageSemaphoreList, fenceList, graphicsQueue, presentQueue, commandIndex, imageIndex, pCommandBufferSubmitList, commandBufferCount, rebuildRendererFlag);
}

uint32_t DLL_Renderer_GetMemoryType(VkPhysicalDevice physicalDevice, uint32_t typeFilter,
    VkMemoryPropertyFlags properties) {
    return Renderer_GetMemoryType(physicalDevice, typeFilter, properties);
}

VkResult DLL_Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo) {
    return Renderer_BeginCommandBuffer(pCommandBufferList, commandBufferBeginInfo);
}

VkResult DLL_Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList) {
    return Renderer_EndCommandBuffer(pCommandBufferList);
}

VkCommandBuffer DLL_Renderer_BeginSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool) {
    return Renderer_BeginSingleUseCommandBuffer(device, commandPool);
}

VkResult DLL_Renderer_EndSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer) {
    return Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
}

void DLL_Renderer_DestroyFences(VkDevice device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount) {
    Renderer_DestroyFences(device, acquireImageSemaphores, presentImageSemaphores, fences, semaphoreCount);
}

void DLL_Renderer_DestroyCommandPool(VkDevice device, VkCommandPool* commandPool) {
    Renderer_DestroyCommandPool(device, commandPool);
}

void DLL_Renderer_DestroyDevice(VkDevice device) {
    Renderer_DestroyDevice(device);
}

void DLL_Renderer_DestroySurface(VkInstance instance, VkSurfaceKHR* surface) {
    Renderer_DestroySurface(instance, surface);
}

void DLL_Renderer_DestroyDebugger(VkInstance* instance) {
    Renderer_DestroyDebugger(instance);
}

VkResult DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR** surfaceFormats, uint32_t* surfaceFormatCount)
{
    return  Renderer_GetSurfaceFormats(physicalDevice, surface, surfaceFormats, surfaceFormatCount);
}

void DLL_Renderer_DestroyInstance(VkInstance* instance) {
    Renderer_DestroyInstance(instance);
}

void DLL_Renderer_DestroyRenderPass(VkDevice device, VkRenderPass* renderPass) {
    Renderer_DestroyRenderPass(device, renderPass);
}

void DLL_Renderer_DestroyFrameBuffers(VkDevice device, VkFramebuffer* frameBufferList, uint32 count) {
    Renderer_DestroyFrameBuffers(device, frameBufferList, count);
}

void DLL_Renderer_DestroyDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool) {
    Renderer_DestroyDescriptorPool(device, descriptorPool);
}

void DLL_Renderer_DestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout) {
    Renderer_DestroyDescriptorSetLayout(device, descriptorSetLayout);
}

void DLL_Renderer_DestroyCommandBuffers(VkDevice device, VkCommandPool* commandPool, VkCommandBuffer* commandBufferList, uint32 count) {
    Renderer_DestroyCommandBuffers(device, commandPool, commandBufferList, count);
}

void DLL_Renderer_DestroyBuffer(VkDevice device, VkBuffer* buffer) {
    Renderer_DestroyBuffer(device, buffer);
}

void DLL_Renderer_FreeDeviceMemory(VkDevice device, VkDeviceMemory* memory) {
    Renderer_FreeDeviceMemory(device, memory);
}

void DLL_Renderer_DestroySwapChainImageView(VkDevice device, VkImageView* pSwapChainImageViewList, uint32 count) {
    Renderer_DestroySwapChainImageView(device, pSwapChainImageViewList, count);
}

void DLL_Renderer_DestroySwapChain(VkDevice device, VkSwapchainKHR* swapChain) {
    Renderer_DestroySwapChain(device, swapChain);
}

void DLL_Renderer_DestroyImageView(VkDevice device, VkImageView* imageView) {
    Renderer_DestroyImageView(device, imageView);
}

void DLL_Renderer_DestroyImage(VkDevice device, VkImage* image) {
    Renderer_DestroyImage(device, image);
}

void DLL_Renderer_DestroySampler(VkDevice device, VkSampler* sampler) {
    Renderer_DestroySampler(device, sampler);
}

void DLL_Renderer_DestroyPipeline(VkDevice device, VkPipeline* pipeline) {
    Renderer_DestroyPipeline(device, pipeline);
}

void DLL_Renderer_DestroyPipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout) {
    Renderer_DestroyPipelineLayout(device, pipelineLayout);
}

void DLL_Renderer_DestroyPipelineCache(VkDevice device, VkPipelineCache* pipelineCache) {
    Renderer_DestroyPipelineCache(device, pipelineCache);
}

int DLL_Renderer_SimpleTestLIB() {
    return Renderer_SimpleTestLIB();
}
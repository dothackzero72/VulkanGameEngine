#include "VulkanImp.h"

void GameEngineAPI_GetWin32Extensions(uint32_t* extensionCount, const char*** enabledExtensions) 
{
    Renderer_GetWin32Extensions(extensionCount, enabledExtensions);
}

 VkInstance GameEngineAPI_CreateVulkanInstance(RichTextBoxCallback callback)
 {
     VkInstance instance = Renderer_CreateVulkanInstance();
     return instance;
}

 VkDebugUtilsMessengerEXT GameEngineAPI_SetupDebugMessenger(RichTextBoxCallback callback, VkInstance instance)
 {
     return Renderer_SetupDebugMessenger(instance);
 }

 void GameEngineAPI_DestroyFences(VkDevice* device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount, int bufferCount)
 {
     Renderer_CSDestroyFences(device, acquireImageSemaphores, presentImageSemaphores, fences, semaphoreCount, bufferCount);
 }

 void GameEngineAPI_DestroyDevice(VkDevice* device)
 {
     Renderer_CSDestroyDevice(device);
 }

 void GameEngineAPI_DestroySurface(VkInstance* instance, VkSurfaceKHR* surface)
 {
     Renderer_CSDestroySurface(instance, surface);
 }

 void GameEngineAPI_DestroyDebugger(VkInstance* instance)
 {
     Renderer_CSDestroyDebugger(instance);
 }

 void GameEngineAPI_DestroyInstance(VkInstance* instance)
 {
     Renderer_CSDestroyInstance(instance);
 }

 void GameEngineAPI_DestroyRenderPass(VkRenderPass* renderPass)
 {
     Renderer_DestroyRenderPass(renderPass);
 }

 void GameEngineAPI_DestroyFrameBuffers(VkFramebuffer* frameBufferList)
 {
     Renderer_DestroyFrameBuffers(frameBufferList);
 }

 void GameEngineAPI_DestroyDescriptorPool(VkDescriptorPool* descriptorPool)
 {
     Renderer_DestroyDescriptorPool(descriptorPool);
 }

 void GameEngineAPI_DestroyDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout)
 {
     Renderer_DestroyDescriptorSetLayout(descriptorSetLayout);
 }

 void GameEngineAPI_DestroyCommandBuffers(VkCommandPool* commandPool, VkCommandBuffer* commandBufferList)
 {
     Renderer_DestroyCommandBuffers(commandPool, commandBufferList);
 }

 void GameEngineAPI_DestroyCommandPool(VkCommandPool* commandPool)
 {
     Renderer_DestroyCommnadPool(commandPool);
 }

 void GameEngineAPI_DestroyBuffer(VkBuffer* buffer)
 {
     Renderer_DestroyBuffer(buffer);
 }

 void GameEngineAPI_FreeMemory(VkDeviceMemory* memory)
 {
     Renderer_FreeMemory(memory);
 }

 void GameEngineAPI_DestroyImageView(VkImageView* imageView)
 {
     Renderer_DestroyImageView(imageView);
 }

 void GameEngineAPI_DestroyImage(VkImage* image)
 {
     Renderer_DestroyImage(image);
 }

 void GameEngineAPI_DestroySampler(VkSampler* sampler)
 {
     Renderer_DestroySampler(sampler);
 }

 void GameEngineAPI_DestroyPipeline(VkPipeline* pipeline)
 {
     Renderer_DestroyPipeline(pipeline);
 }

 void GameEngineAPI_DestroyPipelineLayout(VkPipelineLayout* pipelineLayout)
 {
     Renderer_DestroyPipelineLayout(pipelineLayout);
 }

 void GameEngineAPI_DestroyPipelineCache(VkPipelineCache* pipelineCache)
 {
     Renderer_DestroyPipelineCache(pipelineCache);
 }

 int SimpleTestLIB()
 {
     return Renderer_SimpleTestLIB();
 }

 int SimpleTestDLL()
{
    return 42;
}
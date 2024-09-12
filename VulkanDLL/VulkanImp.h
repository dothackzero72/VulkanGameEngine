#include <windows.h>
#include <vulkan/vulkan.h>
#include <VulkanRenderer.h>

#ifdef VulkanEngine_DLL
#define DLL_EXPORT extern "C" __declspec(dllexport)
#else
#define DLL_EXPORT __declspec(dllimport)
#endif

typedef void (*TextCallback)(const char*);
typedef void (*RichTextBoxCallback)(const char*);

DLL_EXPORT void GameEngineAPI_GetWin32Extensions(uint32_t* extensionCount, const char*** enabledExtensions);
DLL_EXPORT VkInstance GameEngineAPI_CreateVulkanInstance(RichTextBoxCallback callback);
DLL_EXPORT VkDebugUtilsMessengerEXT GameEngineAPI_SetupDebugMessenger(RichTextBoxCallback callback, VkInstance instance);

DLL_EXPORT void GameEngineAPI_DestroyPipelineCache(VkPipelineCache* pipelineCache);
DLL_EXPORT void GameEngineAPI_DestroyFences(VkDevice* device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount, int bufferCount);
DLL_EXPORT void GameEngineAPI_DestroyCommandPool(VkDevice* device, VkCommandPool* commandPool);
DLL_EXPORT void GameEngineAPI_DestroyDevice(VkDevice* device);
DLL_EXPORT void GameEngineAPI_DestroySurface(VkInstance* instance, VkSurfaceKHR* surface);
DLL_EXPORT void GameEngineAPI_DestroyDebugger(VkInstance* instance);
DLL_EXPORT void GameEngineAPI_DestroyInstance(VkInstance* instance);
DLL_EXPORT void GameEngineAPI_DestroyRenderPass(VkRenderPass* renderPass);
DLL_EXPORT void GameEngineAPI_DestroyFrameBuffers(VkFramebuffer* frameBufferList);
DLL_EXPORT void GameEngineAPI_DestroyDescriptorPool(VkDescriptorPool* descriptorPool);
DLL_EXPORT void GameEngineAPI_DestroyDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout);
DLL_EXPORT void GameEngineAPI_DestroyCommandBuffers(VkCommandPool* commandPool, VkCommandBuffer* commandBufferList);
DLL_EXPORT void GameEngineAPI_DestroyCommandPool(VkCommandPool* commandPool);
DLL_EXPORT void GameEngineAPI_DestroyBuffer(VkBuffer* buffer);
DLL_EXPORT void GameEngineAPI_FreeMemory(VkDeviceMemory* memory);
DLL_EXPORT void GameEngineAPI_DestroyImageView(VkImageView* imageView);
DLL_EXPORT void GameEngineAPI_DestroyImage(VkImage* image);
DLL_EXPORT void GameEngineAPI_DestroySampler(VkSampler* sampler);
DLL_EXPORT void GameEngineAPI_DestroyPipeline(VkPipeline* pipeline);
DLL_EXPORT void GameEngineAPI_DestroyPipelineLayout(VkPipelineLayout* pipelineLayout);
DLL_EXPORT void GameEngineAPI_DestroyPipelineCache(VkPipelineCache* pipelineCache);

DLL_EXPORT int SimpleTestLIB();
DLL_EXPORT int SimpleTestDLL();

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    switch (ul_reason_for_call) {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}
//#include <windows.h>
//#include "VulkanDLL.h"
//
//// Wrapper implementations
//VkInstance VulkanAPI_vkCreateInstance(const VkInstanceCreateInfo* pCreateInfo) {
//    VkInstance instance = VK_NULL_HANDLE;
//    if (vkCreateInstance) {
//        vkCreateInstance(pCreateInfo, NULL, &instance);
//    }
//    return instance;
//}
//
//void VulkanAPI_vkDestroyInstance(VkInstance instance, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyInstance) {
//        vkDestroyInstance(instance, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkEnumeratePhysicalDevices(VkInstance instance, uint32_t* pPhysicalDeviceCount, VkPhysicalDevice* pPhysicalDevices) {
//    if (vkEnumeratePhysicalDevices) {
//        return vkEnumeratePhysicalDevices(instance, pPhysicalDeviceCount, pPhysicalDevices);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkGetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice, VkPhysicalDeviceFeatures* pFeatures) {
//    if (vkGetPhysicalDeviceFeatures) {
//        vkGetPhysicalDeviceFeatures(physicalDevice, pFeatures);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkFormatProperties* pFormatProperties) {
//    if (vkGetPhysicalDeviceFormatProperties) {
//        vkGetPhysicalDeviceFormatProperties(physicalDevice, format, pFormatProperties);
//    }
//}
//
//VkResult VulkanAPI_vkGetPhysicalDeviceImageFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkImageTiling tiling, VkImageUsageFlags usage, VkImageCreateFlags flags, VkImageFormatProperties* pImageFormatProperties) {
//    if (vkGetPhysicalDeviceImageFormatProperties) {
//        return vkGetPhysicalDeviceImageFormatProperties(physicalDevice, format, type, tiling, usage, flags, pImageFormatProperties);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkGetPhysicalDeviceProperties(VkPhysicalDevice physicalDevice, VkPhysicalDeviceProperties* pProperties) {
//    if (vkGetPhysicalDeviceProperties) {
//        vkGetPhysicalDeviceProperties(physicalDevice, pProperties);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceQueueFamilyProperties(VkPhysicalDevice physicalDevice, uint32_t* pQueueFamilyPropertyCount, VkQueueFamilyProperties* pQueueFamilyProperties) {
//    if (vkGetPhysicalDeviceQueueFamilyProperties) {
//        vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, pQueueFamilyPropertyCount, pQueueFamilyProperties);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceMemoryProperties(VkPhysicalDevice physicalDevice, VkPhysicalDeviceMemoryProperties* pMemoryProperties) {
//    if (vkGetPhysicalDeviceMemoryProperties) {
//        vkGetPhysicalDeviceMemoryProperties(physicalDevice, pMemoryProperties);
//    }
//}
//
//PFN_vkVoidFunction VulkanAPI_vkGetInstanceProcAddr(VkInstance instance, const char* pName) {
//    if (vkGetInstanceProcAddr) {
//        return vkGetInstanceProcAddr(instance, pName);
//    }
//    return NULL; // Or other appropriate handling
//}
//
//PFN_vkVoidFunction VulkanAPI_vkGetDeviceProcAddr(VkDevice device, const char* pName) {
//    if (vkGetDeviceProcAddr) {
//        return vkGetDeviceProcAddr(device, pName);
//    }
//    return NULL; // Or other appropriate handling
//}
//
//VkResult VulkanAPI_vkCreateDevice(VkPhysicalDevice physicalDevice, const VkDeviceCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDevice* pDevice) {
//    if (vkCreateDevice) {
//        return vkCreateDevice(physicalDevice, pCreateInfo, pAllocator, pDevice);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyDevice(VkDevice device, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyDevice) {
//        vkDestroyDevice(device, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkEnumerateInstanceExtensionProperties(const char* pLayerName, uint32_t* pPropertyCount, VkExtensionProperties* pProperties) {
//    if (vkEnumerateInstanceExtensionProperties) {
//        return vkEnumerateInstanceExtensionProperties(pLayerName, pPropertyCount, pProperties);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkEnumerateDeviceExtensionProperties(VkPhysicalDevice physicalDevice, const char* pLayerName, uint32_t* pPropertyCount, VkExtensionProperties* pProperties) {
//    if (vkEnumerateDeviceExtensionProperties) {
//        return vkEnumerateDeviceExtensionProperties(physicalDevice, pLayerName, pPropertyCount, pProperties);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkEnumerateInstanceLayerProperties(uint32_t* pPropertyCount, VkLayerProperties* pProperties) {
//    if (vkEnumerateInstanceLayerProperties) {
//        return vkEnumerateInstanceLayerProperties(pPropertyCount, pProperties);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkEnumerateDeviceLayerProperties(VkPhysicalDevice physicalDevice, uint32_t* pPropertyCount, VkLayerProperties* pProperties) {
//    if (vkEnumerateDeviceLayerProperties) {
//        return vkEnumerateDeviceLayerProperties(physicalDevice, pPropertyCount, pProperties);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkGetDeviceQueue(VkDevice device, uint32_t queueFamilyIndex, uint32_t queueIndex, VkQueue* pQueue) {
//    if (vkGetDeviceQueue) {
//        vkGetDeviceQueue(device, queueFamilyIndex, queueIndex, pQueue);
//    }
//}
//
//VkResult VulkanAPI_vkQueueSubmit(VkQueue queue, uint32_t submitCount, const VkSubmitInfo* pSubmits, VkFence fence) {
//    if (vkQueueSubmit) {
//        return vkQueueSubmit(queue, submitCount, pSubmits, fence);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkQueueWaitIdle(VkQueue queue) {
//    if (vkQueueWaitIdle) {
//        return vkQueueWaitIdle(queue);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkDeviceWaitIdle(VkDevice device) {
//    if (vkDeviceWaitIdle) {
//        return vkDeviceWaitIdle(device);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkAllocateMemory(VkDevice device, const VkMemoryAllocateInfo* pAllocateInfo, const VkAllocationCallbacks* pAllocator, VkDeviceMemory* pMemory) {
//    if (vkAllocateMemory) {
//        return vkAllocateMemory(device, pAllocateInfo, pAllocator, pMemory);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkFreeMemory(VkDevice device, VkDeviceMemory memory, const VkAllocationCallbacks* pAllocator) {
//    if (vkFreeMemory) {
//        vkFreeMemory(device, memory, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkMapMemory(VkDevice device, VkDeviceMemory memory, VkDeviceSize offset, VkDeviceSize size, VkMemoryMapFlags flags, void** ppData) {
//    if (vkMapMemory) {
//        return vkMapMemory(device, memory, offset, size, flags, ppData);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkUnmapMemory(VkDevice device, VkDeviceMemory memory) {
//    if (vkUnmapMemory) {
//        vkUnmapMemory(device, memory);
//    }
//}
//
//VkResult VulkanAPI_vkFlushMappedMemoryRanges(VkDevice device, uint32_t memoryRangeCount, const VkMappedMemoryRange* pMemoryRanges) {
//    if (vkFlushMappedMemoryRanges) {
//        return vkFlushMappedMemoryRanges(device, memoryRangeCount, pMemoryRanges);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkInvalidateMappedMemoryRanges(VkDevice device, uint32_t memoryRangeCount, const VkMappedMemoryRange* pMemoryRanges) {
//    if (vkInvalidateMappedMemoryRanges) {
//        return vkInvalidateMappedMemoryRanges(device, memoryRangeCount, pMemoryRanges);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkGetDeviceMemoryCommitment(VkDevice device, VkDeviceMemory memory, VkDeviceSize* pCommittedMemoryInBytes) {
//    if (vkGetDeviceMemoryCommitment) {
//        vkGetDeviceMemoryCommitment(device, memory, pCommittedMemoryInBytes);
//    }
//}
//
//VkResult VulkanAPI_vkBindBufferMemory(VkDevice device, VkBuffer buffer, VkDeviceMemory memory, VkDeviceSize memoryOffset) {
//    if (vkBindBufferMemory) {
//        return vkBindBufferMemory(device, buffer, memory, memoryOffset);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkBindImageMemory(VkDevice device, VkImage image, VkDeviceMemory memory, VkDeviceSize memoryOffset) {
//    if (vkBindImageMemory) {
//        return vkBindImageMemory(device, image, memory, memoryOffset);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkGetBufferMemoryRequirements(VkDevice device, VkBuffer buffer, VkMemoryRequirements* pMemoryRequirements) {
//    if (vkGetBufferMemoryRequirements) {
//        vkGetBufferMemoryRequirements(device, buffer, pMemoryRequirements);
//    }
//}
//
//void VulkanAPI_vkGetImageMemoryRequirements(VkDevice device, VkImage image, VkMemoryRequirements* pMemoryRequirements) {
//    if (vkGetImageMemoryRequirements) {
//        vkGetImageMemoryRequirements(device, image, pMemoryRequirements);
//    }
//}
//
//void VulkanAPI_vkGetImageSparseMemoryRequirements(VkDevice device, VkImage image, uint32_t* pSparseMemoryRequirementCount, VkSparseImageMemoryRequirements* pSparseMemoryRequirements) {
//    if (vkGetImageSparseMemoryRequirements) {
//        vkGetImageSparseMemoryRequirements(device, image, pSparseMemoryRequirementCount, pSparseMemoryRequirements);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceSparseImageFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkSampleCountFlagBits samples, VkImageUsageFlags usage, VkImageTiling tiling, uint32_t* pPropertyCount, VkSparseImageFormatProperties* pProperties) {
//    if (vkGetPhysicalDeviceSparseImageFormatProperties) {
//        vkGetPhysicalDeviceSparseImageFormatProperties(physicalDevice, format, type, samples, usage, tiling, pPropertyCount, pProperties);
//    }
//}
//
//VkResult VulkanAPI_vkQueueBindSparse(VkQueue queue, uint32_t bindInfoCount, const VkBindSparseInfo* pBindInfo, VkFence fence) {
//    if (vkQueueBindSparse) {
//        return vkQueueBindSparse(queue, bindInfoCount, pBindInfo, fence);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkCreateFence(VkDevice device, const VkFenceCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkFence* pFence) {
//    if (vkCreateFence) {
//        return vkCreateFence(device, pCreateInfo, pAllocator, pFence);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyFence(VkDevice device, VkFence fence, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyFence) {
//        vkDestroyFence(device, fence, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkResetFences(VkDevice device, uint32_t fenceCount, const VkFence* pFences) {
//    if (vkResetFences) {
//        return vkResetFences(device, fenceCount, pFences);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkGetFenceStatus(VkDevice device, VkFence fence) {
//    if (vkGetFenceStatus) {
//        return vkGetFenceStatus(device, fence);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkWaitForFences(VkDevice device, uint32_t fenceCount, const VkFence* pFences, VkBool32 waitAll, uint64_t timeout) {
//    if (vkWaitForFences) {
//        return vkWaitForFences(device, fenceCount, pFences, waitAll, timeout);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkCreateSemaphore(VkDevice device, const VkSemaphoreCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkSemaphore* pSemaphore) {
//    if (vkCreateSemaphore) {
//        return vkCreateSemaphore(device, pCreateInfo, pAllocator, pSemaphore);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroySemaphore(VkDevice device, VkSemaphore semaphore, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroySemaphore) {
//        vkDestroySemaphore(device, semaphore, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreateEvent(VkDevice device, const VkEventCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkEvent* pEvent) {
//    if (vkCreateEvent) {
//        return vkCreateEvent(device, pCreateInfo, pAllocator, pEvent);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyEvent(VkDevice device, VkEvent event, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyEvent) {
//        vkDestroyEvent(device, event, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkGetEventStatus(VkDevice device, VkEvent event) {
//    if (vkGetEventStatus) {
//        return vkGetEventStatus(device, event);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkSetEvent(VkDevice device, VkEvent event) {
//    if (vkSetEvent) {
//        return vkSetEvent(device, event);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkResetEvent(VkDevice device, VkEvent event) {
//    if (vkResetEvent) {
//        return vkResetEvent(device, event);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkCreateQueryPool(VkDevice device, const VkQueryPoolCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkQueryPool* pQueryPool) {
//    if (vkCreateQueryPool) {
//        return vkCreateQueryPool(device, pCreateInfo, pAllocator, pQueryPool);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyQueryPool(VkDevice device, VkQueryPool queryPool, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyQueryPool) {
//        vkDestroyQueryPool(device, queryPool, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkGetQueryPoolResults(VkDevice device, VkQueryPool queryPool, uint32_t firstQuery, uint32_t queryCount, size_t dataSize, void* pData, VkDeviceSize stride, VkQueryResultFlags flags) {
//    if (vkGetQueryPoolResults) {
//        return vkGetQueryPoolResults(device, queryPool, firstQuery, queryCount, dataSize, pData, stride, flags);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkCreateBuffer(VkDevice device, const VkBufferCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkBuffer* pBuffer) {
//    if (vkCreateBuffer) {
//        return vkCreateBuffer(device, pCreateInfo, pAllocator, pBuffer);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyBuffer(VkDevice device, VkBuffer buffer, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyBuffer) {
//        vkDestroyBuffer(device, buffer, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreateBufferView(VkDevice device, const VkBufferViewCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkBufferView* pView) {
//    if (vkCreateBufferView) {
//        return vkCreateBufferView(device, pCreateInfo, pAllocator, pView);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyBufferView(VkDevice device, VkBufferView bufferView, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyBufferView) {
//        vkDestroyBufferView(device, bufferView, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreateImage(VkDevice device, const VkImageCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkImage* pImage) {
//    if (vkCreateImage) {
//        return vkCreateImage(device, pCreateInfo, pAllocator, pImage);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyImage(VkDevice device, VkImage image, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyImage) {
//        vkDestroyImage(device, image, pAllocator);
//    }
//}
//
//void VulkanAPI_vkGetImageSubresourceLayout(VkDevice device, VkImage image, const VkImageSubresource* pSubresource, VkSubresourceLayout* pLayout) {
//    if (vkGetImageSubresourceLayout) {
//        vkGetImageSubresourceLayout(device, image, pSubresource, pLayout);
//    }
//}
//
//VkResult VulkanAPI_vkCreateImageView(VkDevice device, const VkImageViewCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkImageView* pView) {
//    if (vkCreateImageView) {
//        return vkCreateImageView(device, pCreateInfo, pAllocator, pView);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyImageView(VkDevice device, VkImageView imageView, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyImageView) {
//        vkDestroyImageView(device, imageView, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreateShaderModule(VkDevice device, const VkShaderModuleCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkShaderModule* pShaderModule) {
//    if (vkCreateShaderModule) {
//        return vkCreateShaderModule(device, pCreateInfo, pAllocator, pShaderModule);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyShaderModule(VkDevice device, VkShaderModule shaderModule, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyShaderModule) {
//        vkDestroyShaderModule(device, shaderModule, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreatePipelineCache(VkDevice device, const VkPipelineCacheCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkPipelineCache* pPipelineCache) {
//    if (vkCreatePipelineCache) {
//        return vkCreatePipelineCache(device, pCreateInfo, pAllocator, pPipelineCache);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyPipelineCache(VkDevice device, VkPipelineCache pipelineCache, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyPipelineCache) {
//        vkDestroyPipelineCache(device, pipelineCache, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkGetPipelineCacheData(VkDevice device, VkPipelineCache pipelineCache, size_t* pDataSize, void* pData) {
//    if (vkGetPipelineCacheData) {
//        return vkGetPipelineCacheData(device, pipelineCache, pDataSize, pData);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkMergePipelineCaches(VkDevice device, VkPipelineCache dstCache, uint32_t srcCacheCount, const VkPipelineCache* pSrcCaches) {
//    if (vkMergePipelineCaches) {
//        return vkMergePipelineCaches(device, dstCache, srcCacheCount, pSrcCaches);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, uint32_t createInfoCount, const VkGraphicsPipelineCreateInfo* pCreateInfos, const VkAllocationCallbacks* pAllocator, VkPipeline* pPipelines) {
//    if (vkCreateGraphicsPipelines) {
//        return vkCreateGraphicsPipelines(device, pipelineCache, createInfoCount, pCreateInfos, pAllocator, pPipelines);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkCreateComputePipelines(VkDevice device, VkPipelineCache pipelineCache, uint32_t createInfoCount, const VkComputePipelineCreateInfo* pCreateInfos, const VkAllocationCallbacks* pAllocator, VkPipeline* pPipelines) {
//    if (vkCreateComputePipelines) {
//        return vkCreateComputePipelines(device, pipelineCache, createInfoCount, pCreateInfos, pAllocator, pPipelines);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyPipeline(VkDevice device, VkPipeline pipeline, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyPipeline) {
//        vkDestroyPipeline(device, pipeline, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreatePipelineLayout(VkDevice device, const VkPipelineLayoutCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkPipelineLayout* pPipelineLayout) {
//    if (vkCreatePipelineLayout) {
//        return vkCreatePipelineLayout(device, pCreateInfo, pAllocator, pPipelineLayout);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyPipelineLayout(VkDevice device, VkPipelineLayout pipelineLayout, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyPipelineLayout) {
//        vkDestroyPipelineLayout(device, pipelineLayout, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreateSampler(VkDevice device, const VkSamplerCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkSampler* pSampler) {
//    if (vkCreateSampler) {
//        return vkCreateSampler(device, pCreateInfo, pAllocator, pSampler);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroySampler(VkDevice device, VkSampler sampler, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroySampler) {
//        vkDestroySampler(device, sampler, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreateDescriptorSetLayout(VkDevice device, const VkDescriptorSetLayoutCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDescriptorSetLayout* pSetLayout) {
//    if (vkCreateDescriptorSetLayout) {
//        return vkCreateDescriptorSetLayout(device, pCreateInfo, pAllocator, pSetLayout);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout descriptorSetLayout, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyDescriptorSetLayout) {
//        vkDestroyDescriptorSetLayout(device, descriptorSetLayout, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreateDescriptorPool(VkDevice device, const VkDescriptorPoolCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDescriptorPool* pDescriptorPool) {
//    if (vkCreateDescriptorPool) {
//        return vkCreateDescriptorPool(device, pCreateInfo, pAllocator, pDescriptorPool);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyDescriptorPool) {
//        vkDestroyDescriptorPool(device, descriptorPool, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkResetDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, VkDescriptorPoolResetFlags flags) {
//    if (vkResetDescriptorPool) {
//        return vkResetDescriptorPool(device, descriptorPool, flags);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkAllocateDescriptorSets(VkDevice device, const VkDescriptorSetAllocateInfo* pAllocateInfo, VkDescriptorSet* pDescriptorSets) {
//    if (vkAllocateDescriptorSets) {
//        return vkAllocateDescriptorSets(device, pAllocateInfo, pDescriptorSets);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkFreeDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, uint32_t descriptorSetCount, const VkDescriptorSet* pDescriptorSets) {
//    if (vkFreeDescriptorSets) {
//        return vkFreeDescriptorSets(device, descriptorPool, descriptorSetCount, pDescriptorSets);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkUpdateDescriptorSets(VkDevice device, uint32_t descriptorWriteCount, const VkWriteDescriptorSet* pDescriptorWrites, uint32_t descriptorCopyCount, const VkCopyDescriptorSet* pDescriptorCopies) {
//    if (vkUpdateDescriptorSets) {
//        vkUpdateDescriptorSets(device, descriptorWriteCount, pDescriptorWrites, descriptorCopyCount, pDescriptorCopies);
//    }
//}
//
//VkResult VulkanAPI_vkCreateFramebuffer(VkDevice device, const VkFramebufferCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkFramebuffer* pFramebuffer) {
//    if (vkCreateFramebuffer) {
//        return vkCreateFramebuffer(device, pCreateInfo, pAllocator, pFramebuffer);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyFramebuffer(VkDevice device, VkFramebuffer framebuffer, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyFramebuffer) {
//        vkDestroyFramebuffer(device, framebuffer, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreateRenderPass(VkDevice device, const VkRenderPassCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkRenderPass* pRenderPass) {
//    if (vkCreateRenderPass) {
//        return vkCreateRenderPass(device, pCreateInfo, pAllocator, pRenderPass);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyRenderPass(VkDevice device, VkRenderPass renderPass, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyRenderPass) {
//        vkDestroyRenderPass(device, renderPass, pAllocator);
//    }
//}
//
//void VulkanAPI_vkGetRenderAreaGranularity(VkDevice device, VkRenderPass renderPass, VkExtent2D* pGranularity) {
//    if (vkGetRenderAreaGranularity) {
//        vkGetRenderAreaGranularity(device, renderPass, pGranularity);
//    }
//}
//
//VkResult VulkanAPI_vkCreateCommandPool(VkDevice device, const VkCommandPoolCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkCommandPool* pCommandPool) {
//    if (vkCreateCommandPool) {
//        return vkCreateCommandPool(device, pCreateInfo, pAllocator, pCommandPool);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyCommandPool(VkDevice device, VkCommandPool commandPool, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyCommandPool) {
//        vkDestroyCommandPool(device, commandPool, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkResetCommandPool(VkDevice device, VkCommandPool commandPool, VkCommandPoolResetFlags flags) {
//    if (vkResetCommandPool) {
//        return vkResetCommandPool(device, commandPool, flags);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkAllocateCommandBuffers(VkDevice device, const VkCommandBufferAllocateInfo* pAllocateInfo, VkCommandBuffer* pCommandBuffers) {
//    if (vkAllocateCommandBuffers) {
//        return vkAllocateCommandBuffers(device, pAllocateInfo, pCommandBuffers);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkFreeCommandBuffers(VkDevice device, VkCommandPool commandPool, uint32_t commandBufferCount, const VkCommandBuffer* pCommandBuffers) {
//    if (vkFreeCommandBuffers) {
//        vkFreeCommandBuffers(device, commandPool, commandBufferCount, pCommandBuffers);
//    }
//}
//
//VkResult VulkanAPI_vkBeginCommandBuffer(VkCommandBuffer commandBuffer, const VkCommandBufferBeginInfo* pBeginInfo) {
//    if (vkBeginCommandBuffer) {
//        return vkBeginCommandBuffer(commandBuffer, pBeginInfo);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkEndCommandBuffer(VkCommandBuffer commandBuffer) {
//    if (vkEndCommandBuffer) {
//        return vkEndCommandBuffer(commandBuffer);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkResetCommandBuffer(VkCommandBuffer commandBuffer, VkCommandBufferResetFlags flags) {
//    if (vkResetCommandBuffer) {
//        return vkResetCommandBuffer(commandBuffer, flags);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED; // Or other appropriate error
//}
//
//void VulkanAPI_vkCmdBindPipeline(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipeline pipeline) {
//    if (vkCmdBindPipeline) {
//        vkCmdBindPipeline(commandBuffer, pipelineBindPoint, pipeline);
//    }
//}
//
//// Additional wrappers for commands
//void VulkanAPI_vkCmdSetViewport(VkCommandBuffer commandBuffer, uint32_t firstViewport, uint32_t viewportCount, const VkViewport* pViewports) {
//    if (vkCmdSetViewport) {
//        vkCmdSetViewport(commandBuffer, firstViewport, viewportCount, pViewports);
//    }
//}
//
//void VulkanAPI_vkCmdSetScissor(VkCommandBuffer commandBuffer, uint32_t firstScissor, uint32_t scissorCount, const VkRect2D* pScissors) {
//    if (vkCmdSetScissor) {
//        vkCmdSetScissor(commandBuffer, firstScissor, scissorCount, pScissors);
//    }
//}
//
//void VulkanAPI_vkCmdSetLineWidth(VkCommandBuffer commandBuffer, float lineWidth) {
//    if (vkCmdSetLineWidth) {
//        vkCmdSetLineWidth(commandBuffer, lineWidth);
//    }
//}
//
//void VulkanAPI_vkCmdSetDepthBias(VkCommandBuffer commandBuffer, float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor) {
//    if (vkCmdSetDepthBias) {
//        vkCmdSetDepthBias(commandBuffer, depthBiasConstantFactor, depthBiasClamp, depthBiasSlopeFactor);
//    }
//}
//
//void VulkanAPI_vkCmdSetBlendConstants(VkCommandBuffer commandBuffer, const float blendConstants[4]) {
//    if (vkCmdSetBlendConstants) {
//        vkCmdSetBlendConstants(commandBuffer, blendConstants);
//    }
//}
//
//void VulkanAPI_vkCmdSetDepthBounds(VkCommandBuffer commandBuffer, float minDepthBounds, float maxDepthBounds) {
//    if (vkCmdSetDepthBounds) {
//        vkCmdSetDepthBounds(commandBuffer, minDepthBounds, maxDepthBounds);
//    }
//}
//
//void VulkanAPI_vkCmdSetStencilCompareMask(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, uint32_t compareMask) {
//    if (vkCmdSetStencilCompareMask) {
//        vkCmdSetStencilCompareMask(commandBuffer, faceMask, compareMask);
//    }
//}
//
//void VulkanAPI_vkCmdSetStencilWriteMask(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, uint32_t writeMask) {
//    if (vkCmdSetStencilWriteMask) {
//        vkCmdSetStencilWriteMask(commandBuffer, faceMask, writeMask);
//    }
//}
//
//void VulkanAPI_vkCmdSetStencilReference(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, uint32_t reference) {
//    if (vkCmdSetStencilReference) {
//        vkCmdSetStencilReference(commandBuffer, faceMask, reference);
//    }
//}
//
//void VulkanAPI_vkCmdBindDescriptorSets(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint32_t firstSet, uint32_t descriptorSetCount, const VkDescriptorSet* pDescriptorSets, uint32_t dynamicOffsetCount, const uint32_t* pDynamicOffsets) {
//    if (vkCmdBindDescriptorSets) {
//        vkCmdBindDescriptorSets(commandBuffer, pipelineBindPoint, layout, firstSet, descriptorSetCount, pDescriptorSets, dynamicOffsetCount, pDynamicOffsets);
//    }
//}
//
//// Additional Command Functions
//void VulkanAPI_vkCmdBindIndexBuffer(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, VkIndexType indexType) {
//    if (vkCmdBindIndexBuffer) {
//        vkCmdBindIndexBuffer(commandBuffer, buffer, offset, indexType);
//    }
//}
//
//void VulkanAPI_vkCmdBindVertexBuffers(VkCommandBuffer commandBuffer, uint32_t firstBinding, uint32_t bindingCount, const VkBuffer* pBuffers, const VkDeviceSize* pOffsets) {
//    if (vkCmdBindVertexBuffers) {
//        vkCmdBindVertexBuffers(commandBuffer, firstBinding, bindingCount, pBuffers, pOffsets);
//    }
//}
//
//void VulkanAPI_vkCmdDraw(VkCommandBuffer commandBuffer, uint32_t vertexCount, uint32_t instanceCount, uint32_t firstVertex, uint32_t firstInstance) {
//    if (vkCmdDraw) {
//        vkCmdDraw(commandBuffer, vertexCount, instanceCount, firstVertex, firstInstance);
//    }
//}
//
//void VulkanAPI_vkCmdDrawIndexed(VkCommandBuffer commandBuffer, uint32_t indexCount, uint32_t instanceCount, uint32_t firstIndex, int32_t vertexOffset, uint32_t firstInstance) {
//    if (vkCmdDrawIndexed) {
//        vkCmdDrawIndexed(commandBuffer, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
//    }
//}
//
//void VulkanAPI_vkCmdDrawIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, uint32_t drawCount, uint32_t stride) {
//    if (vkCmdDrawIndirect) {
//        vkCmdDrawIndirect(commandBuffer, buffer, offset, drawCount, stride);
//    }
//}
//
//void VulkanAPI_vkCmdDrawIndexedIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, uint32_t drawCount, uint32_t stride) {
//    if (vkCmdDrawIndexedIndirect) {
//        vkCmdDrawIndexedIndirect(commandBuffer, buffer, offset, drawCount, stride);
//    }
//}
//
//void VulkanAPI_vkCmdDispatch(VkCommandBuffer commandBuffer, uint32_t groupCountX, uint32_t groupCountY, uint32_t groupCountZ) {
//    if (vkCmdDispatch) {
//        vkCmdDispatch(commandBuffer, groupCountX, groupCountY, groupCountZ);
//    }
//}
//
//void VulkanAPI_vkCmdDispatchIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset) {
//    if (vkCmdDispatchIndirect) {
//        vkCmdDispatchIndirect(commandBuffer, buffer, offset);
//    }
//}
//
//void VulkanAPI_vkCmdCopyBuffer(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkBuffer dstBuffer, uint32_t regionCount, const VkBufferCopy* pRegions) {
//    if (vkCmdCopyBuffer) {
//        vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, regionCount, pRegions);
//    }
//}
//
//void VulkanAPI_vkCmdCopyImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint32_t regionCount, const VkImageCopy* pRegions) {
//    if (vkCmdCopyImage) {
//        vkCmdCopyImage(commandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, regionCount, pRegions);
//    }
//}
//
//void VulkanAPI_vkCmdBlitImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint32_t regionCount, const VkImageBlit* pRegions, VkFilter filter) {
//    if (vkCmdBlitImage) {
//        vkCmdBlitImage(commandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, regionCount, pRegions, filter);
//    }
//}
//
//void VulkanAPI_vkCmdCopyBufferToImage(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkImage dstImage, VkImageLayout dstImageLayout, uint32_t regionCount, const VkBufferImageCopy* pRegions) {
//    if (vkCmdCopyBufferToImage) {
//        vkCmdCopyBufferToImage(commandBuffer, srcBuffer, dstImage, dstImageLayout, regionCount, pRegions);
//    }
//}
//
//void VulkanAPI_vkCmdCopyImageToBuffer(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkBuffer dstBuffer, uint32_t regionCount, const VkBufferImageCopy* pRegions) {
//    if (vkCmdCopyImageToBuffer) {
//        vkCmdCopyImageToBuffer(commandBuffer, srcImage, srcImageLayout, dstBuffer, regionCount, pRegions);
//    }
//}
//
//void VulkanAPI_vkCmdUpdateBuffer(VkCommandBuffer commandBuffer, VkBuffer dstBuffer, VkDeviceSize dstOffset, VkDeviceSize dataSize, const void* pData) {
//    if (vkCmdUpdateBuffer) {
//        vkCmdUpdateBuffer(commandBuffer, dstBuffer, dstOffset, dataSize, pData);
//    }
//}
//
//void VulkanAPI_vkCmdFillBuffer(VkCommandBuffer commandBuffer, VkBuffer dstBuffer, VkDeviceSize dstOffset, VkDeviceSize size, uint32_t data) {
//    if (vkCmdFillBuffer) {
//        vkCmdFillBuffer(commandBuffer, dstBuffer, dstOffset, size, data);
//    }
//}
//
//void VulkanAPI_vkCmdClearColorImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, const VkClearColorValue* pColor, uint32_t rangeCount, const VkImageSubresourceRange* pRanges) {
//    if (vkCmdClearColorImage) {
//        vkCmdClearColorImage(commandBuffer, image, imageLayout, pColor, rangeCount, pRanges);
//    }
//}
//
//void VulkanAPI_vkCmdClearDepthStencilImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, const VkClearDepthStencilValue* pDepthStencil, uint32_t rangeCount, const VkImageSubresourceRange* pRanges) {
//    if (vkCmdClearDepthStencilImage) {
//        vkCmdClearDepthStencilImage(commandBuffer, image, imageLayout, pDepthStencil, rangeCount, pRanges);
//    }
//}
//
//void VulkanAPI_vkCmdClearAttachments(VkCommandBuffer commandBuffer, uint32_t attachmentCount, const VkClearAttachment* pAttachments, uint32_t rectCount, const VkClearRect* pRects) {
//    if (vkCmdClearAttachments) {
//        vkCmdClearAttachments(commandBuffer, attachmentCount, pAttachments, rectCount, pRects);
//    }
//}
//
//void VulkanAPI_vkCmdResolveImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint32_t regionCount, const VkImageResolve* pRegions) {
//    if (vkCmdResolveImage) {
//        vkCmdResolveImage(commandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, regionCount, pRegions);
//    }
//}
//
//void VulkanAPI_vkCmdSetEvent(VkCommandBuffer commandBuffer, VkEvent event, VkPipelineStageFlags stageMask) {
//    if (vkCmdSetEvent) {
//        vkCmdSetEvent(commandBuffer, event, stageMask);
//    }
//}
//
//void VulkanAPI_vkCmdResetEvent(VkCommandBuffer commandBuffer, VkEvent event, VkPipelineStageFlags stageMask) {
//    if (vkCmdResetEvent) {
//        vkCmdResetEvent(commandBuffer, event, stageMask);
//    }
//}
//
//void VulkanAPI_vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint32_t eventCount, const VkEvent* pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint32_t memoryBarrierCount, const VkMemoryBarrier* pMemoryBarriers, uint32_t bufferMemoryBarrierCount, const VkBufferMemoryBarrier* pBufferMemoryBarriers, uint32_t imageMemoryBarrierCount, const VkImageMemoryBarrier* pImageMemoryBarriers) {
//    if (vkCmdWaitEvents) {
//        vkCmdWaitEvents(commandBuffer, eventCount, pEvents, srcStageMask, dstStageMask, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers, imageMemoryBarrierCount, pImageMemoryBarriers);
//    }
//}
//
//void VulkanAPI_vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint32_t memoryBarrierCount, const VkMemoryBarrier* pMemoryBarriers, uint32_t bufferMemoryBarrierCount, const VkBufferMemoryBarrier* pBufferMemoryBarriers, uint32_t imageMemoryBarrierCount, const VkImageMemoryBarrier* pImageMemoryBarriers) {
//    if (vkCmdPipelineBarrier) {
//        vkCmdPipelineBarrier(commandBuffer, srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers, imageMemoryBarrierCount, pImageMemoryBarriers);
//    }
//}
//
//void VulkanAPI_vkCmdBeginQuery(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint32_t query, VkQueryControlFlags flags) {
//    if (vkCmdBeginQuery) {
//        vkCmdBeginQuery(commandBuffer, queryPool, query, flags);
//    }
//}
//
//void VulkanAPI_vkCmdEndQuery(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint32_t query) {
//    if (vkCmdEndQuery) {
//        vkCmdEndQuery(commandBuffer, queryPool, query);
//    }
//}
//
//void VulkanAPI_vkCmdResetQueryPool(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint32_t firstQuery, uint32_t queryCount) {
//    if (vkCmdResetQueryPool) {
//        vkCmdResetQueryPool(commandBuffer, queryPool, firstQuery, queryCount);
//    }
//}
//
//void VulkanAPI_vkCmdWriteTimestamp(VkCommandBuffer commandBuffer, VkPipelineStageFlagBits pipelineStage, VkQueryPool queryPool, uint32_t query) {
//    if (vkCmdWriteTimestamp) {
//        vkCmdWriteTimestamp(commandBuffer, pipelineStage, queryPool, query);
//    }
//}
//
//void VulkanAPI_vkCmdCopyQueryPoolResults(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint32_t firstQuery, uint32_t queryCount, VkBuffer dstBuffer, VkDeviceSize dstOffset, VkDeviceSize stride, VkQueryResultFlags flags) {
//    if (vkCmdCopyQueryPoolResults) {
//        vkCmdCopyQueryPoolResults(commandBuffer, queryPool, firstQuery, queryCount, dstBuffer, dstOffset, stride, flags);
//    }
//}
//
//void VulkanAPI_vkCmdPushConstants(VkCommandBuffer commandBuffer, VkPipelineLayout layout, VkShaderStageFlags stageFlags, uint32_t offset, uint32_t size, const void* pValues) {
//    if (vkCmdPushConstants) {
//        vkCmdPushConstants(commandBuffer, layout, stageFlags, offset, size, pValues);
//    }
//}
//
//void VulkanAPI_vkCmdBeginRenderPass(VkCommandBuffer commandBuffer, const VkRenderPassBeginInfo* pRenderPassBegin, VkSubpassContents contents) {
//    if (vkCmdBeginRenderPass) {
//        vkCmdBeginRenderPass(commandBuffer, pRenderPassBegin, contents);
//    }
//}
//
//void VulkanAPI_vkCmdNextSubpass(VkCommandBuffer commandBuffer, VkSubpassContents contents) {
//    if (vkCmdNextSubpass) {
//        vkCmdNextSubpass(commandBuffer, contents);
//    }
//}
//
//void VulkanAPI_vkCmdEndRenderPass(VkCommandBuffer commandBuffer) {
//    if (vkCmdEndRenderPass) {
//        vkCmdEndRenderPass(commandBuffer);
//    }
//}
//
//void VulkanAPI_vkCmdExecuteCommands(VkCommandBuffer commandBuffer, uint32_t commandBufferCount, const VkCommandBuffer* pCommandBuffers) {
//    if (vkCmdExecuteCommands) {
//        vkCmdExecuteCommands(commandBuffer, commandBufferCount, pCommandBuffers);
//    }
//}
//
//VkResult VulkanAPI_vkEnumerateInstanceVersion(uint32_t* pApiVersion) {
//    if (vkEnumerateInstanceVersion) {
//        return vkEnumerateInstanceVersion(pApiVersion);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkBindBufferMemory2(VkDevice device, uint32_t bindInfoCount, const VkBindBufferMemoryInfo* pBindInfos) {
//    if (vkBindBufferMemory2) {
//        return vkBindBufferMemory2(device, bindInfoCount, pBindInfos);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkBindImageMemory2(VkDevice device, uint32_t bindInfoCount, const VkBindImageMemoryInfo* pBindInfos) {
//    if (vkBindImageMemory2) {
//        return vkBindImageMemory2(device, bindInfoCount, pBindInfos);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//void VulkanAPI_vkGetDeviceGroupPeerMemoryFeatures(VkDevice device, uint32_t heapIndex, uint32_t localDeviceIndex, uint32_t remoteDeviceIndex, VkPeerMemoryFeatureFlags* pPeerMemoryFeatures) {
//    if (vkGetDeviceGroupPeerMemoryFeatures) {
//        vkGetDeviceGroupPeerMemoryFeatures(device, heapIndex, localDeviceIndex, remoteDeviceIndex, pPeerMemoryFeatures);
//    }
//}
//
//void VulkanAPI_vkCmdSetDeviceMask(VkCommandBuffer commandBuffer, uint32_t deviceMask) {
//    if (vkCmdSetDeviceMask) {
//        vkCmdSetDeviceMask(commandBuffer, deviceMask);
//    }
//}
//
//void VulkanAPI_vkCmdDispatchBase(VkCommandBuffer commandBuffer, uint32_t baseGroupX, uint32_t baseGroupY, uint32_t baseGroupZ, uint32_t groupCountX, uint32_t groupCountY, uint32_t groupCountZ) {
//    if (vkCmdDispatchBase) {
//        vkCmdDispatchBase(commandBuffer, baseGroupX, baseGroupY, baseGroupZ, groupCountX, groupCountY, groupCountZ);
//    }
//}
//
//VkResult VulkanAPI_vkEnumeratePhysicalDeviceGroups(VkInstance instance, uint32_t* pPhysicalDeviceGroupCount, VkPhysicalDeviceGroupProperties* pPhysicalDeviceGroupProperties) {
//    if (vkEnumeratePhysicalDeviceGroups) {
//        return vkEnumeratePhysicalDeviceGroups(instance, pPhysicalDeviceGroupCount, pPhysicalDeviceGroupProperties);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//void VulkanAPI_vkGetImageMemoryRequirements2(VkDevice device, const VkImageMemoryRequirementsInfo2* pInfo, VkMemoryRequirements2* pMemoryRequirements) {
//    if (vkGetImageMemoryRequirements2) {
//        vkGetImageMemoryRequirements2(device, pInfo, pMemoryRequirements);
//    }
//}
//
//void VulkanAPI_vkGetBufferMemoryRequirements2(VkDevice device, const VkBufferMemoryRequirementsInfo2* pInfo, VkMemoryRequirements2* pMemoryRequirements) {
//    if (vkGetBufferMemoryRequirements2) {
//        vkGetBufferMemoryRequirements2(device, pInfo, pMemoryRequirements);
//    }
//}
//
//void VulkanAPI_vkGetImageSparseMemoryRequirements2(VkDevice device, const VkImageSparseMemoryRequirementsInfo2* pInfo, uint32_t* pSparseMemoryRequirementCount, VkSparseImageMemoryRequirements2* pSparseMemoryRequirements) {
//    if (vkGetImageSparseMemoryRequirements2) {
//        vkGetImageSparseMemoryRequirements2(device, pInfo, pSparseMemoryRequirementCount, pSparseMemoryRequirements);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceFeatures2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceFeatures2* pFeatures) {
//    if (vkGetPhysicalDeviceFeatures2) {
//        vkGetPhysicalDeviceFeatures2(physicalDevice, pFeatures);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceProperties2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceProperties2* pProperties) {
//    if (vkGetPhysicalDeviceProperties2) {
//        vkGetPhysicalDeviceProperties2(physicalDevice, pProperties);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceFormatProperties2(VkPhysicalDevice physicalDevice, VkFormat format, VkFormatProperties2* pFormatProperties) {
//    if (vkGetPhysicalDeviceFormatProperties2) {
//        vkGetPhysicalDeviceFormatProperties2(physicalDevice, format, pFormatProperties);
//    }
//}
//
//VkResult VulkanAPI_vkGetPhysicalDeviceImageFormatProperties2(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceImageFormatInfo2* pImageFormatInfo, VkImageFormatProperties2* pImageFormatProperties) {
//    if (vkGetPhysicalDeviceImageFormatProperties2) {
//        return vkGetPhysicalDeviceImageFormatProperties2(physicalDevice, pImageFormatInfo, pImageFormatProperties);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//void VulkanAPI_vkGetPhysicalDeviceQueueFamilyProperties2(VkPhysicalDevice physicalDevice, uint32_t* pQueueFamilyPropertyCount, VkQueueFamilyProperties2* pQueueFamilyProperties) {
//    if (vkGetPhysicalDeviceQueueFamilyProperties2) {
//        vkGetPhysicalDeviceQueueFamilyProperties2(physicalDevice, pQueueFamilyPropertyCount, pQueueFamilyProperties);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceMemoryProperties2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceMemoryProperties2* pMemoryProperties) {
//    if (vkGetPhysicalDeviceMemoryProperties2) {
//        vkGetPhysicalDeviceMemoryProperties2(physicalDevice, pMemoryProperties);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceSparseImageFormatProperties2(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceSparseImageFormatInfo2* pFormatInfo, uint32_t* pPropertyCount, VkSparseImageFormatProperties2* pProperties) {
//    if (vkGetPhysicalDeviceSparseImageFormatProperties2) {
//        vkGetPhysicalDeviceSparseImageFormatProperties2(physicalDevice, pFormatInfo, pPropertyCount, pProperties);
//    }
//}
//
//void VulkanAPI_vkTrimCommandPool(VkDevice device, VkCommandPool commandPool, VkCommandPoolTrimFlags flags) {
//    if (vkTrimCommandPool) {
//        vkTrimCommandPool(device, commandPool, flags);
//    }
//}
//
//void VulkanAPI_vkGetDeviceQueue2(VkDevice device, const VkDeviceQueueInfo2* pQueueInfo, VkQueue* pQueue) {
//    if (vkGetDeviceQueue2) {
//        vkGetDeviceQueue2(device, pQueueInfo, pQueue);
//    }
//}
//
//VkResult VulkanAPI_vkCreateSamplerYcbcrConversion(VkDevice device, const VkSamplerYcbcrConversionCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkSamplerYcbcrConversion* pYcbcrConversion) {
//    if (vkCreateSamplerYcbcrConversion) {
//        return vkCreateSamplerYcbcrConversion(device, pCreateInfo, pAllocator, pYcbcrConversion);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroySamplerYcbcrConversion(VkDevice device, VkSamplerYcbcrConversion ycbcrConversion, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroySamplerYcbcrConversion) {
//        vkDestroySamplerYcbcrConversion(device, ycbcrConversion, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkCreateDescriptorUpdateTemplate(VkDevice device, const VkDescriptorUpdateTemplateCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDescriptorUpdateTemplate* pDescriptorUpdateTemplate) {
//    if (vkCreateDescriptorUpdateTemplate) {
//        return vkCreateDescriptorUpdateTemplate(device, pCreateInfo, pAllocator, pDescriptorUpdateTemplate);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyDescriptorUpdateTemplate(VkDevice device, VkDescriptorUpdateTemplate descriptorUpdateTemplate, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyDescriptorUpdateTemplate) {
//        vkDestroyDescriptorUpdateTemplate(device, descriptorUpdateTemplate, pAllocator);
//    }
//}
//
//void VulkanAPI_vkUpdateDescriptorSetWithTemplate(VkDevice device, VkDescriptorSet descriptorSet, VkDescriptorUpdateTemplate descriptorUpdateTemplate, const void* pData) {
//    if (vkUpdateDescriptorSetWithTemplate) {
//        vkUpdateDescriptorSetWithTemplate(device, descriptorSet, descriptorUpdateTemplate, pData);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceExternalBufferProperties(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceExternalBufferInfo* pExternalBufferInfo, VkExternalBufferProperties* pExternalBufferProperties) {
//    if (vkGetPhysicalDeviceExternalBufferProperties) {
//        vkGetPhysicalDeviceExternalBufferProperties(physicalDevice, pExternalBufferInfo, pExternalBufferProperties);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceExternalFenceProperties(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceExternalFenceInfo* pExternalFenceInfo, VkExternalFenceProperties* pExternalFenceProperties) {
//    if (vkGetPhysicalDeviceExternalFenceProperties) {
//        vkGetPhysicalDeviceExternalFenceProperties(physicalDevice, pExternalFenceInfo, pExternalFenceProperties);
//    }
//}
//
//void VulkanAPI_vkGetPhysicalDeviceExternalSemaphoreProperties(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceExternalSemaphoreInfo* pExternalSemaphoreInfo, VkExternalSemaphoreProperties* pExternalSemaphoreProperties) {
//    if (vkGetPhysicalDeviceExternalSemaphoreProperties) {
//        vkGetPhysicalDeviceExternalSemaphoreProperties(physicalDevice, pExternalSemaphoreInfo, pExternalSemaphoreProperties);
//    }
//}
//
//void VulkanAPI_vkGetDescriptorSetLayoutSupport(VkDevice device, const VkDescriptorSetLayoutCreateInfo* pCreateInfo, VkDescriptorSetLayoutSupport* pSupport) {
//    if (vkGetDescriptorSetLayoutSupport) {
//        vkGetDescriptorSetLayoutSupport(device, pCreateInfo, pSupport);
//    }
//}
//
//VkResult VulkanAPI_vkGetPhysicalDeviceToolProperties(VkPhysicalDevice physicalDevice, uint32_t* pToolCount, VkPhysicalDeviceToolProperties* pToolProperties) {
//    if (vkGetPhysicalDeviceToolProperties) {
//        return vkGetPhysicalDeviceToolProperties(physicalDevice, pToolCount, pToolProperties);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//VkResult VulkanAPI_vkCreatePrivateDataSlot(VkDevice device, const VkPrivateDataSlotCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkPrivateDataSlot* pPrivateDataSlot) {
//    if (vkCreatePrivateDataSlot) {
//        return vkCreatePrivateDataSlot(device, pCreateInfo, pAllocator, pPrivateDataSlot);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//void VulkanAPI_vkDestroyPrivateDataSlot(VkDevice device, VkPrivateDataSlot privateDataSlot, const VkAllocationCallbacks* pAllocator) {
//    if (vkDestroyPrivateDataSlot) {
//        vkDestroyPrivateDataSlot(device, privateDataSlot, pAllocator);
//    }
//}
//
//VkResult VulkanAPI_vkSetPrivateData(VkDevice device, VkObjectType objectType, uint64_t objectHandle, VkPrivateDataSlot privateDataSlot, uint64_t data) {
//    if (vkSetPrivateData) {
//        return vkSetPrivateData(device, objectType, objectHandle, privateDataSlot, data);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//void VulkanAPI_vkGetPrivateData(VkDevice device, VkObjectType objectType, uint64_t objectHandle, VkPrivateDataSlot privateDataSlot, uint64_t* pData) {
//    if (vkGetPrivateData) {
//        vkGetPrivateData(device, objectType, objectHandle, privateDataSlot, pData);
//    }
//}
//
//void VulkanAPI_vkCmdSetEvent2(VkCommandBuffer commandBuffer, VkEvent event, const VkDependencyInfo* pDependencyInfo) {
//    if (vkCmdSetEvent2) {
//        vkCmdSetEvent2(commandBuffer, event, pDependencyInfo);
//    }
//}
//
//void VulkanAPI_vkCmdResetEvent2(VkCommandBuffer commandBuffer, VkEvent event, VkPipelineStageFlags2 stageMask) {
//    if (vkCmdResetEvent2) {
//        vkCmdResetEvent2(commandBuffer, event, stageMask);
//    }
//}
//
////void VulkanAPI_vkCmdWaitEvents2(VkCommandBuffer commandBuffer, uint32_t eventCount, const VkEvent* pEvents, const VkDependencyInfo* pDependencyInfos) {
////    if (vkCmdWaitEvents2) {
////        vkCmdWaitEvents2(commandBuffer, eventCount, pEvents, pDependencyInfos);
////    }
////}
//
//void VulkanAPI_vkCmdPipelineBarrier2(VkCommandBuffer commandBuffer, const VkDependencyInfo* pDependencyInfo) {
//    if (vkCmdPipelineBarrier2) {
//        vkCmdPipelineBarrier2(commandBuffer, pDependencyInfo);
//    }
//}
//
//void VulkanAPI_vkCmdWriteTimestamp2(VkCommandBuffer commandBuffer, VkPipelineStageFlags2 stage, VkQueryPool queryPool, uint32_t query) {
//    if (vkCmdWriteTimestamp2) {
//        vkCmdWriteTimestamp2(commandBuffer, stage, queryPool, query);
//    }
//}
//
//VkResult VulkanAPI_vkQueueSubmit2(VkQueue queue, uint32_t submitCount, const VkSubmitInfo2* pSubmits, VkFence fence) {
//    if (vkQueueSubmit2) {
//        return vkQueueSubmit2(queue, submitCount, pSubmits, fence);
//    }
//    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
//}
//
//void VulkanAPI_vkCmdCopyBuffer2(VkCommandBuffer commandBuffer, const VkCopyBufferInfo2* pCopyBufferInfo) {
//    if (vkCmdCopyBuffer2) {
//        vkCmdCopyBuffer2(commandBuffer, pCopyBufferInfo);
//    }
//}
//
//void VulkanAPI_vkCmdCopyImage2(VkCommandBuffer commandBuffer, const VkCopyImageInfo2* pCopyImageInfo) {
//    if (vkCmdCopyImage2) {
//        vkCmdCopyImage2(commandBuffer, pCopyImageInfo);
//    }
//}
//
//void VulkanAPI_vkCmdCopyBufferToImage2(VkCommandBuffer commandBuffer, const VkCopyBufferToImageInfo2* pCopyBufferToImageInfo) {
//    if (vkCmdCopyBufferToImage2) {
//        vkCmdCopyBufferToImage2(commandBuffer, pCopyBufferToImageInfo);
//    }
//}
//
//void VulkanAPI_vkCmdCopyImageToBuffer2(VkCommandBuffer commandBuffer, const VkCopyImageToBufferInfo2* pCopyImageToBufferInfo) {
//    if (vkCmdCopyImageToBuffer2) {
//        vkCmdCopyImageToBuffer2(commandBuffer, pCopyImageToBufferInfo);
//    }
//}
//
//void VulkanAPI_vkCmdBlitImage2(VkCommandBuffer commandBuffer, const VkBlitImageInfo2* pBlitImageInfo) {
//    if (vkCmdBlitImage2) {
//        vkCmdBlitImage2(commandBuffer, pBlitImageInfo);
//    }
//}
//
//void VulkanAPI_vkCmdResolveImage2(VkCommandBuffer commandBuffer, const VkResolveImageInfo2* pResolveImageInfo) {
//    if (vkCmdResolveImage2) {
//        vkCmdResolveImage2(commandBuffer, pResolveImageInfo);
//    }
//}
//
//void VulkanAPI_vkCmdBeginRendering(VkCommandBuffer commandBuffer, const VkRenderingInfo* pRenderingInfo) {
//    if (vkCmdBeginRendering) {
//        vkCmdBeginRendering(commandBuffer, pRenderingInfo);
//    }
//}
//
//void VulkanAPI_vkCmdEndRendering(VkCommandBuffer commandBuffer) {
//    if (vkCmdEndRendering) {
//        vkCmdEndRendering(commandBuffer);
//    }
//}
//
//void VulkanAPI_vkCmdSetCullMode(VkCommandBuffer commandBuffer, VkCullModeFlags cullMode) {
//    if (vkCmdSetCullMode) {
//        vkCmdSetCullMode(commandBuffer, cullMode);
//    }
//}
//
//void VulkanAPI_vkCmdSetFrontFace(VkCommandBuffer commandBuffer, VkFrontFace frontFace) {
//    if (vkCmdSetFrontFace) {
//        vkCmdSetFrontFace(commandBuffer, frontFace);
//    }
//}
//
//void VulkanAPI_vkCmdSetPrimitiveTopology(VkCommandBuffer commandBuffer, VkPrimitiveTopology primitiveTopology) {
//    if (vkCmdSetPrimitiveTopology) {
//        vkCmdSetPrimitiveTopology(commandBuffer, primitiveTopology);
//    }
//}
//
//void VulkanAPI_vkCmdSetViewportWithCount(VkCommandBuffer commandBuffer, uint32_t viewportCount, const VkViewport* pViewports) {
//    if (vkCmdSetViewportWithCount) {
//        vkCmdSetViewportWithCount(commandBuffer, viewportCount, pViewports);
//    }
//}
//
//void VulkanAPI_vkCmdSetScissorWithCount(VkCommandBuffer commandBuffer, uint32_t scissorCount, const VkRect2D* pScissors) {
//    if (vkCmdSetScissorWithCount) {
//        vkCmdSetScissorWithCount(commandBuffer, scissorCount, pScissors);
//    }
//}
//
//void VulkanAPI_vkCmdBindVertexBuffers2(VkCommandBuffer commandBuffer, uint32_t firstBinding, uint32_t bindingCount, const VkBuffer* pBuffers, const VkDeviceSize* pOffsets, const VkDeviceSize* pSizes, const VkDeviceSize* pStrides) {
//    if (vkCmdBindVertexBuffers2) {
//        vkCmdBindVertexBuffers2(commandBuffer, firstBinding, bindingCount, pBuffers, pOffsets, pSizes, pStrides);
//    }
//}
//
//void VulkanAPI_vkCmdSetDepthTestEnable(VkCommandBuffer commandBuffer, VkBool32 depthTestEnable) {
//    if (vkCmdSetDepthTestEnable) {
//        vkCmdSetDepthTestEnable(commandBuffer, depthTestEnable);
//    }
//}
//
//void VulkanAPI_vkCmdSetDepthWriteEnable(VkCommandBuffer commandBuffer, VkBool32 depthWriteEnable) {
//    if (vkCmdSetDepthWriteEnable) {
//        vkCmdSetDepthWriteEnable(commandBuffer, depthWriteEnable);
//    }
//}
//
//void VulkanAPI_vkCmdSetDepthCompareOp(VkCommandBuffer commandBuffer, VkCompareOp depthCompareOp) {
//    if (vkCmdSetDepthCompareOp) {
//        vkCmdSetDepthCompareOp(commandBuffer, depthCompareOp);
//    }
//}
//
//void VulkanAPI_vkCmdSetDepthBoundsTestEnable(VkCommandBuffer commandBuffer, VkBool32 depthBoundsTestEnable) {
//    if (vkCmdSetDepthBoundsTestEnable) {
//        vkCmdSetDepthBoundsTestEnable(commandBuffer, depthBoundsTestEnable);
//    }
//}
//
//void VulkanAPI_vkCmdSetStencilTestEnable(VkCommandBuffer commandBuffer, VkBool32 stencilTestEnable) {
//    if (vkCmdSetStencilTestEnable) {
//        vkCmdSetStencilTestEnable(commandBuffer, stencilTestEnable);
//    }
//}
//
//void VulkanAPI_vkCmdSetStencilOp(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, VkStencilOp failOp, VkStencilOp passOp, VkStencilOp depthFailOp, VkCompareOp compareOp) {
//    if (vkCmdSetStencilOp) {
//        vkCmdSetStencilOp(commandBuffer, faceMask, failOp, passOp, depthFailOp, compareOp);
//    }
//}
//
//void VulkanAPI_vkCmdSetRasterizerDiscardEnable(VkCommandBuffer commandBuffer, VkBool32 rasterizerDiscardEnable) {
//    if (vkCmdSetRasterizerDiscardEnable) {
//        vkCmdSetRasterizerDiscardEnable(commandBuffer, rasterizerDiscardEnable);
//    }
//}
//
//void VulkanAPI_vkCmdSetDepthBiasEnable(VkCommandBuffer commandBuffer, VkBool32 depthBiasEnable) {
//    if (vkCmdSetDepthBiasEnable) {
//        vkCmdSetDepthBiasEnable(commandBuffer, depthBiasEnable);
//    }
//}
//
//void VulkanAPI_vkCmdSetPrimitiveRestartEnable(VkCommandBuffer commandBuffer, VkBool32 primitiveRestartEnable) {
//    if (vkCmdSetPrimitiveRestartEnable) {
//        vkCmdSetPrimitiveRestartEnable(commandBuffer, primitiveRestartEnable);
//    }
//}
//
//void VulkanAPI_vkGetDeviceBufferMemoryRequirements(VkDevice device, const VkDeviceBufferMemoryRequirements* pInfo, VkMemoryRequirements2* pMemoryRequirements) {
//    if (vkGetDeviceBufferMemoryRequirements) {
//        vkGetDeviceBufferMemoryRequirements(device, pInfo, pMemoryRequirements);
//    }
//}
//
//void VulkanAPI_vkGetDeviceImageMemoryRequirements(VkDevice device, const VkDeviceImageMemoryRequirements* pInfo, VkMemoryRequirements2* pMemoryRequirements) {
//    if (vkGetDeviceImageMemoryRequirements) {
//        vkGetDeviceImageMemoryRequirements(device, pInfo, pMemoryRequirements);
//    }
//}
//
//void VulkanAPI_vkGetDeviceImageSparseMemoryRequirements(VkDevice device, const VkDeviceImageMemoryRequirements* pInfo, uint32_t* pSparseMemoryRequirementCount, VkSparseImageMemoryRequirements2* pSparseMemoryRequirements) {
//    if (vkGetDeviceImageSparseMemoryRequirements) {
//        vkGetDeviceImageSparseMemoryRequirements(device, pInfo, pSparseMemoryRequirementCount, pSparseMemoryRequirements);
//    }
//}
//
////uint64_t VulkanAPI_vkGetDeviceMemoryOpaqueCaptureAddressKHR(VkDevice device, const VkDeviceMemoryOpaqueCaptureAddressInfo* pInfo) {
////    if (vkGetDeviceMemoryOpaqueCaptureAddressKHR) {
////        return vkGetDeviceMemoryOpaqueCaptureAddressKHR(device, pInfo);
////    }
////    return 0;  // Or some appropriate error code
////}
//
////VkResult VulkanAPI_vkCreateDeferredOperationKHR(VkDevice device, const VkAllocationCallbacks* pAllocator, VkDeferredOperationKHR* pDeferredOperation) {
////    if (vkCreateDeferredOperationKHR) {
////        return vkCreateDeferredOperationKHR(device, pAllocator, pDeferredOperation);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
////}
//
////void VulkanAPI_vkDestroyDeferredOperationKHR(VkDevice device, VkDeferredOperationKHR operation, const VkAllocationCallbacks* pAllocator) {
////    if (vkDestroyDeferredOperationKHR) {
////        vkDestroyDeferredOperationKHR(device, operation, pAllocator);
////    }
////}
//
////uint32_t VulkanAPI_vkGetDeferredOperationMaxConcurrencyKHR(VkDevice device, VkDeferredOperationKHR operation) {
////    if (vkGetDeferredOperationMaxConcurrencyKHR) {
////        return vkGetDeferredOperationMaxConcurrencyKHR(device, operation);
////    }
////    return 0;  // Or some appropriate error code
////}
//
////VkResult VulkanAPI_vkGetDeferredOperationResultKHR(VkDevice device, VkDeferredOperationKHR operation) {
////
////        return vkGetDeferredOperationResultKHR(device, operation);
////}
//
////VkResult VulkanAPI_vkDeferredOperationJoinKHR(VkDevice device, VkDeferredOperationKHR operation) {
////    if (vkDeferredOperationJoinKHR) {
////        return vkDeferredOperationJoinKHR(device, operation);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
////}
//
////VkResult VulkanAPI_vkGetPipelineExecutablePropertiesKHR(VkDevice device, const VkPipelineInfoKHR* pPipelineInfo, uint32_t* pExecutableCount, VkPipelineExecutablePropertiesKHR* pProperties) {
////    if (vkGetPipelineExecutablePropertiesKHR) {
////        return vkGetPipelineExecutablePropertiesKHR(device, pPipelineInfo, pExecutableCount, pProperties);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
////}
//
////VkResult VulkanAPI_vkGetPipelineExecutableStatisticsKHR(VkDevice device, const VkPipelineExecutableInfoKHR* pExecutableInfo, uint32_t* pStatisticCount, VkPipelineExecutableStatisticKHR* pStatistics) {
////    if (vkGetPipelineExecutableStatisticsKHR) {
////        return vkGetPipelineExecutableStatisticsKHR(device, pExecutableInfo, pStatisticCount, pStatistics);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
////}
////
////VkResult VulkanAPI_vkGetPipelineExecutableInternalRepresentationsKHR(VkDevice device, const VkPipelineExecutableInfoKHR* pExecutableInfo, uint32_t* pInternalRepresentationCount, VkPipelineExecutableInternalRepresentationKHR* pInternalRepresentations) {
////    if (vkGetPipelineExecutableInternalRepresentationsKHR) {
////        return vkGetPipelineExecutableInternalRepresentationsKHR(device, pExecutableInfo, pInternalRepresentationCount, pInternalRepresentations);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
////}
//
////VkResult VulkanAPI_vkMapMemory2KHR(VkDevice device, const VkMemoryMapInfoKHR* pMemoryMapInfo, void** ppData) {
////    if (vkMapMemory2KHR) {
////        return vkMapMemory2KHR(device, pMemoryMapInfo, ppData);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
////}
//
////VkResult VulkanAPI_vkUnmapMemory2KHR(VkDevice device, const VkMemoryUnmapInfoKHR* pMemoryUnmapInfo) {
////    if (vkUnmapMemory2KHR) {
////        return vkUnmapMemory2KHR(device, pMemoryUnmapInfo);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
////}
//
////VkResult VulkanAPI_vkGetPhysicalDeviceVideoEncodeQualityLevelPropertiesKHR(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceVideoEncodeQualityLevelInfoKHR* pQualityLevelInfo, VkVideoEncodeQualityLevelPropertiesKHR* pQualityLevelProperties) {
////    if (vkGetPhysicalDeviceVideoEncodeQualityLevelPropertiesKHR) {
////        return vkGetPhysicalDeviceVideoEncodeQualityLevelPropertiesKHR(physicalDevice, pQualityLevelInfo, pQualityLevelProperties);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
////}
//
////VkResult VulkanAPI_vkGetEncodedVideoSessionParametersKHR(VkDevice device, const VkVideoEncodeSessionParametersGetInfoKHR* pVideoSessionParametersInfo, VkVideoEncodeSessionParametersFeedbackInfoKHR* pFeedbackInfo, size_t* pDataSize, void* pData) {
////    if (vkGetEncodedVideoSessionParametersKHR) {
////        return vkGetEncodedVideoSessionParametersKHR(device, pVideoSessionParametersInfo, pFeedbackInfo, pDataSize, pData);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
////}
//
////void VulkanAPI_vkCmdEncodeVideoKHR(VkCommandBuffer commandBuffer, const VkVideoEncodeInfoKHR* pEncodeInfo) {
////    if (vkCmdEncodeVideoKHR) {
////        vkCmdEncodeVideoKHR(commandBuffer, pEncodeInfo);
////    }
////}
//
////void VulkanAPI_vkCmdSetEvent2KHR(VkCommandBuffer commandBuffer, VkEvent event, const VkDependencyInfo* pDependencyInfo) {
////    if (vkCmdSetEvent2KHR) {
////        vkCmdSetEvent2KHR(commandBuffer, event, pDependencyInfo);
////    }
////}
////
////void VulkanAPI_vkCmdResetEvent2KHR(VkCommandBuffer commandBuffer, VkEvent event, VkPipelineStageFlags2 stageMask) {
////    if (vkCmdResetEvent2KHR) {
////        vkCmdResetEvent2KHR(commandBuffer, event, stageMask);
////    }
////}
//
////void VulkanAPI_vkCmdWaitEvents2KHR(VkCommandBuffer commandBuffer, uint32_t eventCount, const VkEvent* pEvents, const VkDependencyInfo* pDependencyInfos) {
////    if (vkCmdWaitEvents2KHR) {
////        vkCmdWaitEvents2KHR(commandBuffer, eventCount, pEvents, pDependencyInfos);
////    }
////}
//
////void VulkanAPI_vkCmdPipelineBarrier2KHR(VkCommandBuffer commandBuffer, const VkDependencyInfo* pDependencyInfo) {
////    if (vkCmdPipelineBarrier2KHR) {
////        vkCmdPipelineBarrier2KHR(commandBuffer, pDependencyInfo);
////    }
////}
//
////void VulkanAPI_vkCmdWriteTimestamp2KHR(VkCommandBuffer commandBuffer, VkPipelineStageFlags2 stage, VkQueryPool queryPool, uint32_t query) {
////    if (vkCmdWriteTimestamp2KHR) {
////        vkCmdWriteTimestamp2KHR(commandBuffer, stage, queryPool, query);
////    }
////}
//
////VkResult VulkanAPI_vkQueueSubmit2KHR(VkQueue queue, uint32_t submitCount, const VkSubmitInfo2* pSubmits, VkFence fence) {
////    if (vkQueueSubmit2KHR) {
////        return vkQueueSubmit2KHR(queue, submitCount, pSubmits, fence);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other appropriate error
////}
//
////VkResult VulkanAPI_vkCreateDeferredOperationKHR(VkDevice device, const VkAllocationCallbacks* pAllocator, VkDeferredOperationKHR* pDeferredOperation) {
////    if (vkCreateDeferredOperationKHR) {
////        return vkCreateDeferredOperationKHR(device, pAllocator, pDeferredOperation);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
//
////void VulkanAPI_vkDestroyDeferredOperationKHR(VkDevice device, VkDeferredOperationKHR operation, const VkAllocationCallbacks* pAllocator) {
////    if (vkDestroyDeferredOperationKHR) {
////        vkDestroyDeferredOperationKHR(device, operation, pAllocator);
////    }
////}
//
////uint32_t VulkanAPI_vkGetDeferredOperationMaxConcurrencyKHR(VkDevice device, VkDeferredOperationKHR operation) {
////    if (vkGetDeferredOperationMaxConcurrencyKHR) {
////        return vkGetDeferredOperationMaxConcurrencyKHR(device, operation);
////    }
////    return 0;  // Or other error handling
////}
//
////VkResult VulkanAPI_vkGetDeferredOperationResultKHR(VkDevice device, VkDeferredOperationKHR operation) {
////    if (vkGetDeferredOperationResultKHR) {
////        return vkGetDeferredOperationResultKHR(device, operation);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
//
////VkResult VulkanAPI_vkDeferredOperationJoinKHR(VkDevice device, VkDeferredOperationKHR operation) {
////    if (vkDeferredOperationJoinKHR) {
////        return vkDeferredOperationJoinKHR(device, operation);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
//
////uint64_t VulkanAPI_vkGetDeviceMemoryOpaqueCaptureAddressKHR(VkDevice device, const VkDeviceMemoryOpaqueCaptureAddressInfo* pInfo) {
////    if (vkGetDeviceMemoryOpaqueCaptureAddressKHR) {
////        return vkGetDeviceMemoryOpaqueCaptureAddressKHR(device, pInfo);
////    }
////    return 0;  // Or other error handling
////}
//
////VkResult VulkanAPI_vkGetPipelineExecutablePropertiesKHR(VkDevice device, const VkPipelineInfoKHR* pPipelineInfo, uint32_t* pExecutableCount, VkPipelineExecutablePropertiesKHR* pProperties) {
////    if (vkGetPipelineExecutablePropertiesKHR) {
////        return vkGetPipelineExecutablePropertiesKHR(device, pPipelineInfo, pExecutableCount, pProperties);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
////
////VkResult VulkanAPI_vkGetPipelineExecutableStatisticsKHR(VkDevice device, const VkPipelineExecutableInfoKHR* pExecutableInfo, uint32_t* pStatisticCount, VkPipelineExecutableStatisticKHR* pStatistics) {
////    if (vkGetPipelineExecutableStatisticsKHR) {
////        return vkGetPipelineExecutableStatisticsKHR(device, pExecutableInfo, pStatisticCount, pStatistics);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
//
////VkResult VulkanAPI_vkGetPipelineExecutableInternalRepresentationsKHR(VkDevice device, const VkPipelineExecutableInfoKHR* pExecutableInfo, uint32_t* pInternalRepresentationCount, VkPipelineExecutableInternalRepresentationKHR* pInternalRepresentations) {
////    if (vkGetPipelineExecutableInternalRepresentationsKHR) {
////        return vkGetPipelineExecutableInternalRepresentationsKHR(device, pExecutableInfo, pInternalRepresentationCount, pInternalRepresentations);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
//
////VkResult VulkanAPI_vkMapMemory2KHR(VkDevice device, const VkMemoryMapInfoKHR* pMemoryMapInfo, void** ppData) {
////    if (vkMapMemory2KHR) {
////        return vkMapMemory2KHR(device, pMemoryMapInfo, ppData);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
//
////VkResult VulkanAPI_vkUnmapMemory2KHR(VkDevice device, const VkMemoryUnmapInfoKHR* pMemoryUnmapInfo) {
////    if (vkUnmapMemory2KHR) {
////        return vkUnmapMemory2KHR(device, pMemoryUnmapInfo);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
//
////VkResult VulkanAPI_vkGetPhysicalDeviceVideoEncodeQualityLevelPropertiesKHR(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceVideoEncodeQualityLevelInfoKHR* pQualityLevelInfo, VkVideoEncodeQualityLevelPropertiesKHR* pQualityLevelProperties) {
////    if (vkGetPhysicalDeviceVideoEncodeQualityLevelPropertiesKHR) {
////        return vkGetPhysicalDeviceVideoEncodeQualityLevelPropertiesKHR(physicalDevice, pQualityLevelInfo, pQualityLevelProperties);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
//
////VkResult VulkanAPI_vkGetEncodedVideoSessionParametersKHR(VkDevice device, const VkVideoEncodeSessionParametersGetInfoKHR* pVideoSessionParametersInfo, VkVideoEncodeSessionParametersFeedbackInfoKHR* pFeedbackInfo, size_t* pDataSize, void* pData) {
////    if (vkGetEncodedVideoSessionParametersKHR) {
////        return vkGetEncodedVideoSessionParametersKHR(device, pVideoSessionParametersInfo, pFeedbackInfo, pDataSize, pData);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
//
////void VulkanAPI_vkCmdEncodeVideoKHR(VkCommandBuffer commandBuffer, const VkVideoEncodeInfoKHR* pEncodeInfo) {
////    if (vkCmdEncodeVideoKHR) {
////        vkCmdEncodeVideoKHR(commandBuffer, pEncodeInfo);
////    }
////}
//
////void VulkanAPI_vkCmdSetEvent2KHR(VkCommandBuffer commandBuffer, VkEvent event, const VkDependencyInfo* pDependencyInfo) {
////    if (vkCmdSetEvent2KHR) {
////        vkCmdSetEvent2KHR(commandBuffer, event, pDependencyInfo);
////    }
////}
//
////void VulkanAPI_vkCmdResetEvent2KHR(VkCommandBuffer commandBuffer, VkEvent event, VkPipelineStageFlags2 stageMask) {
////    if (vkCmdResetEvent2KHR) {
////        vkCmdResetEvent2KHR(commandBuffer, event, stageMask);
////    }
////}
//
////void VulkanAPI_vkCmdWaitEvents2KHR(VkCommandBuffer commandBuffer, uint32_t eventCount, const VkEvent* pEvents, const VkDependencyInfo* pDependencyInfos) {
////    if (vkCmdWaitEvents2KHR) {
////        vkCmdWaitEvents2KHR(commandBuffer, eventCount, pEvents, pDependencyInfos);
////    }
////}
//
////void VulkanAPI_vkCmdPipelineBarrier2KHR(VkCommandBuffer commandBuffer, const VkDependencyInfo* pDependencyInfo) {
////    if (vkCmdPipelineBarrier2KHR) {
////        vkCmdPipelineBarrier2KHR(commandBuffer, pDependencyInfo);
////    }
////}
//
////void VulkanAPI_vkCmdWriteTimestamp2KHR(VkCommandBuffer commandBuffer, VkPipelineStageFlags2 stage, VkQueryPool queryPool, uint32_t query) {
////    if (vkCmdWriteTimestamp2KHR) {
////        vkCmdWriteTimestamp2KHR(commandBuffer, stage, queryPool, query);
////    }
////}
//
////VkResult VulkanAPI_vkQueueSubmit2KHR(VkQueue queue, uint32_t submitCount, const VkSubmitInfo2* pSubmits, VkFence fence) {
////    if (vkQueueSubmit2KHR) {
////        return vkQueueSubmit2KHR(queue, submitCount, pSubmits, fence);
////    }
////    return VK_ERROR_INITIALIZATION_FAILED;  // Or other error handling
////}
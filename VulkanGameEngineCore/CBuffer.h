#pragma once
#include <vulkan/vulkan.h> 
#include <stdint.h>      
#include <stdbool.h>  
#include "Macro.h"

#ifdef __cplusplus
extern "C" {
#endif

typedef struct VulkanBufferInfo {
    VkBuffer* Buffer;
    VkBuffer* StagingBuffer;
    VkDeviceMemory* BufferMemory;
    VkDeviceMemory* StagingBufferMemory;
    VkDeviceSize* BufferSize;
    VkBufferUsageFlags* BufferUsage;
    VkMemoryPropertyFlags* BufferProperties;
    uint64_t* BufferDeviceAddress; 
    VkAccelerationStructureKHR* BufferHandle;
    void** BufferData;
    bool* IsMapped;
} VulkanBufferInfo;

VkResult Buffer_CreateBuffer(VkDevice device, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, size_t bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
VkResult Buffer_CreateStagingBuffer(VkDevice device, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
VkResult Buffer_CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
VkResult Buffer_RenderPassCopyBuffer(VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
VkResult Buffer_UpdateBufferSize(VulkanBufferInfo* bufferInfo, VkDevice device, VkDeviceSize bufferSize);
VkResult Buffer_UnmapBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device);
VkResult Buffer_UpdateBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device, void* dataToCopy, VkDeviceSize bufferSize);
VkResult Buffer_UpdateStagingBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device, void* dataToCopy, VkDeviceSize bufferSize);
void* Buffer_MapBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device);
void Buffer_DestroyBuffer(VulkanBufferInfo* bufferInfo);

#ifdef __cplusplus
}
#endif
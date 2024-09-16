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

    VkResult Buffer_CreateBuffer(VulkanBufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
    VkResult Buffer_CreateStagingBuffer(VulkanBufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
    VkResult Buffer_CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
    VkResult Buffer_CopyStagingBuffer(VulkanBufferInfo* bufferInfo, VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
    VkResult Buffer_UpdateBufferSize(VulkanBufferInfo* bufferInfo, VkDeviceSize bufferSize);
    VkResult Buffer_UnmapBufferMemory(VkDevice device, VkDeviceMemory deviceMemory, bool* isMapped);
    VkResult Buffer_UpdateBufferMemory(VulkanBufferInfo* bufferInfo, void* dataToCopy, VkDeviceSize bufferSize);
    VkResult Buffer_UpdateStagingBufferMemory(VulkanBufferInfo* bufferInfo, void* dataToCopy, VkDeviceSize bufferSize);
    void* Buffer_MapBufferMemory(VkDevice device, VkDeviceMemory deviceMemory, size_t bufferSize, bool* isMapped);
    void Buffer_DestroyBuffer(VulkanBufferInfo* bufferInfo);

#ifdef __cplusplus
}
#endif
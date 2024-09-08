#pragma once
#include <vulkan/vulkan.h> 
#include <stdint.h>      
#include <stdbool.h>  
#include "Macro.h"

#ifdef __cplusplus
extern "C" {
#endif

typedef struct BufferInfo {
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
} BufferInfo;

VkResult Buffer_CreateBuffer(BufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
VkResult Buffer_CreateStagingBuffer(BufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
VkResult Buffer_CopyBuffer(BufferInfo* bufferInfo, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
VkResult Buffer_CopyStagingBuffer(BufferInfo* bufferInfo, VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
VkResult Buffer_UpdateBufferSize(BufferInfo* bufferInfo, VkDeviceSize bufferSize);
VkResult Buffer_UnmapBufferMemory(BufferInfo* bufferInfo);
VkResult Buffer_UpdateBufferMemory(BufferInfo* bufferInfo, void* dataToCopy, VkDeviceSize bufferSize);
VkResult Buffer_UpdateStagingBufferMemory(BufferInfo* bufferInfo, void* dataToCopy, VkDeviceSize bufferSize);
void* Buffer_MapBufferMemory(BufferInfo* bufferInfo);
void* Buffer_CheckBufferContents(BufferInfo* bufferInfo);
void Buffer_DestroyBuffer(BufferInfo* bufferInfo);

#ifdef __cplusplus
}
#endif
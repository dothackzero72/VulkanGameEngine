#pragma once
#include <vulkan/vulkan.h> 
#include <stdint.h>      
#include <stdbool.h>  
#include "Macro.h"

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

DLL_EXPORT VkResult Buffer_CreateBuffer(BufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
DLL_EXPORT VkResult Buffer_CreateStagingBuffer(BufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
DLL_EXPORT VkResult Buffer_CopyBuffer(BufferInfo* bufferInfo, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
DLL_EXPORT VkResult Buffer_CopyStagingBuffer(BufferInfo* bufferInfo, VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
DLL_EXPORT VkResult Buffer_UpdateBufferSize(BufferInfo* bufferInfo, VkDeviceSize bufferSize);
DLL_EXPORT VkResult Buffer_UnmapBufferMemory(BufferInfo* bufferInfo);
DLL_EXPORT VkResult Buffer_UpdateBufferMemory(BufferInfo* bufferInfo, void* dataToCopy, VkDeviceSize bufferSize);
DLL_EXPORT VkResult Buffer_UpdateStagingBufferMemory(BufferInfo* bufferInfo, void* dataToCopy, VkDeviceSize bufferSize);
DLL_EXPORT void* Buffer_MapBufferMemory(BufferInfo* bufferInfo);
DLL_EXPORT void* Buffer_CheckBufferContents(BufferInfo* bufferInfo);
DLL_EXPORT void Buffer_DestroyBuffer(BufferInfo* bufferInfo);
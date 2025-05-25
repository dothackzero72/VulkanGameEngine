#pragma once
#include <vulkan/vulkan.h> 
#include <stdint.h>      
#include <stdbool.h>  
#include <iostream>
#include <memory>
#include <vector>
#include "Macro.h"
#include "Typedef.h"

#ifdef __cplusplus
extern "C" {
#endif
	DLL_EXPORT VkResult Buffer_UpdateBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, void* dataToCopy, VkDeviceSize bufferSize);
	DLL_EXPORT void Buffer_CopyBufferMemory(VkDevice device, VkCommandPool commandPool, VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize bufferSize);
	DLL_EXPORT VkResult Buffer_AllocateMemory(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* bufferData, VkDeviceMemory* bufferMemory, VkMemoryPropertyFlags properties);
	DLL_EXPORT void* Buffer_MapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, VkDeviceSize bufferSize, bool* isMapped);
	DLL_EXPORT VkResult Buffer_UnmapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, bool* isMapped);
	DLL_EXPORT VkResult Buffer_CreateBuffer(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkMemoryPropertyFlags properties, VkBufferUsageFlags usage);
	DLL_EXPORT VkResult Buffer_CreateStagingBuffer(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkBuffer* stagingBuffer, VkBuffer* buffer, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
	DLL_EXPORT VkResult Buffer_CopyBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
	DLL_EXPORT VkResult Buffer_UpdateBufferSize(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize* oldBufferSize, VkDeviceSize newBufferSize, VkBufferUsageFlags bufferUsageFlags, VkMemoryPropertyFlags propertyFlags);
	DLL_EXPORT void Buffer_UpdateBufferData(VkDevice device, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize);
	DLL_EXPORT void Buffer_UpdateStagingBufferData(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkBuffer stagingBuffer, VkBuffer buffer, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize);
	DLL_EXPORT VkResult Buffer_DestroyBuffer(VkDevice device, VkBuffer* buffer, VkBuffer* stagingBuffer, VkDeviceMemory* bufferMemory, VkDeviceMemory* stagingBufferMemory, void* bufferData, VkDeviceSize* bufferSize, VkBufferUsageFlags* bufferUsageFlags, VkMemoryPropertyFlags* propertyFlags);
#ifdef __cplusplus
}
#endif
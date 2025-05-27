#pragma once
#include <vulkan/vulkan.h> 
#include <stdint.h>      
#include <stdbool.h>  
#include <iostream>
#include <memory>
#include <vector>
#include "Macro.h"
#include "Typedef.h"
#include "CoreVulkanRenderer.h"

#ifdef __cplusplus
extern "C" {
#endif
	DLL_EXPORT VkResult Buffer_UpdateBufferMemory(const RendererState& renderState, VkDeviceMemory bufferMemory, void* dataToCopy, VkDeviceSize bufferSize);
	DLL_EXPORT void Buffer_CopyBufferMemory(const RendererState& renderState, VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize bufferSize);
	DLL_EXPORT VkResult Buffer_AllocateMemory(const RendererState& renderState, VkBuffer* bufferData, VkDeviceMemory* bufferMemory, VkMemoryPropertyFlags properties);
	DLL_EXPORT void* Buffer_MapBufferMemory(const RendererState& renderState, VkDeviceMemory bufferMemory, VkDeviceSize bufferSize, bool* isMapped);
	DLL_EXPORT VkResult Buffer_UnmapBufferMemory(const RendererState& renderState, VkDeviceMemory bufferMemory, bool* isMapped);
	DLL_EXPORT VkResult Buffer_CreateBuffer(const RendererState& renderState, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkMemoryPropertyFlags properties, VkBufferUsageFlags usage);
	DLL_EXPORT VkResult Buffer_CreateStagingBuffer(const RendererState& renderState, VkBuffer* stagingBuffer, VkBuffer* buffer, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
	DLL_EXPORT VkResult Buffer_CopyBuffer(const RendererState& renderState, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
	DLL_EXPORT VkResult Buffer_UpdateBufferSize(const RendererState& renderState, VkBuffer buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize* oldBufferSize, VkDeviceSize newBufferSize, VkBufferUsageFlags bufferUsageFlags, VkMemoryPropertyFlags propertyFlags);
	DLL_EXPORT void Buffer_UpdateBufferData(const RendererState& renderState, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize);
	DLL_EXPORT void Buffer_UpdateStagingBufferData(const RendererState& renderState, VkBuffer stagingBuffer, VkBuffer buffer, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize);
	DLL_EXPORT VkResult Buffer_DestroyBuffer(const RendererState& renderState, VkBuffer* buffer, VkBuffer* stagingBuffer, VkDeviceMemory* bufferMemory, VkDeviceMemory* stagingBufferMemory, void* bufferData, VkDeviceSize* bufferSize, VkBufferUsageFlags* bufferUsageFlags, VkMemoryPropertyFlags* propertyFlags);
#ifdef __cplusplus
}
#endif
#pragma once
#include "DLL.h"
#include <VulkanBuffer.h>

extern "C" 
{
	DLL_EXPORT VkResult DLL_Buffer_CreateBuffer(VulkanBufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
	DLL_EXPORT VkResult DLL_Buffer_CreateStagingBuffer(VulkanBufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
	DLL_EXPORT VkResult DLL_Buffer_CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
	DLL_EXPORT VkResult DLL_Buffer_CopyStagingBuffer(VulkanBufferInfo* bufferInfo, VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
	DLL_EXPORT VkResult DLL_Buffer_UpdateBufferSize(VulkanBufferInfo* bufferInfo, VkDevice device, VkDeviceSize bufferSize);
	DLL_EXPORT VkResult DLL_Buffer_UnmapBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device);
	DLL_EXPORT VkResult DLL_Buffer_UpdateBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device, void* dataToCopy, VkDeviceSize bufferSize);
	DLL_EXPORT VkResult DLL_Buffer_UpdateStagingBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device, void* dataToCopy, VkDeviceSize bufferSize);
	DLL_EXPORT void* DLL_Buffer_MapBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device);
	DLL_EXPORT void DLL_Buffer_DestroyBuffer(VulkanBufferInfo* bufferInfo);
	DLL_EXPORT int DLL_BUFFER_BufferTest();
}

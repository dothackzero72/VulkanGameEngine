#pragma once
#include <vulkan/vulkan.h> 
#include <stdint.h>      
#include <stdbool.h>  
#include <iostream>
#include <memory>
#include <vector>
#include "Macro.h"
#include "Typedef.h"

template <class T>
struct VulkanBufferFuncs
{
	void* BufferData;
	bool IsMapped = false;
	bool UsingStagingBuffer = false;

	VkDeviceSize BufferSize = 0;
	uint64 BufferDeviceAddress = 0;

	VkBufferUsageFlags BufferUsage;
	VkMemoryPropertyFlags BufferProperties;
	VkBuffer Buffer = VK_NULL_HANDLE;
	VkBuffer StagingBuffer = VK_NULL_HANDLE;
	VkDeviceMemory StagingBufferMemory = VK_NULL_HANDLE;
	VkDeviceMemory BufferMemory = VK_NULL_HANDLE;
	VkAccelerationStructureKHR BufferHandle = VK_NULL_HANDLE;
};


//VulkanBufferFuncs Buffer_VulkanBuffer(VkBuffer& stagingBuffer, VkDeviceMemory& stagingBufferMemory, VkDeviceMemory& bufferMemory, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags bufferProperties);
//VulkanBufferFuncs Buffer_VulkanBuffer(void* bufferData, uint32 bufferElementCount, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer);
//VulkanBufferFuncs Buffer_VulkanBuffer(T bufferData, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer);
//VulkanBufferFuncs Buffer_VulkanBuffer(Vector<T> bufferDataList, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer);
//VulkanBufferFuncs Buffer_VulkanBuffer(Vector<T> bufferDataList, uint reserveCount, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer);
//	VkResult Buffer_CreateBuffer(void* bufferData);
//	VkResult Buffer_CreateStagingBuffer(void* bufferData);
//	VkResult Buffer_UpdateBufferSize(VkBuffer buffer, VkDeviceMemory bufferMemory, VkDeviceSize newBufferSize);
//	VkResult Buffer_CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
//	void Buffer_UpdateBufferMemory(T& bufferData);
//	void Buffer_UpdateBufferMemory(Vector<T>& bufferData);
//	void Buffer_UpdateBufferMemory(void* bufferData, VkDeviceSize totalBufferSize);
//	Vector<T> Buffer_CheckBufferContents();
//	void Buffer_DestroyBuffer();

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
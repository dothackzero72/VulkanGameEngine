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

enum BufferTypeEnum
{
	BufferType_UInt,
	BufferType_Mat4,
	BufferType_MaterialProperitiesBuffer,
	BufferType_SpriteInstanceStruct,
	BufferType_MeshPropertiesStruct,
	BufferType_SpriteMesh,
	BufferType_LevelLayerMesh,
	BufferType_Material,
	BufferType_Vector2D
};

struct VulkanBuffer
{
	uint BufferId = 0;
	VkBuffer Buffer = VK_NULL_HANDLE;             
	VkBuffer StagingBuffer = VK_NULL_HANDLE;     
	VkDeviceMemory StagingBufferMemory = VK_NULL_HANDLE;
	VkDeviceMemory BufferMemory = VK_NULL_HANDLE;  
	VkDeviceSize BufferSize = 0;                   
	VkBufferUsageFlags BufferUsage = 0;             
	VkMemoryPropertyFlags BufferProperties = 0;    
	uint64_t BufferDeviceAddress = 0;              
	VkAccelerationStructureKHR BufferHandle = VK_NULL_HANDLE; 
	BufferTypeEnum  BufferType;              
	void* BufferData = nullptr;                    
	bool IsMapped = false;                         
	bool UsingStagingBuffer = false;       
};

DLL_EXPORT VulkanBuffer VulkanBuffer_CreateVulkanBuffer(const RendererState& renderer, uint bufferId, void* bufferData, VkDeviceSize bufferElementSize, uint bufferElementCount, BufferTypeEnum bufferTypeEnum, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer);
DLL_EXPORT VulkanBuffer VulkanBuffer_CreateVulkanBuffer(const RendererState& renderer, VulkanBuffer& vulkanBuffer, uint bufferId, void* bufferData, VkDeviceSize bufferElementSize, uint bufferElementCount, BufferTypeEnum bufferTypeEnum, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer);
DLL_EXPORT void VulkanBuffer_UpdateBufferMemory(const RendererState& renderer, VulkanBuffer& vulkanBuffer, void* bufferData, VkDeviceSize bufferElementSize, uint bufferElementCount);
DLL_EXPORT VkResult VulkanBuffer_CopyBuffer(const RendererState& renderer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
DLL_EXPORT void VulkanBuffer_DestroyBuffer(const RendererState& renderer, VulkanBuffer& vulkanBuffer);

void VulkanBuffer_UpdateBufferSize(const RendererState& renderer, VulkanBuffer& vulkanBuffer, VkDeviceSize newBufferElementSize, uint32_t newBufferElementCount);

#ifdef __cplusplus
extern "C" {
#endif
	VkResult Buffer_UpdateBufferMemory(const RendererState& renderState, VkDeviceMemory bufferMemory, void* dataToCopy, VkDeviceSize bufferSize);
	void Buffer_CopyBufferMemory(const RendererState& renderState, VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize bufferSize);
	VkResult Buffer_AllocateMemory(const RendererState& renderState, VkBuffer* bufferData, VkDeviceMemory* bufferMemory, VkMemoryPropertyFlags properties);
	void* Buffer_MapBufferMemory(const RendererState& renderState, VkDeviceMemory bufferMemory, VkDeviceSize bufferSize, bool* isMapped);
	VkResult Buffer_UnmapBufferMemory(const RendererState& renderState, VkDeviceMemory bufferMemory, bool* isMapped);
	VkResult Buffer_CreateBuffer(const RendererState& renderState, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkMemoryPropertyFlags properties, VkBufferUsageFlags usage);
	VkResult Buffer_CreateStagingBuffer(const RendererState& renderState, VkBuffer* stagingBuffer, VkBuffer* buffer, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties);
	VkResult Buffer_CopyBuffer(const RendererState& renderState, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
	VkResult Buffer_UpdateBufferSize(const RendererState& renderState, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize oldBufferSize, VkDeviceSize newBufferSize, VkBufferUsageFlags bufferUsageFlags, VkMemoryPropertyFlags propertyFlags);
	void Buffer_UpdateBufferData(const RendererState& renderState, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize);
	void Buffer_UpdateStagingBufferData(const RendererState& renderState, VkBuffer stagingBuffer, VkBuffer buffer, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize);
	VkResult Buffer_DestroyBuffer(const RendererState& renderState, VkBuffer* buffer, VkBuffer* stagingBuffer, VkDeviceMemory* bufferMemory, VkDeviceMemory* stagingBufferMemory, void** bufferData, VkDeviceSize* bufferSize, VkBufferUsageFlags* bufferUsage, VkMemoryPropertyFlags* propertyFlags);
#ifdef __cplusplus
}
#endif

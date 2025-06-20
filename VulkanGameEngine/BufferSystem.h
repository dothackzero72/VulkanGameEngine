#pragma once
#include <Typedef.h>
#include <VulkanBuffer.h>
#include <Mesh.h>
#include <Material.h>
#include "RenderSystem.h"
#include "Vertex.h"
#include "Sprite.h"

class VulkanBufferSystem
{
private:
	

	template <typename T>
	BufferTypeEnum GetBufferType() 
	{
		if constexpr (std::is_same_v<T, uint32>) { return BufferType_UInt; }
		else if constexpr (std::is_same_v<T, mat4>) { return BufferType_Mat4; }
		else if constexpr (std::is_same_v<T, MaterialProperitiesBuffer>) { return BufferType_MaterialProperitiesBuffer; }
		else if constexpr (std::is_same_v<T, MeshPropertiesStruct>) { return BufferType_MeshPropertiesStruct; }
		else if constexpr (std::is_same_v<T, SpriteInstanceStruct>) { return BufferType_SpriteInstanceStruct; }
		else if constexpr (std::is_same_v<T, Vertex2D>) { return BufferType_Vector2D; }
		else 
		{
			throw std::runtime_error("Buffer type doesn't match");
		}
	}

public:
	static int NextBufferId;
	UnorderedMap<int, VulkanBuffer> VulkanBufferMap;

	template<class T>
	int CreateVulkanBuffer(const GraphicsRenderer& renderer, T& bufferData, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
		VkDeviceSize bufferElementSize = sizeof(T);
		uint bufferElementCount = 1;

		int nextBufferId = ++NextBufferId;
		VulkanBufferMap[nextBufferId] = VulkanBuffer_CreateVulkanBuffer(renderer, nextBufferId, static_cast<void*>(&bufferData), bufferElementSize, bufferElementCount, bufferTypeEnum, usage, properties, usingStagingBuffer);
		return NextBufferId;
	}

	template<class T>
	int CreateVulkanBuffer(const GraphicsRenderer& renderer, Vector<T>& bufferData, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
		VkDeviceSize bufferElementSize = sizeof(T);
		uint bufferElementCount = bufferData.size();

		int nextBufferId = ++NextBufferId;
		VulkanBufferMap[nextBufferId] = VulkanBuffer_CreateVulkanBuffer(renderer, nextBufferId, bufferData.data(), bufferElementSize, bufferElementCount, bufferTypeEnum, usage, properties, usingStagingBuffer);
		return NextBufferId;
	}

	template<class T>
	void UpdateBufferMemory(const GraphicsRenderer& renderer, int bufferId, T& bufferData)
	{
		BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
		if (VulkanBufferMap[bufferId].BufferType != bufferTypeEnum)
		{
			throw std::runtime_error("Buffer type doesn't match");
		}

		VkDeviceSize bufferElementSize = sizeof(T);
		uint bufferElementCount = 1;

		VulkanBuffer_UpdateBufferMemory(renderer, VulkanBufferMap[bufferId], static_cast<void*>(&bufferData), bufferElementSize, bufferElementCount);
	}

	template<class T>
	void UpdateBufferMemory(const GraphicsRenderer& renderer, int bufferId, Vector<T>& bufferData)
	{
		BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
		if (VulkanBufferMap[bufferId].BufferType != bufferTypeEnum)
		{
			throw std::runtime_error("Buffer type doesn't match");
		}

		VkDeviceSize bufferElementSize = sizeof(T);
		uint bufferElementCount = bufferData.size();

		VulkanBuffer_UpdateBufferMemory(renderer, VulkanBufferMap[bufferId], bufferData.data(), bufferElementSize, bufferElementCount);
	}

	const VulkanBuffer& FindVulkanBuffer(int id);
	const Vector<VulkanBuffer>& VulkanBufferList();

	void DestroyBuffer(const GraphicsRenderer& renderer, int vulkanBufferId);
	void DestroyAllBuffers();
	
	static VkResult CopyBuffer(const GraphicsRenderer& renderer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
	{
		return Buffer_CopyBuffer(renderer, srcBuffer, dstBuffer, size);
	}
};
extern VulkanBufferSystem bufferSystem;

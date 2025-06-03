#pragma once
#include <Typedef.h>
#include <VulkanBuffer.h>
#include <Mesh.h>
#include "Material.h"
#include "RenderSystem.h"
#include "Vertex.h"
#include "Sprite.h"

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

class VulkanBufferSystem
{
private:
	static int NextBufferId;

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

	UnorderedMap<int, VulkanBufferStruct> VulkanBuffer;

	template<class T>
	int CreateVulkanBuffer(const rendererState& renderer, T& bufferData, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
		VkDeviceSize bufferElementSize = sizeof(T);
		uint bufferElementCount = 1;

		VulkanBuffer[++NextBufferId] = VulkanBuffer_CreateVulkanBuffer(renderer, static_cast<void*>(&bufferData), bufferElementSize, bufferElementCount, bufferTypeEnum, usage, properties, usingStagingBuffer);
		return NextBufferId;
	}

	template<class T>
	int CreateVulkanBuffer(const rendererState& renderer, Vector<T>& bufferData, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
		VkDeviceSize bufferElementSize = sizeof(T);
		uint bufferElementCount = bufferData.size();

		VulkanBuffer[++NextBufferId] = VulkanBuffer_CreateVulkanBuffer(renderer, bufferData.data(), bufferElementSize, bufferElementCount, bufferTypeEnum, usage, properties, usingStagingBuffer);
		return NextBufferId;
	}

	template<class T>
	void UpdateBufferMemory(const rendererState& renderer, int bufferId, T& bufferData)
	{
		BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
		if (VulkanBuffer[bufferId].BufferType != bufferTypeEnum)
		{
			throw std::runtime_error("Buffer type doesn't match");
		}

		VkDeviceSize bufferElementSize = sizeof(T);
		uint bufferElementCount = 1;

		VulkanBuffer_UpdateBufferMemory(renderer, VulkanBuffer[bufferId], static_cast<void*>(&bufferData), bufferElementSize, bufferElementCount);
	}

	template<class T>
	void UpdateBufferMemory(const rendererState& renderer, int bufferId, Vector<T>& bufferData)
	{
		BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
		if (VulkanBuffer[bufferId].BufferType != bufferTypeEnum)
		{
			throw std::runtime_error("Buffer type doesn't match");
		}

		VkDeviceSize bufferElementSize = sizeof(T);
		uint bufferElementCount = bufferData.size();

		VulkanBuffer_UpdateBufferMemory(renderer, VulkanBuffer[bufferId], bufferData.data(), bufferElementSize, bufferElementCount);
	}

	void DestroyBuffer(const RendererState& renderer, int vulkanBufferId);
	static VkResult CopyBuffer(const rendererState& renderer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
	{
		return Buffer_CopyBuffer(renderer, srcBuffer, dstBuffer, size);
	}
};
extern VulkanBufferSystem bufferSystem;

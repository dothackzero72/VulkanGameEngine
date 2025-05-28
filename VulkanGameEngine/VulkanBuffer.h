#pragma once
#include "Typedef.h"
#include "VulkanRenderer.h"
#include <VulkanBufferFuncs.h>
#include "RenderSystem.h"

template <class T>
class VulkanBuffer
{
	VkResult CreateBuffer(const rendererState& renderer, void* bufferData)
	{
		VkResult result = Buffer_CreateBuffer(renderer, &Buffer, &BufferMemory, bufferData, BufferSize, BufferProperties, BufferUsage);
		return result;
	}

	VkResult CreateStagingBuffer(const rendererState& renderer, void* bufferData)
	{
		return Buffer_CreateStagingBuffer(renderer, &StagingBuffer, &Buffer, &StagingBufferMemory, &BufferMemory, bufferData, BufferSize, BufferUsage, BufferProperties);
	}

	VkResult UpdateBufferSize(const rendererState& renderer, VkBuffer buffer, VkDeviceMemory bufferMemory, VkDeviceSize newBufferSize)
	{
		VkResult result = Buffer_UpdateBufferSize(renderer, buffer, &bufferMemory, BufferData, &BufferSize, newBufferSize, BufferUsage, BufferProperties);
		DestroyBuffer(renderer);
		return result;
	}

public:
	VkBuffer Buffer = VK_NULL_HANDLE;
	VkBuffer StagingBuffer = VK_NULL_HANDLE;
	VkDeviceMemory StagingBufferMemory = VK_NULL_HANDLE;
	VkDeviceMemory BufferMemory = VK_NULL_HANDLE;
	VkDeviceSize BufferSize = 0;
	VkBufferUsageFlags BufferUsage;
	VkMemoryPropertyFlags BufferProperties;
	uint64 BufferDeviceAddress = 0;
	VkAccelerationStructureKHR BufferHandle = VK_NULL_HANDLE;
	void* BufferData = nullptr;
	bool IsMapped = false;
	bool UsingStagingBuffer = false;

	VulkanBuffer()
	{

	}

	VulkanBuffer(const rendererState& renderer, VkBuffer& stagingBuffer, VkDeviceMemory& stagingBufferMemory, VkDeviceMemory& bufferMemory, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags bufferProperties)
	{
		StagingBuffer = stagingBuffer;
		StagingBufferMemory = stagingBufferMemory;
		BufferMemory = bufferMemory;
		BufferSize = bufferSize;
		BufferUsage = bufferUsage;
		BufferProperties = bufferProperties;
	}

	VulkanBuffer(const rendererState& renderer, void* bufferData, uint32 bufferElementCount, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		BufferSize = sizeof(T) * bufferElementCount;
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(renderer, bufferData);
		}
		else
		{
			CreateBuffer(renderer, bufferData);
		}
	}

	VulkanBuffer(const rendererState& renderer, T bufferData, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		BufferSize = sizeof(T);
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		void* bufferDataPtr = static_cast<void*>(&bufferData);

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(renderer, bufferDataPtr);
		}
		else
		{
			CreateBuffer(renderer, bufferDataPtr);
		}
	}

	VulkanBuffer(const rendererState& renderer, Vector<T> bufferDataList, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		BufferSize = sizeof(T) * bufferDataList.size();
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		void* bufferDataPtr = static_cast<void*>(bufferDataList.data());

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(renderer, bufferDataPtr);
		}
		else
		{
			CreateBuffer(renderer, bufferDataPtr);
		}
	}

	VulkanBuffer(const rendererState& renderer, Vector<T> bufferDataList, uint reserveCount, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		BufferSize = sizeof(T) * (reserveCount + bufferDataList.size());
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		void* bufferDataPtr = static_cast<void*>(bufferDataList.data());

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(renderer, bufferDataPtr);
		}
		else
		{
			CreateBuffer(renderer, bufferDataPtr);
		}
	}

	~VulkanBuffer()
	{
	}

	static VkResult CopyBuffer(const rendererState& renderer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
	{
		return Buffer_CopyBuffer(renderer, srcBuffer, dstBuffer, size);
	}

	void UpdateBufferMemory(const rendererState& renderer, T& bufferData)
	{
		void* rawBufferData = static_cast<void*>(&bufferData);
		if (UsingStagingBuffer)
		{
			Buffer_UpdateStagingBufferData(renderer, StagingBuffer, Buffer, &StagingBufferMemory, &BufferMemory, rawBufferData, BufferSize);
		}
		else
		{
			Buffer_UpdateBufferData(renderer, &BufferMemory, rawBufferData, BufferSize);
		}
	}

	void UpdateBufferMemory(const rendererState& renderer, Vector<T>& bufferData)
	{
		const VkDeviceSize newBufferSize = sizeof(T) * bufferData.size();
		if (UsingStagingBuffer)
		{
			if (BufferSize != newBufferSize)
			{
				if (UpdateBufferSize(cRenderer, StagingBuffer, StagingBufferMemory, newBufferSize) != VK_SUCCESS)
				{
					RENDERER_ERROR("Failed to update staging buffer size.");
					return;
				}
			}

			Buffer_UpdateStagingBufferData(renderer, StagingBuffer, Buffer, &StagingBufferMemory, &BufferMemory, (void*)bufferData.data(), BufferSize);
		}
		else
		{
			if (BufferSize != newBufferSize)
			{
				if (UpdateBufferSize(cRenderer, Buffer, BufferMemory, newBufferSize) != VK_SUCCESS)
				{
					RENDERER_ERROR("Failed to update buffer size.");
					return;
				}
			}

			VkResult result = Buffer_UpdateBufferMemory(renderer, BufferMemory, (void*)bufferData.data(), newBufferSize);
			if (result != VK_SUCCESS)
			{
				RENDERER_ERROR("Failed to update buffer memory.");
				return;
			}
		}
	}

	void UpdateBufferMemory(const rendererState& renderer, void* bufferData, VkDeviceSize totalBufferSize)
	{
		const VkDeviceSize newBufferSize = totalBufferSize;
		if (UsingStagingBuffer)
		{
			if (BufferSize != newBufferSize)
			{
				if (UpdateBufferSize(StagingBuffer, StagingBufferMemory, newBufferSize) != VK_SUCCESS)
				{
					RENDERER_ERROR("Failed to update staging buffer size.");
					return;
				}
			}

			Buffer_UpdateStagingBufferData(renderer, StagingBuffer, Buffer, &StagingBufferMemory, &BufferMemory, (void*)bufferData, BufferSize);
		}
		else
		{
			if (BufferSize != newBufferSize)
			{
				if (UpdateBufferSize(Buffer, BufferMemory, newBufferSize) != VK_SUCCESS)
				{
					RENDERER_ERROR("Failed to update buffer size.");
					return;
				}
			}

			VkResult result = Buffer_UpdateBufferMemory(renderer, BufferMemory, (void*)bufferData, newBufferSize);
			if (result != VK_SUCCESS)
			{
				RENDERER_ERROR("Failed to update buffer memory.");
				return;
			}
		}
	}

	std::vector<T> CheckBufferContents(const rendererState& renderer)
	{
		std::vector<T> DataList;
		size_t dataListSize = BufferSize / sizeof(T);

		void* data = Buffer_MapBufferMemory(renderer, BufferMemory, BufferSize, &IsMapped);
		if (data == nullptr) {
			std::cerr << "Failed to map buffer memory\n";
			return DataList;
		}

		char* newPtr = static_cast<char*>(data);
		for (size_t x = 0; x < dataListSize; ++x)
		{
			DataList.emplace_back(*reinterpret_cast<T*>(newPtr));
			newPtr += sizeof(T);
		}
		Buffer_UnmapBufferMemory(renderer, BufferMemory, &IsMapped);

		return DataList;
	}

	void DestroyBuffer(const rendererState& renderer)
	{
		Buffer_DestroyBuffer(renderer, &Buffer, &StagingBuffer, &BufferMemory, &StagingBufferMemory, &BufferData, &BufferSize, &BufferUsage, &BufferProperties);
	}
};

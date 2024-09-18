#pragma once
extern "C"
{
#include <CBuffer.h>
}
#include "VulkanRenderer.h"
#include <iostream>
#include <memory>
#include <vector>
#include "Vertex.h"
#include "Typedef.h"

template <class T>
class VulkanBuffer
{
protected:
	VkBuffer StagingBuffer = VK_NULL_HANDLE;
	VkDeviceMemory StagingBufferMemory = VK_NULL_HANDLE;
	VkDeviceMemory BufferMemory = VK_NULL_HANDLE;
	VkDeviceSize BufferSize = 0;
	VkBufferUsageFlags BufferUsage;
	VkMemoryPropertyFlags BufferProperties;
	uint64 BufferDeviceAddress = 0;
	VkAccelerationStructureKHR BufferHandle = VK_NULL_HANDLE;
	void* BufferData;
	bool IsMapped = false;

	virtual VkResult CreateBuffer(void* bufferData, uint32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties)
	{
		BufferSize = bufferSize;
		BufferUsage = usage;
		BufferProperties = properties;
		return Buffer_CreateBuffer(cRenderer.Device, cRenderer.PhysicalDevice, &Buffer, &BufferMemory, bufferData, bufferSize, usage, properties);
	}

	virtual VkResult CreateStagingBuffer(void* bufferData, uint32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties)
	{
		BufferSize = bufferSize;
		BufferUsage = usage;
		BufferProperties = properties;
		return Buffer_CreateStagingBuffer(cRenderer.Device, cRenderer.PhysicalDevice, &StagingBuffer, &StagingBufferMemory, bufferData, bufferSize, usage, properties);
	}

	VkResult UpdateBufferSize(VkDeviceSize newBufferSize)
	{
		VkResult result = Buffer_UpdateBufferSize(cRenderer.Device, cRenderer.PhysicalDevice, Buffer, &BufferMemory, BufferData, &BufferSize, newBufferSize, BufferUsage, BufferProperties);
		DestroyBuffer();
		return result;
	}

public:
	VkBuffer Buffer = VK_NULL_HANDLE;
	VkDescriptorBufferInfo DescriptorBufferInfo;

	VulkanBuffer()
	{
	}

	VulkanBuffer(void* bufferData, uint32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties)
	{
		BufferData = bufferData;
		CreateBuffer(bufferData, bufferSize, usage, properties);
	}

	virtual ~VulkanBuffer()
	{
	}

	static VkResult CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
	{
		return Buffer_CopyBuffer(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, srcBuffer, dstBuffer, size);
	}

	virtual void UpdateBufferData(T& bufferData)
	{
		if (BufferSize < sizeof(T))
		{
			RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
			return;
		}
		Buffer_UpdateBufferMemory(cRenderer.Device, BufferMemory, static_cast<void*>(&bufferData), sizeof(T));
	}

	virtual void UpdateBufferData(List<T>& bufferData)
	{
		const VkDeviceSize newBufferSize = sizeof(T) * bufferData.size();
		if (BufferSize != newBufferSize)
		{
			if (UpdateBufferSize(newBufferSize) != VK_SUCCESS)
			{
				RENDERER_ERROR("Failed to update buffer size.");
				return;
			}
		}

		if (!IsMapped)
		{
			RENDERER_ERROR("Buffer is not mapped! Cannot update data.");
			return;
		}

		Buffer_UpdateBufferMemory(cRenderer.Device, BufferMemory, static_cast<void*>(bufferData.data()), newBufferSize);
	}

	virtual void UpdateBufferData(void* bufferData, VkDeviceSize bufferListCount)
	{
		const VkDeviceSize newBufferSize = sizeof(T) * bufferListCount;
		if (BufferSize != newBufferSize)
		{
			if (UpdateBufferSize(newBufferSize) != VK_SUCCESS)
			{
				RENDERER_ERROR("Failed to update buffer size.");
				return;
			}
		}

		if (!IsMapped)
		{
			RENDERER_ERROR("Buffer is not mapped! Cannot update data.");
			return;
		}

		Buffer_UpdateBufferMemory(cRenderer.Device, BufferMemory, static_cast<void*>(&bufferData), newBufferSize);
	}

	virtual void UpdateBufferData(void* bufferData)
	{
		if (BufferSize < sizeof(T))
		{
			RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
			return;
		}
		Buffer_UpdateBufferMemory(cRenderer.Device, BufferMemory, bufferData, sizeof(T));
	}

	std::vector<T> CheckBufferContents()
	{
		std::vector<T> DataList;
		size_t dataListSize = BufferSize / sizeof(T);

		void* data = Buffer_MapBufferMemory(cRenderer.Device, BufferMemory, BufferSize, *IsMapped);
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
		Buffer_UnmapBufferMemory(cRenderer.Device, BufferMemory, *IsMapped);

		return DataList;
	}

	VkDescriptorBufferInfo* GetDescriptorbuffer()
	{
		DescriptorBufferInfo = VkDescriptorBufferInfo
		{
			.buffer = Buffer,
			.offset = 0,
			.range = VK_WHOLE_SIZE
		};
		return &DescriptorBufferInfo;
	}

	void DestroyBuffer()
	{
		Buffer_DestroyBuffer(cRenderer.Device, &Buffer, &BufferMemory, &BufferData, &BufferSize, &BufferUsage, &BufferProperties);
	}

	VkBuffer GetBuffer() { return Buffer; }
	VkBuffer* GetBufferPtr() { return &Buffer; }
	VkDeviceMemory GetBufferMemory() { return BufferMemory; }
	VkDeviceMemory* GetBufferMemoryPtr() { return &BufferMemory; }
	VkDeviceSize GetBufferSize() { return BufferSize; }
	uint64 GetBufferDeviceAddress() { return BufferDeviceAddress; }
	VkAccelerationStructureKHR GetBufferHandle() { return BufferHandle; }
	VkAccelerationStructureKHR* GetBufferHandlePtr() { return &BufferHandle; }
};

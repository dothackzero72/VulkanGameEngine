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
private:
	std::shared_ptr<VkDevice> _device;
	std::shared_ptr<VkPhysicalDevice> _physicalDevice;
	std::shared_ptr<VkCommandPool> _commandPool;
	std::shared_ptr<VkQueue> _graphicsQueue;

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
	bool IsStagingBuffer = false;

	VkResult CreateBuffer(void* bufferData)
	{
		VkResult result = Buffer_CreateBuffer(*_device.get(), *_physicalDevice.get(), &Buffer, &BufferMemory, bufferData, BufferSize, BufferProperties, BufferUsage);
		return result;
	}

	VkResult CreateStagingBuffer(void* bufferData)
	{
		return Buffer_CreateStagingBuffer(*_device.get(), *_physicalDevice.get(), *_commandPool.get(), *_graphicsQueue.get(), &StagingBuffer, &Buffer, &StagingBufferMemory, &BufferMemory, bufferData, BufferSize, BufferUsage, BufferProperties);
	}

	VkResult UpdateBufferSize(VkBuffer buffer, VkDeviceMemory bufferMemory, VkDeviceSize newBufferSize)
	{
		VkResult result = Buffer_UpdateBufferSize(*_device.get(), *_physicalDevice.get(), buffer, &bufferMemory, BufferData, &BufferSize, newBufferSize, BufferUsage, BufferProperties);
		DestroyBuffer();
		return result;
	}

public:
	VkBuffer Buffer = VK_NULL_HANDLE;
	VkDescriptorBufferInfo DescriptorBufferInfo;

	VulkanBuffer()
	{
		_device = std::make_shared<VkDevice>(cRenderer.Device);
		_physicalDevice = std::make_shared<VkPhysicalDevice>(cRenderer.PhysicalDevice);
		_commandPool = std::make_shared<VkCommandPool>(cRenderer.CommandPool);
		_graphicsQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.GraphicsQueue);
	}

	VulkanBuffer(void* bufferData, uint32 bufferElementCount, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool isStagingBuffer)
	{
		_device = std::make_shared<VkDevice>(cRenderer.Device);
		_physicalDevice = std::make_shared<VkPhysicalDevice>(cRenderer.PhysicalDevice);
		_commandPool = std::make_shared<VkCommandPool>(cRenderer.CommandPool);
		_graphicsQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.GraphicsQueue);

		BufferSize = sizeof(T) * bufferElementCount;
		BufferProperties = properties;
		IsStagingBuffer = isStagingBuffer;
		BufferUsage = usage;

		if (isStagingBuffer)
		{
			CreateStagingBuffer(bufferData);
		}
		else
		{
			CreateBuffer(bufferData);
		}
	}

	virtual ~VulkanBuffer()
	{
	}

	static VkResult CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
	{
		return Buffer_CopyBuffer(*_device.get(), *_commandPool.get(), cRenderer.SwapChain.GraphicsQueue, srcBuffer, dstBuffer, size);
	}

	void UpdateBufferData(T& bufferData)
	{
		Buffer_UpdateBufferData(*_device.get(), &BufferMemory, static_cast<void*>(&bufferData), BufferSize, IsStagingBuffer);
	}

	void UpdateBufferData(T& bufferData, VkDeviceMemory bufferMemory)
	{
		Buffer_UpdateBufferData(*_device.get(), &BufferMemory, static_cast<void*>(&bufferData), BufferSize, IsStagingBuffer);
	}

	void UpdateBufferData(List<T>& bufferData, VkDeviceMemory bufferMemory)
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

		Buffer_UpdateBufferData(*_device.get(), &StagingBufferMemory, &BufferMemory, static_cast<void*>(bufferData.data()), BufferSize, IsStagingBuffer);
	}

	void UpdateBufferData(void* bufferData, VkDeviceSize bufferListCount, VkDeviceMemory bufferMemory)
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

		Buffer_UpdateBufferData(*_device.get(), &StagingBufferMemory, &BufferMemory, static_cast<void*>(&bufferData), BufferSize, IsStagingBuffer);
	}

	void UpdateBufferData(void* bufferData, VkDeviceMemory bufferMemory)
	{
		Buffer_UpdateBufferData(*_device.get(), &StagingBufferMemory, &BufferMemory, bufferData, BufferSize, IsStagingBuffer);
	}

	void UpdateBufferData(void* bufferData)
	{
		if (BufferSize < sizeof(T))
		{
			RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
			return;
		}

		Buffer_UpdateBufferData(*_device.get(), &StagingBufferMemory, &BufferMemory, bufferData, BufferSize, IsStagingBuffer);
	}

	std::vector<T> CheckBufferContents()
	{
		std::vector<T> DataList;
		size_t dataListSize = BufferSize / sizeof(T);

		void* data = Buffer_MapBufferMemory(_device.get(), BufferMemory, BufferSize, *IsMapped);
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
		Buffer_UnmapBufferMemory(_device.get(), BufferMemory, *IsMapped);

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
		Buffer_DestroyBuffer(*_device.get(), &Buffer, &StagingBuffer, &BufferMemory, &StagingBufferMemory, &BufferData, &BufferSize, &BufferUsage, &BufferProperties);
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

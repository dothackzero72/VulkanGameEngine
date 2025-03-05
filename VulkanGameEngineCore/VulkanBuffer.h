#pragma once
extern "C"
{
	#include <CBuffer.h>
}

#include <iostream>
#include <memory>
#include <vector>
#include "Typedef.h"
#include "VulkanRenderer.h"

template <class T>
class VulkanBuffer
{
private:
	SharedPtr<VkDevice> _device;
	SharedPtr<VkPhysicalDevice> _physicalDevice;
	SharedPtr<VkCommandPool> _commandPool;
	SharedPtr<VkQueue> _graphicsQueue;

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
	bool UsingStagingBuffer = false;

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

	VulkanBuffer(VkBuffer& stagingBuffer, VkDeviceMemory& stagingBufferMemory, VkDeviceMemory& bufferMemory, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags bufferProperties)
	{
		_device = std::make_shared<VkDevice>(cRenderer.Device);
		_physicalDevice = std::make_shared<VkPhysicalDevice>(cRenderer.PhysicalDevice);
		_commandPool = std::make_shared<VkCommandPool>(cRenderer.CommandPool);
		_graphicsQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.GraphicsQueue);

		StagingBuffer = stagingBuffer;
		StagingBufferMemory = stagingBufferMemory;
		BufferMemory = bufferMemory;
		BufferSize = bufferSize;
		BufferUsage = bufferUsage;
		BufferProperties = bufferProperties;
	}

	VulkanBuffer(void* bufferData, uint32 bufferElementCount, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		_device = std::make_shared<VkDevice>(cRenderer.Device);
		_physicalDevice = std::make_shared<VkPhysicalDevice>(cRenderer.PhysicalDevice);
		_commandPool = std::make_shared<VkCommandPool>(cRenderer.CommandPool);
		_graphicsQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.GraphicsQueue);

		BufferSize = sizeof(T) * bufferElementCount;
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(bufferData);
		}
		else
		{
			CreateBuffer(bufferData);
		}
	}

	VulkanBuffer(T bufferData, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		_device = std::make_shared<VkDevice>(cRenderer.Device);
		_physicalDevice = std::make_shared<VkPhysicalDevice>(cRenderer.PhysicalDevice);
		_commandPool = std::make_shared<VkCommandPool>(cRenderer.CommandPool);
		_graphicsQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.GraphicsQueue);

		BufferSize = sizeof(T);
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		void* bufferData2 = static_cast<void*>(&bufferData);

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(bufferData2);
		}
		else
		{
			CreateBuffer(bufferData2);
		}
	}

	VulkanBuffer(Vector<T> bufferDataList, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		_device = std::make_shared<VkDevice>(cRenderer.Device);
		_physicalDevice = std::make_shared<VkPhysicalDevice>(cRenderer.PhysicalDevice);
		_commandPool = std::make_shared<VkCommandPool>(cRenderer.CommandPool);
		_graphicsQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.GraphicsQueue);

		BufferSize = sizeof(T) * bufferDataList.size();
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		void* bufferData2 = static_cast<void*>(bufferDataList.data());

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(bufferData2);
		}
		else
		{
			CreateBuffer(bufferData2);
		}
	}

	VulkanBuffer(Vector<T> bufferDataList, uint reserveCount, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		_device = std::make_shared<VkDevice>(cRenderer.Device);
		_physicalDevice = std::make_shared<VkPhysicalDevice>(cRenderer.PhysicalDevice);
		_commandPool = std::make_shared<VkCommandPool>(cRenderer.CommandPool);
		_graphicsQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.GraphicsQueue);

		BufferSize = sizeof(T) * (reserveCount + bufferDataList.size());
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		void* bufferData2 = static_cast<void*>(bufferDataList.data());

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(bufferData2);
		}
		else
		{
			CreateBuffer(bufferData2);
		}
	}

	virtual ~VulkanBuffer()
	{
	}

	static VkResult CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
	{
		return Buffer_CopyBuffer(*_device.get(), *_commandPool.get(), cRenderer.SwapChain.GraphicsQueue, srcBuffer, dstBuffer, size);
	}

	void UpdateBufferMemory(T& bufferData)
	{
		void* rawBufferData = static_cast<void*>(&bufferData);
		if (UsingStagingBuffer)
		{
			Buffer_UpdateStagingBufferData(*_device.get(), *_commandPool.get(), *_graphicsQueue.get(), StagingBuffer, Buffer, &StagingBufferMemory, &BufferMemory, rawBufferData, BufferSize);
		}
		else
		{
			Buffer_UpdateBufferData(*_device.get(), &BufferMemory, rawBufferData, BufferSize);
		}
	}

	void UpdateBufferMemory(Vector<T>& bufferData)
	{
		const VkDeviceSize newBufferSize = sizeof(T) * bufferData.size();
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

			Buffer_UpdateStagingBufferData(*_device.get(), *_commandPool.get(), *_graphicsQueue.get(), StagingBuffer, Buffer, &StagingBufferMemory, &BufferMemory, (void*)bufferData.data(), BufferSize);
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

			VkResult result = Buffer_UpdateBufferMemory(*_device.get(), BufferMemory, (void*)bufferData.data(), newBufferSize);
			if (result != VK_SUCCESS)
			{
				RENDERER_ERROR("Failed to update buffer memory.");
				return;
			}
		}
	}

	void UpdateBufferMemory(void* bufferData, uint32 totalBufferSize)
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

			Buffer_UpdateStagingBufferData(*_device.get(), *_commandPool.get(), *_graphicsQueue.get(), StagingBuffer, Buffer, &StagingBufferMemory, &BufferMemory, (void*)bufferData.data(), BufferSize);
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

			VkResult result = Buffer_UpdateBufferMemory(*_device.get(), BufferMemory, (void*)bufferData.data(), newBufferSize);
			if (result != VK_SUCCESS)
			{
				RENDERER_ERROR("Failed to update buffer memory.");
				return;
			}
		}
	}

	std::vector<T> CheckBufferContents()
	{
		std::vector<T> DataList;
		size_t dataListSize = BufferSize / sizeof(T);

		void* data = Buffer_MapBufferMemory(*_device.get(), BufferMemory, BufferSize, &IsMapped);
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
		Buffer_UnmapBufferMemory(*_device.get(), BufferMemory, &IsMapped);

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

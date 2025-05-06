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
		VkResult result = Buffer_CreateBuffer(cRenderer.Device, cRenderer.PhysicalDevice, &Buffer, &BufferMemory, bufferData, BufferSize, BufferProperties, BufferUsage);
		return result;
	}

	VkResult CreateStagingBuffer(void* bufferData)
	{
		return Buffer_CreateStagingBuffer(cRenderer.Device, cRenderer.PhysicalDevice, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, &StagingBuffer, &Buffer, &StagingBufferMemory, &BufferMemory, bufferData, BufferSize, BufferUsage, BufferProperties);
	}

	VkResult UpdateBufferSize(VkBuffer buffer, VkDeviceMemory bufferMemory, VkDeviceSize newBufferSize)
	{
		VkResult result = Buffer_UpdateBufferSize(cRenderer.Device, cRenderer.PhysicalDevice, buffer, &bufferMemory, BufferData, &BufferSize, newBufferSize, BufferUsage, BufferProperties);
		DestroyBuffer();
		return result;
	}

public:
	VkBuffer Buffer = VK_NULL_HANDLE;

	VulkanBuffer()
	{

	}

	VulkanBuffer(VkBuffer& stagingBuffer, VkDeviceMemory& stagingBufferMemory, VkDeviceMemory& bufferMemory, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags bufferProperties)
	{
		StagingBuffer = stagingBuffer;
		StagingBufferMemory = stagingBufferMemory;
		BufferMemory = bufferMemory;
		BufferSize = bufferSize;
		BufferUsage = bufferUsage;
		BufferProperties = bufferProperties;
	}

	VulkanBuffer(void* bufferData, uint32 bufferElementCount, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
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
		BufferSize = sizeof(T);
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		void* bufferDataPtr = static_cast<void*>(&bufferData);

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(bufferDataPtr);
		}
		else
		{
			CreateBuffer(bufferDataPtr);
		}
	}

	VulkanBuffer(Vector<T> bufferDataList, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		BufferSize = sizeof(T) * bufferDataList.size();
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		void* bufferDataPtr = static_cast<void*>(bufferDataList.data());

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(bufferDataPtr);
		}
		else
		{
			CreateBuffer(bufferDataPtr);
		}
	}

	VulkanBuffer(Vector<T> bufferDataList, uint reserveCount, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool usingStagingBuffer)
	{
		BufferSize = sizeof(T) * (reserveCount + bufferDataList.size());
		BufferProperties = properties;
		UsingStagingBuffer = usingStagingBuffer;
		BufferUsage = usage;

		void* bufferDataPtr = static_cast<void*>(bufferDataList.data());

		if (UsingStagingBuffer)
		{
			CreateStagingBuffer(bufferDataPtr);
		}
		else
		{
			CreateBuffer(bufferDataPtr);
		}
	}

	~VulkanBuffer()
	{
	}

	static VkResult CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
	{
		return Buffer_CopyBuffer(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, srcBuffer, dstBuffer, size);
	}

	void UpdateBufferMemory(T& bufferData)
	{
		void* rawBufferData = static_cast<void*>(&bufferData);
		if (UsingStagingBuffer)
		{
			Buffer_UpdateStagingBufferData(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, StagingBuffer, Buffer, &StagingBufferMemory, &BufferMemory, rawBufferData, BufferSize);
		}
		else
		{
			Buffer_UpdateBufferData(cRenderer.Device, &BufferMemory, rawBufferData, BufferSize);
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

			Buffer_UpdateStagingBufferData(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, StagingBuffer, Buffer, &StagingBufferMemory, &BufferMemory, (void*)bufferData.data(), BufferSize);
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

			VkResult result = Buffer_UpdateBufferMemory(cRenderer.Device, BufferMemory, (void*)bufferData.data(), newBufferSize);
			if (result != VK_SUCCESS)
			{
				RENDERER_ERROR("Failed to update buffer memory.");
				return;
			}
		}
	}

	void UpdateBufferMemory(void* bufferData, VkDeviceSize totalBufferSize)
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

			Buffer_UpdateStagingBufferData(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, StagingBuffer, Buffer, &StagingBufferMemory, &BufferMemory, (void*)bufferData, BufferSize);
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

			VkResult result = Buffer_UpdateBufferMemory(cRenderer.Device, BufferMemory, (void*)bufferData, newBufferSize);
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

		void* data = Buffer_MapBufferMemory(cRenderer.Device, BufferMemory, BufferSize, &IsMapped);
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
		Buffer_UnmapBufferMemory(cRenderer.Device, BufferMemory, &IsMapped);

		return DataList;
	}

	void DestroyBuffer()
	{
		Buffer_DestroyBuffer(cRenderer.Device, &Buffer, &StagingBuffer, &BufferMemory, &StagingBufferMemory, &BufferData, &BufferSize, &BufferUsage, &BufferProperties);
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

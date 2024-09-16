#pragma once
#include "VulkanBuffer.h"

template <class T>
class DynamicVulkanBuffer : public VulkanBuffer<T>
{
private:

	virtual void UpdateBufferData(const T& bufferData) override
	{
		if (VulkanBuffer<T>::BufferSize < sizeof(T))
		{
			RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
			return;
		}
		Buffer_UpdateStagingBufferMemory(VulkanBuffer<T>::SendCBufferInfo().get(), renderer.Device, const_cast<void*>(static_cast<const void*>(&bufferData)), sizeof(T));
	}

	virtual void UpdateBufferData(const List<T>& bufferData) override
	{
		const VkDeviceSize newBufferSize = sizeof(T) * bufferData.size();
		if (VulkanBuffer<T>::BufferSize != newBufferSize)
		{
			if (VulkanBuffer<T>::UpdateBufferSize(newBufferSize) != VK_SUCCESS)
			{
				RENDERER_ERROR("Failed to update buffer size.");
				return;
			}
		}

		if (!VulkanBuffer<T>::IsMapped)
		{
			RENDERER_ERROR("Buffer is not mapped! Cannot update data.");
			return;
		}

		Buffer_UpdateStagingBufferMemory(VulkanBuffer<T>::SendCBufferInfo().get(), renderer.Device, const_cast<void*>(static_cast<const void*>(bufferData.data())), newBufferSize);
	}

	virtual void UpdateBufferData(void* bufferData) override
	{
		const VkDeviceSize newBufferSize = sizeof(T);
		if (VulkanBuffer<T>::BufferSize != newBufferSize)
		{
			if (VulkanBuffer<T>::UpdateBufferSize(newBufferSize) != VK_SUCCESS)
			{
				RENDERER_ERROR("Failed to update buffer size.");
				return;
			}
		}

		if (!VulkanBuffer<T>::IsMapped)
		{
			RENDERER_ERROR("Buffer is not mapped! Cannot update data.");
			return;
		}

		Buffer_UpdateStagingBufferMemory(VulkanBuffer<T>::SendCBufferInfo().get(), renderer.Device, bufferData, newBufferSize);
	}

	virtual VkResult CopyStagingBuffer() 
	{
		VkBuffer stagingBuffer = VulkanBuffer<T>::StagingBuffer;
		VkBuffer buffer = VulkanBuffer<T>::Buffer;
		VkDeviceSize bufferSize = VulkanBuffer<T>::BufferSize;
		return Buffer_CopyBuffer(&stagingBuffer, &buffer, bufferSize);
	}

	virtual VkResult CopyStagingBuffer(VkCommandBuffer commandBuffer)
	{
		VkBuffer stagingBuffer = VulkanBuffer<T>::StagingBuffer;
		VkBuffer buffer = VulkanBuffer<T>::Buffer;
		VkDeviceSize bufferSize = VulkanBuffer<T>::BufferSize;
		return Buffer_RenderPassCopyBuffer(&commandBuffer, &stagingBuffer, &buffer, bufferSize);
	}

public:

	DynamicVulkanBuffer() : VulkanBuffer<T>::VulkanBuffer()
	{
	}

	DynamicVulkanBuffer(void* bufferData, uint32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties): VulkanBuffer<T>::VulkanBuffer(bufferData, bufferSize, usage, properties) 
	{
		CreateStagingBuffer(bufferData);
		//CopyStagingBuffer();
	}

	VkResult CreateStagingBuffer(void* bufferData) override
	{
		return VulkanBuffer<T>::CreateStagingBuffer(bufferData);
	}

	virtual void UpdateBuffer(VkCommandBuffer& commandBuffer, void* data)
	{
		UpdateBufferData(data);
		CopyStagingBuffer(commandBuffer);
	}
};
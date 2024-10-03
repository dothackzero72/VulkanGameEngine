#pragma once
#include "VulkanBuffer.h"
//
//template <class T>
//class DynamicVulkanBuffer : public VulkanBuffer<T>
//{
//private:
//
//	virtual VkResult CreateBuffer(void* bufferData, uint32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties) override
//	{
//		return VulkanBuffer<T>::CreateStagingBuffer(bufferData, bufferSize, usage, properties);
//	}
//
//	virtual void UpdateBufferData(T& bufferData) override
//	{
//		if (VulkanBuffer<T>::BufferSize < sizeof(T))
//		{
//			RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
//			return;
//		}
//		Buffer_UpdateStagingBufferMemory(cRenderer.Device, VulkanBuffer<T>::StagingBufferMemory, static_cast<void*>(&bufferData), sizeof(T));
//	}
//
//	virtual void UpdateBufferData(List<T>& bufferData) override
//	{
//		const VkDeviceSize newBufferSize = sizeof(T) * bufferData.size();
//		if (VulkanBuffer<T>::BufferSize != newBufferSize)
//		{
//			if (VulkanBuffer<T>::UpdateBufferSize(newBufferSize) != VK_SUCCESS)
//			{
//				RENDERER_ERROR("Failed to update buffer size.");
//				return;
//			}
//		}
//
//		if (!VulkanBuffer<T>::IsMapped)
//		{
//			RENDERER_ERROR("Buffer is not mapped! Cannot update data.");
//			return;
//		}
//
//		Buffer_UpdateStagingBufferMemory(cRenderer.Device, VulkanBuffer<T>::StagingBufferMemory, static_cast<void*>(bufferData.data()), newBufferSize);
//	}
//
//	virtual void UpdateBufferData(void* bufferData, VkDeviceSize bufferListCount) override
//	{
//		const VkDeviceSize newBufferSize = sizeof(T) * bufferListCount;
//		if (VulkanBuffer<T>::BufferSize != newBufferSize)
//		{
//			if (VulkanBuffer<T>::UpdateBufferSize(newBufferSize) != VK_SUCCESS)
//			{
//				RENDERER_ERROR("Failed to update buffer size.");
//				return;
//			}
//		}
//
//		if (!VulkanBuffer<T>::IsMapped)
//		{
//			RENDERER_ERROR("Buffer is not mapped! Cannot update data.");
//			return;
//		}
//
//		Buffer_UpdateStagingBufferMemory(cRenderer.Device, VulkanBuffer<T>::StagingBufferMemory, bufferData, newBufferSize);
//	}
//
//	virtual void UpdateBufferData(void* bufferData) override
//	{
//		VulkanBuffer<T>::UpdateBufferData(bufferData);
//	}
//
//	virtual VkResult CopyStagingBuffer(VkCommandBuffer& commandBuffer)
//	{
//		VkBuffer stagingBuffer = VulkanBuffer<T>::StagingBuffer;
//		VkBuffer buffer = VulkanBuffer<T>::Buffer;
//		VkDeviceSize bufferSize = VulkanBuffer<T>::BufferSize;
//		return Buffer_CopyStagingBuffer(&commandBuffer, &stagingBuffer, &buffer, bufferSize);
//	}
//
//public:
//
//	DynamicVulkanBuffer() : VulkanBuffer<T>::VulkanBuffer()
//	{
//	}
//
//	DynamicVulkanBuffer(void* bufferData, uint32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties) : VulkanBuffer<T>::VulkanBuffer(bufferData, bufferSize, usage, properties)
//	{
//		CreateBuffer(bufferData, bufferSize, usage, properties);
//	}
//
//	virtual void UpdateBuffer(VkCommandBuffer& commandBuffer, void* data)
//	{
//		UpdateBufferData(data);
//		CopyStagingBuffer(commandBuffer);
//	}
//};
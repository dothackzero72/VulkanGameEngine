#pragma once
extern "C"
{
#include <VulkanRenderer.h>
	#include <CBuffer.h>
}
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

		std::unique_ptr<VulkanBufferInfo> SendCBufferInfo()
		{
			std::unique_ptr<VulkanBufferInfo>  bufferInfo = std::make_unique<VulkanBufferInfo>();
			bufferInfo->Buffer = &Buffer;
			bufferInfo->StagingBuffer = &StagingBuffer;
			bufferInfo->BufferMemory = &BufferMemory;
			bufferInfo->StagingBufferMemory = &StagingBufferMemory;
			bufferInfo->BufferSize = &BufferSize;
			bufferInfo->BufferUsage = &BufferUsage;
			bufferInfo->BufferProperties = &BufferProperties;
			bufferInfo->BufferDeviceAddress = &BufferDeviceAddress;
			bufferInfo->BufferHandle = &BufferHandle;
			bufferInfo->BufferData = &BufferData;
			bufferInfo->IsMapped = &IsMapped;
			return bufferInfo;
		}

		VkResult UpdateBufferSize(VkDeviceSize bufferSize)
		{
			BufferSize = bufferSize;
			return Buffer_UpdateBufferSize(SendCBufferInfo().get(), bufferSize);
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
			return Buffer_CopyBuffer(SendCBufferInfo().get(), srcBuffer, dstBuffer, size);
		}

		virtual VkResult CreateBuffer(void* bufferData, uint32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties)
		{
			return Buffer_CreateBuffer(SendCBufferInfo().get(), bufferData, bufferSize, usage, properties);
		}

		virtual void UpdateBufferData(const T& bufferData)
		{
			if (BufferSize < sizeof(T)) 
			{
				RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
				return;
			}
			Buffer_UpdateBufferMemory(SendCBufferInfo().get(), const_cast<void*>(static_cast<const void*>(&bufferData)), sizeof(T));
		}

		virtual void UpdateBufferData(const List<T>& bufferData)
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

			Buffer_UpdateBufferMemory(SendCBufferInfo().get(), const_cast<void*>(static_cast<const void*>(bufferData.data())), newBufferSize);
		}

		virtual void UpdateBufferData(void* bufferData)
		{
			if (BufferSize < sizeof(T))
			{
				RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
				return;
			}
			Buffer_UpdateBufferMemory(SendCBufferInfo().get(), bufferData, sizeof(T));
		}

		std::vector<T> CheckBufferContents() 
		{
			std::vector<T> DataList; 
			size_t dataListSize = BufferSize / sizeof(T);

			void* data = Buffer_MapBufferMemory(SendCBufferInfo().get());
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
			Buffer_UnmapBufferMemory(SendCBufferInfo().get());

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
			Buffer_DestroyBuffer(SendCBufferInfo().get());
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


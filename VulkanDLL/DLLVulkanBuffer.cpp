#include "DLLVulkanBuffer.h"

VkResult DLL_Buffer_AllocateMemory(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* bufferData, VkDeviceMemory* bufferMemory, VkMemoryPropertyFlags properties)
{
    return Buffer_AllocateMemory(device, physicalDevice, bufferData, bufferMemory, properties);
}

VkResult DLL_Buffer_CreateBuffer(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    return Buffer_CreateBuffer(device, physicalDevice, buffer, bufferMemory, bufferData, bufferSize, bufferUsage, properties);
}

VkResult DLL_Buffer_CreateStagingBuffer(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* stagingBuffer, VkDeviceMemory* stagingBufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    return Buffer_CreateStagingBuffer(device, physicalDevice, stagingBuffer, stagingBufferMemory, bufferData, bufferSize, bufferUsage, properties);
}

VkResult DLL_Buffer_CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
{
    return Buffer_CopyBuffer(srcBuffer, dstBuffer, size);
}

VkResult DLL_Buffer_CopyStagingBuffer(VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
{
    return Buffer_CopyStagingBuffer(commandBuffer, srcBuffer, dstBuffer, size);
}

VkResult DLL_Buffer_UpdateBufferSize(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize* oldBufferSize, VkDeviceSize newBufferSize, VkBufferUsageFlags bufferUsageFlags, VkMemoryPropertyFlags propertyFlags)
{
    return Buffer_UpdateBufferSize(device, physicalDevice, buffer, bufferMemory, bufferData, oldBufferSize, newBufferSize, bufferUsageFlags, propertyFlags);
}

VkResult DLL_Buffer_UpdateBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, void* dataToCopy, VkDeviceSize bufferSize)
{
    return Buffer_UpdateBufferMemory(device, bufferMemory, dataToCopy, bufferSize);
}

VkResult DLL_Buffer_UpdateStagingBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, void* dataToCopy, VkDeviceSize bufferSize)
{
    return Buffer_UpdateStagingBufferMemory(device, bufferMemory, dataToCopy, bufferSize);
}

VkResult DLL_Buffer_UnmapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, bool* isMapped)
{
    return Buffer_UnmapBufferMemory(device, bufferMemory, isMapped);
}

void* DLL_Buffer_MapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, VkDeviceSize bufferSize, bool* isMapped)
{
    return Buffer_MapBufferMemory(device, bufferMemory, bufferSize, isMapped);
}

void DLL_Buffer_DestroyBuffer(VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize* bufferSize, VkBufferUsageFlags* bufferUsageFlags, VkMemoryPropertyFlags* propertyFlags)
{
    Buffer_DestroyBuffer(buffer, bufferMemory, bufferData, bufferSize, bufferUsageFlags, propertyFlags);
}

int DLL_BUFFER_BufferTest()
{
    return 45;
}
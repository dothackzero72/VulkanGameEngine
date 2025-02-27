#include "VulkanBufferDLL.h"

VkResult DLL_Buffer_AllocateMemory(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* bufferData, VkDeviceMemory* bufferMemory, VkMemoryPropertyFlags properties)
{
    return Buffer_AllocateMemory(device, physicalDevice, bufferData, bufferMemory, properties);
}

void* DLL_Buffer_MapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, VkDeviceSize bufferSize, bool* isMapped)
{
    return Buffer_MapBufferMemory(device, bufferMemory, bufferSize, isMapped);
}

VkResult DLL_Buffer_UnmapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, bool* isMapped)
{
    return Buffer_UnmapBufferMemory(device, bufferMemory, isMapped);
}

VkResult DLL_Buffer_CreateBuffer(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkMemoryPropertyFlags properties, VkBufferUsageFlags usage)
{
    return Buffer_CreateBuffer(device, physicalDevice, buffer, bufferMemory, bufferData, bufferSize, properties, usage);
}

VkResult DLL_Buffer_CreateStagingBuffer(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkBuffer* stagingBuffer, VkBuffer* buffer, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    return Buffer_CreateStagingBuffer(device, physicalDevice, commandPool, graphicsQueue, stagingBuffer, buffer, stagingBufferMemory, bufferMemory, bufferData, bufferSize, bufferUsage, properties);
}

VkResult DLL_Buffer_CopyBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
{
    return Buffer_CopyBuffer(device, commandPool, graphicsQueue, srcBuffer, dstBuffer, size);
}

VkResult DLL_Buffer_UpdateBufferSize(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize* oldBufferSize, VkDeviceSize newBufferSize, VkBufferUsageFlags bufferUsageFlags, VkMemoryPropertyFlags propertyFlags)
{
    return Buffer_UpdateBufferSize(device, physicalDevice, buffer, bufferMemory, bufferData, oldBufferSize, newBufferSize, bufferUsageFlags, propertyFlags);
}

void DLL_Buffer_UpdateBufferData(VkDevice device, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize)
{
     Buffer_UpdateBufferData( device, bufferMemory,  dataToCopy,  bufferSize);
}

VkResult DLL_Buffer_DestroyBuffer(VkDevice device, VkBuffer* buffer, VkBuffer* stagingBuffer, VkDeviceMemory* bufferMemory, VkDeviceMemory* stagingBufferMemory, void* bufferData, VkDeviceSize* bufferSize, VkBufferUsageFlags* bufferUsageFlags, VkMemoryPropertyFlags* propertyFlags)
{
    return Buffer_DestroyBuffer(device, buffer, stagingBuffer, bufferMemory, stagingBufferMemory, bufferData, bufferSize, bufferUsageFlags, propertyFlags);
}

int DLL_BUFFER_BufferTest()
{
    return 45;
}

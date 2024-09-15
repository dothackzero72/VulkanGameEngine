#include "DLLVulkanBuffer.h"

VkResult DLL_Buffer_CreateBuffer(VulkanBufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    return Buffer_CreateBuffer(bufferInfo, bufferData, bufferSize, bufferUsage, properties);
}

VkResult DLL_Buffer_CreateStagingBuffer(VulkanBufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    return Buffer_CreateStagingBuffer(bufferInfo, bufferData, bufferSize, bufferUsage, properties);
}

VkResult DLL_Buffer_CopyBuffer(VulkanBufferInfo* bufferInfo, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
{
    return Buffer_CopyBuffer(bufferInfo, srcBuffer, dstBuffer, size);
}

VkResult DLL_Buffer_CopyStagingBuffer(VulkanBufferInfo* bufferInfo, VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
{
    return Buffer_CopyStagingBuffer(bufferInfo, commandBuffer, srcBuffer, dstBuffer, size);
}

VkResult DLL_Buffer_UpdateBufferSize(VulkanBufferInfo* bufferInfo, VkDeviceSize bufferSize)
{
    return Buffer_UpdateBufferSize(bufferInfo, bufferSize);
}

 VkResult DLL_Buffer_UnmapBufferMemory(VulkanBufferInfo* bufferInfo)
 {
    return Buffer_UnmapBufferMemory(bufferInfo);
}

 VkResult DLL_Buffer_UpdateBufferMemory(VulkanBufferInfo* bufferInfo, void* dataToCopy, VkDeviceSize bufferSize)
 {
    return Buffer_UpdateBufferMemory(bufferInfo, dataToCopy, bufferSize);
}

 VkResult DLL_Buffer_UpdateStagingBufferMemory(VulkanBufferInfo* bufferInfo, void* dataToCopy, VkDeviceSize bufferSize)
 {
    return Buffer_UpdateStagingBufferMemory(bufferInfo, dataToCopy, bufferSize);
}

 void* DLL_Buffer_MapBufferMemory(VulkanBufferInfo* bufferInfo)
 {
    return Buffer_MapBufferMemory(bufferInfo);
}

 void DLL_Buffer_DestroyBuffer(VulkanBufferInfo* bufferInfo)
 {
    Buffer_DestroyBuffer(bufferInfo);
}
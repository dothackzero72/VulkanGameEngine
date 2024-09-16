#include "DLLVulkanBuffer.h"

VkResult DLL_Buffer_CreateBuffer(VulkanBufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    return Buffer_CreateBuffer(bufferInfo, renderer.Device, bufferData, bufferSize, bufferUsage, properties);
}

VkResult DLL_Buffer_CreateStagingBuffer(VulkanBufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    return Buffer_CreateStagingBuffer(bufferInfo, renderer.Device, bufferData, bufferSize, bufferUsage, properties);
}

VkResult DLL_Buffer_CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
{
    return Buffer_CopyBuffer(srcBuffer, dstBuffer, size);
}

VkResult DLL_Buffer_CopyStagingBuffer(VulkanBufferInfo* bufferInfo, VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
{
    return Buffer_CopyStagingBuffer(bufferInfo, commandBuffer, srcBuffer, dstBuffer, size);
}

VkResult DLL_Buffer_UpdateBufferSize(VulkanBufferInfo* bufferInfo, VkDevice device, VkDeviceSize bufferSize)
{
    return Buffer_UpdateBufferSize(bufferInfo, device, bufferSize);
}

 VkResult DLL_Buffer_UnmapBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device)
 {
    return Buffer_UnmapBufferMemory(bufferInfo, device);
}

 VkResult DLL_Buffer_UpdateBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device, void* dataToCopy, VkDeviceSize bufferSize)
 {
    return Buffer_UpdateBufferMemory(bufferInfo, device, dataToCopy, bufferSize);
}

 VkResult DLL_Buffer_UpdateStagingBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device, void* dataToCopy, VkDeviceSize bufferSize)
 {
    return Buffer_UpdateStagingBufferMemory(bufferInfo, device, dataToCopy, bufferSize);
}

 void* DLL_Buffer_MapBufferMemory(VulkanBufferInfo* bufferInfo, VkDevice device)
 {
    return Buffer_MapBufferMemory(bufferInfo, device);
}

 void DLL_Buffer_DestroyBuffer(VulkanBufferInfo* bufferInfo)
 {
    Buffer_DestroyBuffer(bufferInfo);
}

 int DLL_BUFFER_BufferTest()
 {
     return 45;
 }

#include "CBuffer.h"
#include "VulkanRenderer.h"

static VkResult Buffer_AllocateMemory(VkDevice device, VkBuffer* bufferData, VkDeviceMemory* bufferMemory, VkMemoryPropertyFlags properties)
{
    if (bufferData == NULL)
    {
        RENDERER_ERROR("Buffer Data is NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    VkMemoryRequirements memRequirements;
    vkGetBufferMemoryRequirements(device, *bufferData, &memRequirements);

    VkMemoryAllocateFlagsInfoKHR ExtendedAllocFlagsInfo =
    {
        .sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_FLAGS_INFO_KHR
    };
    VkMemoryAllocateInfo allocInfo =
    {
        .sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
        .allocationSize = memRequirements.size,
        .memoryTypeIndex = Renderer_GetMemoryType(memRequirements.memoryTypeBits, properties),
        .pNext = &ExtendedAllocFlagsInfo,
    };

    return vkAllocateMemory(device, &allocInfo, NULL, bufferMemory);
}

 void* Buffer_MapBufferMemory(struct VulkanBufferInfo* bufferInfo, VkDevice device)
{
    if (*bufferInfo->IsMapped) 
    {
        RENDERER_ERROR("Buffer already mapped!\n");
        return NULL;
    }

    void* mappedData;
    VULKAN_RESULT(vkMapMemory(device, *bufferInfo->BufferMemory, 0, *bufferInfo->BufferSize, 0, &mappedData));
    *bufferInfo->IsMapped = true; 
    return mappedData; 
}

 VkResult Buffer_UnmapBufferMemory(struct VulkanBufferInfo* bufferInfo, VkDevice device)
{
    if (*bufferInfo->IsMapped)
    {
        vkUnmapMemory(device, *bufferInfo->BufferMemory);
        *bufferInfo->IsMapped = false;
    }
    return VK_SUCCESS;
}

VkResult Buffer_CreateBuffer(VkDevice device, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, size_t bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    if (bufferData == NULL || bufferSize == 0)
    {
        RENDERER_ERROR("Buffer Data and Size can't be NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    VkBufferCreateInfo bufferInfoStruct = { 0 };
    bufferInfoStruct.sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO;
    bufferInfoStruct.size = bufferSize;
    bufferInfoStruct.usage = bufferUsage;
    bufferInfoStruct.sharingMode = VK_SHARING_MODE_EXCLUSIVE; 

    VULKAN_RESULT(vkCreateBuffer(device, &bufferInfoStruct, NULL, buffer));
    VULKAN_RESULT(Buffer_AllocateMemory(device, buffer, bufferMemory, properties));
    VULKAN_RESULT(vkBindBufferMemory(device, *buffer, *bufferMemory, 0));

    void* mappedData;
    VULKAN_RESULT(vkMapMemory(device, *bufferMemory, 0, bufferSize, 0, &mappedData));
    memcpy(mappedData, bufferData, bufferSize);
    vkUnmapMemory(device, *bufferMemory);

    return VK_SUCCESS;
}

VkResult Buffer_CreateStagingBuffer(VkDevice device, VkBuffer* stagingBuffer, VkDeviceMemory* stagingBufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    VkMemoryRequirements memRequirements;
    VkBufferCreateInfo bufferCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
        .size = bufferSize,
        .usage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
        .sharingMode = VK_SHARING_MODE_EXCLUSIVE
    };
    VULKAN_RESULT(vkCreateBuffer(device, &bufferCreateInfo, NULL, stagingBuffer));
    vkGetBufferMemoryRequirements(device, *stagingBuffer, &memRequirements);
    VULKAN_RESULT(Buffer_AllocateMemory(device, stagingBuffer, stagingBufferMemory, properties));
    return vkBindBufferMemory(device, *stagingBuffer, *stagingBufferMemory, 0);
}

VkResult Buffer_CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
{
    if (srcBuffer == NULL)
    {
        RENDERER_ERROR("Source Buffer is NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    VkBufferCopy copyRegion =
    {
        .srcOffset = 0,
        .dstOffset = 0,
        .size = size
    };
    VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer();
    vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);
    return Renderer_EndSingleUseCommandBuffer(&commandBuffer);
}

VkResult Buffer_RenderPassCopyBuffer(VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
{
    if (srcBuffer == NULL)
    {
        RENDERER_ERROR("Source Buffer is NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    VkBufferCopy copyRegion =
    {
        .srcOffset = 0,
        .dstOffset = 0,
        .size = size
    };
    vkCmdCopyBuffer(&commandBuffer, *srcBuffer, *dstBuffer, 1, &copyRegion);
    return VK_SUCCESS;
}

VkResult Buffer_UpdateBufferSize(struct VulkanBufferInfo* bufferInfo, VkDevice device, VkDeviceSize bufferSize)
{
    if (bufferInfo->BufferSize < bufferSize)
    {
        RENDERER_ERROR("New buffer size can't be less than the old buffer size.");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    bufferInfo->BufferSize = bufferSize;
    Buffer_DestroyBuffer(bufferInfo);

    VkBufferCreateInfo buffer =
    {
        .sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
        .size = bufferInfo->BufferSize,
        .usage = bufferInfo->BufferUsage,
        .sharingMode = VK_SHARING_MODE_EXCLUSIVE
    };
    VULKAN_RESULT(vkCreateBuffer(device, &buffer, NULL, &bufferInfo->Buffer));
    VULKAN_RESULT(Buffer_AllocateMemory(bufferInfo->Buffer, device, bufferInfo->BufferMemory, bufferInfo->BufferProperties));
    VULKAN_RESULT(vkBindBufferMemory(device, bufferInfo->Buffer, bufferInfo->BufferMemory, 0));
    return vkMapMemory(device, bufferInfo->BufferMemory, 0, bufferInfo->BufferSize, 0, &bufferInfo->BufferData);
}

VkResult Buffer_UpdateBufferMemory(struct VulkanBufferInfo* bufferInfo, VkDevice device, void* dataToCopy, VkDeviceSize bufferSize)
{
    if (dataToCopy == NULL || bufferSize == 0)
    {
        RENDERER_ERROR("Buffer Data and Size can't be NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    void* mappedData;
    VkResult result = vkMapMemory(device, *bufferInfo->BufferMemory, 0, bufferSize, 0, &mappedData);
    if (result != VK_SUCCESS) {
        RENDERER_ERROR("Failed to map buffer memory.");
        return result;
    }

    memcpy(mappedData, dataToCopy, (size_t)bufferSize);
    vkUnmapMemory(device, *bufferInfo->BufferMemory);

    return VK_SUCCESS;
}

VkResult Buffer_UpdateStagingBufferMemory(struct VulkanBufferInfo* bufferInfo, VkDevice device, void* DataToCopy, VkDeviceSize BufferSize)
{
    void* mappedData;
    vkMapMemory(device, bufferInfo->StagingBufferMemory, 0, BufferSize, 0, &mappedData);
    memcpy(mappedData, DataToCopy, (size_t)BufferSize);
    vkUnmapMemory(device, bufferInfo->StagingBufferMemory);
}

void Buffer_DestroyBuffer(struct VulkanBufferInfo* bufferInfo)
{
    *bufferInfo->BufferSize = 0;
    *bufferInfo->BufferUsage = 0;
    *bufferInfo->BufferProperties = 0;
    *bufferInfo->BufferData = VK_NULL_HANDLE;
    Renderer_DestroyBuffer(bufferInfo->Buffer);
    Renderer_FreeMemory(bufferInfo->BufferMemory);
}
#include "CBuffer.h"
#include "VulkanRenderer.h"

static VkResult Buffer_AllocateMemory(VkBuffer* bufferData, VkDeviceMemory* bufferMemory, VkMemoryPropertyFlags properties)
{
    if (bufferData == NULL)
    {
        RENDERER_ERROR("Buffer Data is NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    VkMemoryRequirements memRequirements;
    vkGetBufferMemoryRequirements(renderer.Device, *bufferData, &memRequirements);

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

    return vkAllocateMemory(renderer.Device, &allocInfo, NULL, bufferMemory);
}

void* Buffer_MapBufferMemory(VkDevice device, VkDeviceMemory deviceMemory, size_t bufferSize, bool* isMapped)
{
    if (*isMapped)
    {
        RENDERER_ERROR("Buffer already mapped!\n");
        return NULL;
    }

    void* mappedData;
    VULKAN_RESULT(vkMapMemory(device, deviceMemory, 0, bufferSize, 0, &mappedData));
    *isMapped = true;
    return mappedData;
}

VkResult Buffer_UnmapBufferMemory(VkDevice device, VkDeviceMemory deviceMemory, bool* isMapped)
{
    if (*isMapped)
    {
        vkUnmapMemory(device, deviceMemory);
        *isMapped = false;
    }
    return VK_SUCCESS;
}

VkResult Buffer_CreateBuffer(struct VulkanBufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    if (bufferData == NULL || bufferSize == 0)
    {
        RENDERER_ERROR("Buffer Data and Size can't be NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    *bufferInfo->BufferSize = bufferSize;
    *bufferInfo->BufferUsage = bufferUsage;
    *bufferInfo->BufferProperties = properties;

    VkBufferCreateInfo bufferInfoStruct = { 0 };
    bufferInfoStruct.sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO;
    bufferInfoStruct.size = bufferSize;
    bufferInfoStruct.usage = bufferUsage;
    bufferInfoStruct.sharingMode = VK_SHARING_MODE_EXCLUSIVE;

    VULKAN_RESULT(vkCreateBuffer(renderer.Device, &bufferInfoStruct, NULL, bufferInfo->Buffer));
    VULKAN_RESULT(Buffer_AllocateMemory(bufferInfo->Buffer, bufferInfo->BufferMemory, properties));
    VULKAN_RESULT(vkBindBufferMemory(renderer.Device, *bufferInfo->Buffer, *bufferInfo->BufferMemory, 0));

    void* mappedData;
    VULKAN_RESULT(vkMapMemory(renderer.Device, *bufferInfo->BufferMemory, 0, *bufferInfo->BufferSize, 0, &mappedData));
    memcpy(mappedData, bufferData, (size_t)*bufferInfo->BufferSize);
    vkUnmapMemory(renderer.Device, *bufferInfo->BufferMemory);

    return VK_SUCCESS;
}

VkResult Buffer_CreateStagingBuffer(struct VulkanBufferInfo* bufferInfo, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    VkMemoryRequirements memRequirements;
    VkBufferCreateInfo bufferCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
        .size = bufferSize,
        .usage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
        .sharingMode = VK_SHARING_MODE_EXCLUSIVE
    };
    VULKAN_RESULT(vkCreateBuffer(renderer.Device, &bufferCreateInfo, NULL, bufferInfo->StagingBuffer));
    vkGetBufferMemoryRequirements(renderer.Device, *bufferInfo->StagingBuffer, &memRequirements);
    VULKAN_RESULT(Buffer_AllocateMemory(bufferInfo->StagingBuffer, bufferInfo->StagingBufferMemory, properties));
    return vkBindBufferMemory(renderer.Device, *bufferInfo->StagingBuffer, *bufferInfo->StagingBufferMemory, 0);
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

VkResult Buffer_CopyStagingBuffer(struct VulkanBufferInfo* bufferInfo, VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
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
    vkCmdCopyBuffer(*commandBuffer, *srcBuffer, *dstBuffer, 1, &copyRegion);
    return VK_SUCCESS;
}

VkResult Buffer_UpdateBufferSize(struct VulkanBufferInfo* bufferInfo, VkDeviceSize bufferSize)
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
    VULKAN_RESULT(vkCreateBuffer(renderer.Device, &buffer, NULL, &bufferInfo->Buffer));
    VULKAN_RESULT(Buffer_AllocateMemory(bufferInfo->Buffer, bufferInfo->BufferMemory, bufferInfo->BufferProperties));
    VULKAN_RESULT(vkBindBufferMemory(renderer.Device, bufferInfo->Buffer, bufferInfo->BufferMemory, 0));
    return vkMapMemory(renderer.Device, bufferInfo->BufferMemory, 0, bufferInfo->BufferSize, 0, &bufferInfo->BufferData);
}

VkResult Buffer_UpdateBufferMemory(struct VulkanBufferInfo* bufferInfo, void* dataToCopy, VkDeviceSize bufferSize)
{
    if (dataToCopy == NULL || bufferSize == 0)
    {
        RENDERER_ERROR("Buffer Data and Size can't be NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    void* mappedData;
    VkResult result = vkMapMemory(renderer.Device, *bufferInfo->BufferMemory, 0, bufferSize, 0, &mappedData);
    if (result != VK_SUCCESS) {
        RENDERER_ERROR("Failed to map buffer memory.");
        return result;
    }

    memcpy(mappedData, dataToCopy, (size_t)bufferSize);
    vkUnmapMemory(renderer.Device, *bufferInfo->BufferMemory);

    return VK_SUCCESS;
}

VkResult Buffer_UpdateStagingBufferMemory(struct VulkanBufferInfo* bufferInfo, void* DataToCopy, VkDeviceSize BufferSize)
{
    void* mappedData;
    vkMapMemory(renderer.Device, *bufferInfo->StagingBufferMemory, 0, BufferSize, 0, &mappedData);
    memcpy(mappedData, DataToCopy, (size_t)BufferSize);
    vkUnmapMemory(renderer.Device, *bufferInfo->StagingBufferMemory);
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
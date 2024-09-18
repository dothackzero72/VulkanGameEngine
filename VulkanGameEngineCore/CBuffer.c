#include "CBuffer.h"
#include "CVulkanRenderer.h"

VkResult Buffer_AllocateMemory(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* bufferData, VkDeviceMemory* bufferMemory, VkMemoryPropertyFlags properties)
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
        .memoryTypeIndex = Renderer_GetMemoryType(physicalDevice, memRequirements.memoryTypeBits, properties),
        .pNext = &ExtendedAllocFlagsInfo,
    };
    return vkAllocateMemory(device, &allocInfo, NULL, bufferMemory);
}

void* Buffer_MapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, VkDeviceSize bufferSize, bool* isMapped)
{
    if (*isMapped)
    {
        RENDERER_ERROR("Buffer already mapped!\n");
        return NULL;
    }

    void* mappedData;
    VULKAN_RESULT(vkMapMemory(device, bufferMemory, 0, bufferSize, 0, &mappedData));
    *isMapped = true;
    return mappedData;
}

VkResult Buffer_UnmapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, bool* isMapped)
{
    if (*isMapped)
    {
        vkUnmapMemory(device, bufferMemory);
        *isMapped = false;
    }
    return VK_SUCCESS;
}

VkResult Buffer_CreateBuffer(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
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
    VULKAN_RESULT(Buffer_AllocateMemory(device, physicalDevice, buffer, bufferMemory, properties));
    VULKAN_RESULT(vkBindBufferMemory(device, *buffer, *bufferMemory, 0));

    void* mappedData;
    VULKAN_RESULT(vkMapMemory(device, *bufferMemory, 0, bufferSize, 0, &mappedData));
    memcpy(mappedData, bufferData, (size_t)bufferSize);
    vkUnmapMemory(device, *bufferMemory);

    return VK_SUCCESS;
}

VkResult Buffer_CreateStagingBuffer(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* stagingBuffer, VkDeviceMemory* stagingBufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
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
    VULKAN_RESULT(Buffer_AllocateMemory(device, physicalDevice, stagingBuffer, stagingBufferMemory, properties));
    return vkBindBufferMemory(device, *stagingBuffer, *stagingBufferMemory, 0);
}

VkResult Buffer_CopyBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
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
    VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
    vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);
    return Renderer_EndSingleUseCommandBuffer(device, commandBuffer, &commandBuffer, graphicsQueue);
}

VkResult Buffer_CopyStagingBuffer(VkCommandBuffer* commandBuffer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
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

VkResult Buffer_UpdateBufferSize(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize* oldBufferSize, VkDeviceSize newBufferSize, VkBufferUsageFlags bufferUsageFlags, VkMemoryPropertyFlags propertyFlags)
{
    if (newBufferSize < *oldBufferSize)
    {
        RENDERER_ERROR("New buffer size can't be less than the old buffer size.");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    *oldBufferSize = newBufferSize;

    VkBufferCreateInfo bufferCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
        .size = newBufferSize,
        .usage = bufferUsageFlags,
        .sharingMode = VK_SHARING_MODE_EXCLUSIVE
    };
    VULKAN_RESULT(vkCreateBuffer(device, &bufferCreateInfo, NULL, &buffer));
    VULKAN_RESULT(Buffer_AllocateMemory(device, physicalDevice, &buffer, bufferMemory, propertyFlags));
    VULKAN_RESULT(vkBindBufferMemory(device, &buffer, bufferMemory, 0));
    return vkMapMemory(device, bufferMemory, 0, newBufferSize, 0, bufferData);
}

VkResult Buffer_UpdateBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, void* dataToCopy, VkDeviceSize bufferSize)
{
    if (dataToCopy == NULL || bufferSize == 0)
    {
        RENDERER_ERROR("Buffer Data and Size can't be NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    void* mappedData;
    VkResult result = vkMapMemory(device, bufferMemory, 0, bufferSize, 0, &mappedData);
    memcpy(mappedData, dataToCopy, (size_t)bufferSize);
    vkUnmapMemory(device, bufferMemory);
    return VK_SUCCESS;
}

VkResult Buffer_UpdateStagingBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, void* dataToCopy, VkDeviceSize bufferSize)
{
    if (dataToCopy == NULL || bufferSize == 0)
    {
        RENDERER_ERROR("Buffer Data and Size can't be NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    void* mappedData;
    vkMapMemory(device, bufferMemory, 0, bufferSize, 0, &mappedData);
    memcpy(mappedData, dataToCopy, (size_t)bufferSize);
    vkUnmapMemory(device, bufferMemory);
    return VK_SUCCESS;
}

void Buffer_DestroyBuffer(VkDevice device, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize* bufferSize, VkBufferUsageFlags* bufferUsageFlags, VkMemoryPropertyFlags* propertyFlags)
{
    *bufferSize = 0;
    *bufferUsageFlags = 0;
    *propertyFlags = 0;
    bufferData = VK_NULL_HANDLE;
    Renderer_DestroyBuffer(device, buffer);
    Renderer_FreeMemory(device, bufferMemory);
}
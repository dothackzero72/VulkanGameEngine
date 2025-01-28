#include "CBuffer.h"
#include "CVulkanRenderer.h"

 VkResult Buffer_UpdateBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, void* dataToCopy, VkDeviceSize bufferSize)
{
    if (dataToCopy == NULL || bufferSize == 0)
    {
        RENDERER_ERROR("Buffer data and size cannot be NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    void* mappedData;
    VkResult result = vkMapMemory(device, bufferMemory, 0, bufferSize, 0, &mappedData);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to map buffer memory");
        return result;
    }

    memcpy(mappedData, dataToCopy, (size_t)bufferSize);
    vkUnmapMemory(device, bufferMemory);
    return VK_SUCCESS;
    {
    }
 }

 void Buffer_CopyBufferMemory(VkDevice device, VkCommandPool commandPool, VkDeviceMemory srcBuffer, VkDeviceMemory* dstBuffer, VkDeviceSize bufferSize)
 {
     VkBufferCopy copyRegion =
     {
         .srcOffset = 0,
         .dstOffset = 0,
         .size = bufferSize
     };

     VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
     vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);
     Renderer_EndCommandBuffer(&commandBuffer);
 }

VkResult Buffer_AllocateMemory(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* bufferData, VkDeviceMemory* bufferMemory, VkMemoryPropertyFlags properties)
{
    if (bufferData == NULL || bufferMemory == NULL)
    {
        RENDERER_ERROR("Buffer data or buffer memory is NULL");
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
        RENDERER_ERROR("Buffer already mapped!");
        return NULL;
    }

    void* mappedData;
    VkResult result = vkMapMemory(device, bufferMemory, 0, bufferSize, 0, &mappedData);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to map buffer memory");
        return NULL;
    }
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

VkResult Buffer_CreateBuffer(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkMemoryPropertyFlags properties, VkBufferUsageFlags usage)
{
    VkBufferCreateInfo bufferCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
        .size = bufferSize,
        .usage = usage,
        .sharingMode = VK_SHARING_MODE_EXCLUSIVE
    };

    VkResult result = vkCreateBuffer(device, &bufferCreateInfo, NULL, buffer);
    result = Buffer_AllocateMemory(device, physicalDevice, buffer, bufferMemory, properties);
    result = vkBindBufferMemory(device, *buffer, *bufferMemory, 0);
    result = Buffer_UpdateBufferMemory(device, *bufferMemory, bufferData, bufferSize);

    return VK_SUCCESS;
}

VkResult Buffer_CreateStagingBuffer(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkBuffer* stagingBuffer, VkBuffer* buffer, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    if (stagingBuffer == NULL || buffer == NULL || stagingBufferMemory == NULL || bufferMemory == NULL)
    {
        RENDERER_ERROR("One or more buffer pointers are NULL");
        return VK_ERROR_INITIALIZATION_FAILED;
    }

    VkResult result = Buffer_CreateBuffer(device, physicalDevice, stagingBuffer, stagingBufferMemory, bufferData, bufferSize, properties, bufferUsage);
    if (result != VK_SUCCESS)
    {
        return result;
    }

    result = Buffer_CreateBuffer(device, physicalDevice, buffer, bufferMemory, bufferData, bufferSize, properties, bufferUsage);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to create buffer");
        vkDestroyBuffer(device, *stagingBuffer, NULL);
        Renderer_FreeDeviceMemory(device, stagingBufferMemory);
        return result;
    }

    return Buffer_CopyBuffer(device, commandPool, graphicsQueue, stagingBuffer, buffer, bufferSize);
}

VkResult Buffer_CopyBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
{
    if (srcBuffer == NULL || dstBuffer == NULL)
    {
        RENDERER_ERROR("Source or Destination Buffer is NULL");
        return VK_ERROR_UNKNOWN;
    }

    VkBufferCopy copyRegion =
    {
        .srcOffset = 0,
        .dstOffset = 0,
        .size = size
    };

    VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);

    vkCmdCopyBuffer(commandBuffer, *srcBuffer, *dstBuffer, 1, &copyRegion);

    return Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
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

    VkBuffer newBuffer;
    VkDeviceMemory newBufferMemory;

    VkResult result = vkCreateBuffer(device, &bufferCreateInfo, NULL, &newBuffer);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to create new buffer");
        return result;
    }

    result = Buffer_AllocateMemory(device, physicalDevice, &newBuffer, &newBufferMemory, propertyFlags);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to allocate memory for new buffer");
        vkDestroyBuffer(device, newBuffer, NULL);
        return result;
    }

    result = vkBindBufferMemory(device, newBuffer, newBufferMemory, 0);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to bind memory to the new buffer");
        Renderer_FreeDeviceMemory(device, &newBufferMemory);
        vkDestroyBuffer(device, newBuffer, NULL);
        return result;
    }

    if (bufferData != NULL)
    {
        result = Buffer_UpdateBufferMemory(device, newBufferMemory, bufferData, newBufferSize);
        if (result != VK_SUCCESS)
        {
            RENDERER_ERROR("Failed to update memory with buffer data");
            vkDestroyBuffer(device, newBuffer, NULL);
            Renderer_FreeDeviceMemory(device, &newBufferMemory);
            return result;
        }
    }

    vkDestroyBuffer(device, buffer, NULL);
    Renderer_FreeDeviceMemory(device, bufferMemory);

    buffer = newBuffer;
    *bufferMemory = newBufferMemory;

    return VK_SUCCESS;
}

void Buffer_UpdateBufferData(VkDevice device, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize, bool usingStagingBuffer)
{
    if (usingStagingBuffer == true)
    {
        RENDERER_ERROR("This buffer uses staging");
        return VK_ERROR_INITIALIZATION_FAILED;
    }

    Buffer_UpdateBufferMemory(device, *bufferMemory, dataToCopy, bufferSize);
}

void Buffer_UpdateStagingBufferData(VkDevice device, VkCommandPool commandPool, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize, bool usingStagingBuffer)
{

    VkResult result = Buffer_UpdateBufferMemory(device, *stagingBufferMemory, dataToCopy, bufferSize);
    if (result != VK_SUCCESS)
    {
        return;
    }
    Buffer_CopyBufferMemory(device, commandPool, *stagingBufferMemory, bufferMemory, bufferSize);
}

VkResult Buffer_DestroyBuffer(VkDevice device, VkBuffer* buffer, VkBuffer* stagingBuffer, VkDeviceMemory* bufferMemory, VkDeviceMemory* stagingBufferMemory, void* bufferData, VkDeviceSize* bufferSize, VkBufferUsageFlags* bufferUsageFlags, VkMemoryPropertyFlags* propertyFlags)
{
    if (buffer == NULL && stagingBuffer == NULL)
    {
        RENDERER_ERROR("Both buffer and stagingBuffer are NULL");
        return VK_ERROR_INITIALIZATION_FAILED;
    }

    if (buffer && *buffer != VK_NULL_HANDLE)
    {
        vkDestroyBuffer(device, *buffer, NULL);
        *buffer = VK_NULL_HANDLE;
    }

    if (stagingBuffer && *stagingBuffer != VK_NULL_HANDLE)
    {
        vkDestroyBuffer(device, *stagingBuffer, NULL);
        *stagingBuffer = VK_NULL_HANDLE;
    }

    if (bufferMemory && *bufferMemory != VK_NULL_HANDLE)
    {
        Renderer_FreeDeviceMemory(device, bufferMemory);
        *bufferMemory = VK_NULL_HANDLE;
    }

    if (stagingBufferMemory && *stagingBufferMemory != VK_NULL_HANDLE)
    {
        Renderer_FreeDeviceMemory(device, stagingBufferMemory);
        *stagingBufferMemory = VK_NULL_HANDLE;
    }

    *bufferSize = 0;
    *bufferUsageFlags = 0;
    *propertyFlags = 0;
    bufferData = VK_NULL_HANDLE;

    return VK_SUCCESS;
}
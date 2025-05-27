#include "VulkanBufferFuncs.h"
#include "CVulkanRenderer.h"

VkResult Buffer_UpdateBufferMemory(const RendererState& renderState, VkDeviceMemory bufferMemory, void* dataToCopy, VkDeviceSize bufferSize)
{
    if (dataToCopy == NULL || bufferSize == 0)
    {
        RENDERER_ERROR("Buffer data and size cannot be NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    void* mappedData;
    VkResult result = vkMapMemory(renderState.Device, bufferMemory, 0, bufferSize, 0, &mappedData);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to map buffer memory");
        return result;
    }

    memcpy(mappedData, dataToCopy, (size_t)bufferSize);
    vkUnmapMemory(renderState.Device, bufferMemory);
    return VK_SUCCESS;
    {
    }
}

void Buffer_CopyBufferMemory(const RendererState& renderState, VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize bufferSize)
{
    VkBufferCopy copyRegion =
    {
        .srcOffset = 0,
        .dstOffset = 0,
        .size = bufferSize
    };

    VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(renderState.Device, renderState.CommandPool);
    vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);
    Renderer_EndSingleUseCommandBuffer(renderState.Device, renderState.CommandPool, renderState.GraphicsQueue, commandBuffer);
}

VkResult Buffer_AllocateMemory(const RendererState& renderState, VkBuffer* bufferData, VkDeviceMemory* bufferMemory, VkMemoryPropertyFlags properties)
{
    if (bufferData == NULL ||
        bufferMemory == NULL)
    {
        RENDERER_ERROR("Buffer data or buffer memory is NULL");
        return VK_ERROR_MEMORY_MAP_FAILED;
    }

    VkMemoryRequirements memRequirements;
    vkGetBufferMemoryRequirements(renderState.Device, *bufferData, &memRequirements);

    VkMemoryAllocateFlagsInfoKHR ExtendedAllocFlagsInfo =
    {
        .sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_FLAGS_INFO_KHR
    };
    VkMemoryAllocateInfo allocInfo =
    {
        .sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
        .pNext = &ExtendedAllocFlagsInfo,
        .allocationSize = memRequirements.size,
        .memoryTypeIndex = Renderer_GetMemoryType(renderState.PhysicalDevice, memRequirements.memoryTypeBits, properties),
    };

    return vkAllocateMemory(renderState.Device, &allocInfo, NULL, bufferMemory);
}

void* Buffer_MapBufferMemory(const RendererState& renderState, VkDeviceMemory bufferMemory, VkDeviceSize bufferSize, bool* isMapped)
{
    if (*isMapped)
    {
        RENDERER_ERROR("Buffer already mapped!");
        return NULL;
    }

    void* mappedData;
    VkResult result = vkMapMemory(renderState.Device, bufferMemory, 0, bufferSize, 0, &mappedData);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to map buffer memory");
        return NULL;
    }
    *isMapped = true;
    return mappedData;
}

VkResult Buffer_UnmapBufferMemory(const RendererState& renderState, VkDeviceMemory bufferMemory, bool* isMapped)
{
    if (*isMapped)
    {
        vkUnmapMemory(renderState.Device, bufferMemory);
        *isMapped = false;
    }
    return VK_SUCCESS;
}

VkResult Buffer_CreateBuffer(const RendererState& renderState, VkBuffer* buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkMemoryPropertyFlags properties, VkBufferUsageFlags usage)
{
    VkBufferCreateInfo bufferCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
        .size = bufferSize,
        .usage = usage,
        .sharingMode = VK_SHARING_MODE_EXCLUSIVE
    };

    VULKAN_RESULT(vkCreateBuffer(renderState.Device, &bufferCreateInfo, NULL, buffer));
    VULKAN_RESULT(Buffer_AllocateMemory(renderState, buffer, bufferMemory, properties));
    VULKAN_RESULT(vkBindBufferMemory(renderState.Device, *buffer, *bufferMemory, 0));
    return Buffer_UpdateBufferMemory(renderState, *bufferMemory, bufferData, bufferSize);
}

VkResult Buffer_CreateStagingBuffer(const RendererState& renderState, VkBuffer* stagingBuffer, VkBuffer* buffer, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlags properties)
{
    if (stagingBuffer == NULL || buffer == NULL || stagingBufferMemory == NULL || bufferMemory == NULL)
    {
        RENDERER_ERROR("One or more buffer pointers are NULL");
        return VK_ERROR_INITIALIZATION_FAILED;
    }

    VkResult result = Buffer_CreateBuffer(renderState, stagingBuffer, stagingBufferMemory, bufferData, bufferSize, properties, bufferUsage);
    if (result != VK_SUCCESS)
    {
        return result;
    }

    result = Buffer_CreateBuffer(renderState, buffer, bufferMemory, bufferData, bufferSize, properties, bufferUsage);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to create buffer");
        vkDestroyBuffer(renderState.Device, *stagingBuffer, NULL);
        Renderer_FreeDeviceMemory(renderState.Device, stagingBufferMemory);
        return result;
    }

    return Buffer_CopyBuffer(renderState, stagingBuffer, buffer, bufferSize);
}

VkResult Buffer_CopyBuffer(const RendererState& renderState, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
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

    VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(renderState.Device, renderState.CommandPool);
    vkCmdCopyBuffer(commandBuffer, *srcBuffer, *dstBuffer, 1, &copyRegion);
    return Renderer_EndSingleUseCommandBuffer(renderState.Device, renderState.CommandPool, renderState.GraphicsQueue, commandBuffer);
}

VkResult Buffer_UpdateBufferSize(const RendererState& renderState, VkBuffer buffer, VkDeviceMemory* bufferMemory, void* bufferData, VkDeviceSize* oldBufferSize, VkDeviceSize newBufferSize, VkBufferUsageFlags bufferUsageFlags, VkMemoryPropertyFlags propertyFlags)
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

    VkResult result = vkCreateBuffer(renderState.Device, &bufferCreateInfo, NULL, &newBuffer);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to create new buffer");
        return result;
    }

    result = Buffer_AllocateMemory(renderState, &newBuffer, &newBufferMemory, propertyFlags);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to allocate memory for new buffer");
        vkDestroyBuffer(renderState.Device, newBuffer, NULL);
        return result;
    }

    result = vkBindBufferMemory(renderState.Device, newBuffer, newBufferMemory, 0);
    if (result != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to bind memory to the new buffer");
        Renderer_FreeDeviceMemory(renderState.Device, &newBufferMemory);
        vkDestroyBuffer(renderState.Device, newBuffer, NULL);
        return result;
    }

    if (bufferData != NULL)
    {
        result = Buffer_UpdateBufferMemory(renderState, newBufferMemory, bufferData, newBufferSize);
        if (result != VK_SUCCESS)
        {
            RENDERER_ERROR("Failed to update memory with buffer data");
            vkDestroyBuffer(renderState.Device, newBuffer, NULL);
            Renderer_FreeDeviceMemory(renderState.Device, &newBufferMemory);
            return result;
        }
    }

    vkDestroyBuffer(renderState.Device, buffer, NULL);
    Renderer_FreeDeviceMemory(renderState.Device, bufferMemory);

    buffer = newBuffer;
    *bufferMemory = newBufferMemory;

    return VK_SUCCESS;
}

void Buffer_UpdateBufferData(const RendererState& renderState, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize)
{
    Buffer_UpdateBufferMemory(renderState, *bufferMemory, dataToCopy, bufferSize);
}

void Buffer_UpdateStagingBufferData(const RendererState& renderState, VkBuffer stagingBuffer, VkBuffer buffer, VkDeviceMemory* stagingBufferMemory, VkDeviceMemory* bufferMemory, void* dataToCopy, VkDeviceSize bufferSize)
{
    if (Buffer_UpdateBufferMemory(renderState, *stagingBufferMemory, dataToCopy, bufferSize) != VK_SUCCESS)
    {
        RENDERER_ERROR("Failed to update staging buffer memory.");
        return;
    }
    Buffer_CopyBufferMemory(renderState, stagingBuffer, buffer, bufferSize);
}

VkResult Buffer_DestroyBuffer(const RendererState& renderState, VkBuffer* buffer, VkBuffer* stagingBuffer, VkDeviceMemory* bufferMemory, VkDeviceMemory* stagingBufferMemory, void* bufferData, VkDeviceSize* bufferSize, VkBufferUsageFlags* bufferUsageFlags, VkMemoryPropertyFlags* propertyFlags)
{
    if (buffer == NULL && stagingBuffer == NULL)
    {
        RENDERER_ERROR("Both buffer and stagingBuffer are NULL");
        return VK_ERROR_INITIALIZATION_FAILED;
    }

    if (buffer && *buffer != VK_NULL_HANDLE)
    {
        vkDestroyBuffer(renderState.Device, *buffer, NULL);
        *buffer = VK_NULL_HANDLE;
    }

    if (stagingBuffer &&
        *stagingBuffer != VK_NULL_HANDLE)
    {
        vkDestroyBuffer(renderState.Device, *stagingBuffer, NULL);
        *stagingBuffer = VK_NULL_HANDLE;
    }

    if (bufferMemory &&
        *bufferMemory != VK_NULL_HANDLE)
    {
        Renderer_FreeDeviceMemory(renderState.Device, bufferMemory);
        *bufferMemory = VK_NULL_HANDLE;
    }

    if (stagingBufferMemory &&
        *stagingBufferMemory != VK_NULL_HANDLE)
    {
        Renderer_FreeDeviceMemory(renderState.Device, stagingBufferMemory);
        *stagingBufferMemory = VK_NULL_HANDLE;
    }

    *bufferSize = 0;
    *bufferUsageFlags = 0;
    *propertyFlags = 0;
    bufferData = VK_NULL_HANDLE;

    return VK_SUCCESS;
}
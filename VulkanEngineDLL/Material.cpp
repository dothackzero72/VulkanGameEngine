#include "Material.h"

VulkanBuffer Material_CreateMaterialBuffer(const RendererState& renderer, uint bufferId)
{
    return VulkanBuffer_CreateVulkanBuffer(renderer, bufferId, sizeof(MaterialProperitiesBuffer), 1, BufferTypeEnum::BufferType_MaterialProperitiesBuffer, VK_BUFFER_USAGE_STORAGE_BUFFER_BIT,
                                                                                                                                                           VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                                                                                                           VK_MEMORY_PROPERTY_HOST_COHERENT_BIT |
                                                                                                                                                           VK_MEMORY_ALLOCATE_DEVICE_ADDRESS_BIT, false);
}

void Material_UpdateBuffer(const RendererState& renderer, VulkanBuffer& materialBuffer, MaterialProperitiesBuffer& materialProperties)
{
    VulkanBuffer_UpdateBufferMemory(renderer, materialBuffer, static_cast<void*>(&materialProperties), sizeof(MaterialProperitiesBuffer), 1);
}

void Material_DestroyBuffer(const RendererState& renderer, VulkanBuffer& materialBuffer)
{
    VulkanBuffer_DestroyBuffer(renderer, materialBuffer);
}

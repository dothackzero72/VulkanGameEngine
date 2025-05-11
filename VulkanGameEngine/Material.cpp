#include "Material.h"

Material::Material()
{
}

Material::Material(const String& materialName, Vector<VkGuid>& renderPassIds, VkGuid& materialId)
{
	Name = materialName;
	RenderPassIds = renderPassIds;
	MaterialId = materialId;
	MaterialBufferIndex = 0;
	MaterialBuffer = VulkanBuffer<MaterialProperitiesBuffer>(MaterialInfo,  VK_BUFFER_USAGE_SHADER_DEVICE_ADDRESS_BIT |
																			VK_BUFFER_USAGE_STORAGE_BUFFER_BIT, 
																			VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | 
																			VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);
}

Material::~Material()
{
}

void Material::UpdateMaterialBufferIndex(uint32 bufferIndex)
{
	MaterialBufferIndex = bufferIndex;
}

void Material::UpdateBuffer()
{

	MaterialBuffer.UpdateBufferMemory(MaterialInfo);
}

void Material::Destroy()
{
	MaterialBuffer.DestroyBuffer();
}

void Material::GetMaterialPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& materialBufferList)
{
	UpdateBuffer();
	VkDescriptorBufferInfo meshBufferInfo =
	{
		.buffer = MaterialBuffer.Buffer,
		.offset = 0,
		.range = VK_WHOLE_SIZE
	};
	materialBufferList.emplace_back(meshBufferInfo);
}
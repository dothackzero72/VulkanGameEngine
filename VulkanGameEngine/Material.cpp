#include "Material.h"
#include "RenderSystem.h"

Material::Material()
{
}

Material::Material(const String& materialName, Vector<VkGuid>& renderPassIds, VkGuid& materialId)
{
	Name = materialName;
	RenderPassIds = renderPassIds;
	MaterialId = materialId;
	MaterialBufferIndex = 0;
	MaterialBuffer = VulkanBuffer<MaterialProperitiesBuffer>(MaterialProperitiesBuffer(), VK_BUFFER_USAGE_STORAGE_BUFFER_BIT, 
																						  VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | 
																						  VK_MEMORY_PROPERTY_HOST_COHERENT_BIT |
																						  VK_MEMORY_ALLOCATE_DEVICE_ADDRESS_BIT, false);
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
	MaterialProperitiesBuffer materialBuffer = MaterialProperitiesBuffer
	{
		.AlbedoMapId = AlbedoMapId != VkGuid() ? renderSystem.TextureList[AlbedoMapId].GetTextureBufferIndex() : 0,
		.MetallicRoughnessMapId = MetallicRoughnessMapId != VkGuid() ? renderSystem.TextureList[MetallicRoughnessMapId].GetTextureBufferIndex() : 0,
		.MetallicMapId = MetallicMapId != VkGuid() ? renderSystem.TextureList[MetallicMapId].GetTextureBufferIndex() : 0,
		.RoughnessMapId = RoughnessMapId != VkGuid() ? renderSystem.TextureList[RoughnessMapId].GetTextureBufferIndex() : 0,
		.AmbientOcclusionMapId = AmbientOcclusionMapId != VkGuid() ? renderSystem.TextureList[AmbientOcclusionMapId].GetTextureBufferIndex() : 0,
		.NormalMapId = NormalMapId != VkGuid() ? renderSystem.TextureList[NormalMapId].GetTextureBufferIndex() : 0,
		.DepthMapId = DepthMapId != VkGuid() ? renderSystem.TextureList[DepthMapId].GetTextureBufferIndex() : 0,
		.AlphaMapId = AlphaMapId != VkGuid() ? renderSystem.TextureList[AlphaMapId].GetTextureBufferIndex() : 0,
		.EmissionMapId = EmissionMapId != VkGuid() ? renderSystem.TextureList[EmissionMapId].GetTextureBufferIndex() : 0,
		.HeightMapId = HeightMapId != VkGuid() ? renderSystem.TextureList[HeightMapId].GetTextureBufferIndex() : 0
	};
	MaterialBuffer.UpdateBufferMemory(materialBuffer);
}

void Material::Destroy()
{
	MaterialBuffer.DestroyBuffer();
}

void Material::GetMaterialPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& materialBufferList)
{
	VkDescriptorBufferInfo meshBufferInfo =
	{
		.buffer = MaterialBuffer.Buffer,
		.offset = 0,
		.range = VK_WHOLE_SIZE
	};
	materialBufferList.emplace_back(meshBufferInfo);
}
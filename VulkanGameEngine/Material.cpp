#include "Material.h"
#include "RenderSystem.h"

Material::Material()
{
}

Material::Material(const String& materialName, VkGuid& materialId)
{
	Name = materialName;
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
		.AlbedoMapId = AlbedoMapId != VkGuid() ? renderSystem.TextureList[AlbedoMapId].textureBufferIndex : 0,
		.MetallicRoughnessMapId = MetallicRoughnessMapId != VkGuid() ? renderSystem.TextureList[MetallicRoughnessMapId].textureBufferIndex : 0,
		.MetallicMapId = MetallicMapId != VkGuid() ? renderSystem.TextureList[MetallicMapId].textureBufferIndex : 0,
		.RoughnessMapId = RoughnessMapId != VkGuid() ? renderSystem.TextureList[RoughnessMapId].textureBufferIndex : 0,
		.AmbientOcclusionMapId = AmbientOcclusionMapId != VkGuid() ? renderSystem.TextureList[AmbientOcclusionMapId].textureBufferIndex : 0,
		.NormalMapId = NormalMapId != VkGuid() ? renderSystem.TextureList[NormalMapId].textureBufferIndex : 0,
		.DepthMapId = DepthMapId != VkGuid() ? renderSystem.TextureList[DepthMapId].textureBufferIndex : 0,
		.AlphaMapId = AlphaMapId != VkGuid() ? renderSystem.TextureList[AlphaMapId].textureBufferIndex : 0,
		.EmissionMapId = EmissionMapId != VkGuid() ? renderSystem.TextureList[EmissionMapId].textureBufferIndex : 0,
		.HeightMapId = HeightMapId != VkGuid() ? renderSystem.TextureList[HeightMapId].textureBufferIndex : 0
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
#include "Material.h"
#include "RenderSystem.h"
#include "TextureSystem.h"

Material::Material()
{
}

Material::Material(const String& materialName, VkGuid& materialId)
{
	Name = materialName;
	MaterialId = materialId;
	MaterialBufferIndex = 0;
	MaterialBuffer = VulkanBuffer<MaterialProperitiesBuffer>(cRenderer, MaterialProperitiesBuffer(), VK_BUFFER_USAGE_STORAGE_BUFFER_BIT,
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
		.AlbedoMapId = AlbedoMapId != VkGuid() ? textureSystem.TextureList[AlbedoMapId].textureBufferIndex : 0,
		.MetallicRoughnessMapId = MetallicRoughnessMapId != VkGuid() ? textureSystem.TextureList[MetallicRoughnessMapId].textureBufferIndex : 0,
		.MetallicMapId = MetallicMapId != VkGuid() ? textureSystem.TextureList[MetallicMapId].textureBufferIndex : 0,
		.RoughnessMapId = RoughnessMapId != VkGuid() ? textureSystem.TextureList[RoughnessMapId].textureBufferIndex : 0,
		.AmbientOcclusionMapId = AmbientOcclusionMapId != VkGuid() ? textureSystem.TextureList[AmbientOcclusionMapId].textureBufferIndex : 0,
		.NormalMapId = NormalMapId != VkGuid() ? textureSystem.TextureList[NormalMapId].textureBufferIndex : 0,
		.DepthMapId = DepthMapId != VkGuid() ? textureSystem.TextureList[DepthMapId].textureBufferIndex : 0,
		.AlphaMapId = AlphaMapId != VkGuid() ? textureSystem.TextureList[AlphaMapId].textureBufferIndex : 0,
		.EmissionMapId = EmissionMapId != VkGuid() ? textureSystem.TextureList[EmissionMapId].textureBufferIndex : 0,
		.HeightMapId = HeightMapId != VkGuid() ? textureSystem.TextureList[HeightMapId].textureBufferIndex : 0
	};
	MaterialBuffer.UpdateBufferMemory(cRenderer, materialBuffer);
}

void Material::Destroy()
{
	MaterialBuffer.DestroyBuffer(cRenderer);
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
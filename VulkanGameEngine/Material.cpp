#include "Material.h"
#include "RenderSystem.h"
#include "TextureSystem.h"
#include "VulkanBufferSystem.h"

Material::Material()
{
}

Material::Material(const String& materialName, VkGuid& materialId)
{
	Name = materialName;
	MaterialId = materialId;
	ShaderMaterialBufferIndex = 0;

	MaterialProperitiesBuffer buffer = MaterialProperitiesBuffer();
	MaterialBufferId = bufferSystem.CreateVulkanBuffer<MaterialProperitiesBuffer>(cRenderer, buffer, VK_BUFFER_USAGE_STORAGE_BUFFER_BIT,
																									 VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | 
																									 VK_MEMORY_PROPERTY_HOST_COHERENT_BIT |
																									 VK_MEMORY_ALLOCATE_DEVICE_ADDRESS_BIT, false);
}

Material::~Material()
{
}

void Material::UpdateMaterialBufferIndex(uint32 shaderBufferIndex)
{
	ShaderMaterialBufferIndex = shaderBufferIndex;
}

void Material::UpdateBuffer()
{
	MaterialProperitiesBuffer materialBuffer = MaterialProperitiesBuffer
	{
		.AlbedoMapId = AlbedoMapId != VkGuid() ? textureSystem.FindTexture(AlbedoMapId).textureBufferIndex : 0,
		.MetallicRoughnessMapId = MetallicRoughnessMapId != VkGuid() ? textureSystem.FindTexture(MetallicRoughnessMapId).textureBufferIndex : 0,
		.MetallicMapId = MetallicMapId != VkGuid() ? textureSystem.FindTexture(MetallicMapId).textureBufferIndex : 0,
		.RoughnessMapId = RoughnessMapId != VkGuid() ? textureSystem.FindTexture(RoughnessMapId).textureBufferIndex : 0,
		.AmbientOcclusionMapId = AmbientOcclusionMapId != VkGuid() ? textureSystem.FindTexture(AmbientOcclusionMapId).textureBufferIndex : 0,
		.NormalMapId = NormalMapId != VkGuid() ? textureSystem.FindTexture(NormalMapId).textureBufferIndex : 0,
		.DepthMapId = DepthMapId != VkGuid() ? textureSystem.FindTexture(DepthMapId).textureBufferIndex : 0,
		.AlphaMapId = AlphaMapId != VkGuid() ? textureSystem.FindTexture(AlphaMapId).textureBufferIndex : 0,
		.EmissionMapId = EmissionMapId != VkGuid() ? textureSystem.FindTexture(EmissionMapId).textureBufferIndex : 0,
		.HeightMapId = HeightMapId != VkGuid() ? textureSystem.FindTexture(HeightMapId).textureBufferIndex : 0
	};
	bufferSystem.UpdateBufferMemory<MaterialProperitiesBuffer>(cRenderer, MaterialBufferId, materialBuffer);
}

void Material::Destroy()
{
	bufferSystem.DestroyBuffer(cRenderer, MaterialBufferId);
}

void Material::GetMaterialPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& materialBufferList)
{
	VkDescriptorBufferInfo meshBufferInfo =
	{
		.buffer = bufferSystem.VulkanBuffer[MaterialBufferId].Buffer,
		.offset = 0,
		.range = VK_WHOLE_SIZE
	};
	materialBufferList.emplace_back(meshBufferInfo);
}
#pragma once
#include <Texture.h>
#include "VulkanBuffer.h"
#include "VkGuid.h"

struct MaterialProperitiesBuffer
{
	alignas(16) vec3 Albedo = vec3(0.0f, 0.35f, 0.45);
	alignas(4)  float Metallic = 0.0f;
	alignas(4)  float Roughness = 0.0f;
	alignas(4)  float AmbientOcclusion = 1.0f;
	alignas(16) vec3 Emission = vec3(0.0f);
	alignas(4)  float Alpha = 1.0f;

	alignas(4) uint32 AlbedoMapId = 0;
	alignas(4) uint32 MetallicRoughnessMapId = 0;
	alignas(4) uint32 MetallicMapId = 0;
	alignas(4) uint32 RoughnessMapId = 0;
	alignas(4) uint32 AmbientOcclusionMapId = 0;
	alignas(4) uint32 NormalMapId = 0;
	alignas(4) uint32 DepthMapId = 0;
	alignas(4) uint32 AlphaMapId = 0;
	alignas(4) uint32 EmissionMapId = 0;
	alignas(4) uint32 HeightMapId = 0;
};

class Material
{
private:

public:
	static uint32 NextMaterialId;

	String Name;
	VkGuid MaterialId;
	uint MaterialBufferIndex = 0;

	vec3 Albedo = vec3(0.0f, 0.35f, 0.45);
	float Metallic = 0.0f;
	float Roughness = 0.0f;
	float AmbientOcclusion = 1.0f;
	vec3 Emission = vec3(0.0f);
	float Alpha = 1.0f;

	VkGuid AlbedoMapId;
	VkGuid MetallicRoughnessMapId;
	VkGuid MetallicMapId;
	VkGuid RoughnessMapId;
	VkGuid AmbientOcclusionMapId;
	VkGuid NormalMapId;
	VkGuid DepthMapId;
	VkGuid AlphaMapId;
	VkGuid EmissionMapId;
	VkGuid HeightMapId;

	VulkanBuffer<MaterialProperitiesBuffer> MaterialBuffer;

	Material();
	Material(const String& materialName, VkGuid& materialId);
	virtual ~Material();

	void UpdateBuffer();
	void GetMaterialPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& materialBufferList);
	void UpdateMaterialBufferIndex(uint32 bufferIndex);
	void Destroy();

	const uint32 GetMaterialBufferIndex() { return MaterialBufferIndex; }
	static const uint32 GetNextMaterialId() { return NextMaterialId; }
};

<<<<<<< Updated upstream:VulkanGameEngine/Material.h
=======
static int NextMaterialBufferId = 0;

DLL_EXPORT Material Material_CreateMaterial(const RendererStateDLL& renderer, int bufferIndex, VulkanBuffer& materialBuffer, const char* jsonString);
DLL_EXPORT void Material_UpdateBuffer(RendererStateDLL& rendererStateDLL, VulkanBuffer& materialBuffer, MaterialProperitiesBuffer& materialProperties);
DLL_EXPORT void Material_DestroyBuffer(const RendererStateDLL& rendererStateDLL, VulkanBuffer& materialBuffer);
>>>>>>> Stashed changes:VulkanEngineDLL/Material.h

#pragma once
#include <Texture.h>
#include <VulkanBuffer.h>
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
	
	uint ShaderMaterialBufferIndex = 0;

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

	int MaterialBufferId;

	Material();
	Material(const String& materialName, VkGuid& materialId);
	virtual ~Material();

	void UpdateBuffer();
	void GetMaterialPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& materialBufferList);
	void UpdateMaterialBufferIndex(uint32 shaderBufferIndex);
	void Destroy();

	static const uint32 GetNextMaterialId() { return NextMaterialId; }
};


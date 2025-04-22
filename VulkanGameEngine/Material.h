#pragma once
#include <Texture.h>
#include "VulkanBuffer.h"

struct MaterialProperitiesBuffer
{
	alignas(16) vec3 Albedo = vec3(0.0f, 0.35f, 0.45);
	alignas(4)  float Metallic = 0.0f;
	alignas(4)  float Roughness = 0.0f;
	alignas(4)  float AmbientOcclusion = 1.0f;
	alignas(16) vec3 Emission = vec3(0.0f);
	alignas(4)  float Alpha = 1.0f;

	alignas(4) uint32 AlbedoMap = 0;
	alignas(4) uint32 MetallicRoughnessMap = 0;
	alignas(4) uint32 MetallicMap = 0;
	alignas(4) uint32 RoughnessMap = 0;
	alignas(4) uint32 AmbientOcclusionMap = 0;
	alignas(4) uint32 NormalMap = 0;
	alignas(4) uint32 DepthMap = 0;
	alignas(4) uint32 AlphaMap = 0;
	alignas(4) uint32 EmissionMap = 0;
	alignas(4) uint32 HeightMap = 0;
};

class Material
{
private:
	uint32 MaterialID = 0;
	uint MaterialBufferIndex = 0;

	void UpdateBuffer();

public:
	static uint32 NextMaterialId;

	String Name;

	vec3 Albedo = vec3(0.0f, 0.35f, 0.45);
	float Metallic = 0.0f;
	float Roughness = 0.0f;
	float AmbientOcclusion = 1.0f;
	vec3 Emission = vec3(0.0f);
	float Alpha = 1.0f;

	uint32 AlbedoMap = 0;
	uint32 MetallicRoughnessMap = 0;
	uint32 MetallicMap = 0;
	uint32 RoughnessMap = 0;
	uint32 AmbientOcclusionMap = 0;
	uint32 NormalMap = 0;
	uint32 DepthMap = 0;
	uint32 AlphaMap = 0;
	uint32 EmissionMap = 0;
	uint32 HeightMap = 0;

	MaterialProperitiesBuffer MaterialInfo;
	VulkanBuffer<MaterialProperitiesBuffer> MaterialBuffer;

	Material();
	Material(const String& materialName);
	virtual ~Material();

	void GetMaterialPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& materialBufferList);
	void UpdateMaterialBufferIndex(uint64 bufferIndex);
	void Destroy();

	const uint32 GetMaterialId() { return MaterialID; }
	const uint32 GetMaterialBufferIndex() { return MaterialBufferIndex; }
};


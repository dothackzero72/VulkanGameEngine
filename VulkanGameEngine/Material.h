#pragma once
#include <Texture.h>
#include "VulkanBuffer.h"

struct Material2DProperitiesBuffer
{
	alignas(16) vec3 Albedo = vec3(0.0f, 0.35f, 0.45);
	alignas(16) vec3 Emission = vec3(0.0f);
	alignas(4)  float Alpha = 1.0f;

	alignas(4) uint32 AlbedoMapId = 0;
	alignas(4) uint32 NormalMapId = 0;
	alignas(4) uint32 AlphaMapId = 0;
};

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

class Material2D
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

	uint32 AlbedoMapId = 0;
	uint32 MetallicRoughnessMapId = 0;
	uint32 MetallicMapId = 0;
	uint32 RoughnessMapId = 0;
	uint32 AmbientOcclusionMapId = 0;
	uint32 NormalMapId = 0;
	uint32 DepthMapId = 0;
	uint32 AlphaMapId = 0;
	uint32 EmissionMapId = 0;
	uint32 HeightMapId = 0;

	Material2DProperitiesBuffer MaterialInfo;
	VulkanBuffer<Material2DProperitiesBuffer> MaterialBuffer;

	Material2D();
	Material2D(const String& materialName, uint32 materialId);
	virtual ~Material2D();

	void GetMaterialPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& materialBufferList);
	void UpdateMaterialBufferIndex(uint32 bufferIndex);
	void Destroy();

	const uint32 GetMaterialId() { return MaterialID; }
	const uint32 GetMaterialBufferIndex() { return MaterialBufferIndex; }
};

class Material
{
private:
	void UpdateBuffer();

public:
	static uint32 NextMaterialId;

	String Name;
	uint32 MaterialID = 0;
	uint MaterialBufferIndex = 0;

	vec3 Albedo = vec3(0.0f, 0.35f, 0.45);
	float Metallic = 0.0f;
	float Roughness = 0.0f;
	float AmbientOcclusion = 1.0f;
	vec3 Emission = vec3(0.0f);
	float Alpha = 1.0f;

	uint32 AlbedoMapId = 0;
	uint32 MetallicRoughnessMapId = 0;
	uint32 MetallicMapId = 0;
	uint32 RoughnessMapId = 0;
	uint32 AmbientOcclusionMapId = 0;
	uint32 NormalMapId = 0;
	uint32 DepthMapId = 0;
	uint32 AlphaMapId = 0;
	uint32 EmissionMapId = 0;
	uint32 HeightMapId = 0;

	MaterialProperitiesBuffer MaterialInfo;
	VulkanBuffer<MaterialProperitiesBuffer> MaterialBuffer;

	Material();
	Material(const String& materialName, uint32 materialId);
	virtual ~Material();

	void GetMaterialPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& materialBufferList);
	void UpdateMaterialBufferIndex(uint32 bufferIndex);
	void Destroy();

	const uint32 GetMaterialId() { return MaterialID; }
	const uint32 GetMaterialBufferIndex() { return MaterialBufferIndex; }
	static const uint32 GetNextMaterialId() { return NextMaterialId; }
};


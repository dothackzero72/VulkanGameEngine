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

	alignas(4) uint32 AlbedoMap = -1;
	alignas(4) uint32 MetallicRoughnessMap = -1;
	alignas(4) uint32 MetallicMap = -1;
	alignas(4) uint32 RoughnessMap = -1;
	alignas(4) uint32 AmbientOcclusionMap = -1;
	alignas(4) uint32 NormalMap = -1;
	alignas(4) uint32 DepthMap = -1;
	alignas(4) uint32 AlphaMap = -1;
	alignas(4) uint32 EmissionMap = -1;
	alignas(4) uint32 HeightMap = -1;
};

class Material
{
private:
	void UpdateBuffer();

public:
	static uint32 NextMaterialId;
	uint32 MaterialID = 0;
	uint MaterialBufferIndex = 0;

	VulkanBuffer<MaterialProperitiesBuffer> MaterialBuffer;

	vec3 Albedo = vec3(0.0f, 0.35f, 0.45);
	float Metallic = 0.0f;
	float Roughness = 0.0f;
	float AmbientOcclusion = 1.0f;
	vec3 Emission = vec3(0.0f);
	float Alpha = 1.0f;

	SharedPtr<Texture> AlbedoMap = nullptr;
	SharedPtr<Texture> MetallicRoughnessMap = nullptr;
	SharedPtr<Texture> MetallicMap = nullptr;
	SharedPtr<Texture> RoughnessMap = nullptr;
	SharedPtr<Texture> AmbientOcclusionMap = nullptr;
	SharedPtr<Texture> NormalMap = nullptr;
	SharedPtr<Texture> DepthMap = nullptr;
	SharedPtr<Texture> AlphaMap = nullptr;
	SharedPtr<Texture> EmissionMap = nullptr;
	SharedPtr<Texture> HeightMap = nullptr;

	Material();
	Material(const String& materialName);
	virtual ~Material();

	String Name;
	MaterialProperitiesBuffer MaterialInfo;

	void UpdateMaterialBufferIndex(uint64 bufferIndex);

	void Destroy();
	void GetMaterialPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& materialBufferList);

	void SetAlbedo(vec3 color);
	void SetMetallic(float value);
	void SetRoughness(float value);
	void SetAmbientOcclusion(float value);
	void SetEmission(vec3 color);
	void SetAlpha(float value);

	void SetAlbedoMap(SharedPtr<Texture> texture);
	void SetMetallicRoughnessMap(SharedPtr<Texture> texture);
	void SetMetallicMap(SharedPtr<Texture> texture);
	void SetRoughnessMap(SharedPtr<Texture> texture);
	void SetAmbientOcclusionMap(SharedPtr<Texture> texture);
	void SetNormalMap(SharedPtr<Texture> texture);
	void SetDepthMap(SharedPtr<Texture> texture);
	void SetAlphaMap(SharedPtr<Texture> texture);
	void SetEmissionMap(SharedPtr<Texture> texture);
	void SetHeightMap(SharedPtr<Texture> texture);

	uint64 GetMaterialBufferIndex() { return MaterialBufferIndex; }
	SharedPtr<Texture> GetAlbedoMap() { return AlbedoMap; }
	SharedPtr<Texture> GetMetallicRoughnessMap() { return MetallicRoughnessMap; }
	SharedPtr<Texture> GetMetallicMap() { return MetallicMap; }
	SharedPtr<Texture> GetRoughnessMap() { return RoughnessMap; }
	SharedPtr<Texture> GetAmbientOcclusionMap() { return AmbientOcclusionMap; }
	SharedPtr<Texture> GetNormalMap() { return NormalMap; }
	SharedPtr<Texture> GetDepthMap() { return DepthMap; }
	SharedPtr<Texture> GetAlphaMap() { return AlphaMap; }
	SharedPtr<Texture> GetEmissionMap() { return EmissionMap; }
	SharedPtr<Texture> GetHeightMap() { return HeightMap; }
};


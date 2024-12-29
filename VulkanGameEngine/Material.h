#pragma once
#include "Texture.h"
#include "VulkanBuffer.h"

struct MaterialBufferInfo
{
	alignas(16) glm::vec3 Albedo = glm::vec3(0.0f, 0.35f, 0.45);
	alignas(4) float Metallic = 0.0f;
	alignas(4) float Roughness = 0.0f;
	alignas(4) float AmbientOcclusion = 1.0f;
	alignas(16) glm::vec3 Emission = glm::vec3(0.0f);
	alignas(4) float Alpha = 1.0f;

	alignas(4) uint32_t AlbedoMap = -1;
	alignas(4) uint32_t MetallicRoughnessMap = -1;
	alignas(4) uint32_t MetallicMap = -1;
	alignas(4) uint32_t RoughnessMap = -1;
	alignas(4) uint32_t AmbientOcclusionMap = -1;
	alignas(4) uint32_t NormalMap = -1;
	alignas(4) uint32_t DepthMap = -1;
	alignas(4) uint32_t AlphaMap = -1;
	alignas(4) uint32_t EmissionMap = -1;
	alignas(4) uint32_t HeightMap = -1;
};

class Material
{
private:
	static uint64_t MaterialIDCounter;

	uint64_t MaterialID = 0;
	uint64_t MaterialBufferIndex = 0;

	VulkanBuffer<MaterialBufferInfo> MaterialBuffer;

	glm::vec3 Albedo = glm::vec3(0.0f, 0.35f, 0.45);
	float Metallic = 0.0f;
	float Roughness = 0.0f;
	float AmbientOcclusion = 1.0f;
	glm::vec3 Emission = glm::vec3(0.0f);
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

	void GenerateID();
	void UpdateBuffer();

public:
	Material();
	Material(const std::string& materialName);
	~Material();

	std::string MaterialName;
	MaterialBufferInfo MaterialInfo;

	void UpdateMaterialBufferIndex(uint64_t bufferIndex);

	void Destroy();
	void GetMaterialPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& MaterialBufferList);

	void SetAlbedo(glm::vec3 color);
	void SetMetallic(float value);
	void SetRoughness(float value);
	void SetAmbientOcclusion(float value);
	void SetEmission(glm::vec3 color);
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

	uint64_t GetMaterialBufferIndex() { return MaterialBufferIndex; }
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


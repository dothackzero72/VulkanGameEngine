#pragma once
#include "DLL.h"
#include "Typedef.h"
#include "json.h"
#include "CoreVulkanRenderer.h"
#include "VulkanBuffer.h"

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

struct Material
{
	VkGuid MaterialId;
	uint ShaderMaterialBufferIndex;
	int MaterialBufferId;

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

	vec3 Albedo = vec3(0.0f, 0.35f, 0.45);
	vec3 Emission = vec3(0.0f);
	float Metallic = 0.0f;
	float Roughness = 0.0f;
	float AmbientOcclusion = 1.0f;
	float Alpha = 1.0f;
};

DLL_EXPORT VulkanBuffer Material_CreateMaterialBuffer(const RendererState& renderer, uint bufferId);
DLL_EXPORT void Material_UpdateBuffer(const RendererState& renderer, VulkanBuffer& materialBuffer, MaterialProperitiesBuffer& materialProperties);
DLL_EXPORT void Material_DestroyBuffer(const RendererState& renderer, VulkanBuffer& materialBuffer);
#pragma once
#include <vulkan/vulkan_core.h>
#include "Typedef.h"
#include "VkGuid.h"

struct MeshPropertiesStruct
{
	alignas(4)  uint32 MaterialIndex = 0;
	alignas(16) mat4   MeshTransform = mat4(1.0f);
};

struct MeshStruct
{
	uint32 MeshId = 0;
	uint32 ParentGameObjectID = 0;
	uint32 GameObjectTransform = 0;
	uint32 VertexCount = 0;
	uint32 IndexCount = 0;
	VkGuid MaterialId;

	int	MeshVertexBufferId;
	int	MeshIndexBufferId;
	int MeshTransformBufferId;
	int	PropertiesBufferId;

	vec3 MeshPosition = vec3(0.0f);
	vec3 MeshRotation = vec3(0.0f);
	vec3 MeshScale = vec3(1.0f);
	vec3 LastMeshPosition = vec3(0.0f);
	vec3 LastMeshRotation = vec3(0.0f);
	vec3 LastMeshScale = vec3(1.0f);

	MeshPropertiesStruct MeshProperties;
};
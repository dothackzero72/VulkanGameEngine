#pragma once
#include <vulkan/vulkan_core.h>
#include "DLL.h"
#include "Typedef.h"
#include "VkGuid.h"
#include "VulkanBuffer.h"

struct MeshPropertiesStruct
{
	alignas(4)  uint32 ShaderMaterialBufferIndex = 0;
	alignas(16) mat4   MeshTransform = mat4(1.0f);
};

const VkBufferUsageFlags MeshBufferUsageSettings = VK_BUFFER_USAGE_VERTEX_BUFFER_BIT |
VK_BUFFER_USAGE_INDEX_BUFFER_BIT |
VK_BUFFER_USAGE_STORAGE_BUFFER_BIT |
VK_BUFFER_USAGE_TRANSFER_SRC_BIT |
VK_BUFFER_USAGE_TRANSFER_DST_BIT;

const VkMemoryPropertyFlags MeshBufferPropertySettings = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

struct VertexLoaderStruct
{
	BufferTypeEnum VertexType;
	uint32 MeshVertexBufferId;
	uint32 SizeofVertex;
	uint32 VertexCount;
	void*  VertexData;
};

struct IndexLoaderStruct
{
	uint32 MeshIndexBufferId;
	uint32 SizeofIndex;
	uint32 IndexCount;
	void*  IndexData;
};

struct TransformLoaderStruct
{
	uint32 MeshTransformBufferId;
	uint32 SizeofTransform;
	void*  TransformData;
};

struct MeshPropertiesLoaderStruct
{
	uint32 PropertiesBufferId;
	uint32 SizeofMeshProperties;
	void*  MeshPropertiesData;
};

struct MeshLoader
{
	uint ParentGameObjectID;
	uint MeshId;
	VkGuid MaterialId;

	VertexLoaderStruct VertexLoader;
	IndexLoaderStruct IndexLoader;
	TransformLoaderStruct TransformLoader;
	MeshPropertiesLoaderStruct MeshPropertiesLoader;
};

struct Mesh
{
	uint32 MeshId = 0;
	uint32 ParentGameObjectID = 0;
	uint32 GameObjectTransform = 0;
	uint32 VertexCount = 0;
	uint32 IndexCount = 0;
	VkGuid MaterialId;

	BufferTypeEnum VertexType;
	vec3 MeshPosition = vec3(0.0f);
	vec3 MeshRotation = vec3(0.0f);
	vec3 MeshScale = vec3(1.0f);
	vec3 LastMeshPosition = vec3(0.0f);
	vec3 LastMeshRotation = vec3(0.0f);
	vec3 LastMeshScale = vec3(1.0f);

	int	MeshVertexBufferId;
	int	MeshIndexBufferId;
	int MeshTransformBufferId;
	int	PropertiesBufferId;

	MeshPropertiesStruct MeshProperties;
};

DLL_EXPORT Mesh Mesh_CreateMesh(const RendererState& renderer, const MeshLoader& meshLoader, VulkanBuffer& outVertexBuffer, VulkanBuffer& outIndexBuffer, VulkanBuffer& outTransformBuffer, VulkanBuffer& outPropertiesBufferId);

int Mesh_CreateVertexBuffer(const RendererState& renderer, const VertexLoaderStruct& vertexLoader, VulkanBuffer& outVertexBuffer);
int Mesh_CreateIndexBuffer(const RendererState& renderer, const IndexLoaderStruct& indexLoader, VulkanBuffer& outIndexBuffer);
int Mesh_CreateTransformBuffer(const RendererState& renderer, const TransformLoaderStruct& transformLoader, VulkanBuffer& outTransformBuffer);
int Mesh_CreateMeshPropertiesBuffer(const RendererState& renderer, const MeshPropertiesLoaderStruct& meshPropertiesLoader, VulkanBuffer& outPropertiesBufferId);
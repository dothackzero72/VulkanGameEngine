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

struct MeshLoader
{
	BufferTypeEnum VertexType;
	uint MeshId;
	VkGuid MaterialId;
	uint ParentGameObjectID = 0;

	uint32	MeshVertexBufferId;
	uint32 sizeofVertex;
	uint32 vertexCount;
	void* vertexData;
	
	uint32	MeshIndexBufferId;
	uint32 sizeofIndex;
	uint32 indexCount;
	void* indexData;
	
	uint32 MeshTransformBufferId;
	uint32 sizeofTransform;
	void* transformData;

	uint32	PropertiesBufferId;
	uint32 sizeofMeshProperties;
	void* meshPropertiesData;
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
};

DLL_EXPORT Mesh Mesh_CreateMesh(const RendererState& renderer, MeshLoader& meshLoader, VulkanBuffer& vertexBuffer, VulkanBuffer& indexBuffer, VulkanBuffer& transformBuffer, VulkanBuffer& propertiesBufferId);

int Mesh_CreateVertexBuffer(const RendererState& renderer, uint bufferId, VulkanBuffer& insertVertexBuffer, BufferTypeEnum bufferType, uint32 sizeofVertex, uint32 vertexCount, void* vertexData);
int Mesh_CreateIndexBuffer(const RendererState& renderer, uint bufferId, VulkanBuffer& insertIndexBuffer, uint32 sizeofIndex, uint32 indexCount, void* indexData);
int Mesh_CreateTransformBuffer(const RendererState& renderer, uint bufferId, VulkanBuffer& insertTransformBuffer, uint32 sizeofTransform, void* transformData);
int Mesh_CreateMeshPropertiesBuffer(const RendererState& renderer, uint bufferId, VulkanBuffer& insertMeshBuffer, uint32 sizeofMeshProperties, void* meshPropertiesData);
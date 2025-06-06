#include "Mesh.h"

Mesh Mesh_CreateMesh(const RendererState& renderer, MeshLoader& meshLoader, VulkanBuffer& vertexBuffer, VulkanBuffer& indexBuffer, VulkanBuffer& transformBuffer, VulkanBuffer& propertiesBuffer)
{
	auto vertexType = meshLoader.VertexType;
	Mesh mesh = Mesh
	{
		.MeshId = meshLoader.MeshId,
		.ParentGameObjectID = meshLoader.ParentGameObjectID,
		.VertexCount = meshLoader.vertexCount,
		.IndexCount = meshLoader.indexCount,
		.MaterialId = meshLoader.MaterialId,
		.VertexType = vertexType,
		.MeshVertexBufferId = Mesh_CreateVertexBuffer(cRenderer, meshLoader.MeshVertexBufferId, vertexBuffer, (BufferTypeEnum)meshLoader.VertexType, meshLoader.sizeofVertex, meshLoader.vertexCount, meshLoader.vertexData),
		.MeshIndexBufferId = Mesh_CreateIndexBuffer(cRenderer, meshLoader.MeshIndexBufferId, indexBuffer, meshLoader.sizeofIndex, meshLoader.vertexCount, meshLoader.indexData),
		.MeshTransformBufferId = Mesh_CreateTransformBuffer(cRenderer, meshLoader.MeshTransformBufferId, transformBuffer, meshLoader.sizeofTransform, meshLoader.transformData),
		.PropertiesBufferId = Mesh_CreateMeshPropertiesBuffer(cRenderer, meshLoader.PropertiesBufferId, propertiesBuffer, meshLoader.sizeofMeshProperties, meshLoader.meshPropertiesData),
	};

	return mesh;
}

int Mesh_CreateVertexBuffer(const RendererState& renderer, uint bufferId, VulkanBuffer& insertVertexBuffer, BufferTypeEnum bufferType, uint32 sizeofVertex, uint32 vertexCount, void* vertexData)
{
	insertVertexBuffer = VulkanBuffer_CreateVulkanBuffer(renderer, bufferId, vertexData, sizeofVertex, vertexCount, bufferType, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
	return insertVertexBuffer.BufferId;
}

int Mesh_CreateIndexBuffer(const RendererState& renderer, uint bufferId, VulkanBuffer& insertIndexBuffer, uint32 sizeofIndex, uint32 indexCount, void* indexData)
{
	insertIndexBuffer = VulkanBuffer_CreateVulkanBuffer(renderer, bufferId, indexData, sizeofIndex, indexCount, BufferType_UInt, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
	return insertIndexBuffer.BufferId;
}

int Mesh_CreateTransformBuffer(const RendererState& renderer, uint bufferId, VulkanBuffer& insertTransformBuffer, uint32 sizeofTransform, void* transformData)
{
	insertTransformBuffer = VulkanBuffer_CreateVulkanBuffer(renderer, bufferId, transformData, sizeofTransform, 1, BufferType_Mat4, MeshBufferUsageSettings, MeshBufferPropertySettings, false);
	return insertTransformBuffer.BufferId;
}

int Mesh_CreateMeshPropertiesBuffer(const RendererState& renderer, uint bufferId, VulkanBuffer& insertMeshBuffer, uint32 sizeofMeshProperties, void* meshPropertiesData)
{
	insertMeshBuffer = VulkanBuffer_CreateVulkanBuffer(renderer, bufferId, meshPropertiesData, sizeofMeshProperties, 1, BufferType_MeshPropertiesStruct, MeshBufferUsageSettings, MeshBufferPropertySettings, false);
	return insertMeshBuffer.BufferId;
}

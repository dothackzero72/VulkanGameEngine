#include "Mesh.h"

Mesh Mesh_CreateMesh(const RendererState& renderer, const MeshLoader& meshLoader, VulkanBuffer& outVertexBuffer, VulkanBuffer& outIndexBuffer, VulkanBuffer& outTransformBuffer, VulkanBuffer& outMeshPropertiesBuffer)
{
	return  Mesh
	{
		.MeshId = meshLoader.MeshId,
		.ParentGameObjectID = meshLoader.ParentGameObjectID,
		.VertexCount = meshLoader.VertexLoader.VertexCount,
		.IndexCount = meshLoader.IndexLoader.IndexCount,
		.MaterialId = meshLoader.MaterialId,
		.VertexType = meshLoader.VertexLoader.VertexType,
		.MeshVertexBufferId = Mesh_CreateVertexBuffer(cRenderer, meshLoader.VertexLoader, outVertexBuffer),
		.MeshIndexBufferId = Mesh_CreateIndexBuffer(cRenderer, meshLoader.IndexLoader, outIndexBuffer),
		.MeshTransformBufferId = Mesh_CreateTransformBuffer(cRenderer, meshLoader.TransformLoader, outTransformBuffer),
		.PropertiesBufferId = Mesh_CreateMeshPropertiesBuffer(cRenderer, meshLoader.MeshPropertiesLoader, outMeshPropertiesBuffer),
	};
}

int Mesh_CreateVertexBuffer(const RendererState& renderer, const VertexLoaderStruct& vertexLoader, VulkanBuffer& outVertexBuffer)
{
	outVertexBuffer = VulkanBuffer_CreateVulkanBuffer(renderer, vertexLoader.MeshVertexBufferId, vertexLoader.VertexData, vertexLoader.SizeofVertex, vertexLoader.VertexCount, vertexLoader.VertexType, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
	return outVertexBuffer.BufferId;
}

int Mesh_CreateIndexBuffer(const RendererState& renderer, const IndexLoaderStruct& indexLoader, VulkanBuffer& outIndexBuffer)
{
	outIndexBuffer = VulkanBuffer_CreateVulkanBuffer(renderer, indexLoader.MeshIndexBufferId, indexLoader.IndexData, indexLoader.SizeofIndex, indexLoader.IndexCount, BufferType_UInt, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
	return outIndexBuffer.BufferId;
}

int Mesh_CreateTransformBuffer(const RendererState& renderer, const TransformLoaderStruct& transformLoader, VulkanBuffer& outTransformBuffer)
{
	outTransformBuffer = VulkanBuffer_CreateVulkanBuffer(renderer, transformLoader.MeshTransformBufferId, transformLoader.TransformData, transformLoader.SizeofTransform, 1, BufferType_Mat4, MeshBufferUsageSettings, MeshBufferPropertySettings, false);
	return outTransformBuffer.BufferId;
}

int Mesh_CreateMeshPropertiesBuffer(const RendererState& renderer, const MeshPropertiesLoaderStruct& meshPropertiesLoader, VulkanBuffer& outMeshPropertiesBuffer)
{
	outMeshPropertiesBuffer = VulkanBuffer_CreateVulkanBuffer(renderer, meshPropertiesLoader.PropertiesBufferId, meshPropertiesLoader.MeshPropertiesData, meshPropertiesLoader.SizeofMeshProperties, 1, BufferType_MeshPropertiesStruct, MeshBufferUsageSettings, MeshBufferPropertySettings, false);
	return outMeshPropertiesBuffer.BufferId;
}

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

void Mesh_UpdateMesh(const RendererState& renderer, Mesh& mesh, VulkanBuffer& meshPropertiesBuffer, uint32 shaderMaterialBufferIndex, const float& deltaTime)
{
	const vec3 LastMeshPosition = mesh.MeshPosition;
	const vec3 LastMeshRotation = mesh.MeshRotation;
	const vec3 LastMeshScale = mesh.MeshScale;

	mat4 GameObjectMatrix = mat4(1.0);
	//SharedPtr<Transform2DComponent> transform = GameObjectTransform.lock();
	//if (transform)
	//{
	//		GameObjectMatrix = *transform->GameObjectMatrixTransform.get();
	//}

	mat4 MeshMatrix = mat4(1.0f);
	if (LastMeshPosition != mesh.MeshPosition)
	{
		MeshMatrix = glm::translate(MeshMatrix, mesh.MeshPosition);
	}
	if (LastMeshRotation != mesh.MeshRotation)
	{
		MeshMatrix = glm::rotate(MeshMatrix, glm::radians(mesh.MeshRotation.x), vec3(1.0f, 0.0f, 0.0f));
		MeshMatrix = glm::rotate(MeshMatrix, glm::radians(mesh.MeshRotation.y), vec3(0.0f, 1.0f, 0.0f));
		MeshMatrix = glm::rotate(MeshMatrix, glm::radians(mesh.MeshRotation.z), vec3(0.0f, 0.0f, 1.0f));
	}
	if (LastMeshScale != mesh.MeshScale)
	{
		MeshMatrix = glm::scale(MeshMatrix, mesh.MeshScale);
	}

	MeshPropertiesStruct meshProperties = MeshPropertiesStruct
	{
		.ShaderMaterialBufferIndex = shaderMaterialBufferIndex,
		.MeshTransform = GameObjectMatrix * MeshMatrix,
	};

	VulkanBuffer_UpdateBufferMemory(renderer, meshPropertiesBuffer, static_cast<void*>(&meshProperties), sizeof(MeshPropertiesStruct), 1);
}

void Mesh_DestroyMesh(const RendererState& renderer, Mesh& mesh, VulkanBuffer& vertexBuffer, VulkanBuffer& indexBuffer, VulkanBuffer& transformBuffer, VulkanBuffer& propertiesBuffer)
{
	VulkanBuffer_DestroyBuffer(renderer, vertexBuffer);
	VulkanBuffer_DestroyBuffer(renderer, indexBuffer);
	VulkanBuffer_DestroyBuffer(renderer, transformBuffer);
	VulkanBuffer_DestroyBuffer(renderer, propertiesBuffer);
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

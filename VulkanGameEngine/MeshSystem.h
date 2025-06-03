#pragma once
#include "Vertex.h"
#include <Mesh.h>
#include "VulkanBufferSystem.h"
#include "GameObjectSystem.h"
#include "MaterialSystem.h"
#include "LevelSystem.h"
#include "GameSystem.h"

class MeshSystem
{
private:
	static uint NextMeshId;
	static uint NextSpriteMeshId;
	static uint NextLevelLayerMeshId;

	UnorderedMap<uint, Mesh>							  MeshMap;
	UnorderedMap<UM_SpriteBatchID, Mesh>				  SpriteMeshMap;
	UnorderedMap<LevelGuid, Vector<Mesh>>				  LevelLayerMeshListMap;
	UnorderedMap<uint, Vector<Vertex2D>>				  Vertex2DListMap;
	UnorderedMap<uint, Vector<uint>>					  IndexListMap;

public:
	const VkBufferUsageFlags MeshBufferUsageSettings = VK_BUFFER_USAGE_VERTEX_BUFFER_BIT |
		VK_BUFFER_USAGE_INDEX_BUFFER_BIT |
		VK_BUFFER_USAGE_STORAGE_BUFFER_BIT |
		VK_BUFFER_USAGE_TRANSFER_SRC_BIT |
		VK_BUFFER_USAGE_TRANSFER_DST_BIT;

	const VkMemoryPropertyFlags MeshBufferPropertySettings = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
		VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

	MeshSystem();
	~MeshSystem();

	template<class T>
	int CreateMesh(Vector<T>& vertexList, Vector<uint32>& indexList, VkGuid materialId)
	{
		uint meshId = NextMeshId++;
		mat4 meshMatrix = mat4(1.0f);
		Vertex2DListMap[meshId] = vertexList;
		IndexListMap[meshId] = indexList;
		MeshMap[meshId] = Mesh
		{
			.MaterialId = materialId,
			.MeshVertexList = vertexList,
			.MeshIndexList = indexList,
			.VertexCount = vertexList.size(),
			.IndexCount = indexList.size(),
			.MeshVertexBufferId = bufferSystem.CreateVulkanBuffer<T>(cRenderer, vertexList, MeshBufferUsageSettings, MeshBufferPropertySettings, true),
			.MeshIndexBufferId = bufferSystem.CreateVulkanBuffer<uint32>(cRenderer, indexList, MeshBufferUsageSettings, MeshBufferPropertySettings, true),
			.MeshTransformBufferId = bufferSystem.CreateVulkanBuffer<mat4>(cRenderer, meshMatrix, MeshBufferUsageSettings, MeshBufferPropertySettings, false),
			.PropertiesBufferId = bufferSystem.CreateVulkanBuffer<MeshPropertiesStruct>(cRenderer, MeshMap[meshId].MeshProperties, MeshBufferUsageSettings, MeshBufferPropertySettings, false)
		};
		return meshId;
	}

	template<class T>
	int CreateSpriteLayerMesh(Vector<T>& vertexList, Vector<uint32>& indexList)
	{
		uint meshId = ++NextSpriteMeshId;
		mat4 meshMatrix = mat4(1.0f);

		Vertex2DListMap[meshId] = vertexList;
		IndexListMap[meshId] = indexList;

		SpriteMeshMap[meshId] = Mesh();
		SpriteMeshMap[meshId].MaterialId = VkGuid();
		SpriteMeshMap[meshId].VertexCount = vertexList.size();
		SpriteMeshMap[meshId].IndexCount = indexList.size();
		SpriteMeshMap[meshId].MeshVertexBufferId = bufferSystem.CreateVulkanBuffer<T>(cRenderer, vertexList, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		SpriteMeshMap[meshId].MeshIndexBufferId = bufferSystem.CreateVulkanBuffer<uint32>(cRenderer, indexList, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		SpriteMeshMap[meshId].MeshTransformBufferId = bufferSystem.CreateVulkanBuffer<mat4>(cRenderer, meshMatrix, MeshBufferUsageSettings, MeshBufferPropertySettings, false);
		SpriteMeshMap[meshId].PropertiesBufferId = bufferSystem.CreateVulkanBuffer<MeshPropertiesStruct>(cRenderer, SpriteMeshMap[meshId].MeshProperties, MeshBufferUsageSettings, MeshBufferPropertySettings, false);
		return meshId;
	}

	template<class T>
	int CreateLevelLayerMesh(const VkGuid& levelId, Vector<T>& vertexList, Vector<uint32>& indexList)
	{
		uint meshId = ++NextLevelLayerMeshId;
		mat4 meshMatrix = mat4(1.0f);

		Vertex2DListMap[meshId] = vertexList;
		IndexListMap[meshId] = indexList;

		Vector<Mesh> meshStructList = Vector<Mesh>();
		Mesh meshStruct = Mesh();
		meshStruct.MaterialId = VkGuid();
		meshStruct.VertexCount = vertexList.size();
		meshStruct.IndexCount = indexList.size();
		meshStruct.MeshVertexBufferId = bufferSystem.CreateVulkanBuffer<T>(cRenderer, vertexList, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		meshStruct.MeshIndexBufferId = bufferSystem.CreateVulkanBuffer<uint32>(cRenderer, indexList, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		meshStruct.MeshTransformBufferId = bufferSystem.CreateVulkanBuffer<mat4>(cRenderer, meshMatrix, MeshBufferUsageSettings, MeshBufferPropertySettings, false);
		meshStruct.PropertiesBufferId = bufferSystem.CreateVulkanBuffer<MeshPropertiesStruct>(cRenderer, meshStruct.MeshProperties, MeshBufferUsageSettings, MeshBufferPropertySettings, false);

		meshStructList.emplace_back(meshStruct);
		LevelLayerMeshListMap[levelId] = meshStructList;
		return meshId;
	}

	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
	{
		for (auto& mesh : SpriteMeshMap)
		{
			mat4 GameObjectMatrix = mat4(1.0);
			//SharedPtr<Transform2DComponent> transform = GameObjectTransform.lock();
			//if (transform)
			//{
			//		GameObjectMatrix = *transform->GameObjectMatrixTransform.get();
			//}

			mat4 MeshMatrix = mat4(1.0f);
			if (mesh.second.LastMeshPosition != mesh.second.MeshPosition)
			{
				MeshMatrix = glm::translate(MeshMatrix, mesh.second.MeshPosition);
				mesh.second.LastMeshPosition == mesh.second.MeshPosition;
			}
			if (mesh.second.LastMeshRotation != mesh.second.MeshRotation)
			{
				MeshMatrix = glm::rotate(MeshMatrix, glm::radians(mesh.second.MeshRotation.x), vec3(1.0f, 0.0f, 0.0f));
				MeshMatrix = glm::rotate(MeshMatrix, glm::radians(mesh.second.MeshRotation.y), vec3(0.0f, 1.0f, 0.0f));
				MeshMatrix = glm::rotate(MeshMatrix, glm::radians(mesh.second.MeshRotation.z), vec3(0.0f, 0.0f, 1.0f));
				mesh.second.LastMeshRotation == mesh.second.MeshRotation;
			}
			if (mesh.second.LastMeshScale != mesh.second.MeshScale)
			{
				MeshMatrix = glm::scale(MeshMatrix, mesh.second.MeshScale);
				mesh.second.LastMeshScale == mesh.second.MeshScale;
			}

			mesh.second.MeshProperties.ShaderMaterialBufferIndex = (mesh.second.MaterialId != VkGuid()) ? materialSystem.FindMaterial(mesh.second.MaterialId).ShaderMaterialBufferIndex : 0;
			mesh.second.MeshProperties.MeshTransform = GameObjectMatrix * MeshMatrix;
			bufferSystem.UpdateBufferMemory<MeshPropertiesStruct>(cRenderer, mesh.second.PropertiesBufferId, mesh.second.MeshProperties);
		}
	}

	//void Destroy(uint meshId)
	//{
	//	//const MeshStruct& mesh = MeshMap[meshId];
	//	//bufferSystem.DestroyBuffer(cRenderer, mesh.MeshVertexBufferId);
	//	//bufferSystem.DestroyBuffer(cRenderer, mesh.MeshIndexBufferId);
	//	//bufferSystem.DestroyBuffer(cRenderer, mesh.MeshTransformBufferId);
	//	//bufferSystem.DestroyBuffer(cRenderer, mesh.PropertiesBufferId);
	//}

	//void SystemShutDown()
	//{
	//	/*for (auto& mesh : MeshMap)
	//	{
	//		bufferSystem.DestroyBuffer(cRenderer, mesh.second.MeshVertexBufferId);
	//		bufferSystem.DestroyBuffer(cRenderer, mesh.second.MeshIndexBufferId);
	//		bufferSystem.DestroyBuffer(cRenderer, mesh.second.MeshTransformBufferId);
	//		bufferSystem.DestroyBuffer(cRenderer, mesh.second.PropertiesBufferId);
	//	}*/
	//}

	const Mesh& FindMesh(const uint& id);
	const Mesh& FindSpriteMesh(const uint& id);
	const Vector<Mesh>& FindLevelLayerMeshList(const LevelGuid& guid);
	const Vector<Vertex2D>& FindVertex2DList(const uint& id);
	const Vector<uint>& FindIndexList(const uint& id);

	const Vector<Mesh> MeshList();
	const Vector<Mesh> SpriteMeshList();

	void DestroyAllGameObjects();
};
extern MeshSystem meshSystem;


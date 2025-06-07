#pragma once
#include "Vertex.h"
#include <Mesh.h>
#include "BufferSystem.h"
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

	MeshSystem();
	~MeshSystem();

	template<class T>
	int CreateMesh(Vector<T>& vertexList, Vector<uint32>& indexList, VkGuid materialId)
	{
		uint meshId = NextMeshId++;
		mat4 meshMatrix = mat4(1.0f);
		Vertex2DListMap[meshId] = vertexList;
		IndexListMap[meshId] = indexList;

		MeshLoader meshLoader =
		{
			.ParentGameObjectID = 0,
			.VertexType = BufferTypeEnum::BufferType_Vector2D,
			.MeshId = meshId,
			.MaterialId = materialId,
			.VertexLoader = VertexLoaderStruct
			{
				.MeshVertexBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofVertex = sizeof(T),
				.VertexCount = static_cast<uint32>(vertexList.size()),
				.VertexData = static_cast<void*>(vertexList.data()),
			},
			.IndexLoader = IndexLoaderStruct
			{
				.MeshIndexBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofIndex = sizeof(uint),
				.IndexCount = static_cast<uint32>(indexList.size()),
				.IndexData = static_cast<void*>(indexList.data()),
			},
			.TransformLoader = TransformLoaderStruct
			{
				.MeshTransformBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofTransform = sizeof(mat4),
				.TransformData = static_cast<void*>(&meshMatrix),
			},
			.MeshPropertiesLoader = MeshPropertiesLoaderStruct
			{
				.PropertiesBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofMeshProperties = sizeof(MeshPropertiesStruct),
				.MeshPropertiesData = static_cast<void*>(&MeshMap[meshId].MeshProperties)
			}
		};

		Mesh mesh = Mesh_CreateMesh(cRenderer, meshLoader, bufferSystem.VulkanBufferMap[meshLoader.VertexLoader.MeshVertexBufferId],
														   bufferSystem.VulkanBufferMap[meshLoader.IndexLoader.MeshIndexBufferId],
														   bufferSystem.VulkanBufferMap[meshLoader.TransformLoader.MeshTransformBufferId],
														   bufferSystem.VulkanBufferMap[meshLoader.MeshPropertiesLoader.PropertiesBufferId]);
		return meshId;
	}

	template<class T>
	int CreateSpriteLayerMesh(Vector<T>& vertexList, Vector<uint32>& indexList)
	{
		uint meshId = ++NextSpriteMeshId;
		mat4 meshMatrix = mat4(1.0f);

		Vertex2DListMap[meshId] = vertexList;
		IndexListMap[meshId] = indexList;

		MeshLoader meshLoader =
		{
			.ParentGameObjectID = 0,
			.MeshId = meshId,
			.MaterialId = VkGuid(),
			.VertexLoader = VertexLoaderStruct
			{
				.VertexType = BufferTypeEnum::BufferType_Vector2D,
				.MeshVertexBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofVertex = sizeof(T),
				.VertexCount = static_cast<uint32>(vertexList.size()),
				.VertexData = static_cast<void*>(vertexList.data()),
			},
			.IndexLoader = IndexLoaderStruct
			{
				.MeshIndexBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofIndex = sizeof(uint),
				.IndexCount = static_cast<uint32>(indexList.size()),
				.IndexData = static_cast<void*>(indexList.data()),
			},
			.TransformLoader = TransformLoaderStruct
			{
				.MeshTransformBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofTransform = sizeof(mat4),
				.TransformData = static_cast<void*>(&meshMatrix),
			},
			.MeshPropertiesLoader = MeshPropertiesLoaderStruct
			{
				.PropertiesBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofMeshProperties = sizeof(MeshPropertiesStruct),
				.MeshPropertiesData = static_cast<void*>(&MeshMap[meshId].MeshProperties)
			}
		};

		Mesh mesh = Mesh_CreateMesh(cRenderer, meshLoader, bufferSystem.VulkanBufferMap[meshLoader.VertexLoader.MeshVertexBufferId],
														   bufferSystem.VulkanBufferMap[meshLoader.IndexLoader.MeshIndexBufferId],
														   bufferSystem.VulkanBufferMap[meshLoader.TransformLoader.MeshTransformBufferId],
														   bufferSystem.VulkanBufferMap[meshLoader.MeshPropertiesLoader.PropertiesBufferId]);
		SpriteMeshMap[meshId] = mesh;
		return meshId;
	}

	template<class T>
	int CreateLevelLayerMesh(const VkGuid& levelId, Vector<T>& vertexList, Vector<uint32>& indexList)
	{
		uint meshId = ++NextLevelLayerMeshId;
		mat4 meshMatrix = mat4(1.0f);

		Vertex2DListMap[meshId] = vertexList;
		IndexListMap[meshId] = indexList;

		MeshLoader meshLoader =
		{
			.ParentGameObjectID = 0,
			.MeshId = meshId,
			.MaterialId = VkGuid(),
			.VertexLoader = VertexLoaderStruct
			{
				.VertexType = BufferTypeEnum::BufferType_Vector2D,
				.MeshVertexBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofVertex = sizeof(T),
				.VertexCount = static_cast<uint32>(vertexList.size()),
				.VertexData = static_cast<void*>(vertexList.data()),
			},
			.IndexLoader = IndexLoaderStruct
			{
				.MeshIndexBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofIndex = sizeof(uint),
				.IndexCount = static_cast<uint32>(indexList.size()),
				.IndexData = static_cast<void*>(indexList.data()),
			},
			.TransformLoader = TransformLoaderStruct
			{
				.MeshTransformBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofTransform = sizeof(mat4),
				.TransformData = static_cast<void*>(&meshMatrix),
			},
			.MeshPropertiesLoader = MeshPropertiesLoaderStruct
			{
				.PropertiesBufferId = static_cast<uint32>(++bufferSystem.NextBufferId),
				.SizeofMeshProperties = sizeof(MeshPropertiesStruct),
				.MeshPropertiesData = static_cast<void*>(&MeshMap[meshId].MeshProperties)
			}
		};

		Vector<Mesh> meshList = Vector<Mesh>
		{
			Mesh_CreateMesh(cRenderer, meshLoader, bufferSystem.VulkanBufferMap[meshLoader.VertexLoader.MeshVertexBufferId],
												   bufferSystem.VulkanBufferMap[meshLoader.IndexLoader.MeshIndexBufferId],
												   bufferSystem.VulkanBufferMap[meshLoader.TransformLoader.MeshTransformBufferId],
												   bufferSystem.VulkanBufferMap[meshLoader.MeshPropertiesLoader.PropertiesBufferId])
		};
		LevelLayerMeshListMap[levelId] = meshList;
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


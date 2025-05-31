#pragma once
#include <Mesh.h>
#include "VulkanBufferSystem.h"
#include "AssetManager.h"

typedef Mesh<Vertex2D> SpriteLayerMesh;
typedef Mesh<Vertex2D> LevelLayerMesh;

class MeshSystem
{
private:
	static int NextMeshId;

public:
	const VkBufferUsageFlags MeshBufferUsageSettings = VK_BUFFER_USAGE_VERTEX_BUFFER_BIT |
		VK_BUFFER_USAGE_INDEX_BUFFER_BIT |
		VK_BUFFER_USAGE_STORAGE_BUFFER_BIT |
		VK_BUFFER_USAGE_TRANSFER_SRC_BIT |
		VK_BUFFER_USAGE_TRANSFER_DST_BIT;

	const VkMemoryPropertyFlags MeshBufferPropertySettings = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
		VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

	UnorderedMap<uint, MeshStruct>								  MeshList;
	UnorderedMap<UM_SpriteBatchID, SpriteLayerMesh>               SpriteMeshList;
	UnorderedMap<LevelGuid, Vector<LevelLayerMesh>>               LevelLayerMeshList;

	MeshSystem();
	~MeshSystem();

	template<class T>
	int CreateMesh(Vector<T>& vertexList, Vector<uint32>& indexList, VkGuid materialId)
	{
		int meshId = NextMeshId++;
		mat4 meshMatrix = mat4(1.0f);
		MeshList[meshId] = MeshStruct
		{
			.MaterialId = materialId,
			.MeshVertexList = vertexList,
			.MeshIndexList = indexList,
			.VertexCount = vertexList.size(),
			.IndexCount = indexList.size(),
			.MeshVertexBufferId = bufferSystem.CreateVulkanBuffer<T>(cRenderer, vertexList, MeshBufferUsageSettings, MeshBufferPropertySettings, true),
			.MeshIndexBufferId = bufferSystem.CreateVulkanBuffer<uint32>(cRenderer, indexList, MeshBufferUsageSettings, MeshBufferPropertySettings, true),
			.MeshTransformBufferId = bufferSystem.CreateVulkanBuffer<mat4>(cRenderer, meshMatrix, MeshBufferUsageSettings, MeshBufferPropertySettings, false),
			.PropertiesBufferId = bufferSystem.CreateVulkanBuffer<MeshPropertiesStruct>(cRenderer, MeshList[meshId].MeshProperties, MeshBufferUsageSettings, MeshBufferPropertySettings, false)
		};
		return meshId;
	}

	template<class T>
	int CreateSpriteLayerMesh(Vector<T>& vertexList, Vector<uint32>& indexList)
	{
		int meshId = NextMeshId++;
		mat4 meshMatrix = mat4(1.0f);
		SpriteMeshList[meshId] = MeshStruct
		{
			.MaterialId = VkGuid(),
			.MeshVertexList = vertexList,
			.MeshIndexList = indexList,
			.VertexCount = vertexList.size(),
			.IndexCount = indexList.size(),
			.MeshVertexBufferId = bufferSystem.CreateVulkanBuffer<T>(cRenderer, vertexList, MeshBufferUsageSettings, MeshBufferPropertySettings, true),
			.MeshIndexBufferId = bufferSystem.CreateVulkanBuffer<uint32>(cRenderer, indexList, MeshBufferUsageSettings, MeshBufferPropertySettings, true),
			.MeshTransformBufferId = bufferSystem.CreateVulkanBuffer<mat4>(cRenderer, meshMatrix, MeshBufferUsageSettings, MeshBufferPropertySettings, false),
			.PropertiesBufferId = bufferSystem.CreateVulkanBuffer<MeshPropertiesStruct>(cRenderer, SpriteMeshList[meshId].MeshProperties, MeshBufferUsageSettings, MeshBufferPropertySettings, false)
		};
		return meshId;
	}

	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
	{
		for (auto& mesh : MeshList)
		{
			Material& material = assetManager.MaterialList[mesh.second.MaterialId];
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

			mesh.second.MeshProperties.MaterialIndex = (mesh.second.MaterialId == VkGuid()) ? material.GetMaterialBufferIndex() : 0;
			mesh.second.MeshProperties.MeshTransform = GameObjectMatrix * MeshMatrix;
			bufferSystem.UpdateBufferMemory<MeshPropertiesStruct>(cRenderer, mesh.second.PropertiesBufferId, mesh.second.MeshProperties);
		}
	}

	VkGuid LoadLevelLayout(const String& levelLayoutPath)
	{
		if (levelLayoutPath.empty() ||
			levelLayoutPath == "")
		{
			return VkGuid();
		}

		nlohmann::json json = Json::ReadJson(levelLayoutPath);
		VkGuid levelLayoutId = VkGuid(json["LevelLayoutId"].get<String>().c_str());

		levelLayout.LevelLayoutId = VkGuid(json["LevelLayoutId"].get<String>().c_str());
		levelLayout.LevelBounds = ivec2(json["LevelBounds"][0], json["LevelBounds"][1]);
		levelLayout.TileSizeinPixels = ivec2(json["TileSizeInPixels"][0], json["TileSizeInPixels"][1]);
		for (int x = 0; x < json["LevelLayouts"].size(); x++)
		{
			Vector<uint> levelLayer;
			for (int y = 0; y < json["LevelLayouts"][x].size(); y++)
			{
				for (int z = 0; z < json["LevelLayouts"][x][y].size(); z++)
				{
					levelLayer.emplace_back(json["LevelLayouts"][x][y][z]);
				}
			}
			levelLayout.LevelMapList.emplace_back(levelLayer);
		}
	}

	void Destroy(uint meshId)
	{
		const MeshStruct& mesh = MeshList[meshId];
		bufferSystem.DestroyBuffer(cRenderer, mesh.MeshVertexBufferId);
		bufferSystem.DestroyBuffer(cRenderer, mesh.MeshIndexBufferId);
		bufferSystem.DestroyBuffer(cRenderer, mesh.MeshTransformBufferId);
		bufferSystem.DestroyBuffer(cRenderer, mesh.PropertiesBufferId);
	}

	void SystemShutDown()
	{
		for (auto& mesh : MeshList)
		{
			bufferSystem.DestroyBuffer(cRenderer, mesh.second.MeshVertexBufferId);
			bufferSystem.DestroyBuffer(cRenderer, mesh.second.MeshIndexBufferId);
			bufferSystem.DestroyBuffer(cRenderer, mesh.second.MeshTransformBufferId);
			bufferSystem.DestroyBuffer(cRenderer, mesh.second.PropertiesBufferId);
		}
	}
};
extern MeshSystem meshSystem;


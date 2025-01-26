#pragma once
extern "C"
{
	#include <CTexture.h>
	#include <CBuffer.h>
}
#include "GameObject.h"
#include "VulkanBuffer.h"
#include "SceneDataBuffer.h"
#include "FrameTimer.h"
#include "JsonPipeline.h"
#include "Transform2DComponent.h"
#include "Material.h"

struct MeshProperitiesStruct
{
	alignas(4)  uint32 MaterialIndex = -1;
	alignas(16) mat4   MeshTransform = mat4(1.0f);
};

typedef VulkanBuffer<Vertex2D> VertexBuffer;
typedef VulkanBuffer<uint32> IndexBuffer;
typedef VulkanBuffer<SpriteInstanceStruct> SpriteInstanceBuffer;
typedef VulkanBuffer<MeshProperitiesStruct> MeshPropertiesBuffer;

class Mesh
{
	friend class JsonPipeline;

private:

	const VkBufferUsageFlags MeshBufferUsageSettings = VK_BUFFER_USAGE_VERTEX_BUFFER_BIT |
		VK_BUFFER_USAGE_INDEX_BUFFER_BIT |
		VK_BUFFER_USAGE_STORAGE_BUFFER_BIT |
		VK_BUFFER_USAGE_TRANSFER_SRC_BIT |
		VK_BUFFER_USAGE_TRANSFER_DST_BIT;

	const VkMemoryPropertyFlags MeshBufferPropertySettings = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
		VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

	WeakPtr<GameObject> ParentGameObject;
	WeakPtr<GameObjectComponent> ParentGameObjectComponent;
	WeakPtr<Transform2DComponent> GameObjectTransform;
	
protected:
	//SharedPtr<JsonPipeline> MeshRenderPipeline;
	SharedPtr<Material> MeshMaterial;

public:
	uint64 MeshBufferIndex;
	uint32 VertexCount;
	uint32 IndexCount;

	MeshProperitiesStruct MeshProperties;
	mat4 MeshTransform;
	vec3 MeshPosition;
	vec3 MeshRotation;
	vec3 MeshScale;

	VertexBuffer MeshVertexBuffer;
	IndexBuffer MeshIndexBuffer;

	MeshPropertiesBuffer PropertiesBuffer;

	template<class T>
	void MeshStartUp(List<T>& vertexList, List<uint32>& indexList, uint32 meshBufferIndex)
	{
		MeshBufferIndex = meshBufferIndex;
		VertexCount = vertexList.size();
		IndexCount = indexList.size();

		MeshVertexBuffer = VertexBuffer(vertexList.data(), VertexCount, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		MeshIndexBuffer = IndexBuffer(indexList.data(), IndexCount, MeshBufferUsageSettings , MeshBufferPropertySettings, true);
		PropertiesBuffer = MeshPropertiesBuffer(static_cast<void*>(&MeshProperties), 1, MeshBufferUsageSettings, MeshBufferPropertySettings, false);

		SharedPtr parentGameObject = ParentGameObject.lock();
		if (parentGameObject)
		{
			SharedPtr<GameObjectComponent> component = parentGameObject->GetComponentByComponentType(ComponentTypeEnum::kTransform2DComponent);
			if (component)
			{
				GameObjectTransform = std::dynamic_pointer_cast<Transform2DComponent>(component);
			}
		}
	}

	template<class T>
	void MeshStartUp(List<T>& vertexList, List<uint32>& indexList, SharedPtr<Material> material)
	{
		MeshMaterial = material;
		VertexCount = vertexList.size();
		IndexCount = indexList.size();

		MeshVertexBuffer = VertexBuffer(vertexList.data(), VertexCount, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		MeshIndexBuffer = IndexBuffer(indexList.data(), IndexCount, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		PropertiesBuffer = MeshPropertiesBuffer(MeshProperties, MeshBufferUsageSettings, MeshBufferPropertySettings, false);

		SharedPtr parentGameObject = ParentGameObject.lock();
		if (parentGameObject)
		{
			SharedPtr<GameObjectComponent> component = parentGameObject->GetComponentByComponentType(ComponentTypeEnum::kTransform2DComponent);
			if (component)
			{
				GameObjectTransform = std::dynamic_pointer_cast<Transform2DComponent>(component);
			}
		}
	}

	template<class T>
	void VertexBufferUpdate(const float& deltaTime, List<T>& vertexList, List<uint32>& indexList)
	{
		//MeshVertexBuffer.UpdateBufferMemory(vertexList);
		//MeshIndexBuffer.UpdateBufferMemory(indexList);
	}

	Mesh();
	Mesh(SharedPtr<GameObjectComponent> parentGameObjectComponent);
	virtual ~Mesh();
	virtual void Update(const float& deltaTime);
	virtual void BufferUpdate(VkCommandBuffer& commandBuffer, const float& deltaTime);
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
	virtual void Destroy();

	void GetMeshPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& meshBufferList);

	const VkBufferUsageFlags GetMeshBufferUsageSettings() { return MeshBufferUsageSettings; }
	const VkMemoryPropertyFlags GetMeshBufferPropertySettings() { return MeshBufferPropertySettings; }
	SharedPtr<VkBuffer> GetVertexBuffer() { return std::make_shared<VkBuffer>(MeshVertexBuffer.Buffer); }
	SharedPtr<VkBuffer> GetIndexBuffer() { return std::make_shared<VkBuffer>(MeshIndexBuffer.Buffer); }
};


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

struct MeshProperitiesStruct
{
	alignas(4)  uint32 MaterialIndex = -1;
	alignas(16) mat4   MeshTransform = mat4(1.0f);
};

typedef VulkanBuffer<Vertex2D> VertexBuffer;
typedef VulkanBuffer<uint32> IndexBuffer;
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

public:
	uint64 MeshBufferIndex;
	uint32 VertexCount;
	uint32 IndexCount;

	MeshProperitiesStruct MeshProperties;
	mat4 MeshTransform;
	vec3 MeshPosition;
	vec3 MeshRotation;
	vec3 MeshScale;

	VulkanBuffer<Vertex2D> MeshVertexBuffer;
	IndexBuffer MeshIndexBuffer;
	MeshPropertiesBuffer PropertiesBuffer;

	template<class T>
	void MeshStartUp(List<T>& vertexList, List<uint32>& indexList, uint32 meshBufferIndex)
	{
		//ParentGameObject = parentGameObjectComponent->GetParentGameObject();
		//ParentGameObjectComponent = std::make_shared<GameObjectComponent>(parentGameObjectComponent.get());

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

	Mesh();
	Mesh(SharedPtr<GameObjectComponent> parentGameObjectComponent);
	virtual ~Mesh();
	virtual void Update(const float& deltaTime);
	virtual void BufferUpdate(VkCommandBuffer& commandBuffer, const float& deltaTime);
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
	virtual void Destroy();

	MeshPropertiesBuffer* GetMeshPropertiesBuffer() { return &PropertiesBuffer; }
};


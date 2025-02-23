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
#include "Transform2DComponent.h"
#include "Material.h"

struct MeshProperitiesStruct
{
	alignas(4)  uint32 MaterialIndex = -1;
	alignas(16) mat4   MeshTransform = mat4(1.0f);
};

typedef VulkanBuffer<uint32>				IndexBuffer;
typedef VulkanBuffer<mat4>					TransformBuffer;
typedef VulkanBuffer<SpriteInstanceStruct>  SpriteInstanceBuffer;
typedef VulkanBuffer<MeshProperitiesStruct> MeshPropertiesBuffer;

template<class T>
class Mesh
{
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
	SharedPtr<Material>		  MeshMaterial;
	Vector<T>				  MeshVertexList;
	Vector<uint32>			  MeshIndexList;

public:
	uint64 MeshBufferIndex;
	uint32 VertexCount;
	uint32 IndexCount;

	MeshProperitiesStruct MeshProperties;
	mat4 MeshTransform;
	vec3 MeshPosition;
	vec3 MeshRotation;
	vec3 MeshScale;

	VulkanBuffer<T>		 MeshVertexBuffer;
	IndexBuffer			 MeshIndexBuffer;
	TransformBuffer      MeshTransformBuffer;
	MeshPropertiesBuffer PropertiesBuffer;

	Mesh()
	{
		MeshBufferIndex = 0;
		MeshTransform = mat4(0.0f);
		MeshPosition = vec3(0.0f);
		MeshRotation = vec3(0.0f);
		MeshScale = vec3(1.0f);

		VertexCount = 0;
		IndexCount = 0;
	}

	Mesh(SharedPtr<GameObjectComponent> parentGameObjectComponent)
	{
		MeshBufferIndex = 0;
		MeshTransform = mat4(0.0f);
		MeshPosition = vec3(0.0f);
		MeshRotation = vec3(0.0f);
		MeshScale = vec3(1.0f);

		VertexCount = 0;
		IndexCount = 0;

		ParentGameObject = parentGameObjectComponent->GetParentGameObject();
		ParentGameObjectComponent = parentGameObjectComponent;
	}

	virtual ~Mesh()
	{

	}

	void MeshStartUp(Vector<T>& vertexList, Vector<uint32>& indexList, uint32 meshBufferIndex)
	{
		MeshBufferIndex = meshBufferIndex;
		MeshVertexList = vertexList;
		MeshIndexList = indexList;
		VertexCount = vertexList.size();
		IndexCount = indexList.size();

		MeshVertexBuffer = VulkanBuffer<T>(vertexList.data(), VertexCount, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		MeshIndexBuffer = IndexBuffer(indexList.data(), IndexCount, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		MeshTransformBuffer = TransformBuffer(static_cast<void*>(&MeshTransform), 1, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
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

	void MeshStartUp(Vector<T>& vertexList, Vector<uint32>& indexList, SharedPtr<Material> material)
	{
		MeshMaterial = material;
		MeshVertexList = vertexList;
		MeshIndexList = indexList;
		VertexCount = vertexList.size();
		IndexCount = indexList.size();

		MeshVertexBuffer = VulkanBuffer<T>(vertexList.data(), VertexCount, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		MeshIndexBuffer = IndexBuffer(indexList.data(), IndexCount, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		MeshTransformBuffer = TransformBuffer(static_cast<void*>(&MeshTransform), 1, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
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

	virtual void Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
	{
		mat4 GameObjectMatrix = mat4(1.0);
		//SharedPtr<Transform2DComponent> transform = GameObjectTransform.lock();
		//if (transform)
		//{
		//		GameObjectMatrix = *transform->GameObjectMatrixTransform.get();
		//}

		mat4 MeshMatrix = mat4(1.0f);
		MeshMatrix = glm::translate(MeshMatrix, MeshPosition);
		MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.x), vec3(1.0f, 0.0f, 0.0f));
		MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.y), vec3(0.0f, 1.0f, 0.0f));
		MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.z), vec3(0.0f, 0.0f, 1.0f));
		MeshMatrix = glm::scale(MeshMatrix, MeshScale);
		MeshTransform = GameObjectMatrix * MeshMatrix;

		MeshProperties.MaterialIndex = (MeshMaterial) ? MeshMaterial->GetMaterialBufferIndex() : 0;
		MeshProperties.MeshTransform = MeshTransform;
		PropertiesBuffer.UpdateBufferMemory(MeshProperties);
	}

	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
	{

	}

	virtual void Destroy()
	{
		MeshMaterial.reset();
		MeshVertexBuffer.DestroyBuffer();
		MeshIndexBuffer.DestroyBuffer();
		PropertiesBuffer.DestroyBuffer();
	}

	VkDescriptorBufferInfo GetVertexPropertiesBuffer()
	{
		return VkDescriptorBufferInfo
			{
				.buffer = MeshVertexBuffer.Buffer,
				.offset = 0,
				.range = VK_WHOLE_SIZE
			};
	}

	VkDescriptorBufferInfo GetIndexPropertiesBuffer()
	{
		return VkDescriptorBufferInfo
			{
				.buffer = MeshIndexBuffer.Buffer,
				.offset = 0,
				.range = VK_WHOLE_SIZE
			};
	}

	VkDescriptorBufferInfo GetTransformBuffer()
	{
		return VkDescriptorBufferInfo
			{
				.buffer = MeshTransformBuffer.Buffer,
				.offset = 0,
				.range = VK_WHOLE_SIZE
			};
	}

	VkDescriptorBufferInfo GetMeshPropertiesBuffer()
	{
		return VkDescriptorBufferInfo
			{
				.buffer = PropertiesBuffer.Buffer,
				.offset = 0,
				.range = VK_WHOLE_SIZE
			};
	}

	const VkBufferUsageFlags GetMeshBufferUsageSettings() { return MeshBufferUsageSettings; }
	const VkMemoryPropertyFlags GetMeshBufferPropertySettings() { return MeshBufferPropertySettings; }
	SharedPtr<VkBuffer> GetVertexBuffer() { return std::make_shared<VkBuffer>(MeshVertexBuffer.Buffer); }
	SharedPtr<VkBuffer> GetIndexBuffer() { return std::make_shared<VkBuffer>(MeshIndexBuffer.Buffer); }
};


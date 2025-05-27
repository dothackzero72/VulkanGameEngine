#pragma once
#include "Vertex.h"
#include "Material.h"
#include "Transform2DComponent.h"
#include "VulkanBuffer.h"
#include "Sprite.h"
#include "SpriteBatchLayer.h"

struct MeshProperitiesStruct
{
	alignas(4)  uint32 MaterialIndex = -1;
	alignas(16) mat4   MeshTransform = mat4(1.0f);
};

typedef VulkanBuffer<uint32>				IndexBuffer;
typedef VulkanBuffer<mat4>					TransformBuffer;
typedef VulkanBuffer<SpriteInstanceStruct>  SpriteInstanceBuffer;
typedef VulkanBuffer<MeshProperitiesStruct> MeshPropertiesBuffer;

class RenderSystem;
template<class T>
class Mesh
{
private:
	static uint32 NextMeshId;

	const VkBufferUsageFlags MeshBufferUsageSettings = VK_BUFFER_USAGE_VERTEX_BUFFER_BIT |
		VK_BUFFER_USAGE_INDEX_BUFFER_BIT |
		VK_BUFFER_USAGE_STORAGE_BUFFER_BIT |
		VK_BUFFER_USAGE_TRANSFER_SRC_BIT |
		VK_BUFFER_USAGE_TRANSFER_DST_BIT;

	const VkMemoryPropertyFlags MeshBufferPropertySettings = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
		VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

	uint32 ParentGameObjectID;
	uint32 GameObjectTransform;

protected:
	SharedPtr<Material>		  MeshMaterial;
	Vector<T>				  MeshVertexList;
	Vector<uint32>			  MeshIndexList;

public:
	uint32 MeshId = 0;
	VkGuid MaterialId;
	uint32 VertexCount = 0;
	uint32 IndexCount = 0;

	MeshProperitiesStruct MeshProperties;
	vec3 MeshPosition = vec3(0.0f);
	vec3 MeshRotation = vec3(0.0f);
	vec3 MeshScale = vec3(1.0f);

	VulkanBuffer<T>		 MeshVertexBuffer;
	IndexBuffer			 MeshIndexBuffer;
	TransformBuffer      MeshTransformBuffer;
	MeshPropertiesBuffer PropertiesBuffer;

	Mesh()
	{
	}

	Mesh(Vector<T>& vertexList, Vector<uint32>& indexList, VkGuid materialId)
	{
		MeshId = ++NextMeshId;
		MaterialId = materialId;
		MeshVertexList = vertexList;
		MeshIndexList = indexList;
		VertexCount = vertexList.size();
		IndexCount = indexList.size();

		mat4 tempMat4;
		MeshVertexBuffer = VulkanBuffer<T>(cRenderer, vertexList.data(), VertexCount, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		MeshIndexBuffer = IndexBuffer(cRenderer, indexList.data(), IndexCount, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		MeshTransformBuffer = TransformBuffer(cRenderer, static_cast<void*>(&tempMat4), 1, MeshBufferUsageSettings, MeshBufferPropertySettings, true);
		PropertiesBuffer = MeshPropertiesBuffer(cRenderer, static_cast<void*>(&MeshProperties), 1, MeshBufferUsageSettings, MeshBufferPropertySettings, false);

		//SharedPtr parentGameObject = ParentGameObject.lock();
		//if (parentGameObject)
		//{
		//	SharedPtr<GameObjectComponent> component = parentGameObject->GetComponentByComponentType(ComponentTypeEnum::kTransform2DComponent);
		//	if (component)
		//	{
		//		GameObjectTransform = std::dynamic_pointer_cast<Transform2DComponent>(component);
		//	}
		//}
	}

	~Mesh()
	{

	}

	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
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
	
		MeshProperties.MaterialIndex = (MeshMaterial) ? MeshMaterial->GetMaterialBufferIndex() : 0;
		MeshProperties.MeshTransform = GameObjectMatrix * MeshMatrix;
		PropertiesBuffer.UpdateBufferMemory(cRenderer, MeshProperties);
	}

	void Destroy()
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

	Mesh& operator=(const Mesh& other)
	{
		if (this != &other)
		{
			MeshId = other.MeshId;
			MaterialId = other.MaterialId;
			VertexCount = other.VertexCount;
			IndexCount = other.IndexCount;
			MeshProperties = other.MeshProperties;
			MeshPosition = other.MeshPosition;
			MeshRotation = other.MeshRotation;
			MeshScale = other.MeshScale;

			MeshMaterial = other.MeshMaterial;
			GameObjectTransform = other.GameObjectTransform;

			MeshVertexList = other.MeshVertexList;
			MeshIndexList = other.MeshIndexList;

			MeshVertexBuffer = other.MeshVertexBuffer;
			MeshIndexBuffer = other.MeshIndexBuffer;
			MeshTransformBuffer = other.MeshTransformBuffer;
			PropertiesBuffer = other.PropertiesBuffer;
		}
		return *this;
	}

	const VkBufferUsageFlags GetMeshBufferUsageSettings() { return MeshBufferUsageSettings; }
	const VkMemoryPropertyFlags GetMeshBufferPropertySettings() { return MeshBufferPropertySettings; }
	SharedPtr<VkBuffer> GetVertexBuffer() { return std::make_shared<VkBuffer>(MeshVertexBuffer.Buffer); }
	SharedPtr<VkBuffer> GetIndexBuffer() { return std::make_shared<VkBuffer>(MeshIndexBuffer.Buffer); }
	static const uint32 GetNextIdNumber()
	{
		uint32 nextId = NextMeshId;
		return ++nextId;
	}
};

typedef Mesh<Vertex2D> SpriteMesh;
typedef Mesh<Vertex2D> LevelLayerMesh;
typedef VulkanBuffer<SpriteMesh>			SpriteMeshBuffer;
typedef VulkanBuffer<LevelLayerMesh>		LevelLayerBuffer;
uint32 SpriteMesh::NextMeshId = 0;
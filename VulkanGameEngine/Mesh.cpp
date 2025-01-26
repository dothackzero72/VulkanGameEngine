#include "Mesh.h"
#include "SceneDataBuffer.h"

Mesh::Mesh()
{
	MeshBufferIndex = 0;
	MeshTransform = mat4(0.0f);
	MeshPosition = vec3(0.0f);
	MeshRotation = vec3(0.0f);
	MeshScale = vec3(1.0f);

	VertexCount = 0;
	IndexCount = 0;
}

Mesh::Mesh(SharedPtr<GameObjectComponent> parentGameObjectComponent)
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

Mesh::~Mesh()
{
}

void Mesh::Update(const float& deltaTime)
{
}

void Mesh::BufferUpdate(VkCommandBuffer& commandBuffer, const float& deltaTime)
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
	PropertiesBuffer.UpdateBufferMemory(MeshProperties);
}

void Mesh::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
}

void Mesh::Destroy()
{
	MeshMaterial.reset();
	MeshVertexBuffer.DestroyBuffer();
	MeshIndexBuffer.DestroyBuffer();
	PropertiesBuffer.DestroyBuffer();
}

void Mesh::GetMeshPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& meshBufferList)
{
	VkDescriptorBufferInfo meshBufferInfo =
	{
		.buffer = PropertiesBuffer.Buffer,
		.offset = 0,
		.range = VK_WHOLE_SIZE
	};
	meshBufferList.emplace_back(meshBufferInfo);
}

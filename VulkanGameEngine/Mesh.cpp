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

Mesh::Mesh(std::shared_ptr<GameObjectComponent> parentGameObjectComponent)
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
	if (GameObjectTransform)
	{
		GameObjectTransform->GameObjectPosition.get()->x += 0.01f;
		GameObjectMatrix = glm::translate(GameObjectMatrix, vec3(GameObjectTransform->GameObjectPosition.get()->x, GameObjectTransform->GameObjectPosition.get()->y, 0.0f));
		GameObjectMatrix = glm::rotate(GameObjectMatrix, glm::radians(GameObjectTransform->GameObjectRotation.get()->x), vec3(1.0f, 0.0f, 0.0f));
		GameObjectMatrix = glm::rotate(GameObjectMatrix, glm::radians(GameObjectTransform->GameObjectRotation.get()->y), vec3(0.0f, 1.0f, 0.0f));
		GameObjectMatrix = glm::rotate(GameObjectMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
		//GameObjectMatrix = glm::scale(GameObjectMatrix, vec3(GameObjectTransform->GameObjectScale.get()->x, GameObjectTransform->GameObjectScale.get()->y, 0.0f));
	}

	mat4 MeshMatrix = mat4(1.0f);
	MeshMatrix = glm::translate(MeshMatrix, MeshPosition);
	MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.x), vec3(1.0f, 0.0f, 0.0f));
	MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.y), vec3(0.0f, 1.0f, 0.0f));
	MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.z), vec3(0.0f, 0.0f, 1.0f));
	MeshMatrix = glm::scale(MeshMatrix, MeshScale);

	MeshProperties.MaterialIndex = MeshBufferIndex;
	MeshProperties.MeshTransform = GameObjectMatrix * MeshMatrix;
	PropertiesBuffer.UpdateBufferData(static_cast<void*>(&MeshProperties));
}

void Mesh::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
	/*VkDeviceSize offsets[] = { 0 };
	vkCmdPushConstants(commandBuffer, MeshRenderPipeline->PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneProperties);
	vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, MeshRenderPipeline->Pipeline);
	vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, MeshRenderPipeline->PipelineLayout, 0, 1, &MeshRenderPipeline->DescriptorSet, 0, nullptr);
	vkCmdBindVertexBuffers(commandBuffer, 0, 1, &MeshVertexBuffer.Buffer, offsets);
	vkCmdBindIndexBuffer(commandBuffer, MeshIndexBuffer.Buffer, 0, VK_INDEX_TYPE_UINT32);
	vkCmdDrawIndexed(commandBuffer, IndexCount, 1, 0, 0, 0);*/
}

void Mesh::Destroy()
{
	MeshVertexBuffer.DestroyBuffer();
	MeshIndexBuffer.DestroyBuffer();
	PropertiesBuffer.DestroyBuffer();
}

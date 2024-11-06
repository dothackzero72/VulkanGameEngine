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

Mesh::~Mesh()
{
}

void Mesh::Update(const float& deltaTime)
{
}

void Mesh::BufferUpdate(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	mat4 MeshMatrix = mat4(1.0f);
	MeshMatrix = glm::translate(MeshMatrix, MeshPosition);
	MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.x), vec3(1.0f, 0.0f, 0.0f));
	MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.y), vec3(0.0f, 1.0f, 0.0f));
	MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.z), vec3(0.0f, 0.0f, 1.0f));
	MeshMatrix = glm::scale(MeshMatrix, MeshScale);

	MeshProperties.MaterialIndex = MeshBufferIndex;
	MeshProperties.MeshTransform = MeshMatrix;
	//auto b = PropertiesBuffer.CheckStagingBufferContents();
	//auto c = PropertiesBuffer.CheckBufferContents();
	PropertiesBuffer.UpdateBufferData(static_cast<void*>(&MeshProperties));
	//auto a = PropertiesBuffer.CheckBufferContents();
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

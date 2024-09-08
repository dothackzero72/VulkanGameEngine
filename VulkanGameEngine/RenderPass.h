#pragma once
#include <array>
#include <vector>
#include <memory>
#include "RenderedColorTexture.h"
#include "Mesh.h"

class RenderPass
{
	private:
	protected:
		glm::ivec2 RenderPassResolution;
		VkSampleCountFlagBits SampleCount;

		VkRenderPass RenderPassPtr;
		std::vector<VkCommandBuffer> CommandBufferList;
		std::vector<VkFramebuffer> FrameBufferList;

		VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
		VkDescriptorSetLayout DescriptorSetLayout = VK_NULL_HANDLE;
		VkDescriptorSet DescriptorSet = VK_NULL_HANDLE;
		VkPipeline ShaderPipeline = VK_NULL_HANDLE;
		VkPipelineLayout ShaderPipelineLayout = VK_NULL_HANDLE;
		VkPipelineCache PipelineCache = VK_NULL_HANDLE;

		VkWriteDescriptorSet CreateTextureDescriptorSet(std::shared_ptr<Texture> texture, uint32 bindingSlot);
		VkWriteDescriptorSet CreateTextureDescriptorSet(std::shared_ptr<Texture> texture, uint32 bindingSlot, uint32 descriptorCount);
		VkWriteDescriptorSet CreateTextureDescriptorSet(std::shared_ptr<Texture> texture, uint32 bindingSlot, uint32 descriptorCount, uint32 arrayElement);
		VkWriteDescriptorSet CreateStorageDescriptorSet(std::shared_ptr<Mesh> mesh, uint32 bindingSlot);
		VkWriteDescriptorSet CreateStorageDescriptorSet(std::shared_ptr<Mesh> mesh, uint32 bindingSlot, uint32 arrayElement);
		//VkWriteDescriptorSet CreateUnimformDescriptorSet();

	public:
		RenderPass();
		virtual ~RenderPass();
		virtual VkCommandBuffer Draw();
		virtual void Destroy();
};


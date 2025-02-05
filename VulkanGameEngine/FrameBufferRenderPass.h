#pragma once
#include "Texture.h"
#include "vertex.h"

class FrameBufferRenderPass
{
private:
	ivec2 RenderPassResolution;
	VkSampleCountFlagBits SampleCount;

	VkRenderPass RenderPass = VK_NULL_HANDLE;
	VkCommandBuffer CommandBuffer = VK_NULL_HANDLE;
	std::vector<VkFramebuffer> FrameBufferList;

	VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
	VkDescriptorSetLayout DescriptorSetLayout = VK_NULL_HANDLE;
	VkDescriptorSet DescriptorSet = VK_NULL_HANDLE;
	VkPipeline ShaderPipeline = VK_NULL_HANDLE;
	VkPipelineLayout ShaderPipelineLayout = VK_NULL_HANDLE;
	VkPipelineCache PipelineCache = VK_NULL_HANDLE;

	VkRenderPass CreateRenderPass();
	Vector<VkFramebuffer> CreateFramebuffer();
	VkDescriptorPool CreateDescriptorPoolBinding();
	VkDescriptorSetLayout CreateDescriptorSetLayout();
    VkDescriptorSet CreateDescriptorSets();
    void UpdateDescriptorSet(SharedPtr<Texture> texture);
    VkPipelineLayout CreatePipelineLayout();
	Vector<VkPipelineShaderStageCreateInfo> CreateShaders();

public:
	FrameBufferRenderPass();
	virtual ~FrameBufferRenderPass();

	void BuildRenderPass(SharedPtr<Texture> renderedTexture);
	void BuildRenderPipeline(SharedPtr<Texture> renderedTexture);
	void UpdateRenderPass(SharedPtr<Texture> texture);
	VkCommandBuffer Draw();
	void Destroy();
};
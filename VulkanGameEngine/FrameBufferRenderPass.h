#pragma once
#include "Texture.h"
#include "RenderPass.h"
#include "vertex.h"

class FrameBufferRenderPass : public Renderpass
{
private:
	VkRenderPass CreateRenderPass();
	List<VkFramebuffer> CreateFramebuffer();
	VkDescriptorPool CreateDescriptorPoolBinding();
	VkDescriptorSetLayout CreateDescriptorSetLayout();
    VkDescriptorSet CreateDescriptorSets();
    void UpdateDescriptorSet(SharedPtr<Texture> texture);
    VkPipelineLayout CreatePipelineLayout();
    List<VkPipelineShaderStageCreateInfo> CreateShaders();

public:
	FrameBufferRenderPass();
	virtual ~FrameBufferRenderPass();

	void BuildRenderPass(SharedPtr<Texture> renderedTexture);
	void BuildRenderPipeline(SharedPtr<Texture> renderedTexture);
	void UpdateRenderPass(SharedPtr<Texture> texture);
	VkCommandBuffer Draw();
	void Destroy() override;
};
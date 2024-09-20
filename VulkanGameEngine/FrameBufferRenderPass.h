#pragma once
#include "Texture.h"
#include "RenderPass.h"
#include "vertex.h"

class FrameBufferRenderPass : public RenderPass
{
private:
	void CreateRenderPass();
	void CreateFrameBuffer();
	void CreateDescriptorPool();
	void CreateDescriptorSetLayout();
	void AllocateDescriptorSet();
	void UpdateDescriptorSet();
	void CreatePipelineLayout();
public:
	FrameBufferRenderPass();
	virtual ~FrameBufferRenderPass();

	void BuildRenderPass(std::shared_ptr<Texture> renderedTexture);
	void BuildRenderPipeline(std::shared_ptr<Texture> renderedTexture);
	void UpdateRenderPass(std::shared_ptr<Texture> texture);
	VkCommandBuffer Draw();
	void Destroy() override;
};
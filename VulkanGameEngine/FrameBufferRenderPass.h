#pragma once
#include "Texture.h"
#include "RenderPass.h"
#include "vertex.h"

class FrameBufferRenderPass : public Renderpass
{
private:
public:
	FrameBufferRenderPass();
	virtual ~FrameBufferRenderPass();

	void BuildRenderPass(std::shared_ptr<Texture> renderedTexture);
	void BuildRenderPipeline(std::shared_ptr<Texture> renderedTexture);
	void UpdateRenderPass(std::shared_ptr<Texture> texture);
	VkCommandBuffer Draw();
	void Destroy() override;
};
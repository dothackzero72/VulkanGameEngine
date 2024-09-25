#pragma once
#include <vulkan/vulkan.h>
#include "RenderedColorTexture.h"
#include "Mesh2D.h"
#include "RenderPass.h"
#include "vertex.h"
#include "SceneDataBuffer.h"
#include "RenderedTexture.h"

class RenderPass2D : public Renderpass
{
private:
	std::shared_ptr<RenderedTexture> renderedTexture;

public:
	RenderPass2D();
	virtual ~RenderPass2D();

	void BuildRenderPass(std::shared_ptr<Mesh2D> mesh);
	void BuildRenderPipeline(std::shared_ptr<Mesh2D> mesh);
	void UpdateRenderPass(std::shared_ptr<Mesh2D> mesh);
	VkCommandBuffer Draw(std::shared_ptr<Mesh2D> mesh, SceneDataBuffer& sceneProperties);
	void Destroy() override;

	std::shared_ptr<RenderedTexture> GetRenderedTexture() { return renderedTexture; }
};
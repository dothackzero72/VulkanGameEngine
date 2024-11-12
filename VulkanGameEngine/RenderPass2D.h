#pragma once
#include <vulkan/vulkan.h>
#include "RenderedColorTexture.h"
#include "Mesh2D.h"
#include "RenderPass.h"
#include "vertex.h"
#include "SceneDataBuffer.h"
#include "RenderedTexture.h"
#include "GameObject.h"

class RenderPass2D : public Renderpass
{
private:
	std::shared_ptr<RenderedTexture> renderedTexture;

public:
	RenderPass2D();
	virtual ~RenderPass2D();

	//JsonRenderPass renderPasss;
	void BuildRenderPass(List<std::shared_ptr<Texture>> texture, std::shared_ptr<Texture> texture2);
	void BuildRenderPipeline(List<std::shared_ptr<Texture>> texture, std::shared_ptr<Texture> texture2);
	void UpdateRenderPass(List<std::shared_ptr<Texture>> texture, std::shared_ptr<Texture> texture2);
	VkCommandBuffer Draw(List<std::shared_ptr<GameObject>> meshList, SceneDataBuffer& sceneProperties);
	void Destroy() override;

	std::shared_ptr<RenderedTexture> GetRenderedTexture() { return renderedTexture; }
};
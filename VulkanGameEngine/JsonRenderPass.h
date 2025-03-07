#pragma once
#include <vulkan/vulkan_core.h>
#include "json.h"
#include "Typedef.h"
#include "JsonStructs.h"
#include <RenderedTexture.h>
#include <DepthTexture.h>
#include "JsonPipeline.h"
#include "GameObject.h"

struct GPUImport;
class JsonRenderPass
{
	friend class JsonPipeline;

protected:
	ivec2 RenderPassResolution;
	VkSampleCountFlagBits SampleCount;

	VkRenderPass RenderPass = VK_NULL_HANDLE;
	VkCommandBuffer CommandBuffer;
	std::vector<VkFramebuffer> FrameBufferList;

	Vector<SharedPtr<JsonPipeline>> JsonPipelineList;
	Vector<SharedPtr<Texture>> InputTextureList;

	virtual void BuildRenderPipelines(const RenderPassBuildInfoModel& renderPassBuildInfo, GPUImport& renderGraphics);
	virtual void BuildRenderPass(const RenderPassBuildInfoModel& renderPassBuildInfo);
	virtual void BuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo);

public:
	String Name;
	Vector<SharedPtr<RenderedTexture>> RenderedColorTextureList = Vector<SharedPtr<RenderedTexture>>();
	SharedPtr<DepthTexture> depthTexture;

	JsonRenderPass();
	JsonRenderPass(const String& jsonPath, GPUImport renderGraphics, ivec2 renderPassResolution);
	JsonRenderPass(const String& jsonPath, GPUImport renderGraphics, VkExtent2D renderPassResolution);
	virtual ~JsonRenderPass();

	virtual void Update(const float& deltaTime);
	virtual VkCommandBuffer Draw(Vector<SharedPtr<GameObject>> meshList);
	virtual void Destroy();
};


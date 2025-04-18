#pragma once
#include <vulkan/vulkan_core.h>
#include <json.h>
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
	Vector<VkClearValue> ClearValueList;
	VkRenderPassBeginInfo RenderPassInfo;

	Vector<SharedPtr<JsonPipeline>> JsonPipelineList;
	Vector<SharedPtr<Texture>> InputTextureList;

	virtual void BuildRenderPipelines(const RenderPassBuildInfoModel& renderPassBuildInfo, GPUImport& renderGraphics, SceneDataBuffer& sceneDataBuffer);
	virtual void BuildRenderPass(const RenderPassBuildInfoModel& renderPassBuildInfo);
	virtual void BuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo);

public:
	String Name;
	Vector<SharedPtr<RenderedTexture>> RenderedColorTextureList = Vector<SharedPtr<RenderedTexture>>();
	SharedPtr<DepthTexture> depthTexture;

	JsonRenderPass();
	JsonRenderPass(const String& jsonPath, GPUImport renderGraphics, ivec2 renderPassResolution, SceneDataBuffer& sceneDataBuffer);
	JsonRenderPass(const String& jsonPath, GPUImport renderGraphics, VkExtent2D renderPassResolution, SceneDataBuffer& sceneDataBuffer);
	virtual ~JsonRenderPass();

	virtual void Update(const float& deltaTime);
	VkCommandBuffer DrawFrameBuffer();
	virtual VkCommandBuffer Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneDataBuffer);
	virtual void Destroy();
};


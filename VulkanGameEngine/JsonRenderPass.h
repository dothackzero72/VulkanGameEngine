#pragma once
#include <vulkan/vulkan_core.h>
#include "json.h"
#include "Typedef.h"
#include "JsonStructs.h"
#include "RenderPass.h"
#include "RenderedTexture.h"
#include "DepthTexture.h"
#include "JsonPipeline.h"

class JsonRenderPass
{
	friend class JsonPipeline;
private:
	ivec2 RenderPassResolution;
	VkSampleCountFlagBits SampleCount;

	VkRenderPass RenderPass = VK_NULL_HANDLE;
	std::vector<VkCommandBuffer> CommandBufferList;
	std::vector<VkFramebuffer> FrameBufferList;

	void BuildRenderPass(RenderPassBuildInfoModel renderPassBuildInfo);
	void BuildFrameBuffer();

	VkAttachmentDescription JsonToVulkanAttachmentDescription(nlohmann::json& json);
	VkImageCreateInfo JsonToVulkanImageCreateInfo(nlohmann::json& json);
	VkSamplerCreateInfo JsonToVulkanSamplerCreateInfo(nlohmann::json& json);
	VkSubpassDependency JsonToVulkanSubpassDependency(nlohmann::json& json);

	RenderedTextureInfoModel JsonToRenderedTextureInfoModel(nlohmann::json& json);
	RenderPassBuildInfoModel JsonToRenderPassBuildInfoModel(nlohmann::json& json);

	std::shared_ptr<JsonPipeline> jsonPipeline;

public:
	
	List<std::shared_ptr<RenderedTexture>> RenderedColorTextureList = List<std::shared_ptr<RenderedTexture>>();
	DepthTexture depthTexture = DepthTexture();

	JsonRenderPass();
	virtual ~JsonRenderPass();
	void JsonCreateRenderPass(String JsonPath, ivec2 RenderPassResolution);
	VkCommandBuffer Draw(List<std::shared_ptr<GameObject>> meshList, SceneDataBuffer& sceneProperties);

	void Destroy();
};


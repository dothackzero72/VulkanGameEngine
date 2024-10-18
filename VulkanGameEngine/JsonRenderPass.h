#pragma once
#include <vulkan/vulkan_core.h>
#include "json.h"
#include "Typedef.h"
#include "JsonStructs.h"
#include "RenderPass.h"
#include "RenderedTexture.h"
#include "DepthTexture.h"

class JsonRenderPass
{
private:
	VkRenderPass RenderPass = VK_NULL_HANDLE;
	List<RenderedTexture> RenderedColorTextureList = List<RenderedTexture>();
	DepthTexture depthTexture = DepthTexture();

	VkAttachmentDescription JsonToVulkanAttachmentDescription(nlohmann::json& json);
	VkImageCreateInfo JsonToVulkanImageCreateInfo(nlohmann::json& json);
	VkSamplerCreateInfo JsonToVulkanSamplerCreateInfo(nlohmann::json& json);
	VkSubpassDependency JsonToVulkanSubpassDependency(nlohmann::json& json);

	RenderedTextureInfoModel JsonToRenderedTextureInfoModel(nlohmann::json& json);
	RenderPassBuildInfoModel JsonToRenderPassBuildInfoModel(nlohmann::json& json);

public:
	JsonRenderPass();
	virtual ~JsonRenderPass();
	void JsonCreateRenderPass(std::string JsonPath);
};


#pragma once
#include <vulkan/vulkan.h>

typedef struct
{
	VkRenderPass* pRenderPass;
	const VkAttachmentDescription* pAttachmentList;
	const VkSubpassDescription* pSubpassDescriptionList;
	const VkSubpassDependency* pSubpassDependencyList;
	uint32						AttachmentCount;
	uint32						SubpassCount;
	uint32						DependencyCount;
	uint32						Width;
	uint32						Height;
}Renderer_RenderPassCreateInfoStruct;
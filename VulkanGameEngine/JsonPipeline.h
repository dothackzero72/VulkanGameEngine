#pragma once
#include <vulkan/vulkan_core.h>
#include "json.h"
#include "Typedef.h"
#include "ECSid.h"
#include "VkGuid.h"
#include <JsonStructs.h>
#include <CoreVulkanRenderer.h>
#include <VulkanPipeline.h>

class JsonPipeline
{
private:
public:
    VulkanPipeline vulkanPipeline;

    JsonPipeline();
    JsonPipeline(VkGuid& renderPassId, VkGuid& levelLayerId, uint renderPipelineId, String jsonPath, VkRenderPass renderPass, uint constBufferSize, ivec2& renderPassResolution);
    ~JsonPipeline();

    void RecreateSwapchain(VkRenderPass renderPass, uint constBufferSize, int newWidth, int newHeight);
    void Destroy();
};
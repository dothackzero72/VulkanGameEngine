#pragma once
#include <vulkan/vulkan_core.h>
#include "json.h"
#include "Typedef.h"
#include "JsonStructs.h"

void VkPipeline_BuildRenderPass(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
void VkPipeline_BuildFrameBuffer(VkDevice device, VkDescriptorPool descriptorPool, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList);
#pragma once
extern "C"
{
#include "VulkanWindow.h"
#include "CVulkanRenderer.h"

}
#include "Typedef.h"
#include <CoreVulkanRenderer.h>

class VulkanRenderer
{
private:


public:

    VkResult RendererSetUp(void* windowHandle);
    VkResult RebuildSwapChain();

    VkResult StartFrame();
    VkResult EndFrame(Vector<VkCommandBuffer> commandBufferSubmitList);

    VkResult CreateRenderPass(RenderPassCreateInfoStruct& renderPassCreateInfo);
    VkResult CreateGraphicsPipelines(VkPipeline& graphicPipeline, VkGraphicsPipelineCreateInfo createGraphicPipelines);
    VkResult CreateDescriptorPool(VkDescriptorPool& descriptorPool, VkDescriptorPoolCreateInfo& descriptorPoolCreateInfo);
    VkResult CreateDescriptorSetLayout(VkDescriptorSetLayout& descriptorSetLayout, VkDescriptorSetLayoutCreateInfo& descriptorSetLayoutCreateInfo);
    VkResult CreatePipelineLayout(VkPipelineLayout& pipelineLayout, VkPipelineLayoutCreateInfo& pipelineLayoutCreateInfo);
    VkResult AllocateDescriptorSets(VkDescriptorSet& descriptorSet, VkDescriptorSetAllocateInfo& descriptorSetAllocateInfo);
    VkResult AllocateCommandBuffers(VkCommandBuffer commandBuffer, VkCommandBufferAllocateInfo& commandBufferAllocateInfo);
    VkResult CreateGraphicsPipelines(VkPipeline& graphicPipeline, Vector<VkGraphicsPipelineCreateInfo> createGraphicPipelines);
    VkResult CreateCommandPool(VkCommandPool& commandPool, VkCommandPoolCreateInfo commandPoolInfo);
    void UpdateDescriptorSet(Vector<VkWriteDescriptorSet> writeDescriptorSetList);

    static VkCommandBuffer  BeginSingleTimeCommands();
    static VkCommandBuffer  BeginSingleTimeCommands(VkCommandPool& commandPool);
    static VkResult  EndSingleTimeCommands(VkCommandBuffer commandBuffer);
    static VkResult  EndSingleTimeCommands(VkCommandBuffer commandBuffer, VkCommandPool& commandPool);

    void DestroyRenderer();
    void DestroyRenderPass(VkRenderPass& renderPass);
    void DestroyFrameBuffers(Vector<VkFramebuffer>& frameBufferList);
    void DestroyCommandBuffers(VkCommandBuffer& commandBuffer);
    void DestroyBuffer(VkBuffer& buffer);
};
static VulkanRenderer renderer;


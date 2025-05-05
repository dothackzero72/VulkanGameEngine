#pragma once
extern "C"
{
#include <VulkanWindow.h>
#include <CVulkanRenderer.h>

}
#include "Typedef.h"
#include "CoreVulkanRenderer.h"

class VulkanRenderer
{
private:


public:

    VkResult RendererSetUp();
    VkResult SetUpSwapChain();
    VkResult RebuildSwapChain();

    VkResult StartFrame();
    VkResult EndFrame(Vector<VkCommandBuffer> commandBufferSubmitList);

    VkResult CreateCommandBuffer(VkCommandBuffer& commandBuffer);
    VkResult CreateFrameBuffer(VkFramebuffer frameBuffer, VkFramebufferCreateInfo& frameBufferCreateInfo);
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
    void DestroyFences();
    void DestroyCommandPool();
    void DestroyDevice();
    void DestroySurface();
    void DestroyDebugger();
    void DestroyInstance();
    void DestroyRenderPass(VkRenderPass& renderPass);
    void DestroyFrameBuffers(Vector<VkFramebuffer>& frameBufferList);
    void DestroyDescriptorPool(VkDescriptorPool& descriptorPool);
    void DestroyDescriptorSetLayout(VkDescriptorSetLayout& descriptorSetLayout);
    void DestroyCommandBuffers(VkCommandBuffer& commandBuffer);
    void DestroyBuffer(VkBuffer& buffer);
    void FreeDeviceMemory(VkDeviceMemory& memory);
    void DestroySwapChainImageView();
    void DestroySwapChain();
    void DestroyImageView(VkImageView& imageView);
    void DestroyImage(VkImage& image);
    void DestroySampler(VkSampler& sampler);
    void DestroyPipeline(VkPipeline& pipeline);
    void DestroyPipelineLayout(VkPipelineLayout& pipelineLayout);
    void DestroyPipelineCache(VkPipelineCache& pipelineCache);
};
static VulkanRenderer renderer;


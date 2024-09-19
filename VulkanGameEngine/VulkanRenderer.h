#pragma once
extern "C"
{
#include <VulkanWindow.h>
#include <CVulkanRenderer.h>
#include <CBuffer.h>
}
#include "Typedef.h"

class VulkanRenderer
{
    private:
        VkResult SetUpSwapChain();

    public:
        VkResult RendererSetUp();
        VkResult RebuildSwapChain();

        VkResult StartFrame();
        VkResult EndFrame(List<VkCommandBuffer> commandBufferSubmitList);
        VkResult SubmitDraw(List<VkCommandBuffer> commandBufferSubmitList);

        VkResult CreateCommandBuffers(List<VkCommandBuffer>& commandBufferList);
        VkResult CreateFrameBuffer(VkFramebuffer frameBuffer, VkFramebufferCreateInfo& frameBufferCreateInfo);
        VkResult CreateRenderPass(RenderPassCreateInfoStruct& renderPassCreateInfo);
        VkResult CreateGraphicsPipelines(VkPipeline& graphicPipeline, VkGraphicsPipelineCreateInfo createGraphicPipelines);
        VkResult CreateDescriptorPool(VkDescriptorPool& descriptorPool, VkDescriptorPoolCreateInfo& descriptorPoolCreateInfo);
        VkResult CreateDescriptorSetLayout(VkDescriptorSetLayout& descriptorSetLayout, VkDescriptorSetLayoutCreateInfo& descriptorSetLayoutCreateInfo);
        VkResult CreatePipelineLayout(VkPipelineLayout& pipelineLayout, VkPipelineLayoutCreateInfo& pipelineLayoutCreateInfo);
        VkResult AllocateDescriptorSets(VkDescriptorSet& descriptorSet, VkDescriptorSetAllocateInfo& descriptorSetAllocateInfo);
        VkResult AllocateCommandBuffers(VkCommandBuffer commandBuffer, VkCommandBufferAllocateInfo& commandBufferAllocateInfo);
        VkResult CreateGraphicsPipelines(VkPipeline& graphicPipeline, List<VkGraphicsPipelineCreateInfo> createGraphicPipelines);
        VkResult CreateCommandPool(VkCommandPool& commandPool, VkCommandPoolCreateInfo commandPoolInfo);
        void UpdateDescriptorSet(List<VkWriteDescriptorSet> writeDescriptorSetList);

        VkCommandBuffer BeginCommandBuffer();
        VkResult BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo);
        VkResult EndCommandBuffer(VkCommandBuffer* pCommandBufferList);
        VkResult EndCommandBuffer(VkCommandBuffer commandBuffer);

        void DestroyRenderer();
        void DestroyFences();
        void DestroyCommandPool();
        void DestroyDevice();
        void DestroySurface();
        void DestroyDebugger();
        void DestroyInstance();
        void DestroyRenderPass(VkRenderPass& renderPass);
        void DestroyFrameBuffers(List<VkFramebuffer>& frameBufferList);
        void DestroyDescriptorPool(VkDescriptorPool& descriptorPool);
        void DestroyDescriptorSetLayout(VkDescriptorSetLayout& descriptorSetLayout);
        void DestroyCommandBuffers(List<VkCommandBuffer>& commandBufferList);
        void DestroyBuffer(VkBuffer& buffer);
        void FreeMemory(VkDeviceMemory& memory);
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


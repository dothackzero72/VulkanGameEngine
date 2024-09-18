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
        static VkResult SetUpSwapChain();

    public:
        static VkResult RendererSetUp();
        static VkResult RebuildSwapChain();

        static VkResult StartFrame();
        static VkResult EndFrame(List<VkCommandBuffer> commandBufferSubmitList);
        static VkResult SubmitDraw(List<VkCommandBuffer> commandBufferSubmitList);

        static VkResult CreateCommandBuffers(List<VkCommandBuffer>& commandBufferList);
        static VkResult CreateFrameBuffer(VkFramebuffer frameBuffer, VkFramebufferCreateInfo& frameBufferCreateInfo);
        static VkResult CreateRenderPass(RenderPassCreateInfoStruct& renderPassCreateInfo);
        static VkResult CreateGraphicsPipelines(VkPipeline& graphicPipeline, VkGraphicsPipelineCreateInfo createGraphicPipelines);
        static VkResult CreateDescriptorPool(VkDescriptorPool& descriptorPool, VkDescriptorPoolCreateInfo& descriptorPoolCreateInfo);
        static VkResult CreateDescriptorSetLayout(VkDescriptorSetLayout& descriptorSetLayout, VkDescriptorSetLayoutCreateInfo& descriptorSetLayoutCreateInfo);
        static VkResult CreatePipelineLayout(VkPipelineLayout& pipelineLayout, VkPipelineLayoutCreateInfo& pipelineLayoutCreateInfo);
        static VkResult AllocateDescriptorSets(VkDescriptorSet& descriptorSet, VkDescriptorSetAllocateInfo& descriptorSetAllocateInfo);
        static VkResult AllocateCommandBuffers(VkCommandBuffer commandBuffer, VkCommandBufferAllocateInfo& commandBufferAllocateInfo);
        static VkResult CreateGraphicsPipelines(VkPipeline& graphicPipeline, List<VkGraphicsPipelineCreateInfo> createGraphicPipelines);
        static VkResult CreateCommandPool(VkCommandPool& commandPool, VkCommandPoolCreateInfo commandPoolInfo);
        static void UpdateDescriptorSet(List<VkWriteDescriptorSet> writeDescriptorSetList);

        static VkCommandBuffer BeginCommandBuffer();
        static VkResult BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo);
        static VkResult EndCommandBuffer(VkCommandBuffer* pCommandBufferList);
        static VkResult EndCommandBuffer(VkCommandBuffer commandBuffer);

        static void DestroyRenderer();
        static void DestroyFences();
        static void DestroyCommandPool();
        static void DestroyDevice();
        static void DestroySurface();
        static void DestroyDebugger();
        static void DestroyInstance();
        static void DestroyRenderPass(VkRenderPass& renderPass);
        static void DestroyFrameBuffers(List<VkFramebuffer>& frameBufferList);
        static void DestroyDescriptorPool(VkDescriptorPool& descriptorPool);
        static void DestroyDescriptorSetLayout(VkDescriptorSetLayout& descriptorSetLayout);
        static void DestroyCommandBuffers(List<VkCommandBuffer>& commandBufferList);
        static void DestroyBuffer(VkBuffer& buffer);
        static void FreeMemory(VkDeviceMemory& memory);
        static void DestroySwapChainImageView(List<VkImageView>& swapChainImageViewList);
        static void DestroySwapChain(VkSwapchainKHR& swapChain);
        static void DestroyImageView(VkImageView& imageView);
        static void DestroyImage(VkImage& image);
        static void DestroySampler(VkSampler& sampler);
        static void DestroyPipeline(VkPipeline& pipeline);
        static void DestroyPipelineLayout(VkPipelineLayout& pipelineLayout);
        static void DestroyPipelineCache(VkPipelineCache& pipelineCache);
};


#include "VulkanRenderer.h"
#include <iostream>

VkResult VulkanRenderer::RendererSetUp(void* windowHandle)
{
    RendererState state = Renderer_RendererSetUp(windowHandle);
    cRenderer = state;
    return VK_SUCCESS;
}

VkResult VulkanRenderer::RebuildSwapChain()
{
    //cRenderer.RebuildRendererFlag = true;

    //VULKAN_RESULT(vkDeviceWaitIdle(cRenderer.Device));
    //Renderer_DestroySwapChainImageView(cRenderer.Device, cRenderer.Surface, &cRenderer.SwapChainImageViews[0], MAX_FRAMES_IN_FLIGHT);
    //vkDestroySwapchainKHR(cRenderer.Device, cRenderer.Swapchain, NULL);
    //renderer.SetUpSwapChain(vulkanWindow->WindowHandle);
    return VK_SUCCESS;
}

VkResult VulkanRenderer::StartFrame()
{
    return Renderer_StartFrame(cRenderer.Device,
        cRenderer.Swapchain,
        cRenderer.InFlightFences.data(),
        cRenderer.AcquireImageSemaphores.data(),
        &cRenderer.ImageIndex,
        &cRenderer.CommandIndex,
        &cRenderer.RebuildRendererFlag);
}

VkResult VulkanRenderer::EndFrame(Vector<VkCommandBuffer> commandBufferSubmitList)
{
    return Renderer_EndFrame(cRenderer.Swapchain,
        cRenderer.AcquireImageSemaphores.data(),
        cRenderer.PresentImageSemaphores.data(),
        cRenderer.InFlightFences.data(),
        cRenderer.GraphicsQueue,
        cRenderer.PresentQueue,
        cRenderer.CommandIndex,
        cRenderer.ImageIndex,
        commandBufferSubmitList.data(),
        commandBufferSubmitList.size(),
        &cRenderer.RebuildRendererFlag);
}

void VulkanRenderer::DestroyRenderer()
{
    Renderer_DestroyRenderer(cRenderer);
}

VkResult VulkanRenderer::CreateRenderPass(RenderPassCreateInfoStruct& renderPassCreateInfo)
{
    return Renderer_CreateRenderPass(cRenderer.Device, &renderPassCreateInfo);
}

VkResult VulkanRenderer::CreateDescriptorPool(VkDescriptorPool& descriptorPool, VkDescriptorPoolCreateInfo& descriptorPoolCreateInfo)
{
    return Renderer_CreateDescriptorPool(cRenderer.Device, &descriptorPool, &descriptorPoolCreateInfo);
}

VkResult VulkanRenderer::CreateDescriptorSetLayout(VkDescriptorSetLayout& descriptorSetLayout, VkDescriptorSetLayoutCreateInfo& descriptorSetLayoutCreateInfo)
{
    return Renderer_CreateDescriptorSetLayout(cRenderer.Device, &descriptorSetLayout, &descriptorSetLayoutCreateInfo);
}

VkResult VulkanRenderer::CreatePipelineLayout(VkPipelineLayout& pipelineLayout, VkPipelineLayoutCreateInfo& pipelineLayoutCreateInfo)
{
    return Renderer_CreatePipelineLayout(cRenderer.Device, &pipelineLayout, &pipelineLayoutCreateInfo);
}

VkResult VulkanRenderer::AllocateDescriptorSets(VkDescriptorSet& descriptorSet, VkDescriptorSetAllocateInfo& descriptorSetAllocateInfo)
{
    return Renderer_AllocateDescriptorSets(cRenderer.Device, &descriptorSet, &descriptorSetAllocateInfo);
}

VkResult VulkanRenderer::AllocateCommandBuffers(VkCommandBuffer commandBuffer, VkCommandBufferAllocateInfo& commandBufferAllocateInfo)
{
    return Renderer_AllocateCommandBuffers(cRenderer.Device, &commandBuffer, &commandBufferAllocateInfo);
}

VkResult VulkanRenderer::CreateGraphicsPipelines(VkPipeline& graphicPipeline, VkGraphicsPipelineCreateInfo createGraphicPipelines)
{
    return Renderer_CreateGraphicsPipelines(cRenderer.Device, &graphicPipeline, &createGraphicPipelines, 1);
}

VkResult VulkanRenderer::CreateGraphicsPipelines(VkPipeline& graphicPipeline, Vector<VkGraphicsPipelineCreateInfo> createGraphicPipelines)
{
    return Renderer_CreateGraphicsPipelines(cRenderer.Device, &graphicPipeline, createGraphicPipelines.data(), createGraphicPipelines.size());
}

VkResult VulkanRenderer::CreateCommandPool(VkCommandPool& commandPool, VkCommandPoolCreateInfo commandPoolInfo)
{
    return Renderer_CreateCommandPool(cRenderer.Device, &commandPool, &commandPoolInfo);
}

void VulkanRenderer::UpdateDescriptorSet(Vector<VkWriteDescriptorSet> writeDescriptorSetList)
{
    Renderer_UpdateDescriptorSet(cRenderer.Device, writeDescriptorSetList.data(), writeDescriptorSetList.size());
}

VkCommandBuffer  VulkanRenderer::BeginSingleTimeCommands() {
    VkCommandBufferAllocateInfo allocInfo{};
    allocInfo.sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
    allocInfo.level = VK_COMMAND_BUFFER_LEVEL_PRIMARY;
    allocInfo.commandPool = cRenderer.CommandPool;
    allocInfo.commandBufferCount = 1;

    VkCommandBuffer commandBuffer;
    vkAllocateCommandBuffers(cRenderer.Device, &allocInfo, &commandBuffer);

    VkCommandBufferBeginInfo beginInfo{};
    beginInfo.sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
    beginInfo.flags = VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

    vkBeginCommandBuffer(commandBuffer, &beginInfo);

    return commandBuffer;
}

VkCommandBuffer  VulkanRenderer::BeginSingleTimeCommands(VkCommandPool& commandPool) {
    VkCommandBufferAllocateInfo allocInfo{};
    allocInfo.sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
    allocInfo.level = VK_COMMAND_BUFFER_LEVEL_PRIMARY;
    allocInfo.commandPool = commandPool;
    allocInfo.commandBufferCount = 1;

    VkCommandBuffer commandBuffer;
    vkAllocateCommandBuffers(cRenderer.Device, &allocInfo, &commandBuffer);

    VkCommandBufferBeginInfo beginInfo{};
    beginInfo.sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
    beginInfo.flags = VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

    vkBeginCommandBuffer(commandBuffer, &beginInfo);

    return commandBuffer;
}

VkResult  VulkanRenderer::EndSingleTimeCommands(VkCommandBuffer commandBuffer) {
    vkEndCommandBuffer(commandBuffer);

    VkSubmitInfo submitInfo{};
    submitInfo.sType = VK_STRUCTURE_TYPE_SUBMIT_INFO;
    submitInfo.commandBufferCount = 1;
    submitInfo.pCommandBuffers = &commandBuffer;

    VkResult result = vkQueueSubmit(cRenderer.GraphicsQueue, 1, &submitInfo, VK_NULL_HANDLE);
    result = vkQueueWaitIdle(cRenderer.GraphicsQueue);

    vkFreeCommandBuffers(cRenderer.Device, cRenderer.CommandPool, 1, &commandBuffer);

    return result;
}

VkResult  VulkanRenderer::EndSingleTimeCommands(VkCommandBuffer commandBuffer, VkCommandPool& commandPool) {
    vkEndCommandBuffer(commandBuffer);

    VkSubmitInfo submitInfo{};
    submitInfo.sType = VK_STRUCTURE_TYPE_SUBMIT_INFO;
    submitInfo.commandBufferCount = 1;
    submitInfo.pCommandBuffers = &commandBuffer;

    VkResult result = vkQueueSubmit(cRenderer.GraphicsQueue, 1, &submitInfo, VK_NULL_HANDLE);
    result = vkQueueWaitIdle(cRenderer.GraphicsQueue);

    vkFreeCommandBuffers(cRenderer.Device, commandPool, 1, &commandBuffer);

    return result;
}

void VulkanRenderer::DestroyRenderPass(VkRenderPass& renderPass)
{
    Renderer_DestroyRenderPass(cRenderer.Device, &renderPass);
}

void VulkanRenderer::DestroyFrameBuffers(Vector<VkFramebuffer>& frameBufferList)
{
    Renderer_DestroyFrameBuffers(cRenderer.Device, frameBufferList.data(), frameBufferList.size());
}

void VulkanRenderer::DestroyCommandBuffers(VkCommandBuffer& commandBuffer)
{
    Renderer_DestroyCommandBuffers(cRenderer.Device, &cRenderer.CommandPool, &commandBuffer, 1);
}

void VulkanRenderer::DestroyBuffer(VkBuffer& buffer)
{
    Renderer_DestroyBuffer(cRenderer.Device, &buffer);
}
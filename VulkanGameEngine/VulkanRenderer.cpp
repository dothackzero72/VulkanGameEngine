#include "VulkanRenderer.h"

VkResult VulkanRenderer::SetUpSwapChain()
{
    int width = 0;
    int height = 0;
    uint32 surfaceFormatCount = 0;
    uint32 presentModeCount = 0;
    VkSurfaceFormatKHR* compatibleSwapChainFormatList = NULL;
    VkPresentModeKHR* compatiblePresentModesList = NULL;
    VkSurfaceCapabilitiesKHR surfaceCapabilities = { 0 };

    VULKAN_RESULT(SwapChain_GetSurfaceCapabilities(cRenderer.PhysicalDevice, cRenderer.Surface, &surfaceCapabilities));
    VULKAN_RESULT(SwapChain_GetPhysicalDeviceFormats(cRenderer.PhysicalDevice, cRenderer.Surface, &compatibleSwapChainFormatList, &surfaceFormatCount));
    VULKAN_RESULT(SwapChain_GetQueueFamilies(cRenderer.PhysicalDevice, cRenderer.Surface, &cRenderer.SwapChain.GraphicsFamily, &cRenderer.SwapChain.PresentFamily));
    VULKAN_RESULT(SwapChain_GetPhysicalDevicePresentModes(cRenderer.PhysicalDevice, cRenderer.Surface, &compatiblePresentModesList, &presentModeCount));
    VkSurfaceFormatKHR swapChainImageFormat = SwapChain_FindSwapSurfaceFormat(compatibleSwapChainFormatList, surfaceFormatCount);
    VkPresentModeKHR swapChainPresentMode = SwapChain_FindSwapPresentMode(compatiblePresentModesList, presentModeCount);
    vulkanWindow->GetFrameBufferSize(vulkanWindow, &width, &height);
    cRenderer.SwapChain.Swapchain = SwapChain_SetUpSwapChain(cRenderer.Device, cRenderer.Surface, surfaceCapabilities, swapChainImageFormat, swapChainPresentMode, cRenderer.SwapChain.GraphicsFamily, cRenderer.SwapChain.PresentFamily, width, height, &cRenderer.SwapChain.SwapChainImageCount);
    cRenderer.SwapChain.SwapChainImages = SwapChain_SetUpSwapChainImages(cRenderer.Device, cRenderer.SwapChain.Swapchain, cRenderer.SwapChain.SwapChainImageCount);
    cRenderer.SwapChain.SwapChainImageViews = SwapChain_SetUpSwapChainImageViews(cRenderer.Device, cRenderer.SwapChain.SwapChainImages, compatibleSwapChainFormatList, cRenderer.SwapChain.SwapChainImageCount);

    cRenderer.SwapChain.SwapChainResolution.width = width;
    cRenderer.SwapChain.SwapChainResolution.height = height;
    return VK_SUCCESS;
}

VkResult VulkanRenderer::RendererSetUp()
{
    cRenderer.RebuildRendererFlag = false;
    VkDebugUtilsMessengerEXT debugMessenger = VK_NULL_HANDLE;
    cRenderer.Instance = Renderer_CreateVulkanInstance();
    cRenderer.DebugMessenger = Renderer_SetupDebugMessenger(cRenderer.Instance);
    vulkanWindow->CreateSurface(vulkanWindow, &cRenderer.Instance, &cRenderer.Surface);
    VkResult deviceSetupResult = Renderer_SetUpPhysicalDevice(cRenderer.Instance, &cRenderer.PhysicalDevice, cRenderer.Surface, &cRenderer.PhysicalDeviceFeatures, &cRenderer.SwapChain.GraphicsFamily, &cRenderer.SwapChain.PresentFamily);
    cRenderer.Device = Renderer_SetUpDevice(cRenderer.PhysicalDevice, cRenderer.SwapChain.GraphicsFamily, cRenderer.SwapChain.PresentFamily);
    VULKAN_RESULT(renderer.SetUpSwapChain());
    cRenderer.CommandPool = Renderer_SetUpCommandPool(cRenderer.Device, cRenderer.SwapChain.GraphicsFamily);
    VULKAN_RESULT(Renderer_SetUpSemaphores(cRenderer.Device, &cRenderer.InFlightFences, &cRenderer.AcquireImageSemaphores, &cRenderer.PresentImageSemaphores, MAX_FRAMES_IN_FLIGHT));
    VULKAN_RESULT(Renderer_GetDeviceQueue(cRenderer.Device, cRenderer.SwapChain.GraphicsFamily, cRenderer.SwapChain.PresentFamily, &cRenderer.SwapChain.GraphicsQueue, &cRenderer.SwapChain.PresentQueue));
    return VK_SUCCESS;
}

VkResult VulkanRenderer::RebuildSwapChain()
{
    cRenderer.RebuildRendererFlag = true;

    VULKAN_RESULT(vkDeviceWaitIdle(cRenderer.Device));
    Renderer_DestroySwapChainImageView(cRenderer.Device, cRenderer.SwapChain.SwapChainImageViews, MAX_FRAMES_IN_FLIGHT);
    vkDestroySwapchainKHR(cRenderer.Device, cRenderer.SwapChain.Swapchain, NULL);
    renderer.SetUpSwapChain();
    return VK_SUCCESS;
}

VkResult VulkanRenderer::StartFrame()
{
	return Renderer_StartFrame(cRenderer.Device, 
                               cRenderer.SwapChain.Swapchain, 
                               cRenderer.InFlightFences, 
                               cRenderer.AcquireImageSemaphores, 
                               &cRenderer.ImageIndex, 
                               &cRenderer.CommandIndex,
                               &cRenderer.RebuildRendererFlag);
}

VkResult VulkanRenderer::EndFrame(List<VkCommandBuffer> commandBufferSubmitList)
{
    return Renderer_EndFrame(cRenderer.SwapChain.Swapchain,
                             cRenderer.AcquireImageSemaphores, 
                             cRenderer.PresentImageSemaphores, 
                             cRenderer.InFlightFences,
                             cRenderer.SwapChain.GraphicsQueue, 
                             cRenderer.SwapChain.PresentQueue,
                             cRenderer.CommandIndex,
                             cRenderer.ImageIndex,
                             commandBufferSubmitList.data(), 
                             commandBufferSubmitList.size(),
                             &cRenderer.RebuildRendererFlag);
}

void VulkanRenderer::DestroyRenderer()
{
    renderer.DestroySwapChainImageView();
    renderer.DestroySwapChain();
    renderer.DestroyFences();
    renderer.DestroyCommandPool();
    renderer.DestroyDevice();
    renderer.DestroyDebugger();
    renderer.DestroySurface();
    renderer.DestroyInstance();
}

VkResult VulkanRenderer::CreateCommandBuffers(List<VkCommandBuffer>& commandBufferList)
{
    return Renderer_CreateCommandBuffers(cRenderer.Device, cRenderer.CommandPool, commandBufferList.data(), commandBufferList.size());
}

VkResult VulkanRenderer::CreateFrameBuffer(VkFramebuffer frameBuffer, VkFramebufferCreateInfo& frameBufferCreateInfo)
{
    return Renderer_CreateFrameBuffer(cRenderer.Device, &frameBuffer, &frameBufferCreateInfo);
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

VkResult VulkanRenderer::CreateGraphicsPipelines(VkPipeline& graphicPipeline, List<VkGraphicsPipelineCreateInfo> createGraphicPipelines)
{
    return Renderer_CreateGraphicsPipelines(cRenderer.Device, &graphicPipeline, createGraphicPipelines.data(), createGraphicPipelines.size());
}

VkResult VulkanRenderer::CreateCommandPool(VkCommandPool& commandPool, VkCommandPoolCreateInfo commandPoolInfo)
{
    return Renderer_CreateCommandPool(cRenderer.Device, &commandPool, &commandPoolInfo);
}

void VulkanRenderer::UpdateDescriptorSet(List<VkWriteDescriptorSet> writeDescriptorSetList)
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

    VkResult result = vkQueueSubmit(cRenderer.SwapChain.GraphicsQueue, 1, &submitInfo, VK_NULL_HANDLE);
    result = vkQueueWaitIdle(cRenderer.SwapChain.GraphicsQueue);

    vkFreeCommandBuffers(cRenderer.Device, cRenderer.CommandPool, 1, &commandBuffer);

    return result;
}

VkResult  VulkanRenderer::EndSingleTimeCommands(VkCommandBuffer commandBuffer, VkCommandPool& commandPool) {
    vkEndCommandBuffer(commandBuffer);

    VkSubmitInfo submitInfo{};
    submitInfo.sType = VK_STRUCTURE_TYPE_SUBMIT_INFO;
    submitInfo.commandBufferCount = 1;
    submitInfo.pCommandBuffers = &commandBuffer;

    VkResult result = vkQueueSubmit(cRenderer.SwapChain.GraphicsQueue, 1, &submitInfo, VK_NULL_HANDLE);
    result = vkQueueWaitIdle(cRenderer.SwapChain.GraphicsQueue);

    vkFreeCommandBuffers(cRenderer.Device, commandPool, 1, &commandBuffer);

    return result;
}

//VkCommandBuffer VulkanRenderer::BeginCommandBuffer()
//{
//    return Renderer_BeginSingleUseCommandBuffer(cRenderer.Device, cRenderer.CommandPool);
//}
//
//VkResult VulkanRenderer::BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo)
//{
//    return Renderer_BeginCommandBuffer(pCommandBufferList, commandBufferBeginInfo);
//}
//
//VkResult VulkanRenderer::EndCommandBuffer(VkCommandBuffer* pCommandBufferList)
//{
//	return Renderer_EndCommandBuffer(pCommandBufferList);
//}
//
//VkResult VulkanRenderer::EndCommandBuffer(VkCommandBuffer commandBuffer)
//{
//	return Renderer_EndSingleUseCommandBuffer(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, commandBuffer);
//}

void VulkanRenderer::DestroyFences()
{
    Renderer_DestroyFences(cRenderer.Device, cRenderer.AcquireImageSemaphores, cRenderer.PresentImageSemaphores, cRenderer.InFlightFences, MAX_FRAMES_IN_FLIGHT);
}

void VulkanRenderer::DestroyCommandPool()
{
    Renderer_DestroyCommandPool(cRenderer.Device, &cRenderer.CommandPool);
}

void VulkanRenderer::DestroyDevice()
{
    Renderer_DestroyDevice(cRenderer.Device);
}

void VulkanRenderer::DestroySurface()
{
    Renderer_DestroySurface(cRenderer.Instance, &cRenderer.Surface);
}

void VulkanRenderer::DestroyDebugger()
{
    Renderer_DestroyDebugger(&cRenderer.Instance);
}

void VulkanRenderer::DestroyInstance()
{
    Renderer_DestroyInstance(&cRenderer.Instance);
}

void VulkanRenderer::DestroyRenderPass(VkRenderPass& renderPass)
{
    Renderer_DestroyRenderPass(cRenderer.Device, &renderPass);
}

void VulkanRenderer::DestroyFrameBuffers(List<VkFramebuffer>& frameBufferList)
{
    Renderer_DestroyFrameBuffers(cRenderer.Device, frameBufferList.data(), frameBufferList.size());
}

void VulkanRenderer::DestroyDescriptorPool(VkDescriptorPool& descriptorPool)
{
    Renderer_DestroyDescriptorPool(cRenderer.Device, &descriptorPool);
}

void VulkanRenderer::DestroyDescriptorSetLayout(VkDescriptorSetLayout& descriptorSetLayout)
{
    Renderer_DestroyDescriptorSetLayout(cRenderer.Device, &descriptorSetLayout);
}

void VulkanRenderer::DestroyCommandBuffers(List<VkCommandBuffer>& commandBufferList)
{
    Renderer_DestroyCommandBuffers(cRenderer.Device, &cRenderer.CommandPool, commandBufferList.data(), commandBufferList.size());
}

void VulkanRenderer::DestroyBuffer(VkBuffer& buffer)
{
    Renderer_DestroyBuffer(cRenderer.Device, &buffer);
}

void VulkanRenderer::FreeDeviceMemory(VkDeviceMemory& memory)
{
    Renderer_FreeDeviceMemory(cRenderer.Device, &memory);
}

void VulkanRenderer::DestroySwapChainImageView()
{
    Renderer_DestroySwapChainImageView(cRenderer.Device, cRenderer.SwapChain.SwapChainImageViews, MAX_FRAMES_IN_FLIGHT);
}

void VulkanRenderer::DestroySwapChain()
{
    Renderer_DestroySwapChain(cRenderer.Device, &cRenderer.SwapChain.Swapchain);
}

void VulkanRenderer::DestroyImageView(VkImageView& imageView)
{
    Renderer_DestroyImageView(cRenderer.Device, &imageView);
}

void VulkanRenderer::DestroyImage(VkImage& image)
{
    Renderer_DestroyImage(cRenderer.Device, &image);
}

void VulkanRenderer::DestroySampler(VkSampler& sampler)
{
    Renderer_DestroySampler(cRenderer.Device, &sampler);
}

void VulkanRenderer::DestroyPipeline(VkPipeline& pipeline)
{
    Renderer_DestroyPipeline(cRenderer.Device, &pipeline);
}

void VulkanRenderer::DestroyPipelineLayout(VkPipelineLayout& pipelineLayout)
{
    Renderer_DestroyPipelineLayout(cRenderer.Device, &pipelineLayout);
}

void VulkanRenderer::DestroyPipelineCache(VkPipelineCache& pipelineCache)
{
    Renderer_DestroyPipelineCache(cRenderer.Device, &pipelineCache);
}

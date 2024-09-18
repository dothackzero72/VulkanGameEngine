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

    VULKAN_RESULT(SwapChain_GetSurfaceCapabilities(renderer.PhysicalDevice, renderer.Surface, &surfaceCapabilities));
    VULKAN_RESULT(SwapChain_GetPhysicalDeviceFormats(renderer.PhysicalDevice, renderer.Surface, &compatibleSwapChainFormatList, &surfaceFormatCount));
    VULKAN_RESULT(SwapChain_GetQueueFamilies(renderer.PhysicalDevice, renderer.Surface, &renderer.SwapChain.GraphicsFamily, &renderer.SwapChain.PresentFamily));
    VULKAN_RESULT(SwapChain_GetPhysicalDevicePresentModes(renderer.PhysicalDevice, renderer.Surface, &compatiblePresentModesList, &presentModeCount));
    VkSurfaceFormatKHR swapChainImageFormat = SwapChain_FindSwapSurfaceFormat(compatibleSwapChainFormatList, surfaceFormatCount);
    VkPresentModeKHR swapChainPresentMode = SwapChain_FindSwapPresentMode(compatiblePresentModesList, presentModeCount);
    vulkanWindow->GetFrameBufferSize(vulkanWindow, &width, &height);
    renderer.SwapChain.Swapchain = SwapChain_SetUpSwapChain(renderer.Device, renderer.Surface, surfaceCapabilities, swapChainImageFormat, swapChainPresentMode, renderer.SwapChain.GraphicsFamily, renderer.SwapChain.PresentFamily, width, height, &renderer.SwapChain.SwapChainImageCount);
    renderer.SwapChain.SwapChainImages = SwapChain_SetUpSwapChainImages(renderer.Device, renderer.SwapChain.Swapchain, renderer.SwapChain.SwapChainImageCount);
    renderer.SwapChain.SwapChainImageViews = SwapChain_SetUpSwapChainImageViews(renderer.Device, renderer.SwapChain.SwapChainImages, compatibleSwapChainFormatList, renderer.SwapChain.SwapChainImageCount);

    renderer.SwapChain.SwapChainResolution.width = width;
    renderer.SwapChain.SwapChainResolution.height = height;
    return VK_SUCCESS;
}

VkResult VulkanRenderer::RendererSetUp()
{
    renderer.RebuildRendererFlag = false;
    VkDebugUtilsMessengerEXT debugMessenger = VK_NULL_HANDLE;
    renderer.Instance = Renderer_CreateVulkanInstance();
    renderer.DebugMessenger = Renderer_SetupDebugMessenger(renderer.Instance);
    vulkanWindow->CreateSurface(vulkanWindow, &renderer.Instance, &renderer.Surface);
    VkResult deviceSetupResult = Renderer_SetUpPhysicalDevice(renderer.Instance, &renderer.PhysicalDevice, renderer.Surface, &renderer.PhysicalDeviceFeatures, &renderer.SwapChain.GraphicsFamily, &renderer.SwapChain.PresentFamily);
    renderer.Device = Renderer_SetUpDevice(renderer.PhysicalDevice, renderer.SwapChain.GraphicsFamily, renderer.SwapChain.PresentFamily);
    VULKAN_RESULT(VulkanRenderer::SetUpSwapChain());
    renderer.CommandPool = Renderer_SetUpCommandPool(renderer.Device, renderer.SwapChain.GraphicsFamily);
    VULKAN_RESULT(Renderer_SetUpSemaphores(renderer.Device, &renderer.InFlightFences, &renderer.AcquireImageSemaphores, &renderer.PresentImageSemaphores, MAX_FRAMES_IN_FLIGHT));
    VULKAN_RESULT(Renderer_GetDeviceQueue(renderer.Device, renderer.SwapChain.GraphicsFamily, renderer.SwapChain.PresentFamily, &renderer.SwapChain.GraphicsQueue, &renderer.SwapChain.PresentQueue));
    return VK_SUCCESS;
}

VkResult VulkanRenderer::RebuildSwapChain()
{
    renderer.RebuildRendererFlag = true;

    VULKAN_RESULT(vkDeviceWaitIdle(renderer.Device));
    Renderer_DestroySwapChainImageView(renderer.Device, renderer.SwapChain.SwapChainImageViews, MAX_FRAMES_IN_FLIGHT);
    vkDestroySwapchainKHR(renderer.Device, renderer.SwapChain.Swapchain, NULL);
    VulkanRenderer::SetUpSwapChain();
    return VK_SUCCESS;
}

VkResult VulkanRenderer::StartFrame()
{
	return Renderer_StartFrame(renderer.Device, 
                               renderer.SwapChain.Swapchain, 
                               renderer.InFlightFences, 
                               renderer.AcquireImageSemaphores, 
                               &renderer.ImageIndex, 
                               &renderer.CommandIndex,
                               &renderer.RebuildRendererFlag);
}

VkResult VulkanRenderer::EndFrame(List<VkCommandBuffer> commandBufferSubmitList)
{
    return Renderer_EndFrame(renderer.SwapChain.Swapchain,
                             renderer.AcquireImageSemaphores, 
                             renderer.PresentImageSemaphores, 
                             renderer.InFlightFences,
                             renderer.SwapChain.GraphicsQueue, 
                             renderer.SwapChain.PresentQueue,
                             renderer.CommandIndex,
                             renderer.ImageIndex,
                             commandBufferSubmitList.data(), 
                             commandBufferSubmitList.size(),
                             &renderer.RebuildRendererFlag);
}

VkResult VulkanRenderer::SubmitDraw(List<VkCommandBuffer> commandBufferSubmitList)
{
    return Renderer_SubmitDraw(renderer.SwapChain.Swapchain,
                               renderer.AcquireImageSemaphores,
                               renderer.PresentImageSemaphores,
                               renderer.InFlightFences,
                               renderer.SwapChain.GraphicsQueue,
                               renderer.SwapChain.PresentQueue,
                               renderer.CommandIndex,
                               renderer.ImageIndex,
                               commandBufferSubmitList.data(),
                               commandBufferSubmitList.size(),
                               &renderer.RebuildRendererFlag);
}

void VulkanRenderer::DestroyRenderer()
{
    VulkanRenderer::DestroyFences();
    VulkanRenderer::DestroyCommandPool();
    VulkanRenderer::DestroyFences();
    VulkanRenderer::DestroyDevice();
    VulkanRenderer::DestroyDebugger();
    VulkanRenderer::DestroySurface();
    VulkanRenderer::DestroyInstance();
}

VkResult VulkanRenderer::CreateCommandBuffers(List<VkCommandBuffer>& commandBufferList)
{
    return Renderer_CreateCommandBuffers(renderer.Device, renderer.CommandPool, commandBufferList.data(), commandBufferList.size());
}

VkResult VulkanRenderer::CreateFrameBuffer(VkFramebuffer frameBuffer, VkFramebufferCreateInfo& frameBufferCreateInfo)
{
    return Renderer_CreateFrameBuffer(renderer.Device, &frameBuffer, &frameBufferCreateInfo);
}

VkResult VulkanRenderer::CreateRenderPass(RenderPassCreateInfoStruct& renderPassCreateInfo)
{
    return Renderer_CreateRenderPass(renderer.Device, &renderPassCreateInfo);
}

VkResult VulkanRenderer::CreateDescriptorPool(VkDescriptorPool& descriptorPool, VkDescriptorPoolCreateInfo& descriptorPoolCreateInfo)
{
    return Renderer_CreateDescriptorPool(renderer.Device, &descriptorPool, &descriptorPoolCreateInfo);
}

VkResult VulkanRenderer::CreateDescriptorSetLayout(VkDescriptorSetLayout& descriptorSetLayout, VkDescriptorSetLayoutCreateInfo& descriptorSetLayoutCreateInfo)
{
    return Renderer_CreateDescriptorSetLayout(renderer.Device, &descriptorSetLayout, &descriptorSetLayoutCreateInfo);
}

VkResult VulkanRenderer::CreatePipelineLayout(VkPipelineLayout& pipelineLayout, VkPipelineLayoutCreateInfo& pipelineLayoutCreateInfo)
{
    return Renderer_CreatePipelineLayout(renderer.Device, &pipelineLayout, &pipelineLayoutCreateInfo);
}

VkResult VulkanRenderer::AllocateDescriptorSets(VkDescriptorSet& descriptorSet, VkDescriptorSetAllocateInfo& descriptorSetAllocateInfo)
{
    return Renderer_AllocateDescriptorSets(renderer.Device, &descriptorSet, &descriptorSetAllocateInfo);
}

VkResult VulkanRenderer::AllocateCommandBuffers(VkCommandBuffer commandBuffer, VkCommandBufferAllocateInfo& commandBufferAllocateInfo)
{
    return Renderer_AllocateCommandBuffers(renderer.Device, &commandBuffer, &commandBufferAllocateInfo);
}

VkResult VulkanRenderer::CreateGraphicsPipelines(VkPipeline& graphicPipeline, VkGraphicsPipelineCreateInfo createGraphicPipelines)
{
    return Renderer_CreateGraphicsPipelines(renderer.Device, &graphicPipeline, &createGraphicPipelines, 1);
}

VkResult VulkanRenderer::CreateGraphicsPipelines(VkPipeline& graphicPipeline, List<VkGraphicsPipelineCreateInfo> createGraphicPipelines)
{
    return Renderer_CreateGraphicsPipelines(renderer.Device, &graphicPipeline, createGraphicPipelines.data(), createGraphicPipelines.size());
}

VkResult VulkanRenderer::CreateCommandPool(VkCommandPool& commandPool, VkCommandPoolCreateInfo commandPoolInfo)
{
    return Renderer_CreateCommandPool(renderer.Device, &commandPool, &commandPoolInfo);
}

void VulkanRenderer::UpdateDescriptorSet(List<VkWriteDescriptorSet> writeDescriptorSetList)
{
    Renderer_UpdateDescriptorSet(renderer.Device, writeDescriptorSetList.data(), writeDescriptorSetList.size());
}

VkCommandBuffer VulkanRenderer::BeginCommandBuffer()
{
    return Renderer_BeginSingleUseCommandBuffer(renderer.Device, renderer.CommandPool);
}

VkResult VulkanRenderer::BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo)
{
    return Renderer_BeginCommandBuffer(pCommandBufferList, commandBufferBeginInfo);
}

VkResult VulkanRenderer::EndCommandBuffer(VkCommandBuffer* pCommandBufferList)
{
	return Renderer_EndCommandBuffer(pCommandBufferList);
}

VkResult VulkanRenderer::EndCommandBuffer(VkCommandBuffer commandBuffer)
{
	return Renderer_EndSingleUseCommandBuffer(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, commandBuffer);
}

void VulkanRenderer::DestroyFences()
{
    Renderer_DestroyFences(&renderer.Device, renderer.AcquireImageSemaphores, renderer.PresentImageSemaphores, renderer.InFlightFences, MAX_FRAMES_IN_FLIGHT);
}

void VulkanRenderer::DestroyCommandPool()
{
    Renderer_DestroyCommandPool(&renderer.Device, &renderer.CommandPool);
}

void VulkanRenderer::DestroyDevice()
{
    Renderer_DestroyDevice(&renderer.Device);
}

void VulkanRenderer::DestroySurface()
{
    Renderer_DestroySurface(&renderer.Instance, &renderer.Surface);
}

void VulkanRenderer::DestroyDebugger()
{
    Renderer_DestroyDebugger(&renderer.Instance);
}

void VulkanRenderer::DestroyInstance()
{
    Renderer_DestroyInstance(&renderer.Instance);
}

void VulkanRenderer::DestroyRenderPass(VkRenderPass& renderPass)
{
    Renderer_DestroyRenderPass(renderer.Device, &renderPass);
}

void VulkanRenderer::DestroyFrameBuffers(List<VkFramebuffer>& frameBufferList)
{
    Renderer_DestroyFrameBuffers(renderer.Device, frameBufferList.data(), frameBufferList.size());
}

void VulkanRenderer::DestroyDescriptorPool(VkDescriptorPool& descriptorPool)
{
    Renderer_DestroyDescriptorPool(renderer.Device, &descriptorPool);
}

void VulkanRenderer::DestroyDescriptorSetLayout(VkDescriptorSetLayout& descriptorSetLayout)
{
    Renderer_DestroyDescriptorSetLayout(renderer.Device, &descriptorSetLayout);
}

void VulkanRenderer::DestroyCommandBuffers(List<VkCommandBuffer>& commandBufferList)
{
    Renderer_DestroyCommandBuffers(renderer.Device, &renderer.CommandPool, commandBufferList.data(), commandBufferList.size());
}

void VulkanRenderer::DestroyBuffer(VkBuffer& buffer)
{
    Renderer_DestroyBuffer(renderer.Device, &buffer);
}

void VulkanRenderer::FreeMemory(VkDeviceMemory& memory)
{
    Renderer_FreeMemory(renderer.Device, &memory);
}

void VulkanRenderer::DestroySwapChainImageView(List<VkImageView>& swapChainImageViewList)
{
    Renderer_DestroySwapChainImageView(renderer.Device, swapChainImageViewList.data(), swapChainImageViewList.size());
}

void VulkanRenderer::DestroySwapChain(VkSwapchainKHR& swapChain)
{
    Renderer_DestroySwapChain(renderer.Device, &swapChain);
}

void VulkanRenderer::DestroyImageView(VkImageView& imageView)
{
    Renderer_DestroyImageView(renderer.Device, &imageView);
}

void VulkanRenderer::DestroyImage(VkImage& image)
{
    Renderer_DestroyImage(renderer.Device, &image);
}

void VulkanRenderer::DestroySampler(VkSampler& sampler)
{
    Renderer_DestroySampler(renderer.Device, &sampler);
}

void VulkanRenderer::DestroyPipeline(VkPipeline& pipeline)
{
    Renderer_DestroyPipeline(renderer.Device, &pipeline);
}

void VulkanRenderer::DestroyPipelineLayout(VkPipelineLayout& pipelineLayout)
{
    Renderer_DestroyPipelineLayout(renderer.Device, &pipelineLayout);
}

void VulkanRenderer::DestroyPipelineCache(VkPipelineCache& pipelineCache)
{
    Renderer_DestroyPipelineCache(renderer.Device, &pipelineCache);
}

#include "VulkanRenderPass.h"

// VulkanRenderPass RenderPass_CreateVulkanRenderPass(VkDevice device, VkGuid& levelId, RenderPassBuildInfoModel& model, ivec2& renderPassResolution)
//{
//    VulkanRenderPass vulkanRenderPass;
//    vulkanRenderPass.RenderPassId = model.RenderPassId;
//    vulkanRenderPass.SampleCount = VK_SAMPLE_COUNT_1_BIT;
//
//    RenderPass_BuildRenderPass(device, model);
//    //RenderPass_BuildFrameBuffer(device, model);
//   // RenderPass_BuildCommandBuffer();
//
//    //return DLL_EXPORT VulkanRenderPass();
//}

void RenderPass_DestroyRenderPass(RendererState& rendererState, VulkanRenderPass& renderPass, Vector<Texture>& renderedTextureList)
{
    for (auto renderedTexture : renderedTextureList)
    {
        Texture_DestroyTexture(rendererState.Device, renderedTexture);
    }

    Renderer_DestroyRenderPass(rendererState.Device, &renderPass.RenderPass);
    Renderer_DestroyCommandBuffers(rendererState.Device, &rendererState.CommandPool, &renderPass.CommandBuffer, rendererState.SwapChainImageCount);
    Renderer_DestroyFrameBuffers(rendererState.Device, &renderPass.FrameBufferList[0], rendererState.SwapChainImageCount);
}

VkResult RenderPass_CreateCommandBuffers(VkDevice device, VkCommandPool commandPool, VkCommandBuffer* commandBufferList, uint32 commandBufferCount)
{
    for (size_t x = 0; x < commandBufferCount; x++)
    {
        VkCommandBufferAllocateInfo commandBufferAllocateInfo =
        {
            .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
            .commandPool = commandPool,
            .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
            .commandBufferCount = 1
        };

        VULKAN_RESULT(vkAllocateCommandBuffers(device, &commandBufferAllocateInfo, &commandBufferList[x]));
    }
    return VK_SUCCESS;
}

VkResult RenderPass_CreateFrameBuffer(VkDevice device, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo)
{
    return vkCreateFramebuffer(device, *&frameBufferCreateInfo, NULL, pFrameBuffer);
}

VkRenderPass RenderPass_BuildRenderPass(VkDevice device, const RenderPassBuildInfoModel& renderPassBuildInfo)
{
    Vector<VkAttachmentDescription> attachmentDescriptionList = Vector<VkAttachmentDescription>();
    Vector<VkAttachmentReference> inputAttachmentReferenceList = Vector<VkAttachmentReference>();
    Vector<VkAttachmentReference> colorAttachmentReferenceList = Vector<VkAttachmentReference>();
    Vector<VkAttachmentReference> resolveAttachmentReferenceList = Vector<VkAttachmentReference>();
    Vector<VkSubpassDescription> preserveAttachmentReferenceList = Vector<VkSubpassDescription>();
    Vector<VkAttachmentReference> depthReference = Vector<VkAttachmentReference>();
    for (RenderedTextureInfoModel renderedTextureInfoModel : renderPassBuildInfo.RenderedTextureInfoModelList)
    {
        attachmentDescriptionList.emplace_back(renderedTextureInfoModel.AttachmentDescription);
        switch (renderedTextureInfoModel.TextureType)
        {
        case RenderedTextureType::ColorRenderedTexture:
        {
            colorAttachmentReferenceList.emplace_back(VkAttachmentReference
                {
                    .attachment = static_cast<uint32>(colorAttachmentReferenceList.size()),
                    .layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                });
            break;
        }
        case RenderedTextureType::InputAttachmentTexture:
        {
            inputAttachmentReferenceList.emplace_back(VkAttachmentReference
                {
                    .attachment = static_cast<uint32>(inputAttachmentReferenceList.size()),
                    .layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                });
            break;
        }
        case RenderedTextureType::ResolveAttachmentTexture:
        {
            resolveAttachmentReferenceList.emplace_back(VkAttachmentReference
                {
                    .attachment = static_cast<uint32>(colorAttachmentReferenceList.size() + 1),
                    .layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                });
            break;
        }
        default:
        {
            throw std::runtime_error("Case doesn't exist: RenderedTextureType");
        }
        case RenderedTextureType::DepthRenderedTexture:
        {
            depthReference.emplace_back(VkAttachmentReference
                {
                    .attachment = (uint)(colorAttachmentReferenceList.size() + resolveAttachmentReferenceList.size()),
                    .layout = VK_IMAGE_LAYOUT_DEPTH_ATTACHMENT_OPTIMAL
                });
            break;
        }
        }
    }

    Vector<VkSubpassDescription> subpassDescriptionList =
    {
        VkSubpassDescription
        {
            .flags = 0,
            .pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS,
            .inputAttachmentCount = static_cast<uint32>(inputAttachmentReferenceList.size()),
            .pInputAttachments = inputAttachmentReferenceList.data(),
            .colorAttachmentCount = static_cast<uint32>(colorAttachmentReferenceList.size()),
            .pColorAttachments = colorAttachmentReferenceList.data(),
            .pResolveAttachments = resolveAttachmentReferenceList.data(),
            .pDepthStencilAttachment = nullptr,
            .preserveAttachmentCount = static_cast<uint32>(inputAttachmentReferenceList.size()),
            .pPreserveAttachments = nullptr,
        }
    };
    if (depthReference.size() > 0)
    {
        subpassDescriptionList[0].pDepthStencilAttachment = &depthReference[0];
    }

    Vector<VkSubpassDependency> subPassList = Vector<VkSubpassDependency>();
    for (VkSubpassDependency subpass : renderPassBuildInfo.SubpassDependencyModelList)
    {
        subPassList.emplace_back(subpass);
    }

    VkRenderPassCreateInfo renderPassInfo =
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
        .attachmentCount = static_cast<uint32>(attachmentDescriptionList.size()),
        .pAttachments = attachmentDescriptionList.data(),
        .subpassCount = static_cast<uint32>(subpassDescriptionList.size()),
        .pSubpasses = subpassDescriptionList.data(),
        .dependencyCount = static_cast<uint32>(subPassList.size()),
        .pDependencies = subPassList.data()
    };

    VkRenderPass renderPass = VK_NULL_HANDLE;
    VULKAN_RESULT(vkCreateRenderPass(device, &renderPassInfo, nullptr, &renderPass));
    return renderPass;
}

Vector<VkFramebuffer> RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<VkImageView>& imageViewList, VkImageView* depthImageView, Vector<VkImageView>& swapChainImageViews, ivec2 renderPassResolution)
{
    Vector<VkFramebuffer> frameBufferList = Vector<VkFramebuffer>(swapChainImageViews.size());
    for (size_t x = 0; x < swapChainImageViews.size(); x++)
    {
        std::vector<VkImageView> TextureAttachmentList;
        for (int y = 0; y < imageViewList.size(); y++)
        {
            if (renderPassBuildInfo.IsRenderedToSwapchain)
            {
                TextureAttachmentList.emplace_back(swapChainImageViews[x]);
            }
            else
            {
                TextureAttachmentList.emplace_back(imageViewList[y]);
            }
        }
        if (depthImageView != nullptr &&
            *depthImageView != VK_NULL_HANDLE)
        {
            TextureAttachmentList.emplace_back(*depthImageView);
        }

        VkFramebufferCreateInfo framebufferInfo = {
            .sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
            .renderPass = renderPass,
            .attachmentCount = static_cast<uint32_t>(TextureAttachmentList.size()),
            .pAttachments = TextureAttachmentList.data(),
            .width = static_cast<uint32_t>(renderPassResolution.x),
            .height = static_cast<uint32_t>(renderPassResolution.y),
            .layers = 1,
        };
        VULKAN_RESULT(vkCreateFramebuffer(device, &framebufferInfo, nullptr, &frameBufferList[x]));
    }
    return frameBufferList;
}
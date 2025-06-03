#include "VulkanRenderPass.h"

VulkanRenderPass RenderPass_CreateVulkanRenderPass(const RendererState& renderState, RenderPassBuildInfoModel& model, ivec2& renderPassResolution, int ConstBuffer, Vector<Texture>& renderedTextureList, Texture& depthTexture)
{
       VulkanRenderPass vulkanRenderPass;
       vulkanRenderPass.RenderPassId = model.RenderPassId; 
       vulkanRenderPass.SampleCount = VK_SAMPLE_COUNT_1_BIT; 
       vulkanRenderPass.RenderPass = RenderPass_BuildRenderPass(renderState, vulkanRenderPass, model, renderedTextureList, depthTexture);
       vulkanRenderPass.FrameBufferList = RenderPass_BuildFrameBuffer(renderState, vulkanRenderPass, model, renderedTextureList, depthTexture, renderPassResolution);
       vulkanRenderPass.ClearValueList = model.ClearValueList;
       vulkanRenderPass.RenderArea = model.RenderArea.RenderArea;
       RenderPass_CreateCommandBuffers(renderState, &vulkanRenderPass.CommandBuffer, 1);

       return vulkanRenderPass;
}

void RenderPass_DestroyRenderPass(RendererState& renderState, VulkanRenderPass& renderPass)
{
    Renderer_DestroyRenderPass(renderState.Device, &renderPass.RenderPass);
    Renderer_DestroyCommandBuffers(renderState.Device, &renderState.CommandPool, &renderPass.CommandBuffer, 1);
    Renderer_DestroyFrameBuffers(renderState.Device, &renderPass.FrameBufferList[0], renderState.SwapChainImageCount);

    renderPass.FrameBufferList.clear();
    renderPass.ClearValueList.clear();
    renderPass.RenderPassId = VkGuid();
    renderPass.SampleCount = VK_SAMPLE_COUNT_FLAG_BITS_MAX_ENUM;
    renderPass.RenderArea = VkRect2D();
    renderPass.UseFrameBufferResolution = false;
}

VkResult RenderPass_CreateCommandBuffers(const RendererState& renderState, VkCommandBuffer* commandBufferList, uint32 commandBufferCount)
{
    for (size_t x = 0; x < commandBufferCount; x++)
    {
        VkCommandBufferAllocateInfo commandBufferAllocateInfo =
        {
            .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
            .commandPool = renderState.CommandPool,
            .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
            .commandBufferCount = 1
        };

        VULKAN_RESULT(vkAllocateCommandBuffers(renderState.Device, &commandBufferAllocateInfo, &commandBufferList[x]));
    }
    return VK_SUCCESS;
}

VkResult RenderPass_CreateFrameBuffer(const RendererState& renderState, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo)
{
    return vkCreateFramebuffer(renderState.Device, *&frameBufferCreateInfo, NULL, pFrameBuffer);
}

VkRenderPass RenderPass_BuildRenderPass(const RendererState& renderState, VulkanRenderPass& vulkanRenderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<Texture>& renderedTextureList, Texture& depthTexture)
{
    for (auto& texture : renderPassBuildInfo.RenderedTextureInfoModelList)
    {
        VkImageCreateInfo imageCreateInfo = texture.ImageCreateInfo;
        VkSamplerCreateInfo samplerCreateInfo = texture.SamplerCreateInfo;
        switch (texture.TextureType)
        {
            case ColorRenderedTexture: renderedTextureList.emplace_back(Texture_CreateTexture(renderState, VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case InputAttachmentTexture: renderedTextureList.emplace_back(Texture_CreateTexture(renderState, VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case ResolveAttachmentTexture: renderedTextureList.emplace_back(Texture_CreateTexture(renderState, VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case DepthRenderedTexture: depthTexture = Texture_CreateTexture(renderState, VK_IMAGE_ASPECT_DEPTH_BIT, imageCreateInfo, samplerCreateInfo); break;
        };
    }

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
    VULKAN_RESULT(vkCreateRenderPass(renderState.Device, &renderPassInfo, nullptr, &renderPass));
    return renderPass;
}

Vector<VkFramebuffer> RenderPass_BuildFrameBuffer(const RendererState& renderState, const VulkanRenderPass& vulkanRenderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<Texture>& renderedTextureList, Texture& depthTexture, ivec2& renderPassResolution)
{
    Vector<VkFramebuffer> frameBufferList = Vector<VkFramebuffer>(renderState.SwapChainImageViews.size());
    for (size_t x = 0; x < renderState.SwapChainImageViews.size(); x++)
    {
        std::vector<VkImageView> TextureAttachmentList;
        for (int y = 0; y < renderedTextureList.size(); y++)
        {
            if (renderPassBuildInfo.IsRenderedToSwapchain)
            {
                TextureAttachmentList.emplace_back(renderState.SwapChainImageViews[x]);
            }
            else
            {
                TextureAttachmentList.emplace_back(renderedTextureList[y].textureView);
            }
        }
        if (depthTexture.textureView != VK_NULL_HANDLE)
        {
            TextureAttachmentList.emplace_back(depthTexture.textureView);
        }

        VkFramebufferCreateInfo framebufferInfo = {
            .sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
            .renderPass = vulkanRenderPass.RenderPass,
            .attachmentCount = static_cast<uint32_t>(TextureAttachmentList.size()),
            .pAttachments = TextureAttachmentList.data(),
            .width = static_cast<uint32_t>(renderPassResolution.x),
            .height = static_cast<uint32_t>(renderPassResolution.y),
            .layers = 1,
        };
        VULKAN_RESULT(vkCreateFramebuffer(renderState.Device, &framebufferInfo, nullptr, &frameBufferList[x]));
    }
    return frameBufferList;
}
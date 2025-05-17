#include "JsonRenderPass.h"
#include "CVulkanRenderer.h"
#include <stdexcept>
#include "RenderSystem.h"
#include "JsonPipeline.h"
#include "GameSystem.h"

JsonRenderPass::JsonRenderPass()
{
}

JsonRenderPass::JsonRenderPass(VkGuid& levelId, RenderPassBuildInfoModel& model, ivec2& renderPassResolution)
{
    RenderPassId = model.RenderPassId;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;

    renderSystem.renderPassBuildInfoList[RenderPassId] = model;
    renderSystem.RenderPassResolutionList[RenderPassId] = renderPassResolution;
    UseFrameBufferResolution = renderSystem.renderPassBuildInfoList[RenderPassId].IsRenderedToSwapchain;

    BuildRenderPass(renderSystem.renderPassBuildInfoList[RenderPassId]);
    BuildFrameBuffer(renderSystem.renderPassBuildInfoList[RenderPassId]);
    BuildCommandBuffer();

    for (auto& renderPipeline : model.RenderPipelineList)
    {
        uint id = renderSystem.RenderPipelineList.size();
        renderSystem.RenderPipelineList[RenderPassId].emplace_back(JsonPipeline(RenderPassId, levelId, id, renderPipeline, RenderPass, sizeof(SceneDataBuffer), renderPassResolution));
    }
    renderSystem.SpriteBatchLayerList[RenderPassId].emplace_back(SpriteBatchLayer(RenderPassId));

    renderSystem.ClearValueList[RenderPassId] = renderSystem.renderPassBuildInfoList[RenderPassId].ClearValueList;
    renderArea = renderSystem.renderPassBuildInfoList[RenderPassId].RenderArea.RenderArea;
    renderSystem.RenderPassInfoList[RenderPassId] = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPass,
        .framebuffer = FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderSystem.renderPassBuildInfoList[RenderPassId].RenderArea.RenderArea,
        .clearValueCount = static_cast<uint32>(renderSystem.ClearValueList[RenderPassId].size()),
        .pClearValues = renderSystem.ClearValueList[RenderPassId].data()
    };
}

JsonRenderPass::JsonRenderPass(VkGuid& levelId, RenderPassBuildInfoModel& model, Texture& inputTexture, ivec2& renderPassResolution)
{
    RenderPassId = model.RenderPassId;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;

    renderSystem.renderPassBuildInfoList[RenderPassId] = model;
    renderSystem.RenderPassResolutionList[RenderPassId] = renderPassResolution;
    UseFrameBufferResolution = renderSystem.renderPassBuildInfoList[RenderPassId].IsRenderedToSwapchain;

    BuildRenderPass(renderSystem.renderPassBuildInfoList[RenderPassId]);
    BuildFrameBuffer(renderSystem.renderPassBuildInfoList[RenderPassId]);
    BuildCommandBuffer();

    uint id = renderSystem.RenderPipelineList.size();
    renderSystem.InputTextureList[id].emplace_back(std::make_shared<Texture>(inputTexture));
    renderSystem.RenderPipelineList[RenderPassId].emplace_back(JsonPipeline(RenderPassId, levelId, id, renderSystem.renderPassBuildInfoList[RenderPassId].RenderPipelineList[0], RenderPass, sizeof(SceneDataBuffer), renderPassResolution));
    renderArea = renderSystem.renderPassBuildInfoList[RenderPassId].RenderArea.RenderArea;
    renderSystem.ClearValueList[RenderPassId] = renderSystem.renderPassBuildInfoList[RenderPassId].ClearValueList;
    renderSystem.RenderPassInfoList[RenderPassId] = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPass,
        .framebuffer = FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderSystem.renderPassBuildInfoList[RenderPassId].RenderArea.RenderArea,
        .clearValueCount = static_cast<uint32>(renderSystem.ClearValueList[RenderPassId].size()),
        .pClearValues = renderSystem.ClearValueList[RenderPassId].data()
    };
}

JsonRenderPass::~JsonRenderPass()
{
}

void JsonRenderPass::RecreateSwapchain(int newWidth, int newHeight)
{
    renderer.DestroyRenderPass(RenderPass);
    renderer.DestroyCommandBuffers(CommandBuffer);
    renderer.DestroyFrameBuffers(FrameBufferList);
    FrameBufferList.clear();

    Vector<VkImageView> imageViewList;
    for (auto& renderedTexture : renderSystem.RenderedTextureList[RenderPassId])
    {
        imageViewList.emplace_back(renderedTexture.View);
    }
    VkImageView depthTexture = renderSystem.DepthTextureList[RenderPassId].View;

    RenderPass = RenderPass_BuildRenderPass(cRenderer.Device, renderSystem.renderPassBuildInfoList[RenderPassId]);
    FrameBufferList = RenderPass_BuildFrameBuffer(cRenderer.Device, RenderPass, renderSystem.renderPassBuildInfoList[RenderPassId], imageViewList, &depthTexture, cRenderer.SwapChainImageViews, renderSystem.RenderPassResolutionList[RenderPassId]);
    BuildCommandBuffer();

    for (auto& pipeline : renderSystem.RenderPipelineList[RenderPassId])
    {
        pipeline.RecreateSwapchain(RenderPass, sizeof(SceneDataBuffer), newWidth, newHeight);
    }

    renderSystem.ClearValueList[RenderPassId] = renderSystem.renderPassBuildInfoList[RenderPassId].ClearValueList;
    renderSystem.RenderPassInfoList[RenderPassId] = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPass,
        .framebuffer = FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderSystem.renderPassBuildInfoList[RenderPassId].RenderArea.RenderArea,
        .clearValueCount = static_cast<uint32>(renderSystem.ClearValueList[RenderPassId].size()),
        .pClearValues = renderSystem.ClearValueList[RenderPassId].data()
    };
}

void JsonRenderPass::BuildRenderPass(const RenderPassBuildInfoModel& renderPassBuildInfo)
{
    Vector<RenderedTexture> RenderedColorTextureList;
    for (auto& texture : renderPassBuildInfo.RenderedTextureInfoModelList)
    {
        VkImageCreateInfo imageCreateInfo = texture.ImageCreateInfo;
        VkSamplerCreateInfo samplerCreateInfo = texture.SamplerCreateInfo;
        switch (texture.TextureType)
        {
            case ColorRenderedTexture: renderSystem.RenderedTextureList[RenderPassId].emplace_back(RenderedTexture(VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case InputAttachmentTexture: renderSystem.RenderedTextureList[RenderPassId].emplace_back(RenderedTexture(VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case ResolveAttachmentTexture: renderSystem.RenderedTextureList[RenderPassId].emplace_back(RenderedTexture(VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case DepthRenderedTexture: renderSystem.DepthTextureList[RenderPassId] = DepthTexture(imageCreateInfo, samplerCreateInfo); break;
            default: throw std::runtime_error("Case doesn't exist: RenderedTextureType");
        };
    }

    RenderPass = RenderPass_BuildRenderPass(cRenderer.Device, renderPassBuildInfo);
}

void JsonRenderPass::BuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo)
{
    Vector<VkImageView> imageViewList;
    for (int x = 0; x < renderSystem.RenderedTextureList[RenderPassId].size(); x++)
    {
        imageViewList.emplace_back(renderSystem.RenderedTextureList[RenderPassId][x].View);
    }

    SharedPtr<VkImageView> depthTextureView = nullptr;
    if (renderSystem.DepthTextureList.find(RenderPassId) != renderSystem.DepthTextureList.end())
    {
        depthTextureView = std::make_shared<VkImageView>(renderSystem.DepthTextureList[RenderPassId].View);
    }

    VkRenderPass& renderPass = RenderPass;
    FrameBufferList.resize(cRenderer.SwapChainImageCount);
    FrameBufferList = RenderPass_BuildFrameBuffer(cRenderer.Device, renderPass, renderPassBuildInfo, imageViewList, depthTextureView.get(), cRenderer.SwapChainImageViews, renderSystem.RenderPassResolutionList[RenderPassId]);
}

void JsonRenderPass::RebuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo)
{
    VkRenderPass& renderPass = RenderPass;
    Vector<RenderedTexture> renderedTextureList = renderSystem.RenderedTextureList[RenderPassId];
    FrameBufferList.resize(cRenderer.SwapChainImageCount);
   // FrameBufferList = RenderPass_BuildFrameBuffer(cRenderer.Device, renderPass, renderPassBuildInfo, renderedTextureList, depthTextureView.get(), cRenderer.SwapChain.SwapChainImageViews, renderSystem.RenderPassResolutionList[RenderPassId]);
}

void JsonRenderPass::BuildCommandBuffer()
{
    Renderer_CreateCommandBuffers(cRenderer.Device, cRenderer.CommandPool, &CommandBuffer, 1);
}

void JsonRenderPass::Destroy()
{
    for (auto renderedTexture : renderSystem.RenderedTextureList[RenderPassId])
    {
        renderedTexture.Destroy();
    }

    renderer.DestroyRenderPass(RenderPass);
    renderer.DestroyCommandBuffers(CommandBuffer);
    renderer.DestroyFrameBuffers(FrameBufferList);
    FrameBufferList.clear();
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
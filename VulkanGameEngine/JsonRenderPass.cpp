#include "JsonRenderPass.h"
#include "MemoryManager.h"


JsonRenderPass::JsonRenderPass()
{
}

JsonRenderPass::JsonRenderPass(const JsonRenderPass& df)
{
}

JsonRenderPass::JsonRenderPass(String jsonPath, ivec2 renderPassResolution)
{
    RenderPassResolution = renderPassResolution;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;

    CommandBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);
    FrameBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);

    VULKAN_RESULT(renderer.CreateCommandBuffers(CommandBufferList));

    nlohmann::json json= Json::ReadJson("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\RenderPass\\Default2DRenderPass.json");
    RenderPassBuildInfoModel renderPassBuildInfo = RenderPassBuildInfoModel::from_json(json, renderPassResolution);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer();

    JsonPipelineList.emplace_back(JsonPipeline::CreateJsonRenderPass("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines\\Default2DPipeline.json", RenderPass, sizeof(SceneDataBuffer)));
}

JsonRenderPass::~JsonRenderPass()
{
}

std::shared_ptr<JsonRenderPass> JsonRenderPass::JsonCreateRenderPass(String jsonPath, ivec2 renderPassResolution)
{
    std::shared_ptr<JsonRenderPass> renderPass = MemoryManager::AllocateJsonRenderPass();
    new (renderPass.get()) JsonRenderPass(jsonPath, renderPassResolution);
    return renderPass;
}

VkCommandBuffer JsonRenderPass::Draw(List<std::shared_ptr<GameObject>> meshList, SceneDataBuffer& sceneProperties)
{
    std::vector<VkClearValue> clearValues
    {
        VkClearValue{.color = { {0.0f, 0.0f, 0.0f, 1.0f} } },
        VkClearValue{.depthStencil = { 1.0f, 0 } }
    };

    VkRenderPassBeginInfo renderPassInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPass,
        .framebuffer = FrameBufferList[cRenderer.ImageIndex],
        .renderArea
        {
            .offset = {0, 0},
            .extent =
            {
                static_cast<uint32>(RenderPassResolution.x),
                static_cast<uint32>(RenderPassResolution.y)
            }
        },
        .clearValueCount = static_cast<uint32>(clearValues.size()),
        .pClearValues = clearValues.data()
    };

    VkViewport viewport = VkViewport
    {
        .x = 0.0f,
        .y = 0.0f,
        .width = (float)RenderPassResolution.x,
        .height = (float)RenderPassResolution.y,
        .minDepth = 0.0f,
        .maxDepth = 1.0f
    };

    VkRect2D scissor = VkRect2D
    {
        .offset =  VkOffset2D(0, 0),
        .extent =  VkExtent2D
        {
            .width = (uint)RenderPassResolution.x,
            .height = (uint)RenderPassResolution.y
        }
    };

    VkCommandBufferBeginInfo CommandBufferBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
    };

    VULKAN_RESULT(vkBeginCommandBuffer(CommandBufferList[cRenderer.CommandIndex], &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(CommandBufferList[cRenderer.CommandIndex], &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    vkCmdSetViewport(CommandBufferList[cRenderer.CommandIndex], 0, 1, &viewport);
    vkCmdSetScissor(CommandBufferList[cRenderer.CommandIndex], 0, 1, &scissor);
    for (auto mesh : meshList)
    {
        mesh->Draw(CommandBufferList[cRenderer.CommandIndex], JsonPipelineList[0]->Pipeline, JsonPipelineList[0]->PipelineLayout, JsonPipelineList[0]->DescriptorSetList[0], sceneProperties);
    }
    vkCmdEndRenderPass(CommandBufferList[cRenderer.CommandIndex]);
    vkEndCommandBuffer(CommandBufferList[cRenderer.CommandIndex]);
    return CommandBufferList[cRenderer.CommandIndex];
}

void JsonRenderPass::BuildRenderPass(RenderPassBuildInfoModel renderPassBuildInfo)
{
    List<VkAttachmentDescription> attachmentDescriptionList = List<VkAttachmentDescription>();
    List<VkAttachmentReference> inputAttachmentReferenceList = List<VkAttachmentReference>();
    List<VkAttachmentReference> colorAttachmentReferenceList = List<VkAttachmentReference>();
    List<VkAttachmentReference> resolveAttachmentReferenceList = List<VkAttachmentReference>();
    List<VkSubpassDescription> preserveAttachmentReferenceList = List<VkSubpassDescription>();
    List<VkAttachmentReference> depthReference = List<VkAttachmentReference>();
    for (RenderedTextureInfoModel renderedTextureInfoModel : renderPassBuildInfo.RenderedTextureInfoModelList)
    {
        attachmentDescriptionList.emplace_back(renderedTextureInfoModel.AttachmentDescription);
        switch (renderedTextureInfoModel.TextureType)
        {
            case RenderedTextureType::ColorRenderedTexture:
            {
                if (!renderPassBuildInfo.IsRenderedToSwapchain)
                {
                    RenderedColorTextureList.emplace_back(nullptr);
                }
                else
                {
                    RenderedColorTextureList.emplace_back(std::make_shared<RenderedTexture>(RenderedTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo)));
                }
                colorAttachmentReferenceList.emplace_back(VkAttachmentReference
                    {
                        .attachment = static_cast<uint32>(colorAttachmentReferenceList.size()),
                        .layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                    });
                break;
            }
            case RenderedTextureType::DepthRenderedTexture:
            {
                depthTexture = std::make_shared<DepthTexture>(DepthTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
                depthReference.emplace_back(VkAttachmentReference
                {
                    .attachment = (uint)(colorAttachmentReferenceList.size() + resolveAttachmentReferenceList.size()),
                    .layout = VK_IMAGE_LAYOUT_DEPTH_ATTACHMENT_OPTIMAL
                });
                break;
            }
            case RenderedTextureType::InputAttachmentTexture:
            {
                RenderedColorTextureList.emplace_back(std::make_shared<RenderedTexture>(RenderedTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo)));
                inputAttachmentReferenceList.emplace_back(VkAttachmentReference
                    {
                        .attachment = static_cast<uint32>(inputAttachmentReferenceList.size()),
                        .layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                    });
                break;
            }
            case RenderedTextureType::ResolveAttachmentTexture:
            {
                RenderedColorTextureList.emplace_back(std::make_shared<RenderedTexture>(RenderedTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo)));
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
        }
    }
    List<VkSubpassDescription> subpassDescriptionList =
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

    List<VkSubpassDependency> subPassList = List<VkSubpassDependency>();
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
    VULKAN_RESULT(vkCreateRenderPass(cRenderer.Device, &renderPassInfo, nullptr, &RenderPass));
}

void JsonRenderPass::BuildFrameBuffer()
{
    for (size_t x = 0; x < cRenderer.SwapChain.SwapChainImageCount; x++)
    {
        std::vector<VkImageView> TextureAttachmentList;
        for (auto texture : RenderedColorTextureList)
        {
            if (texture == nullptr)
            {
                TextureAttachmentList.emplace_back(cRenderer.SwapChain.SwapChainImageViews[x]);
            }
            else
            {
                TextureAttachmentList.emplace_back(texture->View);
            }
        }
        if (depthTexture != nullptr)
        {
            TextureAttachmentList.emplace_back(depthTexture->View);
        }

        VkFramebufferCreateInfo framebufferInfo
        {
            .sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
            .renderPass = RenderPass,
            .attachmentCount = static_cast<uint32>(TextureAttachmentList.size()),
            .pAttachments = TextureAttachmentList.data(),
            .width = static_cast<uint32>(RenderPassResolution.x),
            .height = static_cast<uint32>(RenderPassResolution.y),
            .layers = 1,
        };
        VULKAN_RESULT(vkCreateFramebuffer(cRenderer.Device, &framebufferInfo, nullptr, &FrameBufferList[x]));
    }
}


void JsonRenderPass::Destroy()
{
    for (auto renderedTexture : RenderedColorTextureList)
    {
        renderedTexture->Destroy();
    }
    for (auto pipeline : JsonPipelineList)
    {
        pipeline->Destroy();
    }
    depthTexture->Destroy();
    renderer.DestroyRenderPass(RenderPass);
    renderer.DestroyCommandBuffers(CommandBufferList);
    renderer.DestroyFrameBuffers(FrameBufferList);

}

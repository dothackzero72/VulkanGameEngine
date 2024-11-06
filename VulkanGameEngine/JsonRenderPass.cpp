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

    nlohmann::json jsonLoader;
    RenderPassBuildInfoModel renderPassBuildInfo = JsonToRenderPassBuildInfoModel(jsonLoader);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer();

    JsonPipelineList.emplace_back(JsonPipeline::CreateJsonRenderPass("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines\\DefaultPipeline.json"));
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
        VkClearValue{.color = { {0.0f, 0.0f, 0.0f, 1.0f} } }
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
                static_cast<uint32>(cRenderer.SwapChain.SwapChainResolution.width),
                static_cast<uint32>(cRenderer.SwapChain.SwapChainResolution.height)
            }
        },
        .clearValueCount = static_cast<uint32>(clearValues.size()),
        .pClearValues = clearValues.data()
    };

    VkCommandBufferBeginInfo CommandBufferBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
    };

    VULKAN_RESULT(vkBeginCommandBuffer(CommandBufferList[cRenderer.CommandIndex], &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(CommandBufferList[cRenderer.CommandIndex], &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    for (auto mesh : meshList)
    {
        mesh->Draw(CommandBufferList[cRenderer.CommandIndex], JsonPipelineList[0]->Pipeline, JsonPipelineList[0]->PipelineLayout, JsonPipelineList[0]->DescriptorSet, sceneProperties);
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
    VkAttachmentReference depthReference = VkAttachmentReference();
    for (RenderedTextureInfoModel renderedTextureInfoModel : renderPassBuildInfo.RenderedTextureInfoModelList)
    {
        attachmentDescriptionList.emplace_back(renderedTextureInfoModel.AttachmentDescription);
        switch (renderedTextureInfoModel.TextureType)
        {
            case RenderedTextureType::ColorRenderedTexture:
            {
                if (!renderedTextureInfoModel.IsRenderedToSwapchain)
                {
                    RenderedColorTextureList.emplace_back(nullptr);
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
               // depthTexture = std::make_shared<DepthTexture>(DepthTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
                depthReference = VkAttachmentReference
                {
                    .attachment = (uint)(colorAttachmentReferenceList.size() + resolveAttachmentReferenceList.size()),
                    .layout = VK_IMAGE_LAYOUT_DEPTH_ATTACHMENT_OPTIMAL
                };
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
              //  RenderedColorTextureList.emplace_back(std::make_shared<RenderedTexture>(RenderedTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo)));
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
                .pDepthStencilAttachment = &depthReference,
                .preserveAttachmentCount = static_cast<uint32>(inputAttachmentReferenceList.size()),
                .pPreserveAttachments = nullptr,
            }
        };

        List<VkSubpassDependency> subPassList = List<VkSubpassDependency>();
      /*  for (VkSubpassDependency subpass : renderPassBuildInfo.SubpassDependencyList)
        {
            subPassList.emplace_back(subpass);
        }*/

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
        vkCreateRenderPass(cRenderer.Device, &renderPassInfo, nullptr, &RenderPass);
    }
}

void JsonRenderPass::BuildFrameBuffer()
{
    for (size_t x = 0; x < cRenderer.SwapChain.SwapChainImageCount; x++)
    {
        std::vector<VkImageView> TextureAttachmentList;
        for (auto texture : RenderedColorTextureList)
        {
            TextureAttachmentList.emplace_back(texture->View);
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

VkAttachmentDescription JsonRenderPass::JsonToVulkanAttachmentDescription(nlohmann::json& json)
{
    return VkAttachmentDescription
    {
        .flags = json["flags"],
        .format = json["format"],
        .samples = json["samples"],
        .loadOp = json["loadOp"],
        .storeOp = json["storeOp"],
        .stencilLoadOp = json["stencilLoadOp"],
        .stencilStoreOp = json["stencilStoreOp"],
        .initialLayout = json["initialLayout"],
        .finalLayout = json["finalLayout"]
    };
}

VkImageCreateInfo JsonRenderPass::JsonToVulkanImageCreateInfo(nlohmann::json& json)
{
    return VkImageCreateInfo
    {
        .sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
        .pNext = nullptr,
        .flags = json["flags"],
        .imageType = json["imageType"],
        .format = json["format"],
        .extent = VkExtent3D{
            json["extent"]["width"],
            json["extent"]["height"],
            json["extent"]["depth"]
        },
        .mipLevels = json["mipLevels"],
        .arrayLayers = json["arrayLayers"],
        .samples = json["samples"],
        .tiling = json["tiling"],
        .usage = json["usage"],
        .sharingMode = json["sharingMode"],
        .queueFamilyIndexCount = json["queueFamilyIndexCount"],
        .pQueueFamilyIndices = json["pQueueFamilyIndices"].get<std::vector<uint32_t>>().data(),
        .initialLayout = json["initialLayout"]
    };
}

VkSamplerCreateInfo JsonRenderPass::JsonToVulkanSamplerCreateInfo(nlohmann::json& json)
{
    return VkSamplerCreateInfo
    {
        .sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
        .pNext = nullptr,
        .flags = json["flags"],
        .magFilter = json["magFilter"],
        .minFilter = json["minFilter"],
        .mipmapMode = json["mipmapMode"],
        .addressModeU = json["addressModeU"],
        .addressModeV = json["addressModeV"],
        .addressModeW = json["addressModeW"],
        .mipLodBias = json["mipLodBias"],
        .anisotropyEnable = json["anisotropyEnable"],
        .maxAnisotropy = json["maxAnisotropy"],
        .compareEnable = json["compareEnable"],
        .compareOp = json["compareOp"],
        .minLod = json["minLod"],
        .maxLod = json["maxLod"],
        .borderColor = json["borderColor"],
        .unnormalizedCoordinates = json["unnormalizedCoordinates"]
    };
}

VkSubpassDependency JsonRenderPass::JsonToVulkanSubpassDependency(nlohmann::json& json)
{
    return VkSubpassDependency
    {
        .srcSubpass = json["srcSubpass"],
        .dstSubpass = json["dstSubpass"],
        .srcStageMask = json["srcStageMask"],
        .dstStageMask = json["dstStageMask"],
        .srcAccessMask = json["srcAccessMask"],
        .dstAccessMask = json["dstAccessMask"],
        .dependencyFlags = json["dependencyFlags"]
    };
}

RenderedTextureInfoModel JsonRenderPass::JsonToRenderedTextureInfoModel(nlohmann::json& json)
{
    return RenderedTextureInfoModel
    {
         .IsRenderedToSwapchain = json["IsRenderedToSwapchain"],
         .RenderedTextureInfoName = json["RenderedTextureInfoName"],
         .ImageCreateInfo = json["ImageCreateInfo"],
         .SamplerCreateInfo = json["SamplerCreateInfo"],
         .AttachmentDescription = json["AttachmentDescription"],
         .TextureType = json["TextureType"]
    };
}

RenderPassBuildInfoModel JsonRenderPass::JsonToRenderPassBuildInfoModel(nlohmann::json& json)
{
    RenderPassBuildInfoModel asdf = RenderPassBuildInfoModel();
    asdf.Name = json["RenderedTextureInfoName"];

    return  RenderPassBuildInfoModel
    {
       /*     .RenderedTextureInfoModelList = JsonToRenderedTextureInfoModel(json["RenderedTextureInfoModelList"]),
            .RenderPipelineList
         .SamplerCreateInfo = json["SamplerCreateInfo"],
         .AttachmentDescription = json["AttachmentDescription"],
         .TextureType = json["TextureType"]*/
    };
}

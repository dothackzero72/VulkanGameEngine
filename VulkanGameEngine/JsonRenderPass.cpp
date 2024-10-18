#include "JsonRenderPass.h"

JsonRenderPass::JsonRenderPass()
{
}

JsonRenderPass::~JsonRenderPass()
{
}

void JsonRenderPass::JsonCreateRenderPass(String JsonPath)
{
    nlohmann::json jsonLoader;
    RenderPassBuildInfoModel renderPassBuildInfo = JsonToRenderPassBuildInfoModel(jsonLoader);

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
                    RenderedColorTextureList.emplace_back(RenderedTexture());
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
                 depthTexture = DepthTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo);
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
                RenderedColorTextureList.emplace_back(RenderedTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
                resolveAttachmentReferenceList.emplace_back(VkAttachmentReference
                    {
                        .attachment = static_cast<uint32>(colorAttachmentReferenceList.size() + 1),
                        .layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                    });
                break;
            }
            default:
            {

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
        for (VkSubpassDependency subpass : renderPassBuildInfo.SubpassDependencyList)
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
        vkCreateRenderPass(cRenderer.Device, &renderPassInfo, nullptr, &RenderPass);
    }
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
    return RenderPassBuildInfoModel();
}